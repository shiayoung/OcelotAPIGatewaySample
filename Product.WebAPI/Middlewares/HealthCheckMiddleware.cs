using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Product.WebAPI.Middlewares
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HealthCheckMiddleware> _logger;
        private readonly string _healthPath = "/api/health/status";
        public HealthCheckMiddleware(RequestDelegate next, ILogger<HealthCheckMiddleware> logger,
            IConfiguration config)
        {
            _next = next;
            _logger = logger;

            var healthPath = config["Consul:HealthPath"];
            if (!string.IsNullOrEmpty(healthPath))
                _healthPath = healthPath;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == _healthPath)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync("I'm OK!");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
