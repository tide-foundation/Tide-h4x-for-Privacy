using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raziel.Library.Models;
using Raziel.Ork.Models;

namespace Raziel.Ork {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            var settings = new Settings();
            Configuration.Bind("Settings", settings);
            services.AddSingleton(settings);

            services.AddSingleton<ITideAuthentication, EosTideAuthentication>();
            services.AddSingleton<IAdminTideAuthentication, EosAdminTideAuthentication>();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}