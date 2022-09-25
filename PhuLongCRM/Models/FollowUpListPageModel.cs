using System;
namespace PhuLongCRM.Models
{
    public class FollowUpListPageModel
    {
        public string bsd_followuplistid { get; set; }
        public string bsd_followuplistcode { get; set; }
        public int statuscode { get; set; }
        public DateTime bsd_date { get; set; }
        public int bsd_type { get; set; }
        public DateTime bsd_expiredate { get; set; }
        public string bsd_name { get; set; }
        public string _bsd_project_value { get; set; }
        public int bsd_group { get; set; }
        public string _bsd_reservation_value { get; set; }
        public DateTime createdon { get; set; }
        public string _bsd_units_value { get; set; }
        public string bsd_name_project { get; set; }
        public string name_unit { get; set; }
        public string customerid_quote { get; set; }
    }
}
