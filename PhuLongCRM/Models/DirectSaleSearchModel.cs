using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace PhuLongCRM.Models
{
    public class DirectSaleSearchModel
    {
        public string Project { get; set; }
        [JsonProperty("Block", NullValueHandling = NullValueHandling.Ignore)]
        public string Block { get; set; }
        [JsonProperty("Phase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phase { get; set; }
        [JsonProperty("Event", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Event { get; set; }
        [JsonProperty("Unit", NullValueHandling = NullValueHandling.Ignore)]
        public string Unit { get; set; }
        [JsonProperty("Direction", NullValueHandling = NullValueHandling.Ignore)]
        public string Direction { get; set; }
        [JsonProperty("view", NullValueHandling = NullValueHandling.Ignore)]
        public string view { get; set; }
        [JsonProperty("stsUnit", NullValueHandling = NullValueHandling.Ignore)]
        public string stsUnit { get; set; }
        [JsonProperty("Area", NullValueHandling = NullValueHandling.Ignore)]
        public string Area { get; set; }
        [JsonProperty("Price", NullValueHandling = NullValueHandling.Ignore)]
        public string Price { get; set; }
        public DirectSaleSearchModel(string projectId, string phasesLanchId = null, bool? isEvent = null, string unitCode = null, string directions = null, string views = null, string unitStatuses = null, string netArea = null, string price = null)
        {
            Project = projectId;
            Phase = phasesLanchId;
            Event = isEvent;
            Unit = unitCode;
            Direction = directions;
            view = views;
            stsUnit = unitStatuses;
            Area = netArea;
            Price = price;
        }
    }
}
