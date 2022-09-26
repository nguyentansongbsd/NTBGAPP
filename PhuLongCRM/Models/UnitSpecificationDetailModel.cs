using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class UnitSpecificationDetailModel
    {
        public Guid bsd_unitsspecificationdetailsid { get; set; }
        public string unit_spec_name { get; set; }
        public string bsd_typeofroomvn { get; set; }
        public string bsd_typeofroomother { get; set; }
        public DateTime createdon { get; set; }
    }
}
