using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AcceptanceListModel
    {
        public Guid bsd_acceptanceid { get; set; }
        public string bsd_name { get;set;}
        public string statuscode { get; set; }
        public string statuscode_format { get { return !string.IsNullOrWhiteSpace(statuscode) ? AcceptanceStatus.GetAcceptanceStatusById(statuscode)?.Name : null; } }
        public string statuscode_color { get { return !string.IsNullOrWhiteSpace(statuscode) ? AcceptanceStatus.GetAcceptanceStatusById(statuscode)?.Background : "#808080"; } }
        public string contract_name { get; set; }
        public string project_name { get; set; }
        public string unit_name { get; set; }
    }
}
