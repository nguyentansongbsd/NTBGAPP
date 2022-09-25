using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class HandoverConditionModel :OptionSet
    {
        public Guid bsd_packagesellingid { get; set; }
        public Guid _bsd_unittype_value { get; set; }
        public bool bsd_byunittype { get; set; }
        public string byunittype_format { get { return bsd_byunittype ? Language.co : Language.khong; } }
        public string bsd_method { get; set; }
        public string method_format { get { return bsd_method != string.Empty ? HandoverConditionMethod.GetHandoverConditionMethodById(bsd_method)?.Label : null; } }
        public decimal bsd_priceperm2 { get; set; }
        public decimal bsd_amount { get; set; }
        public decimal bsd_percent { get; set; }
        public string amount_format
        {
            get
            {
                if (bsd_method == "100000000")
                    return StringFormatHelper.FormatCurrency(bsd_priceperm2) + " đ";
                else if (bsd_method == "100000001")
                    return StringFormatHelper.FormatCurrency(bsd_amount) + " đ";
                else if (bsd_method == "100000002")
                    return StringFormatHelper.FormatPercent(bsd_percent) + "%";
                return null;
            }
        }
        public string bsd_name { get; set; }
        public string bsd_description { get; set; }

        private DateTime? _bsd_startdate;
        public DateTime? bsd_startdate
        {
            get => this._bsd_startdate;
            set
            {
                if (_bsd_startdate.HasValue)
                {
                    _bsd_startdate = value;
                    OnPropertyChanged(nameof(bsd_startdate));
                }
            }
        }
        public bool hide_startdate { get { return _bsd_startdate.HasValue ? true : false; } }

        private DateTime? _bsd_enddate;
        public DateTime? bsd_enddate
        {
            get => this._bsd_enddate;
            set
            {
                if (_bsd_enddate.HasValue)
                {
                    _bsd_enddate = value;
                    OnPropertyChanged(nameof(bsd_enddate));
                }
            }
        }
        public bool hide_enddate { get { return _bsd_enddate.HasValue ? true : false; } }
        public string bsd_type { get; set; }
        public string type_format { get { return bsd_type != string.Empty ? HandoverCoditionMinimumData.GetHandoverCoditionMinimum(bsd_type)?.Label : null; } }
        public bool hide_byunittype { get { return _bsd_enddate.HasValue ? true : false; } }
        public int bsd_unittype { get; set; }
        public string name_unit_type { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
    }
    public class HandoverConditionMethod
    {
        public static OptionSet GetHandoverConditionMethodById(string Id)
        {
            return HandoverConditionMethodData().SingleOrDefault(x => x.Val == Id);
        }
        public static List<OptionSet> HandoverConditionMethodData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.handover_price_per_sqm_method),// "Price per sqm" // handover_price_per_sqm_method
                new OptionSet("100000001",Language.handover_amount_method),//"Amount" // handover_amount_method
                new OptionSet("100000002",Language.handover_percent_method),//"Percent (%)" // handover_percent_method
            };
        }
    }
}
