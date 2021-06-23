using DAL.Models;
using Detached.Mappers.EntityFramework;
using Detached.Mappers.Model;
using FluentFTP;
using GP_API.FileEnvironments;
using GP_API.Repos;
using GP_API.Services;
using GP_API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddControllers().AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
          );

            //services.Configure<RemoteServerSettings>(Configuration.GetSection("RemoteServerSettings"));

            //services.AddSingleton<RemoteServerSettings>(sp => {
            //    return sp.GetRequiredService<IOptions<RemoteServerSettings>>().Value;
            //});

            services.Configure<FileServiceSettings>(Configuration.GetSection("FileServiceSettings"));

            services.AddSingleton<IFileServiceSettings>(sp => {

                var value = sp.GetRequiredService<IOptions<FileServiceSettings>>().Value;
                return value;
            });

            services.AddSingleton<ILocalFileEnvironment, LocalFileEnvironment>();
            services.AddSingleton<IRemoteFileEnvironment, RemoteFileEnvironment>();
            services.AddSingleton<ICacheFileEnvironment, CacheFileEnvironment>();
            services.AddSingleton<IFileEnvironment>(ser=>
            {
                var options = ser.GetService<IFileServiceSettings>();
                switch (options.Mode)
                {
                    case FileServiceMode.Local:
                        return ser.GetService<ILocalFileEnvironment>();
                    case FileServiceMode.Remote:
                        return ser.GetService<IRemoteFileEnvironment>();
                    case FileServiceMode.RemoteWithCache:
                        return ser.GetService<ICacheFileEnvironment>();
                    default:
                        return ser.GetService<IRemoteFileEnvironment>();
                }
            });

            //services.Configure<LocalServerSettings>(Configuration.GetSection("LocalServerSettings"));

            //services.AddSingleton<LocalServerSettings>(sp => {
            //    return sp.GetRequiredService<IOptions<LocalServerSettings>>().Value;
            //});




            services.AddTransient<FtpClient>(op=>
            {
                var remoteServer = op.GetService<IFileServiceSettings>();
                return new FtpClient(remoteServer.RemoteServer.Url,remoteServer.RemoteServer.Username,remoteServer.RemoteServer.Password);
            });
            services.AddScoped<IRemoteFileService, RemoteFileService>();
            services.AddScoped<ILocalFileService,LocalFileService>();
            services.AddScoped<ICachedRemoteFileService, CachedRemoteFileService>();

            services.AddScoped<IFileService>(ser=> {
                var options = ser.GetService<IFileServiceSettings>();
                switch (options.Mode)
                {
                    case FileServiceMode.Local:
                        return ser.GetService<ILocalFileService>();
                    case FileServiceMode.Remote:
                        return ser.GetService<IRemoteFileService>();
                    case FileServiceMode.RemoteWithCache:
                        return ser.GetService<ICachedRemoteFileService>();
                    default:
                        return ser.GetService<IRemoteFileService>();
                }
            });


            services.AddScoped<ICaseRepo,CaseService>();
            services.AddScoped<IFileRepo, DataBaseFileService>();

            services.AddDbContext<CaseContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("CaseConn"));
                options.UseDetached();
            });
            services.Configure<MapperOptions>(m =>
            {
                m.Configure<Case>().IsEntity();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GP_API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GP_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
