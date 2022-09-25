using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ActivityListModel : BaseViewModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string activitytypecode { get; set; }
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
        public int statecode { get; set; }
        public string statecode_format
        {
            get
            {
                switch (statecode)
                {
                    case 0:
                        return Language.activity_open_sts; // activity_open_sts  Open
                    case 1:
                        return Language.activity_completed_sts; // activity_completed_sts  Completed
                    case 2:
                        return Language.activity_cancelled_sts; // activity_canceled_sts Canceled
                    case 3:
                        return Language.activity_scheduled_sts; // activity_scheduled_sts
                    default:
                        return " ";
                }
            }
        }
        public string statecode_color
        {
            get
            {
                switch (statecode)
                {
                    case 0:
                        return "#06CF79"; // open
                    case 1:
                        return "#03ACF5"; //com
                    case 2:
                        return "#333333"; //can
                    case 3:
                        return "#04A388"; //sha
                    default:
                        return "#333333";
                }
            }
        }
        public DateTime scheduledstart { get; set; }
        public DateTime scheduledstart_format
        {
            get => this.scheduledstart.ToLocalTime();
        }
        public DateTime scheduledend { get; set; }
        public DateTime scheduledend_format
        {
            get => this.scheduledend.ToLocalTime();
        }
        // khách hàng của task
        public string task_contact_name { get; set; }
        public string task_account_name { get; set; }
        public string task_lead_name { get; set; }
        public string task__customer
        {
            get
            {
                if (task_contact_name != null)
                    return task_contact_name;
                else if (task_account_name != null)
                    return task_account_name;
                else if (task_lead_name != null)
                    return task_lead_name;
                else
                    return null;
            }
        }
        // khách hàng của phone call (call to)
        public string phonecall_contact_name { get; set; }
        public string phonecall_account_name { get; set; }
        public string phonecall_lead_name { get; set; }
        public string phonecall_customer
        {
            get
            {
                if (phonecall_lead_name != null)
                    return phonecall_lead_name;
                else if (phonecall_contact_name != null)
                    return phonecall_contact_name;
                else if (phonecall_account_name != null)
                    return phonecall_account_name;
                else
                    return null;
            }
        }
        // cuoc hop
        public string requiredattendees { get; set; }
        public string customer { get; set; }
    }
}
