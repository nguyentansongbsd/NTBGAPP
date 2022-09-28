using System;
using PhuLongCRM.Helper;

namespace PhuLongCRM.Models
{
    public class PinkBookHandoverPageModel
    {
        public Guid bsd_pinkbookhandoverid { get; set; }
        public string bsd_name { get; set; }
        public string statuscode { get; set; }
        public string bsd_handovernumber { get; set; }
        public decimal? bsd_totalpaidamount { get; set; }
        public string bsd_description { get; set; }
        public DateTime? bsd_printdate { get; set; }
        public DateTime? bsd_confirmdate { get; set; }
        public string bsd_symbol { get; set; }
        public string bsd_registrationtax { get; set; }
        public string bsd_placeofissue { get; set; }
        public DateTime? bsd_pinkbookreceiptdate { get; set; }
        public decimal bsd_pinkbookarea { get; set; }
        public string bsd_otherhandoverdocument { get; set; }
        public DateTime? bsd_issuedon { get; set; }
        public string bsd_issuancefee { get; set; }
        public string bsd_inspectionfee { get; set; }
        public DateTime? bsd_handoverdate { get; set; }
        public string bsd_certificatenumber { get; set; }

        public string project_name { get; set; }
        public Guid project_id { get; set; }
        public string unit_name { get; set; }
        public Guid unit_id { get; set; }
        public string optionentry_name { get; set; }
        public Guid optionentry_id { get; set; }
        public string pinter_name { get; set; }
        public Guid pinter_id { get; set; }
        public string confirmer_name { get; set; }
        public Guid confirmer_id { get; set; }

        public StatusCodeModel status_format { get => PinkBookHandoverStatusData.GetPinkBookHandoverStatusById(this.statuscode); }
        public string totalpaidamount_format { get => StringHelper.DecimalToCurrencyText(bsd_totalpaidamount); }
    }
}
