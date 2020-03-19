using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FrontEnd.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(ILogger<ApiClient> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            try
            {
                _httpClient.BaseAddress = configuration.GetValue<Uri>("Connections:BackEnd");
                _logger.LogDebug($"http client uses address: {configuration.GetValue<Uri>("Connections:BackEnd")}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while creating new HttpClient");
            }
        }

        public async Task<List<TransactionEscalation>> GetTransactionsWithEscalationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/elastic/transaction");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<List<TransactionEscalation>>();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}