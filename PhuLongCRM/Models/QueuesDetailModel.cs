using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;
using System;
namespace PhuLongCRM.Models
{
    public class QueuesDetailModel : BaseViewModel
    {
        public Guid opportunityid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Guid _bsd_units_value { get; set; }
        public string unit_name { get; set; }
        public string unit_status { get; set; }
        public Guid _bsd_project_value { get; set; }
        public string project_name { get; set; }
        public string bsd_queuenumber { get; set; }
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public string PhoneContact { get; set; }
        public Guid account_id { get; set; }
        public string account_name { get; set; }
        public string PhoneAccount { get; set; }
        public Guid _bsd_salesagentcompany_value { get; set; }
        public string salesagentcompany_name { get; set; }

        public decimal? bsd_queuingfee { get; set; }
        public string bsd_queuingfee_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfee); }
        public double? budgetamount { get; set; }

        public Guid _bsd_phaselaunch_value { get; set; }
        public Guid bsd_phaseslaunch_id { get; set; }
        public string phaselaunch_name { get; set; }

        public string bsd_nameofstaffagent { get; set; }

        public int statuscode { get; set; }

        public DateTime _bsd_bookingtime;
        public DateTime bsd_bookingtime { get => _bsd_bookingtime.AddHours(7); set { _bsd_bookingtime = value; OnPropertyChanged(nameof(bsd_bookingtime)); } }

        public DateTime _createdon;
        public DateTime createdon { get => _createdon.AddHours(7); set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        public DateTime _bsd_queuingexpired;
        public DateTime bsd_queuingexpired { get => _bsd_queuingexpired.AddHours(7); set { _bsd_queuingexpired = value; OnPropertyChanged(nameof(bsd_queuingexpired)); } }

        private int _bsd_ordernumber;
        public int bsd_ordernumber { get => _bsd_ordernumber; set { _bsd_ordernumber = value; OnPropertyChanged(nameof(bsd_ordernumber)); } }

        private int _bsd_priorityqueue;
        public int bsd_priorityqueue { get => _bsd_priorityqueue; set { _bsd_priorityqueue = value; OnPropertyChanged(nameof(bsd_priorityqueue)); } }

        private int _bsd_prioritynumber;
        public int bsd_prioritynumber { get => _bsd_prioritynumber; set { _bsd_prioritynumber = value; OnPropertyChanged(nameof(bsd_prioritynumber)); } }

        private DateTime _bsd_dateorder;
        public DateTime bsd_dateorder { get => _bsd_dateorder.AddHours(7); set { _bsd_dateorder = value; OnPropertyChanged(nameof(bsd_dateorder)); } }

        private bool _bsd_expired;
        public bool bsd_expired { get => _bsd_expired; set { _bsd_expired = value; OnPropertyChanged(nameof(bsd_expired)); } }
        public string bsd_expired_format { get { return BoolToStringData.GetStringByBool(bsd_expired); } }
        public string statuscode_format { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString()).Name; } }
        public Guid collaborator_id { get; set; }
        public string collaborator_name { get; set; }
        public Guid customerreferral_id { get; set; }
        public string customerreferral_name { get; set; }
        //bsd_queuingfeepaid
        public decimal bsd_queuingfeepaid { get; set; }
        public string bsd_queuingfeepaid_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfeepaid); }
    }
}
