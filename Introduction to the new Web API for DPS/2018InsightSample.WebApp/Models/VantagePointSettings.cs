using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightAPISample.WebApp.Models
{
    public class VantagePointSettings
    {
        public string BaseURL { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string Database { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public string grant_type { get; set; } = "password";
        public string integrated { get; set; } = "N";
    }
}
