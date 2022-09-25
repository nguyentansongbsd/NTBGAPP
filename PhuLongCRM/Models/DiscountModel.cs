using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class DiscountModel : BaseViewModel
    {
        public Guid bsd_discountid { get; set; }
        public string bsd_discountnumber { get; set; }
        public string bsd_name { get; set; }
        public string discount_name { get {
                return !string.IsNullOrWhiteSpace(bsd_discountnumber) ? bsd_discountnumber : bsd_name;
            } }
        public string bsd_discounttype { get; set; }
        public string discounttype_format { get { return bsd_discounttype != string.Empty ? DiscountType.GetDiscountTypeById(bsd_discounttype)?.Name : null; } }
        public int bsd_priority { get; set; }
        public DateTime bsd_startdate { get; set; }
        public DateTime bsd_enddate { get; set; }
        public bool bsd_isconditionsapplied { get; set; }
        public string bsd_conditionsapply { get; set; }
        public bool hide_byunit { get => bsd_conditionsapply == "100000000" ? true : false; }
        public bool hide_list_price { get => bsd_conditionsapply == "100000001" ? true : false; }
        public string conditionsapply_format { get { return bsd_conditionsapply != string.Empty ? DiscountCondition.GetDiscountConditionById(bsd_conditionsapply)?.Name : null; } }
        public bool bsd_special { get; set;}
        public string bsd_special_format { get => BoolToStringData.GetStringByBool(bsd_special); }
        public int bsd_method { get; set; }
        public string bsd_method_format
        {
            get
            {
                if (bsd_method == 100000001)
                    return Language.handover_amount_method;//"Amount";
                else if (bsd_method == 100000000)
                    return Language.handover_percent_method;// "Percent";\
                else
                    return null;
            }
        }
        public string value_format
        {
            get
            {
                if (bsd_method == 100000001)
                    return StringFormatHelper.FormatCurrency(bsd_amount)+ " đ";
                else if (bsd_method == 100000000)
                    return StringFormatHelper.FormatPercent(bsd_percentage) + " %";
                else
                    return null;
            }
        }
        public decimal bsd_amount { get; set; }
        public decimal bsd_percentage { get; set; }
        public decimal bsd_fromvalue { get; set; }
        public string bsd_fromvalue_format { get => StringFormatHelper.FormatCurrency(bsd_fromvalue); }
        public decimal bsd_tovalue { get; set; }
        public string bsd_tovalue_format { get => StringFormatHelper.FormatCurrency(bsd_tovalue); }

        private ObservableCollection<OptionSet> _distcount_list;
        public ObservableCollection<OptionSet> distcount_list { get => _distcount_list; set { _distcount_list = value; OnPropertyChanged(nameof(distcount_list)); } }
    }
    public class DiscountType
    {
        public static List<StatusCodeModel> DiscountTypeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000006","Exchange Discount","#06CF79"),
                new StatusCodeModel("100000003","Fast Payment Discount","#03ACF5"),
                new StatusCodeModel("100000000","General Discount","#FDC206"),
                new StatusCodeModel("100000004","Internal Discount","#03ACF5"),
                new StatusCodeModel("100000002","Payment Scheme Discount","#FDC206"),
                new StatusCodeModel("100000001","Wholsale Discount","#f1f1f1"),
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetDiscountTypeById(string id)
        {
            return DiscountTypeData().SingleOrDefault(x => x.Id == id);
        }
    }
    public class DiscountCondition
    {
        public static List<StatusCodeModel> DiscountConditionData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000","By Unit","#06CF79"),
                new StatusCodeModel("100000001","By Listed Price","#03ACF5"),
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetDiscountConditionById(string id)
        {
            return DiscountConditionData().SingleOrDefault(x => x.Id == id);
        }
    }
}
