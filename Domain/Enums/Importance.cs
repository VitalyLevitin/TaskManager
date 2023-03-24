using System.Text.Json.Serialization;

namespace HomeAssignment.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Importance
    {
        Low,
        Medium,
        High
    }
}