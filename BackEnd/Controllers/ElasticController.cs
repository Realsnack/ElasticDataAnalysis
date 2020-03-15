using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
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

        /// <summary>
        /// GET method providing TransactionEscalations. Loads ALL escalations and then finds transactions for them.
        /// </summary>
        /// <returns>IEnumerable of TransactionEscalation.</returns>
        [HttpGet("transaction")]
        public async Task<IEnumerable<TransactionEscalation>> GetTransactionEscalationsAsync()
        {
            _logger.LogDebug("Started api/elastic/transaction.");
            _logger.LogInformation($"{DateTime.Now}:GetTransactionEscalations()");
            var escalationsResponse = await _client.GetEscalationsAsync();
            var escalations = escalationsResponse.ToList();
            _logger.LogDebug($"Loaded {escalations.Count} from GetEscalationsAsync()");

            List<TransactionEscalation> transactionsList= new List<TransactionEscalation>();

            foreach (var escalation in escalations)
            {
                try
                {
                    var transaction = await _client.GetTransactionAsync(escalation.Sid);
                    if (transaction != null)
                    {
                        _logger.LogDebug($"Valid transaciton");
                        var inDone = transaction.ToList();
                        _logger.LogDebug($"Transaction is {inDone[0].JmsId} from ID{escalation}");
                        transactionsList.Add(new TransactionEscalation(inDone[0], escalation));
                    }
                    else
                        _logger.LogDebug("Returned null");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error brácho");
                }
            }

            _logger.LogInformation($"{DateTime.Now}:Returining {transactionsList.Count} transactions");
            return transactionsList;
        }
    }
}