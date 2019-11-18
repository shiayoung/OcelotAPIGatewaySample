using System;
using System.Net;
using Consul;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product.WebAPI.Infrastructrue;
using Product.WebAPI.Middlewares;

namespace Product.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //private IServerAddressesFeature _serverAddressesFeature;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IDnsQuery>(p =>
            {
                var strConsulDnsLookupIP = Configuration.GetValue<string>("consulDnsLookupIp");
                IPAddress consulDnsLookupIp;
                if (!string.IsNullOrEmpty(strConsulDnsLookupIP))
                    consulDnsLookupIp = IPAddress.Parse(strConsulDnsLookupIP);
                else
                    consulDnsLookupIp = IPAddress.Loopback;

                return new LookupClient(consulDnsLookupIp, 8600);
            });
            services.AddSingleton<IHostedService, ConsulHostedService>();
            services.Configure<ConsulConfig>(Configuration.GetSection("consulConfig"));
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = Configuration["consulConfig:address"];
                consulConfig.Address = new Uri(address);
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<HealthCheckMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
