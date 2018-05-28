using Tenderfoot.TfSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Tenderfoot.Mvc
{
    public class TfStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            foreach (var origin in TfSettings.Web.AllowOrigins)
            {
                app.UseCors(
                    options => options
                        .WithOrigins(origin)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    );
            }
            
            app.UseMvc();
        }
    }
}
