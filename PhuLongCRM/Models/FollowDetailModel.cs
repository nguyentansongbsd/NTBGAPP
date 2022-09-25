using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class FollowDetailModel
    {
        public string bsd_followuplistcode { get; set; }
        public string bsd_followuplistid { get; set; }
        public int statuscode { get; set; }
        public DateTime bsd_date { get; set; }
        public double bsd_sellingprice_base { get; set; }
        public double bsd_totalamount_base { get; set; }
        public double bsd_totalamountpaid_base { get; set; }
        public double bsd_forfeitureamount_base { get; set; }
        public double bsd_totalforfeitureamount_base { get; set; }
        public int bsd_takeoutmoney { get; set; }
        public bool? bsd_terminateletter { get; set; }
        public bool? bsd_termination { get; set; }
        public bool? bsd_resell { get; set; }
        public string bsd_name_dotmoban { get; set; }
        public string resell
        {
            get
            {
                if (bsd_resell == true)
                {
                    return Language.co;
                }
                else if (bsd_resell == false)
                {
                    return Language.khong;
                }
                else
                {
                    return null;
                }
            }
        }
        public string takeoutmoney
        {
            get
            {
                if (bsd_takeoutmoney == 100000000)
                {
                    return Language.takeoutmoney_refund;
                }
                else if (bsd_takeoutmoney == 100000001)
                {
                    return Language.takeoutmoney_forfeiture;
                }
                else
                {
                    return null;
                }
            }
        }


        public int bsd_type { get; set; }
        public string type
        {
            get
            {
                if (bsd_type == 100000007)
                {
                    return Language.units;// "Sản phẩm";
                }
                else if (bsd_type == 100000000)
                {
                    return Language.dat_coc_sign_off_rf;//"Ký Phiếu đặt cọc";
                }
                else if (bsd_type == 100000001)
                {
                    return Language.dat_coc_tt_du_tien_coc;//"Đặt cọc";
                }
                else if (bsd_type == 100000005)
                {
                    return Language.dat_coc_da_thanh_ly;//"Thanh lý đặt cọc";
                }
                else if (bsd_type == 100000002)
                {
                    return Language.giao_dich_tt_du_dot_1;//"Thanh toán đợt 1";
                }
                else if (bsd_type == 100000003)
                {
                    return Language.giao_dich_da_ky_hdmb;//"Hợp đồng";
                }
                else if (bsd_type == 100000004)
                {
                    return Language.giao_dich_installments;//"Đợt thanh toán";
                }
                else if (bsd_type == 100000006)
                {
                    return Language.giao_dich_da_thanh_ly;//"Thanh lý hợp đồng";
                }
                else { return null; }
            }
        }

        public DateTime bsd_expiredate { get; set; }
        public string bsd_name { get; set; }
        public string _bsd_project_value { get; set; }
        public int bsd_group { get; set; }
        public string _bsd_reservation_value { get; set; }
        public DateTime createdon { get; set; }
        public string _bsd_units_value { get; set; }

        public string bsd_name_project { get; set; }
        public double bsd_bookingfee_project { get; set; }
        public string bsd_projectcode_project { get; set; }

        public double totalamount_quote { get; set; }
        public double totallineitemamount_base_quote { get; set; }
        public string name_quote { get; set; }
        public string customer_name_contact { get; set; }
        public string customer_name_account_quote { get; set; }

        public string name_salesorder { get; set; }
        public double bsd_totalpaidincludecoa_salesorder { get; set; }
        public double bsd_totalamountlessfreight_salesorder { get; set; }
        public string customer_name_account { get; set; }

        public string name_work
        {
            get
            {
                if (this.name_quote != null)
                {
                    return this.name_quote;
                }
                else if (this.name_salesorder != null)
                {
                    return this.name_salesorder;
                }
                else
                {
                    return null;
                }
            }
        }

        public string customer
        {
            get
            {
                if (this.customer_name_account != null)
                {
                    return this.customer_name_account;
                }
                else if (this.customer_name_contact != null)
                {
                    return this.customer_name_contact;
                }
                else if (this.customer_name_account_quote != null)
                {
                    return this.customer_name_account_quote;
                }
                else
                {
                    return null;
                }
            }
        }
        public string name_unit { get; set; }
        public string productnumber_unit { get; set; }
        public string bsd_areavariance_unit { get; set; }
        public double price_unit { get; set; }
        public string block { get; set; }
        public string floor { get; set; }

    }
}
