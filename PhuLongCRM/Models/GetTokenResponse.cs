using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class GetTokenResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
