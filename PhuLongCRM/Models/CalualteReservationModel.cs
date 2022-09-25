using System;
using Newtonsoft.Json;

namespace PhuLongCRM.Models
{
    public class CalualteReservationModel
    {
        [JsonProperty("DKBG", NullValueHandling = NullValueHandling.Ignore)]
        public string DKBG { get; set; }
        [JsonProperty("CKChung", NullValueHandling = NullValueHandling.Ignore)]
        public string CKChung { get; set; }
        [JsonProperty("CKNoiBo", NullValueHandling = NullValueHandling.Ignore)]
        public string CKNoiBo { get; set; }
        [JsonProperty("CKQuyDoi", NullValueHandling = NullValueHandling.Ignore)]
        public string CKQuyDoi { get; set; }
        [JsonProperty("CKPTTT", NullValueHandling = NullValueHandling.Ignore)]
        public string CKPTTT { get; set; }
    }
}
