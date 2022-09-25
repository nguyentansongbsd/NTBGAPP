using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class PhasesLanchModel
    {
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }
        public Guid bsd_phaseslaunchid { get; set; }
        public DateTime startdate_event { get; set; }
        public DateTime enddate_event { get; set; }
        public string statuscode_event { get; set; }

        public Guid discount_id { get; set; }
        public string discount_name { get; set; }
        public Guid internel_id { get; set; }
        public string internel_name { get; set; }
        public Guid paymentscheme_id { get; set; }
        public string paymentscheme_name { get; set; }
        public Guid promotion_id { get; set; }
        public string promotion_name { get; set; }

        public DateTime startdate_discountlist { get; set; }
        public DateTime enddate_discountlist { get; set; }
        public DateTime startdate_internellist { get; set; }
        public DateTime enddate_internellist { get; set; }
        public DateTime startdate_exchangelist { get; set; }
        public DateTime enddate_exchangelist { get; set; }
    }
}
