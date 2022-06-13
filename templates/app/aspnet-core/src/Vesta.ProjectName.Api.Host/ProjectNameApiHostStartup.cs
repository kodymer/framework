using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Vesta.ProjectName.Configuration;
using System.Text.Json;

namespace Vesta.ProjectName
{
    /// <summary>
    /// Clase de inicio
    /// </summary>
    public class ProjectNameApiHostStartup
    {

        public ProjectNameApiHostStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProjectNameApi(Configuration);

            services.AddSwagger(Configuration);
            services.AddApplicationInsights(Configuration);
            services.AddHealthChecks(Configuration);
            services.AddAuthentication(Configuration);
        }

 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger(Configuration);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireAuthorization(); // <- Comment, if you don't want protection

                endpoints
                    .MapHealthChecks();

                endpoints
                    .MapDefaultControllerRoute();
            });
        }
    }
}
