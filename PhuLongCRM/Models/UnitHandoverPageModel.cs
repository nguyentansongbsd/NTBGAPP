using System;
using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;

namespace PhuLongCRM.Models
{
    public class UnitHandoverPageModel
    {
        public Guid bsd_handoverid { get; set; }
        public string bsd_name { get; set; }
        public string statuscode { get; set; }
        public string bsd_handovernumber { get; set; }
        public string bsd_description { get; set; }
        public decimal bsd_totalpaidamount { get; set; }
        public DateTime? bsd_handoverformprintdate { get; set; }
        public DateTime? bsd_confirmdate { get; set; }
        public string bsd_cancelledreason { get; set; }
        public DateTime? bsd_cancelleddate { get; set; }
        public DateTime? bsd_estimatehandoverdate { get; set; }
        public decimal bsd_actualnsa { get; set; }
        public DateTime? bsd_handoverdate { get; set; }
        public string bsd_producterror { get; set; }

        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public Guid unit_id { get; set; }
        public string unit_name { get; set; }
        public Guid optionentry_id { get; set; }
        public string optionentry_name { get; set; }
        public Guid handovernotice_id { get; set; }
        public string handovernotice_name { get; set; }
        public Guid pinter_id { get; set; }
        public string pinter_name { get; set; }
        public Guid confirmer_id { get; set; }
        public string confirmer_name { get; set; }
        public Guid canceller_id { get; set; }
        public string canceller_name { get; set; }

        public StatusCodeModel status_format { get => UnitHandoverStatusData.GetUnitHandoverById(statuscode); }
        public string totalpaidamount_format { get => StringHelper.DecimalToCurrencyText(bsd_totalpaidamount); }
    }
}
