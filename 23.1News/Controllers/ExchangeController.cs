using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Security.Claims;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.CodeAnalysis.CSharp;
using _23._1News.Models.Db;
using Microsoft.Extensions.Logging;

namespace _23._1News.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly ILogger<ExchangeController> _logger;
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly ISubscriptionService _subscriptionService;

        public ExchangeController(IExchangeRatesService exchangeRatesService, 
            ISubscriptionService subscriptionService,
            ILogger<ExchangeController> logger)
        {
            _exchangeRatesService = exchangeRatesService;
            _subscriptionService = subscriptionService;
            _logger = logger;
        }



        public async Task<IActionResult> LatestRates()
        {
            // Get the user ID of the logged-in user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null && _subscriptionService.isEnteprise(userId))
            {
                var newRates = await _exchangeRatesService.GetRateAsync();
                return View(newRates);
            }

            return RedirectToAction("Index", "Home");
        }


        //[Route("nr")]
        //public async Task<IActionResult> NewRates()
        //{
        //    var newR = await _exchangeRatesService.GetRateAsync();


        //    return View(newR);
        //}



        [Route("gh")]

        public async Task<ActionResult> GetHostorical(Dictionary<string, decimal> exchangeRates, DateTime date)
        {
            try
            {
                var data = await _exchangeRatesService.HistoricalData(exchangeRates, date);
                return View(data);
            }
            catch (FormatException ex)
            {
                // Log the exception details for debugging
                _logger.LogError(ex, "Error parsing connection string");
                throw; // Rethrow the exception if needed
            }

        }

    }
}
