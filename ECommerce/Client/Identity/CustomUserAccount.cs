using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Client.Identity
{
    public class CustomUserAccount:RemoteUserAccount
    {
        [JsonPropertyName("picture")]
        public string ProfilePicture { get; set; }
    }
}
