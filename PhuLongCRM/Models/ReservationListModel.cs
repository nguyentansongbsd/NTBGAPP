using PhuLongCRM.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ReservationListModel
    {
        public Guid quoteid { get; set; }
        public string name { get; set; }

        public decimal totalamount { get; set; }
        public string totalamount_format { get => StringFormatHelper.FormatCurrency(totalamount); }

        public Guid bsd_project_id { get; set; }
        public string bsd_project_name { get; set; }

        public Guid bsd_unitno_id { get; set; }
        public string bsd_unitno_name { get; set; }

        public string purchaser_accountname { get; set; }

        public string purchaser_contactname { get; set; }

        public string purchaser
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(purchaser_accountname))
                {
                    return purchaser_accountname;
                }
                else
                {
                    return purchaser_contactname ?? "";
                }
            }
        }

        public int statuscode { get; set; }
        public string statuscode_format { get => QuoteStatusCodeData.GetQuoteStatusCodeById(statuscode.ToString())?.Name; }
        public string statuscode_color { get => QuoteStatusCodeData.GetQuoteStatusCodeById(statuscode.ToString())?.Background; }
    }
}
