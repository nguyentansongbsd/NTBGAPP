using System;
using Newtonsoft.Json;

namespace PhuLongCRM.Models
{
    public class OTPModel
    {
        [JsonIgnore]
        public string key { get; set; }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Phone { get; set; }
        public string OTPCode { get; set; }
        public bool IsSend { get; set; }
        public DateTime Date { get; set; }
    }
}
