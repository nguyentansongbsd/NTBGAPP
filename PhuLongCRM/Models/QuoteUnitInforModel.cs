using System;
namespace PhuLongCRM.Models
{
    public class QuoteUnitInforModel
    {
        public Guid productid { get; set; }
        public string name { get; set; }
        public Guid _bsd_projectcode_value { get; set; }
        public Guid _bsd_phaseslaunchid_value { get; set; }
        public Guid _bsd_unittype_value { get; set; }

        public string statuscode { get; set; }
        public decimal bsd_constructionarea { get; set; }
        public decimal bsd_netsaleablearea { get; set; }
        public decimal bsd_actualarea { get; set; }

        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public string phaseslaunch_name { get; set; }
        public Guid pricelist_id_phaseslaunch { get; set; }
        public string pricelist_name_phaseslaunch { get; set; }
        public Guid pricelist_id_unit { get; set; }
        public string pricelist_name_unit { get; set; }

        public decimal bsd_taxpercent { get; set; }
        public decimal bsd_queuingfee { get; set; }
        public decimal bsd_depositamount { get; set; }

        public decimal price { get; set; }
        public decimal bsd_landvalueofunit { get; set; }
        public decimal bsd_maintenancefeespercent { get; set; }
        public int bsd_numberofmonthspaidmf { get; set; }
        public decimal bsd_managementamountmonth { get; set; }

        public Guid _defaultuomid_value { get; set; }
    }
}
