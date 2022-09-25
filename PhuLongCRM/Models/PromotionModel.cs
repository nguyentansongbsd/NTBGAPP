using PhuLongCRM.Helper;
using System;
namespace PhuLongCRM.Models
{
    public class PromotionModel : OptionSet
    {
        public Guid bsd_promotionid { get; set; }

        private string _bsd_name;
        public string bsd_name { get => _bsd_name; set { _bsd_name = value; OnPropertyChanged(nameof(bsd_name)); } }

        private decimal _bsd_values;
        public decimal bsd_values { get => _bsd_values; set { _bsd_values = value; OnPropertyChanged(nameof(bsd_values)); } }
        public string bsd_values_format { get => StringFormatHelper.FormatCurrency(bsd_values); }

        private string _bsd_description;
        public string bsd_description { get => _bsd_description; set { _bsd_description = value; OnPropertyChanged(nameof(bsd_description)); } }

        public DateTime? _bsd_startdate;
        public DateTime? bsd_startdate
        {
            get => _bsd_startdate;
            set
            {
                _bsd_startdate = value;
                OnPropertyChanged(nameof(bsd_startdate));
            }
        }
        public bool hide_startdate
        {
            get
            {
                if (_bsd_startdate.HasValue == true)
                    return true;
                else
                    return false;
            }
        }
        public DateTime? _bsd_enddate;
        public DateTime? bsd_enddate
        {
            get => this._bsd_enddate;
            set
            {
                _bsd_enddate = value;
                OnPropertyChanged(nameof(bsd_enddate));
            }
        }
        public bool hide_enddate
        {
            get
            {
                if (_bsd_enddate.HasValue == true)
                    return true;
                else
                    return false;
            }
        }
    }
}
