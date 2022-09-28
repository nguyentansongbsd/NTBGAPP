using System;
using System.Collections.Generic;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class UnitSpecificationListModel
    {
        public Guid bsd_unitsspecificationid { get; set; }
        public string bsd_name {get;set;}
        public string statuscode { get; set; }
        public string statuscode_format { get { return !string.IsNullOrWhiteSpace(statuscode) ? UnitSpecificationStatus.GetUnitSpecStatusById(statuscode)?.Name : null; } }
        public string statuscode_color { get { return !string.IsNullOrWhiteSpace(statuscode) ? UnitSpecificationStatus.GetUnitSpecStatusById(statuscode)?.Background : "#808080"; } }
        public bool bsd_typeofunitspec { get; set; }
        public string bsd_typeofunitspec_format { get { return bsd_typeofunitspec ? Language.moi : Language.cu; } }
        public string project_name { get; set; }
        public string unittype_name { get; set; }
    }
}
