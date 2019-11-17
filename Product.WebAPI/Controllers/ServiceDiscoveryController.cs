using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.WebAPI.Infrastructrue;
using System.Threading.Tasks;

namespace Product.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceDiscoveryController : ControllerBase
    {
        private readonly ILogger<ServiceDiscoveryController> _logger;
        private readonly IConsulClient _consulClient;
        private readonly IOptions<ConsulConfig> _consulConfig;

        public ServiceDiscoveryController(IConsulClient consulClient, IOptions<ConsulConfig> consulConfig, ILogger<ServiceDiscoveryController> logger)
        {
            _consulClient = consulClient;
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
    }
}
