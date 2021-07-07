using DAL.Models;
using Detached.Mappers.EntityFramework;
using Detached.Mappers.Model;
using FluentFTP;
using GP_API;
using GP_API.FileEnvironments;
using GP_API.Repos;
using GP_API.Services;
using GP_API.Settings;
using GP_API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Cores and NewtonsoftJson

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            services.AddControllers().AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
          );
            #endregion



            #region Configuration of File Server: local and remote

            services.Configure<FileServiceSettings>(Configuration.GetSection("FileServiceSettings"));

            services.AddSingleton<IFileServiceSettings>(sp =>
            {

                var value = sp.GetRequiredService<IOptions<FileServiceSettings>>().Value;
                return value;
            });

            services.AddScoped<IFtpClient,FtpClient>(op =>
            {
                var remoteServer = op.GetService<IFileServiceSettings>();
                var client =  new FtpClient(remoteServer.RemoteServer.Url, remoteServer.RemoteServer.Username, remoteServer.RemoteServer.Password);
                client.Connect();
                return client;
            });

            services.AddTransient<ILocalFileEnvironment, LocalFileEnvironment>();
            services.AddTransient<IRemoteFileEnvironment, RemoteFileEnvironment>();
            services.AddSingleton<IFileEnvironment>(ser =>
            {
                var factory = ser.GetService<IFileEnvironmentFactory>();
                var options = ser.GetService<IFileServiceSettings>();
                return factory.GetFileEnvironment(options.Mode);
            });

            
            services.AddTransient<IRemoteFileService, RemoteFileService>();
            services.AddTransient<ILocalFileService, LocalFileService>();
            
            services.AddScoped<IFileService>(ser =>
            {
                var factory = ser.GetService<IFileServiceFactory>();
                var options = ser.GetService<IFileServiceSettings>();
                return factory.GetFileService(options.Mode);
            });



            services.AddScoped<IFileEnvironmentFactory>(ser =>
            {
                
                return new FileEnvironmentFactory((mode, pathKey) =>
                {
                    var options = ser.GetService<IFileServiceSettings>();
                    IFileEnvironment env;
                    switch (mode)
                    {
                        case FileServiceMode.Local:
                            env = ser.GetService<ILocalFileEnvironment>();
                            if(pathKey != null)
                                env.UserInternalPath(options.InternalPaths[pathKey]);
                            break;
                        case FileServiceMode.Remote:
                            env = ser.GetService<IRemoteFileEnvironment>();
                            if (pathKey != null)
                                env.UserInternalPath(options.InternalPaths[pathKey]);
                            break;
                        default:
                            env = ser.GetService<IRemoteFileEnvironment>();
                            if (pathKey != null)
                                env.UserInternalPath(options.InternalPaths[pathKey]);
                            break;
                    }

                    return env;
                });
            });

            services.AddScoped<IFileServiceFactory>(ser =>
            {

                return new FileServiceFactory((mode, pathKey) =>
                {
                    var envFactory = ser.GetService<IFileEnvironmentFactory>();
                    IFileService serv;
                    switch (mode)
                    {
                        case FileServiceMode.Local:
                            serv = ser.GetService<ILocalFileService>();
                            break;
                        case FileServiceMode.Remote:
                            serv = ser.GetService<IRemoteFileService>();
                            break;
                        default:
                            serv = ser.GetService<IRemoteFileService>();
                            break;
                    }
                    serv.UseInternalEnvironment(envFactory.GetFileEnvironment(mode, pathKey));

                    return serv;
                });
            });

            #endregion


            #region configurations of the ScheduledCaseFileWorkerService

            // this line register the service that run every interval of time 
            // to move files to directory per case
            //services.Configure<DirectoryPerCaseServiceSettings>(Configuration.GetSection(nameof(DirectoryPerCaseServiceSettings)));
            //services.AddSingleton<DirectoryPerCaseServiceSettings>(sp =>
            //{
            //    return sp.GetRequiredService<IOptions<DirectoryPerCaseServiceSettings>>().Value;
            //});

            //services.AddHostedService<ScheduledCaseFileWorkerService>();
            #endregion


            #region Configurations of Case Discription Mapper
            services.AddSingleton<ICaseFileUrlMapper, CaseFileUrlMapper>((ser) =>
            {
                var actionrouteString = Configuration.GetValue<string>("DownloadActionUrl");
                return new CaseFileUrlMapper(actionrouteString);
            });
            #endregion


            #region Repositories, Database Context, Mapper ,and Identity
            services.AddScoped<ICaseRepo, CaseRepo>();
            services.AddScoped<ICaseFileRepo, CaseFileRepo>();

            services.AddDbContext<CaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CaseConn"));
                options.UseDetached();
            });

            //continue of identity
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<CaseContext>()
                .AddDefaultTokenProviders();

            services.Configure<MapperOptions>(m =>
            {
                m.Configure<Case>().IsEntity();
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var test = context.Request.Cookies.TryGetValue("X-Access-Token", out string test2);

                        foreach (var cookie in context.Request.Cookies)
                        {
                            Trace.WriteLine(cookie);
                        }

                        if (context.Request.Cookies.ContainsKey("X-Access-Token") && !context.Request.Headers.ContainsKey("Authorization"))
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                        }

                        return Task.CompletedTask;
                    }
                };
            });




            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;
            });
            #endregion


            #region Swagger


            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "GP_API", Version = "v1" });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                swagger.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line

            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GP_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
