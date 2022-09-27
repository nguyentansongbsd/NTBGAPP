using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AcceptanceModel : BaseViewModel
    {
        public Guid bsd_acceptanceid { get; set; }
        public string bsd_name { get; set; }
        public string statuscode { get; set; }
        public string statuscode_format { get { return !string.IsNullOrWhiteSpace(statuscode) ? AcceptanceStatus.GetAcceptanceStatusById(statuscode)?.Name : null; } }
        public string statuscode_color { get { return !string.IsNullOrWhiteSpace(statuscode) ? AcceptanceStatus.GetAcceptanceStatusById(statuscode)?.Background : "#808080"; } }
        public Guid contract_id { get; set; }
        public string contract_name { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public Guid unit_id { get; set; }
        public string unit_name { get; set; }
        public string bsd_acceptancenumber { get; set; }
        public string bsd_acceptancetype { get; set; }
        public string acceptancetype_format { get { return !string.IsNullOrWhiteSpace(bsd_acceptancetype) ? AcceptanceType.GetAcceptanceTypeById(bsd_acceptancetype)?.Label : null; } }
        public Guid customer_id { get; set; }
        public string customer_name { get; set; }
        public string bsd_typeresult { get; set; }
        public string typeresult_format { get { return !string.IsNullOrWhiteSpace(bsd_typeresult) ? AcceptanceTypeResult.GetAcceptanceTypeResultById(bsd_typeresult)?.Label : null; } }
        public Guid acceptance_noti_id { get; set; }
        public string acceptance_noti_name { get; set; }
        
        public DateTime? _bsd_actualacceptancedate;
        public DateTime? bsd_actualacceptancedate { get => _bsd_actualacceptancedate; set { if (value.HasValue) { _bsd_actualacceptancedate = value; OnPropertyChanged(nameof(bsd_actualacceptancedate)); } } }
        public decimal bsd_expense { get; set; }
        public string expense_format { get => StringFormatHelper.FormatCurrency(bsd_expense); }
        public Guid installment_id { get; set; }
        public string installment_name { get; set; }

        public DateTime? _bsd_reacceptancedate;
        public DateTime? bsd_reacceptancedate { get => _bsd_reacceptancedate; set { if (value.HasValue) { _bsd_reacceptancedate = value; OnPropertyChanged(nameof(bsd_reacceptancedate)); } } }
        public string bsd_repairtimeday { get; set; }
        public string bsd_remark { get; set; }
        public string printer_name { get; set; }
        public DateTime? bsd_printedate { get; set; }
        public string confirmer_name { get; set; }
        public DateTime? bsd_confirmdate { get; set; }
        public string canceller_name { get; set; }
        public DateTime? bsd_cancelleddate { get; set; }
        public string bsd_cancelledreason { get; set; }
        public string bsd_deactivereason { get; set; }
    }
}
