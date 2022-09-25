using System;
using Newtonsoft.Json;

namespace PhuLongCRM.Models
{
    public class GrapDownLoadUrlModel
    {
        [JsonProperty("@microsoft.graph.downloadUrl")]
        public string MicrosoftGraphDownloadUrl { get; set; }
        public DateTime createdDateTime { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
    }
}
