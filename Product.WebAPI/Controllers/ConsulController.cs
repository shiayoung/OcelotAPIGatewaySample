using Consul;
using DnsClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.WebAPI.Infrastructrue;
using System.Threading.Tasks;

namespace Product.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsulController : ControllerBase
    {
        private readonly ILogger<ConsulController> _logger;
        private readonly IConsulClient _consulClient;
        private readonly IDnsQuery _dns;
        private readonly IOptions<ConsulConfig> _consulConfig;

        public ConsulController(IConsulClient consulClient,
            IDnsQuery dns,
            IOptions<ConsulConfig> consulConfig, 
            ILogger<ConsulController> logger)
        {
            _consulClient = consulClient;
            _dns = dns;
            _consulConfig = consulConfig;
            _logger = logger;
        }

        [HttpGet("listService")]
        public async Task<IActionResult> ListService()
        {
            // /api/servicediscovery/listService
            return Ok(await _consulClient.Catalog.Service(_consulConfig.Value.ServiceName));
        }

        [HttpGet("listHealth")]
        public async Task<IActionResult> ListHealth()
        {
            // /api/servicediscovery/listHealth
            return Ok(await _consulClient.Health.Service(_consulConfig.Value.ServiceName, tag: null, passingOnly: true));
        }
        
        [HttpGet("resolveService")]
        public async Task<IActionResult> ResolveService()
        {
            // /api/servicediscovery/resolveService
            return Ok(await _dns.ResolveServiceAsync("service.consul", _consulConfig.Value.ServiceName));
        }
    }
}
