using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CurrencyExchanger.Services
{
    public interface ICurrencyConverter
    {
        public Task<decimal?> ConvertCurrency(string[] input);
        public Task<decimal?> ProcessConversion(decimal amount, string originCurrencyISOCode, string targetCurrencyISOCode);
    }

    public class CurrencyConverter: ICurrencyConverter
    {
        private readonly IExchangeRateFetcher exchangeRateFetcher;
        public CurrencyConverter(IExchangeRateFetcher _exchangeRateFetcher)
        {
            exchangeRateFetcher = _exchangeRateFetcher;
        }

        // Service containing the mathematical functionality for manual currency conversion.
        public async Task<decimal?> ProcessConversion(decimal amount, string originCurrencyISOCode, string targetCurrencyISOCode)
        {
            var exchangeRates = await exchangeRateFetcher.GetRealTimeExchangeRateAsync();
            if (exchangeRates == null)
            {
                Console.WriteLine("Failed to fetch real-time exchange rates. Using static rates instead.");
                exchangeRates = exchangeRateFetcher.GetStaticExchangeRate();
            }
            // Normalise the currency codes to uppercase to ensure consistency.
            originCurrencyISOCode = originCurrencyISOCode.ToUpperInvariant();
            targetCurrencyISOCode = targetCurrencyISOCode.ToUpperInvariant();

            if (!exchangeRates.ContainsKey(originCurrencyISOCode) || !exchangeRates.ContainsKey(targetCurrencyISOCode))
                throw new ArgumentException("Invalid currency code.");

            decimal originCurrencyRate = exchangeRates[originCurrencyISOCode];
            decimal targetCurrencyRate = exchangeRates[targetCurrencyISOCode];

            return amount * (targetCurrencyRate / originCurrencyRate);
        }

        // Wrapper main service to process the input and call the conversion method.
        public async Task<Decimal?> ConvertCurrency(string[] input)
        {
            // Check if the input provided by the user in the command line is in the desired format.
            if (input.Length != 2)
            {
                Console.WriteLine("<Currency Pair> <Amount to Exchange>");
                return null;
            }

            // Accept the input currency pair and amount to exchange.
            string[] inputCurrencyCodes = input[0].Split("/");

            // Validate the input currency pair format.
            if (inputCurrencyCodes.Length != 2)
            {
                Console.WriteLine("Invalid currency pair format. Use 'FROM/TO'.");
                return null;
            }

            // Validate the currency code ISO formats.
            if (inputCurrencyCodes[0].Length != 3 || inputCurrencyCodes[1].Length != 3)
            {
                Console.WriteLine("Please provide the ISO codes of the currencies.");
                return null;
            }

            // Validate the amount to exchange.
            if (!decimal.TryParse(input[1], out decimal amountToExchange) || amountToExchange < 0)
            {
                Console.WriteLine("Invalid amount to exchange.");
                return null;
            }


            try
            {
                var convertedAmount = await ProcessConversion(amountToExchange, inputCurrencyCodes[0], inputCurrencyCodes[1]);
                Console.WriteLine($"Converted Amount: {System.String.Format("{0:F4}", convertedAmount)} {inputCurrencyCodes[1]}");
                return convertedAmount;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        
    }
}
