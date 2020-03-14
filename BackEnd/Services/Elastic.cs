using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;

namespace BackEnd.Services
{
    public class Elastic : IElastic
    {
        private readonly ILogger<Elastic> _logger;
        private readonly ElasticClient _client;
        private readonly string _escalationsIndex;
        private readonly string _inDoneIndex;

        public Elastic(ILogger<Elastic> logger, IConfiguration configuration)
        {
            _logger = logger;
            var uriString = configuration.GetValue<string>("Elastic:Host");
            var uriStringArray = uriString.Split(",");
            _logger.LogDebug($"uriString: {uriString}");
            Uri[] uris = new Uri[uriStringArray.Length];
            for (int i = 0; i < uriStringArray.Length; i++)
            {
                uris[i] = new Uri(uriStringArray[i]);
                _logger.LogDebug($"Created uri: {uris[i].ToString()}");
            }

            var connectionPool = new SniffingConnectionPool(uris);

            var username = configuration.GetValue<string>("Elastic:Username");
            var password = configuration.GetValue<string>("Elastic:Password");

            _escalationsIndex = configuration.GetValue<string>("Elastic:Escalations-index");
            _inDoneIndex = configuration.GetValue<string>("Elastic:InDone-index");

            var settings = new ConnectionSettings(uris[0]).BasicAuthentication(username, password).DefaultIndex(_escalationsIndex);
            _client = new ElasticClient(settings);
            _logger.LogDebug($"Created new ElasticClient");
        }

        public async Task<IEnumerable<Escalation>> GetEscalationsAsync()
        {
            _logger.LogDebug("Started GetEscalationsAsync method");
            var count = await _client.CountAsync<Escalation>(e => e.Index(_escalationsIndex));
            _logger.LogInformation($"Got count of {count.Count} escalations from {_escalationsIndex}");

            _logger.LogDebug("Started query of escalations");
            var escalations = await _client.SearchAsync<Escalation>(e => e
                .Size(Convert.ToInt32(count.Count))
                .Query(q => q
                    .MatchAll()));
            _logger.LogInformation($"Got {escalations.Hits.Count} Escalations.");

            return escalations.HitsMetadata.Hits.Select(s => s.Source).ToList();
        }

        public async Task<IEnumerable<InDone>> GetTransactionAsync(string dionId)
        {
            var searchTrans = await _client.SearchAsync<InDone>(d => d
                    .Index(_inDoneIndex)
                    .Query(q => q
                        .Match(m => m
                            .Field(f => f.OtherParameters.DionHeaderScreeningRequestUniqueId)
                            .Query(dionId))));

            if (searchTrans.Hits.Count == 0)
                return null;

            return searchTrans.HitsMetadata.Hits.Select(s => s.Source);
        }
    }
}