using System.Text.Json.Serialization;

namespace CurrencyExchanger.Data
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("base")]
        public string BaseCurrency { get; set; } = "DKK";

        [JsonPropertyName("results")]
        public Dictionary<string, decimal> Rates { get; set; } 

        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonPropertyName("ms")]
        public int Milliseconds { get; set; }
    }
}
