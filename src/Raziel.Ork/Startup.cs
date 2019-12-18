// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raziel.Library.Classes;
using Raziel.Library.Models;
using Raziel.Ork.Classes;
using Tide.Encryption.EcDSA;
using Tide.Encryption.Threshold.Authentication;

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
            services.AddSingleton(new TParams(EcDSAKey.FromPrivate(settings.EcDSAKey)));
            services.AddScoped<ITideLogger, TideLogger>();
            services.AddSingleton<ITideAuthentication, EosTideAuthentication>();
            services.AddSingleton(new EnvironmentalAccountManager(EcDSAKey.FromPrivate(Configuration[settings.UserShare])) as IAccountManager);
            services.AddSingleton<IAdminTideAuthentication, EosAdminTideAuthentication>();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyHeader());

            app.UseMvc();
        }
    }
}