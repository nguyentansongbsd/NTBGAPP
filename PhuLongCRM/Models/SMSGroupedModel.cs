using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PhuLongCRM.Models
{
    public class SMSGroupedModel
    {
        public string name { get; set; }
        public List<SMSModel> lstSMS { get; set; }
        public string phone { get; set; }
        public DateTime lastSMSTime { get; set; }
        public string lastSMSContent { get; set; }

        public string name_display { get { return name == null ? phone : name; } }

        public SMSGroupedModel()
        {
            lstSMS = new List<SMSModel>();
        }

        public void addSMS(SMSModel s)
        {
            lstSMS.Add(s);
        }

    }
}
