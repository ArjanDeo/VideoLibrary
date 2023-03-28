using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace VideoLibrary_WebApp
{
    public class Program
    {
        private static IConfigurationRoot _config;

        private static Lazy<ConnectionMultiplexer> LazyCacheConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = _config.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return LazyCacheConnection.Value;
            }
        }

        private static void CreateConfig()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public static void Main(string[] args)
        {
            CreateConfig();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}