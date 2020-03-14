using System.Linq;
using System.Threading.Tasks;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElasticController : ControllerBase
    {
        private readonly ILogger<ElasticController> _logger;
        private readonly IElastic _client;

        public ElasticController(ILogger<ElasticController> logger, IElastic elasticClient)
        {
            _logger = logger;
            _client = elasticClient;
        }

        [HttpGet]
        public string Get()
        {
            return "OK";
        }

        [HttpGet("test")]
        public async Task<string> Test()
        {
            var response = await _client.GetEscalationsAsync();

            try
            {
                return response.Select(s => s.MatchingRule).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed in TestGET");
                return $"FAILED: {ex.Message}";
            }
        }

        [HttpGet("transaction")]
        
    }
}