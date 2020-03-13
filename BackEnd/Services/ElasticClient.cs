using System;
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

            var escalationsIndex = configuration.GetValue<string>("Elastic:Escalations-index");
            var inDoneIndex = configuration.GetValue<string>("Elastic:InDone-index");

            var settings = new ConnectionSettings(connectionPool).BasicAuthentication(username, password).DefaultIndex(escalationsIndex);
            _client = new ElasticClient(settings);
        }

        
    }
}