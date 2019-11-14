using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Payment.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetPayment")]
        public async Task<string> Get()
        {
            return await Task.FromResult<string>("This is payment service");
        }
    }
}
