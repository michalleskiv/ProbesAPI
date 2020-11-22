using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                        DefinitionsEndpoint = Configuration.GetSection("DefinitionsEndpoint").Value,
                        ProbeEndpoint = Configuration.GetSection("ProbeEndpoint").Value,
                        IdApp = Configuration.GetSection("IdApp").Value,
                        IdSchema = Configuration.GetSection("IdSchema").Value,
                        Token = Configuration.GetSection("Token").Value,
                    }
                    ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
