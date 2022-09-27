using System;
namespace PhuLongCRM.Models
{
    public class UnitHandoversModel
    {
        public Guid bsd_handoverid { get; set; }
        public string bsd_name { get; set; }
        public string statuscode { get; set; }
        public string project_name { get; set; }
        public string unit_name { get; set; }
        public StatusCodeModel status_format { get => UnitHandoverStatusData.GetUnitHandoverById(statuscode); }
    }
}
