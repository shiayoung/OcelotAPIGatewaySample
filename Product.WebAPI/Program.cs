using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Product.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            (CreateHostBuilder(args)).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            //var freePort = FreeTcpPort();

            //return WebHost.CreateDefaultBuilder(args)
            //    .UseUrls($"http://localhost:{freePort}")
            //    .UseStartup<Startup>();

            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(config.GetValue<string>("applicationUrls"))
                .UseStartup<Startup>();

            //return (Host.CreateDefaultBuilder(args)
            //   .ConfigureWebHostDefaults(webBuilder =>
            //   {
            //       webBuilder.UseStartup<Startup>();
            //   }));
        }


        private static int FreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
