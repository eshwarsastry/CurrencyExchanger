using CurrencyExchanger.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CurrencyExchanger.Tests
{
    public class SystemTests
    {
        #region Positive cases Tests.

        // Test for the right input format and valid ISO codes.
        [Fact]
        public async Task ConvertCurrencyAsync_ReturnsExpectedResult1()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["EUR/USD", "45.5"]);

            // Assert
            Assert.True(result > 0);
        }

        // Test for the right input format and valid ISO codes - checking for case sensitivity.
        [Fact]
        public async Task ConvertCurrencyAsync_ReturnsExpectedResult2()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["usd/EUR", "130."]);

            // Assert
            Assert.True(result > 0);
        }
        #endregion

        #region Negative cases Tests.
        // Test for the input with wrong input format.
        [Fact]
        public async Task ConvertCurrencyAsync_InputWrongFormat1()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["EURO/DOLLAR", "45.5"]);

            // Assert
            Assert.True(result == null);
        }

        // Test for the input with wrong input format.
        [Fact]
        public async Task ConvertCurrencyAsync_InputWrongFormat2()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["EUR&DOLLAR", "45.5"]);

            // Assert
            Assert.True(result == null);
        }

        // Test for the input with wrong ISO codes.
        [Fact]
        public async Task ConvertCurrencyAsync_InputWrongISOCodes()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["XYZ/USD", "45.5"]);

            // Assert
            Assert.True(result == null);
        }
        #endregion

        #region Testing edge cases.

        // Test for the input with same ISO codes.
        [Fact]
        public async Task ConvertCurrencyAsync_TestSameISOInput()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();
            string amount = "10.55";
            // Act
            var result = await converter.ConvertCurrency(["DKK/DKK", amount]);

            // Assert
            Assert.True(result == Decimal.Parse(amount));
        }

        // Test for the input with conversion amount = 0.
        [Fact]
        public async Task ConvertCurrencyAsync_TestBaseInputAmount()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["EUR/USD", "0"]);

            // Assert
            Assert.True(result == 0);
        }

        // Test for the negative input amount.
        [Fact]
        public async Task ConvertCurrencyAsync_TestNegativeInputAmount()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ConvertCurrency(["EUR/USD", "-2"]);

            // Assert
            Assert.True(result == null);
        }
        #endregion
    }
}
