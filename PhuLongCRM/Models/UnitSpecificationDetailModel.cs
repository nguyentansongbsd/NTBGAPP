using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class UnitSpecificationDetailModel
    {
        public Guid bsd_unitsspecificationdetailsid { get; set; }
        public string bsd_itemvn { get; set; }
        public string bsd_typeofroomvn { get; set; }
        public string bsd_details { get; set; }
        public string bsd_typeno { get; set; }
    }
}
