using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class QueueListModel : BaseViewModel
    {
        public Guid opportunityid { get; set; }
        public string name { get; set; }
        public Guid customer_id { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public string contact_name { get; set; }
        public string account_name { get; set; }
        public bool bsd_queueforproject { get; set; }

        public decimal bsd_queuingfee { get; set; }
        public string bsd_queuingfee_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_queuingfee);
        }

        public string unit_name { get; set; }

        private DateTime _createdon;
        public DateTime createdon { get => _createdon.AddHours(7); set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        private DateTime _bsd_queuingexpired;
        public DateTime bsd_queuingexpired { get => _bsd_queuingexpired.AddHours(7); set { _bsd_queuingexpired = value; OnPropertyChanged(nameof(bsd_queuingexpired)); } }
        public string bsd_queuingexpired_format
        {
            get => StringHelper.DateFormat(bsd_queuingexpired);
        }

        public DateTime? actualclosedate { get; set; }
        public string actualclosedate_format { get { return actualclosedate.HasValue ? actualclosedate.Value.ToString("dd/MM/yyyy") : null; } }
        public int statuscode { get; set; }
        public string statuscode_label { get; set; }

        public string statuscode_format { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString()).Name; } }
        public string statuscode_color { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString()).BackGroundColor; } }

        public string bsd_queuenumber { get; set; }
        public decimal? estimatedvalue { get; set; }
        public string estimatedvalue_format => estimatedvalue.HasValue ? string.Format("{0:#,0.#}", estimatedvalue.Value) + " đ" : null;

        public string customername
        {
            get { return contact_name ?? account_name ?? ""; }
        }

        public string createdon_format
        {
            get { return StringHelper.DateFormat(createdon); }
        }

        public string telephone { get; set; }
        public QueueListModel()
        {
            this.unit_name = " ";
        }
        public decimal bsd_queuingfeepaid { get; set; }
        public string bsd_queuingfeepaid_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfeepaid); }
        public bool bsd_collectedqueuingfee { get; set; }
        public string bsd_collectedqueuingfee_format { get { return BoolToStringData.GetStringByBool(bsd_collectedqueuingfee); } }
    }
}
