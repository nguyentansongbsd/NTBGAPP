using System;
using PhuLongCRM.ViewModels;

namespace PhuLongCRM.Models
{
    public class OptionEntry : BaseViewModel
    {
        private string _salesorderid;
        public string salesorderid { get => _salesorderid; set { _salesorderid = value; OnPropertyChanged(nameof(salesorderid)); } }

        private string _bsd_project_label;
        public string bsd_project_label { get => _bsd_project_label; set { _bsd_project_label = value; OnPropertyChanged(nameof(bsd_project_label)); } }

        private string _bsd_unitnumber_label;
        public string bsd_unitnumber_label { get => _bsd_unitnumber_label; set { _bsd_unitnumber_label = value; OnPropertyChanged(nameof(bsd_unitnumber_label)); } }

        private string _bsd_optionno;
        public string bsd_optionno { get => _bsd_optionno; set { _bsd_optionno = value; OnPropertyChanged(nameof(bsd_optionno)); } }

        private string _customerid_label_account;
        private string customerid_label_account { get => _customerid_label_account; set { _customerid_label_account = value; if (value != null) { customerid_label_account = null; customerid_label = value; } OnPropertyChanged(nameof(customerid_label_account)); } }

        private string _customerid_label_contact;
        public string customerid_label_contact { get => _customerid_label_contact; set { _customerid_label_contact = value; if (value != null) { customerid_label_account = null; customerid_label = value; } OnPropertyChanged(nameof(customerid_label_contact)); } }

        private string _customerid_label;
        public string customerid_label { get => _customerid_label; set { _customerid_label = value; OnPropertyChanged(nameof(customerid_label)); } }

        private string _statuscode;
        public string statuscode { get => _statuscode; set { _statuscode = value; OnPropertyChanged(nameof(statuscode)); } }

        public string statuscode_label { get { return statuscode != null ? ContractStatusCodeData.GetContractStatusCodeById(statuscode.ToString())?.Name : null; } }
        //public string statuscode_label {
        //    get
        //    {
        //        switch (statuscode)
        //        {
        //            case "2": return "Pending";
        //            case "1":return "Open";
        //            case "3":return "Submitted - In Progress";
        //            case "100000000":return "Option";
        //            case "100000006":return "Terminated";
        //            case "100000005":return "Handover";
        //            case "100000004":return "Complete Payment";
        //            case "100000003":return "Being Payment";
        //            case "100000002":return "Signed Contract";
        //            case "100000008":return "Đã ký TT/HĐ cọc";
        //            case "100000007":return "Đủ điều kiện";
        //            case "100000001":return "1st Installment";
        //            case "4":return "Canceled";
        //            case "100001":return "Complete";
        //            case "100002":return "Partial";
        //            case "100003":return "Invoiced";
        //            default: return null;
        //        }
        //    }
        //}

        private decimal? _totalamount;
        public decimal? totalamount { get => _totalamount; set { _totalamount = value; OnPropertyChanged(nameof(totalamount)); } }

        public string totalamount_format { get { return totalamount.HasValue ? string.Format("{0:#,0.#}", totalamount.Value) + transactioncurrency : null; } }

        private string _transactioncurrency;
        public string transactioncurrency { get => _transactioncurrency; set { _transactioncurrency = value; OnPropertyChanged(nameof(transactioncurrency)); } }

        private DateTime? _bsd_signingexpired;
        public DateTime? bsd_signingexpired { get => _bsd_signingexpired; set { _bsd_signingexpired = value; OnPropertyChanged(nameof(bsd_signingexpired)); } }
        public string bsd_signingexpired_format { get => bsd_signingexpired.HasValue ? bsd_signingexpired.Value.ToString("dd/MM/yyyy") : null; }

        private DateTime? _createdon;
        public DateTime? createdon { get => _createdon; set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }
        public string createdon_format { get => createdon.HasValue ? createdon.Value.ToString("dd/MM/yyyy") : null; }
        public OptionEntry()
        {
            this.salesorderid = null;
            this.totalamount = null;
            this.bsd_optionno = " ";
        }
    }
}
