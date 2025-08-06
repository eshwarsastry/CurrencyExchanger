using CurrencyExchanger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
internal class Program
{
    private static void Main(string[] args)
    {
        // Set up dependency injection for the CurrencyConverter service.
        var services = new ServiceCollection();

        services.AddHttpClient();
        services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
        services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

        var serviceProvider = services.BuildServiceProvider();

        var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

        var result = converter.ConvertCurrency(args).Result;
    }
}