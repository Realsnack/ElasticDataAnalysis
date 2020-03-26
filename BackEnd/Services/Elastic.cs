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
        private readonly string _scoringDoneIndex;
        private readonly string _errorMessage;
        private bool _successfulConnection;

        /// <summary>
        /// Elastic service constructor. Prepares the configuration and creates an ElasticClient.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
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
            _scoringDoneIndex = configuration.GetValue<string>("Elastic:ScoringDone-index");
            _errorMessage = configuration.GetValue<string>("Elastic:ErrorMessage");

            var settings = new ConnectionSettings(uris[0]).BasicAuthentication(username, password).DefaultIndex(_escalationsIndex).RequestTimeout(new TimeSpan(0,0,10));
            _client = new ElasticClient(settings);
            _logger.LogDebug($"Created new ElasticClient");

            TestConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Escalation>> GetEscalationsAsync()
        {
            _logger.LogDebug("Started GetEscalationsAsync method");
            if (!_successfulConnection)
                await TestConnection();

            if (_successfulConnection)
            {
                var count = await _client.CountAsync<Escalation>(e => e.Index(_escalationsIndex));
                _logger.LogInformation($"Got count of {count.Count} escalations from {_escalationsIndex}");

                _logger.LogDebug($"{BackEnd.Startup.LogTimeStamp()}Started query of escalations");
                var escalations = await _client.SearchAsync<Escalation>(e => e
                    .Size(Convert.ToInt32(count.Count))
                    .Query(q => q
                        .MatchAll()));
                _logger.LogInformation($"{BackEnd.Startup.LogTimeStamp()}Got {escalations.Hits.Count} Escalations.");

                return escalations.HitsMetadata.Hits.Select(s => s.Source).ToList();
            }
            else
            {
                _logger.LogWarning($"{BackEnd.Startup.LogTimeStamp()}Couldn't return any escalations due to unsuccessful connection");
                return new List<Escalation> { new Escalation("", "", "", "", "", "Error", "", "", "", "", "", "", "", "") };
            }
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

        public async Task<IEnumerable<ScoringDone>> GetTransactionResultAsync(string dionId)
        {
            var searchResult = await _client.SearchAsync<ScoringDone>(d => d
                .Index(_scoringDoneIndex)
                .Query(q =>
                    q.Match(m => m
                        .Field(f => f.DionId)
                        .Query(dionId))));

            if (searchResult.Hits.Count == 0)
                return null;

            return searchResult.HitsMetadata.Hits.Select(s => s.Source);
        }

        public async Task<IEnumerable<ErrorTransaction>> GetErrorTransactionsAsync()
        {
            var count = await _client.CountAsync<ScoringDone>(e => e
                .Index(_scoringDoneIndex)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Error.Code)
                        .Query(_errorMessage))));
                        
            _logger.LogDebug($"Got {count.Count} errors");

            var searchError = await _client.SearchAsync<ScoringDone>(e => e
                .Index(_scoringDoneIndex)
                .Size(Convert.ToInt32(count.Count))
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Error.Code)
                        .Query(_errorMessage))));

            if (searchError.Hits.Count == 0)
                return null;

            var returnList = new List<ErrorTransaction>();
            foreach (var trans in searchError.HitsMetadata.Hits.Select(s => s.Source))
            {
                returnList.Add(new ErrorTransaction(trans));
            }
            
            return returnList;
        }

        private async Task TestConnection()
        {
            _logger.LogDebug($"{BackEnd.Startup.LogTimeStamp()}Started PING to Elastic");
            var connectionTest = await _client.PingAsync();

            if (connectionTest.ApiCall.HttpStatusCode == null)
            {
                _logger.LogWarning($"{BackEnd.Startup.LogTimeStamp()}Unsuccessful PING to Elasticsearch most likely due to the server not responding in time ({_client.ConnectionSettings.RequestTimeout.TotalSeconds}).\nConnection information:\n\tURI: {connectionTest.ApiCall.Uri}\n\tException: {connectionTest.OriginalException.Message}\n\tStackTrace{connectionTest.OriginalException.StackTrace}");
                _successfulConnection = false;
            }
            else if (connectionTest.ApiCall.HttpStatusCode == 200)
            {
                _logger.LogDebug($"{BackEnd.Startup.LogTimeStamp()}Successful PING to Elasticsearch\nCode: {connectionTest.ApiCall.HttpStatusCode}");
                _successfulConnection = true;
            }
            else
            {
                _logger.LogWarning($"{BackEnd.Startup.LogTimeStamp()}Unsuccessful PING to Elasticsearch\nCode: {connectionTest.ApiCall.HttpStatusCode}");
                _successfulConnection = false;
            }
        }
    }
}