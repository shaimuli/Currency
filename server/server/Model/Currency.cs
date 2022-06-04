using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace server
{
    public class CurrenciesList
    {
        public Currencies Currencies { get; set; }
    }
    public class Currencies
    {
        public DateTime Last_Update { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
    }

    public class Currency
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("currencycode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }
    }
}