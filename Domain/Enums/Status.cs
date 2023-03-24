using System.Text.Json.Serialization;

namespace HomeAssignment.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Pending,
        InProgress,
        Done,
        Canceled
    }
}