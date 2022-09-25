using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class PhasesLaunch
    {
        public Guid bsd_phaseslaunchid { get; set; }
        public string bsd_name { get; set; }
        public Guid bsd_salesagentcompany { get; set; }
        public string salesagentcompany_name { get; set; }
        public bool bsd_locked { get; set; }
    }
}
