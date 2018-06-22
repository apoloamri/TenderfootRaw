using Tenderfoot.TfSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Tenderfoot.Mvc
{
    public class TfStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddMemoryCache();
            services.Configure<RazorViewEngineOptions>(razor =>
            {
                // {2} is area, {1} is controller,{0} is the action    
                razor.ViewLocationFormats.Clear();
                razor.ViewLocationFormats.Add("/wwwroot/{1}/{0}" + RazorViewEngine.ViewExtension);
                razor.ViewLocationFormats.Add("/wwwroot/shared/{0}" + RazorViewEngine.ViewExtension);
                razor.ViewLocationFormats.Add("/wwwroot/{0}" + RazorViewEngine.ViewExtension);
            });
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
            app.UseStaticFiles();
        }
    }
}
