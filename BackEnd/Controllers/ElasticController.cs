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
            _logger.LogInformation($"{DateTime.Now}:GetTransactionEscalationsAsync()");
            var escalationsResponse = await _client.GetEscalationsAsync();
            var escalations = escalationsResponse.ToList();
            _logger.LogDebug($"Loaded {escalations.Count} from GetEscalationsAsync()");

            List<TransactionEscalation> transactionsList = new List<TransactionEscalation>();

            foreach (var escalation in escalations)
            {
                try
                {
                    var transaction = await _client.GetTransactionAsync(escalation.Sid);
                    var result = await _client.GetTransactionResultAsync(escalation.Sid);
                    if (transaction != null)
                    {
                        _logger.LogDebug($"Valid transaciton");
                        var inDone = transaction.ToList();
                        var scoringDone = result.ToList();
                        _logger.LogDebug($"Transaction is {inDone[0].JmsId} from ID{escalation.Sid}");
                        if (result != null)
                        {
                            _logger.LogDebug($"Added TransactionEscalation JMS ID: {inDone[0].JmsId}\nDionID: {escalation.Sid}\nResult: {scoringDone[0].ScoreNum}");
                            transactionsList.Add(new TransactionEscalation(inDone[0], escalation, scoringDone[0]));
                        }
                        else
                        {
                            _logger.LogInformation($"Couldn't get result of transaction {escalation.Sid}");
                        }
                    }
                    else
                        _logger.LogInformation($"Couldn't get result of transaction {escalation.Sid}");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception");
                }
            }

            _logger.LogInformation($"{DateTime.Now}:Returining {transactionsList.Count} transactions");
            return transactionsList;
        }

        [HttpGet("errors")]
        public async Task<IEnumerable<ErrorTransaction>> GetErrorTransactionsAsync()
        {
            _logger.LogDebug("Started api/elastic/errors.");
            _logger.LogInformation($"{DateTime.Now}:GetErrorTransactionsAsync()");
            var errorTransactionResponse = await _client.GetErrorTransactionsAsync();
            // var errorTransactions = errorTransactionResponse.ToList();
            _logger.LogDebug($"Loaded {errorTransactionResponse.Count()} from GetErrorTransactionsAsync()");

            return errorTransactionResponse;
        }
    }
}