using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Raziel.Ork {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging => {
                    logging.SetMinimumLevel(LogLevel.Error);
                    logging.AddFilter((provider, category, logLevel) => category == $"Ork-{Environment.GetEnvironmentVariable("Settings:Account")}");
                });
        }
    }
}