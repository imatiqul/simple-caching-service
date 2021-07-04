using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace Simple.Caching.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("HostName", Environment.MachineName)
                .Enrich.WithProperty("Application", typeof(Program).Assembly.GetName().Name)
                .Enrich.WithProperty("ApplicationVersion", typeof(Program).Assembly.GetName().Version)
                .Enrich.WithProperty("Environment", configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"))
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                CreateWebHostBuilder(args, configuration).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
            .UseEnvironment(configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"))
            .UseSerilog()
            .UseStartup<Startup>();
    }
}
