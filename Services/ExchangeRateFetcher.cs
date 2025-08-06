using CurrencyExchanger.Data;
using System.Text.Json;

namespace CurrencyExchanger.Services
{
    public interface IExchangeRateFetcher
    {
        public Task<Dictionary<string, decimal>> GetRealTimeExchangeRateAsync();
        public Dictionary<string, decimal> GetStaticExchangeRate();
    }
    public class ExchangeRateFetcher : IExchangeRateFetcher
    {
        private readonly HttpClient httpClient;
        // Static exchange rates for fallback when real-time rates are not available.
        private readonly Dictionary<string, decimal> staticExchangeRates = new()
            {
                {"DKK", 100.0m }, // Taken as the Base Currency.
                { "EUR", 743.94m },
                { "USD", 663.11m },
                { "GBP", 852.85m },
                { "JPY", 5.9740m },
                { "SEK", 76.1m },
                { "NOK", 78.4m },
                { "CHF", 683.58m },
            };

        public ExchangeRateFetcher(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        // Service to fetch real-time exchange rates from external fastforex API.
        public async Task<Dictionary<string, decimal>> GetRealTimeExchangeRateAsync()
        {
            string apiKey = Environment.GetEnvironmentVariable("FASTFOREX_API_KEY") ?? String.Empty;

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("API key for FastForex is not set.");
                return null;
            }
            var response = await httpClient.GetAsync($"https://api.fastforex.io/fetch-all?api_key={apiKey}");

            if (!response.IsSuccessStatusCode)
                return null;

            string content = await response.Content.ReadAsStringAsync();


            if (string.IsNullOrEmpty(content))
                return null;

            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(content);

            return data.Rates;
        }

        public Dictionary<string, decimal> GetStaticExchangeRate()
        {
            var staticRatesCoversion = new Dictionary<string, decimal>();
            foreach (var rate in staticExchangeRates)  // staticExchangeRates has to be converted to DKK base.
            {
                staticRatesCoversion.Add(rate.Key, (staticExchangeRates["DKK"] / rate.Value)); // Convert to DKK base.
            }

            return staticRatesCoversion;

        }
    }
}
