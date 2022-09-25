using System;
using PhuLongCRM.Helper;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class ListQuotationAcc : ContentView
    {
        public string customerid { get; set; }
        public string statuscode { get; set; }
        public decimal? totalamount { get; set; }
        public string totalamountformat => totalamount.HasValue ? string.Format("{0:#,0.#}", totalamount.Value) + " đ" : null;
        public string bsd_quotationnumber { get; set; }
        public string statuscodevalue { get { return statuscode != null ? QuoteStatusCodeData.GetQuoteStatusCodeById(statuscode.ToString())?.Name : null; } }
        public DateTime createdon { get; set; }
        public string createdonformat
        {
            get => StringHelper.DateFormat(createdon);
        }
        public string quo_nameproject { get; set; }
        public string quo_nameaccount { get; set; }
        public string quo_namecontact { get; set; }
        public string quo_nameproduct { get; set; }
    }
}

