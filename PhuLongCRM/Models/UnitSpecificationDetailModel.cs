using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string bsd_no { get; set; }
        public bool showbtn { get; set; }
    }
    public class UnitSpecificationDetailModelGroup : List<UnitSpecificationDetailModel>
    {
        public string group { get; set; }
        public ObservableCollection<UnitSpecificationDetailModel> source { get; set; } = new ObservableCollection<UnitSpecificationDetailModel>();

        public UnitSpecificationDetailModelGroup(string group, ObservableCollection<UnitSpecificationDetailModel> source) 
        {
            this.group = group;
            this.source = source;
        }
    }
}
