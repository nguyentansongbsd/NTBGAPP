
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ProjectList 
    {
        public string bsd_projectcode { get; set; }       
        public string bsd_name { get; set; }
        public Decimal? bsd_landvalueofproject { get; set; }
        public DateTime? bsd_esttopdate { get; set; }
        public DateTime? bsd_acttopdate { get; set; }

        public string bsd_projectid { get; set; }
    }
}
