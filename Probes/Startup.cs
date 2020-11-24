using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProbesAPI.Middleware;
using ProbesLib.Configurations.Models;
using ProbesLib.Interfaces;
using ProbesLib.Models;

namespace ProbesAPI
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

            services.AddTransient<IProbesWorker>(s => 
                new ProbesWorker(
                    config:new Config
                    {
                        Url = Configuration.GetSection("Url").Value,
                        Endpoint = Configuration.GetSection("Endpoint").Value,
                        FindProbeEndpoint = Configuration.GetSection("FindProbeEndpoint").Value,
                        FilterEndpoint = Configuration.GetSection("FilterEndpoint").Value,
                        ProbeEndpoint = Configuration.GetSection("ProbeEndpoint").Value,
                        AppId = Configuration.GetSection("AppId").Value,
                        TableId = Configuration.GetSection("TableId").Value,
                        Token = Configuration.GetSection("Token").Value,
                    }
                    ));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Probes API",
                    Description = "Extends functionality of DrMax API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(err => err.UseCustomErrors(env));

            app.UseHsts();

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Probes API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
