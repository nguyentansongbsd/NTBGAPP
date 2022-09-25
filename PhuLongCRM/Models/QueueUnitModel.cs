using System;
namespace PhuLongCRM.Models
{
    public class QueueUnitModel
    {
        public bool bsd_status { get; set; }
        public Guid opportunityproductid { get; set; }
        public string bsd_pricelist { get; set; }
        public string bsd_booking { get; set; }
        public string bsd_project { get; set; }
        public string bsd_block { get; set; }
        public string bsd_units { get; set; }
        public string bsd_phaseslaunch { get; set; }
        public string bsd_floor { get; set; }
        public string uomid { get; set; }
        public bool isproductoverridden { get; set; }
        public string productid { get; set; }
        public bool ispriceoverridden { get; set; }
        public decimal priceperunit { get; set; }
        public decimal volumediscountamount { get; set; }
        public decimal quantity { get; set; }
        public decimal baseamount { get; set; }
        public decimal manualdiscountamount { get; set; }
        public decimal tax { get; set; }
        public decimal extendedamount { get; set; }
        public string transactioncurrencyid { get; set; }
        public string createdby { get; set; }
    }
}
