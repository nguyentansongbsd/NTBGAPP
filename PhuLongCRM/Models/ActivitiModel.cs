using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
namespace PhuLongCRM.Models
{
    public class ActivitiModel : BaseViewModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string activitytypecode { get; set; }

        public string _customer;
        public string customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(customer)); } }
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid lead_id { get; set; }
        public string lead_name { get; set; }

        public DateTime scheduledstart { get; set; }
        public DateTime scheduledend { get; set; }
        public DateTime createdon { get; set; }
        public string callto_contact_name { get; set; }
        public string callto_accounts_name { get; set; }
        public string callto_lead_name { get; set; }
        public string activitytypecode_format
        {
            get
            {
                switch (activitytypecode)
                {
                    case "task":
                        return Language.cong_viec;
                    case "phonecall":
                        return Language.cuoc_goi;
                    case "appointment":
                        return Language.cuoc_hop;
                    default:
                        return "Error";
                }
            }
        }
        public string activitytypecode_color
        {
            get
            {
                switch (activitytypecode)
                {
                    case "task":
                        return "#2196F3";
                    case "phonecall":
                        return "#0DB302";
                    case "appointment":
                        return "#D42A16";
                    default:
                        return "#808080";
                }
            }
        }
    }
}
