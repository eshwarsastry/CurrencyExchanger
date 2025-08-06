using CurrencyExchanger.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangerTest
{
    public class UnitTest2
    {
        #region Positive case Test.
        [Fact]
        public async Task ProcessConversionAsync_ReturnsExpectedResult1()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddSingleton<IExchangeRateFetcher, ExchangeRateFetcher>();
            services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

            var serviceProvider = services.BuildServiceProvider();

            var converter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            // Act
            var result = await converter.ProcessConversion(45, "EUR", "USD");

            // Assert
            Assert.True(result > 0);
        }
        #endregion
    }
}
