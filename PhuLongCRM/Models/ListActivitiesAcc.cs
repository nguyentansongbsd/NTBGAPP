using System;
using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class ListActivitiesAcc 
    {
        public string subject { get; set; }
        public string description { get; set; }
        public string activitytypecode { get; set; }
        public string activitytypecodevalue
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
                        return " ";
                }
            }
        }

        public DateTime createdon { get; set; }
        public string createdonformat
        {
            get => StringHelper.DateFormat(createdon);
        }

        public string statecode { get; set; }
        public string statuscode { get; set; }
        public DateTime scheduledstart { get; set; }
        public DateTime scheduledend { get; set; }
        public Guid activityid { get; set; }
        public string regardingobjectid_label_contact { get; set; }
        public string regardingobjectid_label_account { get; set; }
        public string regardingobjectid_label_lead { get; set; }
        public string regardingobjectid_label { get; set; }
    }
}

