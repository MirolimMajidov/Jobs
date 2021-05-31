using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TestGrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            var host = CreateHostBuilder(configuration, args).Build();
            host.Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureKestrel(options =>
                    {
                        var (httpPort, grpcPort) = GetDefinedPorts(configuration);
                        options.Listen(IPAddress.Any, httpPort,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
                        options.Listen(IPAddress.Any, grpcPort,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                    })
                    .UseStartup<Startup>();
                });

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration configuration)
        {
            var grpcPort = configuration.GetValue("GRPC_PORT", 5101);
            var port = configuration.GetValue("PORT", 5001);
            return (port, grpcPort);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
