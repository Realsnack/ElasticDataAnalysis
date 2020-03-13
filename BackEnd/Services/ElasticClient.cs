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
            Uri[] uris = new Uri[uriStringArray.Length];
            for (int i = 0; i < uriStringArray.Length; i++)
                uris[i] = new Uri(uriStringArray[i]);
            var connectionPool = new SniffingConnectionPool(uris);

            var username = configuration.GetValue<string>("Elastic:Username");
            var password = configuration.GetValue<string>("Elastic:Password");

            _escalationsIndex = configuration.GetValue<string>("Elastic:Escalations-index");
            _inDoneIndex = configuration.GetValue<string>("Elastic:InDone-index");

            var settings = new ConnectionSettings(connectionPool).BasicAuthentication(username, password).DefaultIndex(_escalationsIndex);
            _client = new ElasticClient(settings);
        }

        public async Task<IEnumerable<Escalation>> GetEscalationsAsync()
        {
            var count = _client.Count<Escalation>(e => e
                .Query(q => q
                    .MatchAll()));

            var escalations = await _client.SearchAsync<Escalation>(e => e
                .Size(Convert.ToInt32(count.Count)));

            return escalations.HitsMetadata.Hits.Select(s => s.Source);
        }

        public async Task<IEnumerable<InDone>> GetTransactionAsync(string dionId)
        {
            var searchTrans = await _client.SearchAsync<InDone>(d => d
                    .Index(_inDoneIndex)
                    .Query(q => q
                        .Match(m => m
                            .Field(f => f.OtherParameters.DionHeaderScreeningRequestUniqueId)
                            .Query(dionId))));
            
            return searchTrans.HitsMetadata.Hits.Select(s => s.Source);
        }
    }
}