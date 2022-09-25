using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class FollowUpModel : BaseViewModel
    {
        public Guid bsd_followuplistid { get; set; }
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }

        public DateTime? _bsd_expiredate; // ngày hết hạn
        public DateTime? bsd_expiredate
        {
            get
            {
                if (_bsd_expiredate.HasValue)
                    return _bsd_expiredate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_expiredate = value;
                    OnPropertyChanged(nameof(bsd_expiredate));
                }
            }
        }
        public int statuscode { get; set; }
        public string bsd_followuplistcode { get; set; }
        public Guid product_id { get; set; }
        public string bsd_units { get; set; }
        public Guid bsd_reservation_id { get; set; }
        public string name_reservation { get; set; } // đặt cọc
        public Guid bsd_optionentry_id { get; set; }
        public string name_optionentry { get; set; }
        public Guid contact_id_oe { get; set; } // id khách hàng Option Entry
        public Guid account_id_oe { get; set; } // id khách hàng Option Entry
        public Guid contact_id_re { get; set; } // id khách hàng Reservation
        public Guid account_id_re { get; set; } // id khách hàng Reservation
        public string contact_name_oe { get; set; } // khách hàng Option Entry
        public string account_name_oe { get; set; } // khách hàng Option Entry
        public string contact_name_re { get; set; } // khách hàng Reservation
        public string account_name_re { get; set; } // khách hàng Reservation
        public string customer
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(contact_name_oe))
                    return contact_name_oe;
                else if (!string.IsNullOrWhiteSpace(account_name_oe))
                    return account_name_oe;
                else if (!string.IsNullOrWhiteSpace(contact_name_re))
                    return contact_name_re;
                else
                    return account_name_re;
            }
        }
        public string statuscode_format { get { return FollowUpStatusData.GetFollowUpStatusCodeById(statuscode.ToString())?.Name; } }
        public string statuscode_color { get { return FollowUpStatusData.GetFollowUpStatusCodeById(statuscode.ToString())?.Background; } }
        public int bsd_type { get; set; }
        public string bsd_type_format { get { return FollowUpType.GetFollowUpTypeById(bsd_type.ToString())?.Name; } }
        public int bsd_terminationtype { get; set; }
        public string bsd_terminationtype_format { get { return FollowUpTerminationType.GetFollowUpTerminationTypeById(bsd_terminationtype.ToString())?.Name; } }
        public int bsd_group { get; set; }
        public string bsd_group_format { get { return FollowUpGroup.GetFollowUpGroupById(bsd_group.ToString())?.Name; } }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public decimal bsd_sellingprice { get; set; } // giá bán
        public string bsd_sellingprice_format { get => StringFormatHelper.FormatCurrency(bsd_sellingprice); }
        public decimal bsd_totalamount { get; set; } // tổng tiền
        public string bsd_totalamount_format { get => StringFormatHelper.FormatCurrency(bsd_totalamount); }
        public decimal bsd_totalamountpaid { get; set; } // tổng tiền thanh toán 
        public string bsd_totalamountpaid_format { get => StringFormatHelper.FormatCurrency(bsd_totalamountpaid); }
        public decimal bsd_totalforfeitureamount { get; set; } // tổng tiền phạt
        public decimal bsd_forfeitureamount { get; set; } // hoàn tiền
        public string bsd_forfeitureamount_format { get => StringFormatHelper.FormatCurrency(bsd_forfeitureamount); }
        public int bsd_takeoutmoney { get; set; } // phương thức phạt
        public string bsd_takeoutmoney_format
        {
            get
            {
                if (bsd_takeoutmoney == 100000001) //takeoutmoney_forfeiture
                    return Language.takeoutmoney_forfeiture;// "Forfeiture";
                else if (bsd_takeoutmoney == 100000000) //takeoutmoney_refund
                    return Language.takeoutmoney_refund;//"Refund";
                else
                    return "";
            }
        }
        public decimal bsd_forfeiturepercent { get; set; } // hoàn tiền
        public string bsd_forfeiturepercent_format { get => StringFormatHelper.FormatCurrency(bsd_forfeiturepercent); }
        public bool bsd_terminateletter { get; set; } // thư thanh lý
        public string bsd_terminateletter_format { get { return BoolToStringData.GetStringByBool(bsd_terminateletter); } }
        public bool bsd_termination { get; set; } // thanh lý
        public string bsd_termination_format { get { return BoolToStringData.GetStringByBool(bsd_termination); } }
        public bool bsd_resell { get; set; } // bán lại
        public string bsd_resell_format { get { return BoolToStringData.GetStringByBool(bsd_resell); } }
        public Guid phaseslaunch_id { get; set; }
        public string phaseslaunch_name { get; set; } // đợt mở bán
        public Guid bsd_collectionmeeting_id { get; set; }
        public string bsd_collectionmeeting_subject { get; set; } // cuộc họp
        public string bsd_description { get; set; } //bình luận và quyết định nội dung
        public decimal bsd_depositfee { get; set; }
        public string bsd_depositfee_format { get => StringFormatHelper.FormatCurrency(bsd_depositfee); }
        public string project_code { get; set; }

        private decimal _bsd_totalforfeitureamount_new;// tổng tiền phạt
        public decimal bsd_totalforfeitureamount_new { get { return _bsd_totalforfeitureamount_new; } set { _bsd_totalforfeitureamount_new = value; OnPropertyChanged(nameof(bsd_totalforfeitureamount_new)); } }

        private string _bsd_totalforfeitureamount_format;// tổng tiền phạt format
        public string bsd_totalforfeitureamount_format { get { return _bsd_totalforfeitureamount_format; } set { _bsd_totalforfeitureamount_format = value; OnPropertyChanged(nameof(bsd_totalforfeitureamount_format)); } }
        public string totalforfeitureamount { get => StringFormatHelper.FormatCurrency(bsd_totalforfeitureamount_new); }
        public string bsd_salecomment { get; set; } //s&m comment

        private bool _isForfeiture;
        public bool isForfeiture { get { return _isForfeiture; } set { _isForfeiture = value; OnPropertyChanged(nameof(isForfeiture)); } }

        private bool _isRefund;
        public bool isRefund { get { return _isRefund; } set { _isRefund = value; OnPropertyChanged(nameof(isRefund)); } }
        public DateTime bsd_date { get; set; }
    }
}
