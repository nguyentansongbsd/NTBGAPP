using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class DiscountSpecialModel
    {
        public Guid bsd_discountspecialid { get; set; }
        public string bsd_name { get; set; }

        public string bsd_cchtnh { get; set; }
        public decimal bsd_amountdiscount { get; set; }
        public string bsd_amountdiscount_format { get => StringFormatHelper.FormatCurrency(bsd_amountdiscount) + " đ"; }
        public decimal bsd_percentdiscount { get; set; }
        public string percentdiscount_format { get { return StringFormatHelper.FormatPercent(bsd_percentdiscount) + "%"; } }

        public string value_format { get {
                if (bsd_cchtnh == "100000000") // amount
                {
                    return StringFormatHelper.FormatCurrency(bsd_amountdiscount) + " đ";
                }
                else // percent
                {
                    return StringFormatHelper.FormatPercent(bsd_percentdiscount) + "%";
                }
            } }

        public decimal bsd_totalamount { get; set; }
        public string totalamount_format { get { return StringFormatHelper.FormatCurrency(bsd_totalamount) + " đ"; } }
        public string statuscode { get; set; }
        public string statuscode_format { get { return statuscode != string.Empty ? DiscountSpecialStatus.GetDiscountSpecialStatusById(statuscode)?.Name : null; } }
        public string statuscode_color { get { return statuscode != string.Empty ? DiscountSpecialStatus.GetDiscountSpecialStatusById(statuscode)?.Background : "#f1f1f1"; } }
    }
    public class DiscountSpecialStatus
    {
        public static List<StatusCodeModel> DiscountSpecialStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.discountspecial_active_sts,"#06CF79"), //discountspecial_active_sts Active
                new StatusCodeModel("100000000",Language.discountspecial_approved_sts,"#03ACF5"), //discountspecial_approved_sts Approved
                new StatusCodeModel("100000001",Language.discountspecial_reject_sts,"#FDC206"), //discountspecial_reject_sts Reject
                new StatusCodeModel("100000002",Language.discountspecial_cancelled_sts,"#03ACF5"), //discountspecial_cancelled_sts Canceled
                new StatusCodeModel("2",Language.discountspecial_inactive_sts,"#FDC206"), //discountspecial_inactive_sts Inactive
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetDiscountSpecialStatusById(string id)
        {
            return DiscountSpecialStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
