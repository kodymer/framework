using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Vesta.Banks.Configuration;
using System.Text.Json;

namespace Vesta.Banks
{
    /// <summary>
    /// Clase de inicio
    /// </summary>
    public class BanksApiHostStartup
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        /// <param name="configuration"></param>
        public BanksApiHostStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Inyecci�n de la configuraci�n
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// M�todo llamado en tiempo de ejecuci�n.  Utilizado para agregar servicios al contenedor.
        /// </summary>
        /// <param name="services">lista de servicios a agregar</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBanksApi(Configuration);

            services.AddSwagger(Configuration);
            services.AddApplicationInsights(Configuration);
            services.AddHealthChecks(Configuration);
            services.AddAuthentication(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
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
                endpoints.MapControllers()
                    /*.RequireAuthorization()*/;
                endpoints.MapHealthChecks();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
