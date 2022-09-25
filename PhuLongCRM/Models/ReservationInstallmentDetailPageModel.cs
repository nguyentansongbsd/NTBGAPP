using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ReservationInstallmentDetailPageModel : BaseViewModel
    {
        public Guid bsd_paymentschemedetailid { get; set; }
        public string bsd_name { get; set; }
        public DateTime? bsd_duedate { get; set; } // ngày đến hạn
        public bool hide_duedate { get { return bsd_duedate.HasValue ? true : false; } }
        public int statuscode { get; set; } // tình trạng.
        public string statuscode_format { get => InstallmentsStatusCodeData.GetInstallmentsStatusCodeById(statuscode.ToString()).Name; }
        public string statuscode_color { get => InstallmentsStatusCodeData.GetInstallmentsStatusCodeById(statuscode.ToString()).Background; }
        public decimal bsd_amountofthisphase { get; set; } // số tiền đợi thnah toán.
        public string bsd_amountofthisphase_format { get => StringFormatHelper.FormatCurrency(bsd_amountofthisphase); }
        public decimal bsd_amountwaspaid { get; set; } // số tiền đã thanh toán
        public string bsd_amountwaspaid_format { get => StringFormatHelper.FormatCurrency(bsd_amountwaspaid); }
        public decimal bsd_depositamount { get; set; } // số tiền đặt cọc
        public string bsd_depositamount_format { get => StringFormatHelper.FormatCurrency(bsd_depositamount); }
        public decimal bsd_interestchargeamount { get; set; }

        public int bsd_ordernumber { get; set; } // đợt
        public bool bsd_depositamount_hide
        {
            get
            {
                if (bsd_ordernumber == 1) // tien dat coc chi hien thi o dot 1
                    return true;
                else
                    return false;
            }
        }

        public decimal bsd_amountpercent { get; set; }  // phần trăm thah toán

        public string bsd_name_format
        {
            get
            {
                string name = "";
                //if (!string.IsNullOrWhiteSpace(bsd_name))
                //    name += bsd_name;
                if (bsd_ordernumber != 0)
                    name += $" {Language.dot_thanh_toan} {bsd_ordernumber}";
                if (bsd_amountpercent != 0)
                    name += string.Format(" - {0:#,0}%", bsd_amountpercent);
                return name;
            }
        }
        public decimal bsd_maintenanceamount { get; set; } // phí bảo trì
        public string bsd_maintenanceamount_format { get => StringFormatHelper.FormatCurrency(bsd_maintenanceamount); }
        public bool bsd_maintenanceamount_hide
        {
            get
            {
                if (bsd_maintenanceamount == 0)
                    return false;
                else
                    return true;
            }
        }
        public decimal bsd_managementamount { get;set;} // phí quản lý
        public string bsd_managementamount_format { get => StringFormatHelper.FormatCurrency(bsd_managementamount); }
        public bool bsd_managementamount_hide
        {
            get
            {
                if (bsd_managementamount == 0)
                    return false;
                else
                    return true;
            }
        }
        public bool bsd_signcontractinstallment { get; set; }
        public int bsd_duedatecalculatingmethod { get; set; }
        public string bsd_icon_format
        {
            get
            {
                if (bsd_signcontractinstallment == true)
                    return "\uf044";
                else if (bsd_duedatecalculatingmethod == 100000002)
                    return "\uf2b5";
                else
                     return "\uf111";
            }
        }
        public string bsd_icon_color
        {
            get
            {
                if (bsd_signcontractinstallment == true)
                    return "#0DB302";
                else if (bsd_duedatecalculatingmethod == 100000002)
                    return "#D42A16";
                else
                    return "#1399D5";
            }
        }

        public string overdue
        {
            get
            {
                if (this.statuscode == 100000000) // Not paid
                {
                    if (this.bsd_interestchargeamount != 0 && this.bsd_duedate < DateTime.Now)
                    {
                        return Language.phat_cham_tt;
                    }
                    else if (this.bsd_duedate < DateTime.Now)
                    {
                        return Language.tre_han_tt;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
