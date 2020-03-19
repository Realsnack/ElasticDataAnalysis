using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FrontEnd.Models;
using System.Net.Http;
using FrontEnd.Services;
using Microsoft.Extensions.Configuration;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiClient _client;
        private readonly string Env;

        public IEnumerable<TransactionEscalation> TransactionEscalations { get; set; }

        public HomeController(ILogger<HomeController> logger, IApiClient client, IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            Env = configuration.GetValue<string>("Environment");
        }

        public async Task<IActionResult> Index()
        {
            // Load from BackEnd API
            var data = await _client.GetTransactionsWithEscalationsAsync();
            ViewBag.transactions = data.ToList();
            _logger.LogInformation($"Loaded {data.Count} transactions from BackEnd API");
            ViewBag.environment = Env;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
