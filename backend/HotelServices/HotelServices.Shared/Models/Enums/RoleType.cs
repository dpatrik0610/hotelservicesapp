using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelServices.Shared.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "Manager")]
        Manager,
        [EnumMember(Value = "Admin")]
        Admin
    }
}
