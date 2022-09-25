using System;
namespace PhuLongCRM.Models
{
    public class PaymentSchemeModel
    {
        public Guid bsd_paymentschemeid { get; set; }
        public string bsd_name { get; set; }
        public DateTime bsd_startdate { get; set; }
        public DateTime bsd_enddate { get; set; }
    }
}
