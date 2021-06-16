using DAL.Models;
using FluentFTP;
using GP_API.Repos;
using GP_API.Services;
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

            services.AddControllers();

            services.Configure<FTPServerSettings>(Configuration.GetSection("FTPServerSettings"));

            services.AddSingleton<IFtpServerSettings>(sp => {
                return sp.GetRequiredService<IOptions<FTPServerSettings>>().Value;
            });

            services.AddSingleton<ILocalFileEnvironment, LocalFileEnvironment>();
            services.AddSingleton<IRemoteFileEnvironment, RemoteFileEnvironment>();
            services.AddSingleton<IFileEnvironment>(ser=>
            {
                if (Configuration.GetValue<bool>("UseLocalServer"))
                    return ser.GetService<ILocalFileEnvironment>();
                else
                    return ser.GetService<IRemoteFileEnvironment>();
            });

            services.Configure<LocalServerSettings>(Configuration.GetSection("LocalServerSettings"));

            services.AddSingleton<ILocalServerSettings>(sp => {
                return sp.GetRequiredService<IOptions<LocalServerSettings>>().Value;
            });

            services.AddTransient<FtpClient>(op=>
            {
                var remoteServer = op.GetService<IFtpServerSettings>();
                return new FtpClient(remoteServer.Uri,remoteServer.Username,remoteServer.Password);
            });
            services.AddTransient<IRemoteFileService, RemoteFileService>();
            services.AddTransient<ILocalFileService,LocalFileService>();

            services.AddScoped<IFileService>(op=> {
                if (Configuration.GetValue<bool>("UseLocalServer"))
                    return op.GetService<ILocalFileService>();
                else
                    return op.GetService<IRemoteFileService>();
            });


            services.AddScoped<IFileService, RemoteFileService>();
            services.AddScoped<ICaseRepo,CaseService>();
            services.AddScoped<IFileRepo, DataBaseFileService>();

            services.AddDbContext<CaseContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("CaseConn"));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
