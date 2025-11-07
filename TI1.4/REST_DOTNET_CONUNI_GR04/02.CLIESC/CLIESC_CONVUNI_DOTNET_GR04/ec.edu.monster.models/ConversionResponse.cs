using System.Text.Json.Serialization;

namespace CLIESC_CONVUNI_DOTNET_GR04.Models
{
    public sealed class ConversionResponse
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        // El backend envía texto ("45,00 °C" / "113,00 °F")
        [JsonPropertyName("input")]
        public string? Input { get; set; }

        [JsonPropertyName("output")]
        public string? Output { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        // opcional: para depurar
        [JsonIgnore]
        public string? Raw { get; set; }
    }
}
