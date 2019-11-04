using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightAPISample.WebApp.Models
{
    public class TokenInfo
    {
        public string access_token { get; set; } = "";
        public string token_type { get; set; } = "";
        public int expires_in { get; set; } = 0;
        public string refresh_token { get; set; } = "";
    }
}
