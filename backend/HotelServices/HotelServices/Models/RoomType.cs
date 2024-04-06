using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HotelServices.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoomType
    {
        [EnumMember(Value = "Standard")]
        Standard,
        [EnumMember(Value = "Comfort")]
        Comfort,
        [EnumMember(Value = "Comfort+")]
        ComfortPlus,
        [EnumMember(Value = "Presidential")]
        Presidential
    }
}
