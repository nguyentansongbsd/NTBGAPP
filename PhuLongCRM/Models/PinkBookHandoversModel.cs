using System;
namespace PhuLongCRM.Models
{
    public class PinkBookHandoversModel
    {
        public Guid bsd_pinkbookhandoverid { get; set; }
        public string bsd_name { get; set; }
        public string statuscode { get; set; }

        public string project_name { get; set; }
        public string unit_name { get; set; }
        public string optionentry_name { get; set; }

        public StatusCodeModel status_format { get => PinkBookHandoverStatusData.GetPinkBookHandoverStatusById(this.statuscode); }
    }
}
