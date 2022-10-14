using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ContractModel : BaseViewModel
    {
        // list
        public Guid salesorderid { get; set; } // id hợp đồng
        public string salesorder_name { get; set; } // tên hợp đồng
        public string bsd_optionno { get; set; } // no hợp đồng
        public string ordernumber { get; set; } // số hợp đồng
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public Guid unit_id { get; set; }
        public string unit_name { get; set; }
        public decimal totalamount { get; set; } // tổng tiền
        public string totalamount_format { get => StringFormatHelper.FormatCurrency(totalamount); }
        public int statuscode { get; set; }
        public string statuscode_format { get { return statuscode != 0 ? ContractStatusCodeData.GetContractStatusCodeById(statuscode.ToString())?.Name : null; } }
        public string statuscode_color { get { return statuscode != 0 ? ContractStatusCodeData.GetContractStatusCodeById(statuscode.ToString())?.Background : "#808080"; } }
        public Guid customerid { get; set; } // id khách hàng
        public string customer_name // tên khách hàng 
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(account_name))
                {
                    return account_name;
                }
                else
                {
                    return contact_name ?? "";
                }
            }
        }
        public string contact_name { get; set; } // tên khách hàng cá nhân
        public string account_name { get; set; } // tên khách hàng doanh nghiệp
        public Guid contact_id { get; set; } // tên khách hàng cá nhân
        public Guid account_id { get; set; } // tên khách hàng doanh nghiệp

        // detail
        public Guid queue_id { get; set; } // id giữ chô
        public string queue_name { get; set; } // title giữ chỗ
        public Guid reservation_id { get; set; } // id đặt cọc
        public string reservation_name { get; set; } // title đặt cọc
        public Guid salesagentcompany_id { get; set; } //id đại lý/ sàn giao dịch
        public string salesagentcompany_name { get; set; } // tên đại lý/ sàn giao dịch
        public string nameofstaffagent { get; set; } // tên nhân viên
        public string bsd_referral { get; set; } // giới thiệu
        public int bsd_unitstatus { get; set; } // trạng thái unit
        public string bsd_unitstatus_format { get => StatusCodeUnit.GetStatusCodeById(bsd_unitstatus.ToString())?.Name; }
        public decimal bsd_constructionarea { get; set; } // diện tích xây dựng
        public decimal bsd_netusablearea { get; set; } // diện tích sử dụng
        public decimal unit_actualarea { get; set; } // diện tích thực
        public Guid phaseslaunch_id { get; set; } //id đợt mở bán
        public string phaseslaunch_name { get; set; } // tên đợt mở bán
        public Guid pricelevelid { get; set; } // bảng giá gốc
        public string pricelevel_name { get; set; } // tên bảng giá gốc
        public Guid taxcode_id { get; set; } // id mã số thuế
        public string taxcode_name { get; set; } // mã số thuể
        public decimal bsd_queuingfee { get; set; } // phí giũ chỗ
        public string bsd_queuingfee_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfee); }
        public decimal bsd_depositamount { get; set; } // phí đặt cọc
        public string bsd_depositamount_format { get => StringFormatHelper.FormatCurrency(bsd_depositamount); }
        public bool bsd_allowchangeunitsspec { get; set; } // thay đổi đđktsp
        public string bsd_allowchangeunitsspec_format { get { return BoolToStringData.GetStringByBool(bsd_allowchangeunitsspec); } }
        public Guid bsd_unitsspecification_id { get; set; } // id đđktsp

        private string _bsd_unitsspecification_name;
        public string bsd_unitsspecification_name { get => _bsd_unitsspecification_name; set { _bsd_unitsspecification_name = value; OnPropertyChanged(nameof(bsd_unitsspecification_name)); } } // tên đđktsp
        public Guid bsd_exchangeratedetailid { get; set; } // id tỷ giá

        private string _bsd_exchangeratedetail_name;
        public string bsd_exchangeratedetail_name { get => _bsd_exchangeratedetail_name; set { _bsd_exchangeratedetail_name = value; OnPropertyChanged(nameof(bsd_exchangeratedetail_name)); } } // tên tỷ giá

        public DateTime? _bsd_estimatehandoverdatecontract; // bàn giao dự kiến
        public DateTime? bsd_estimatehandoverdatecontract
        {
            get
            {
                if (_bsd_estimatehandoverdatecontract.HasValue)
                    return _bsd_estimatehandoverdatecontract.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_estimatehandoverdatecontract = value;
                    OnPropertyChanged(nameof(bsd_estimatehandoverdatecontract));
                }
            }
        }
        public bool bsd_followuplist { get; set; } // danh sách theo dõi
        public string bsd_followuplist_format { get { return BoolToStringData.GetStringByBool(bsd_followuplist); } }
        public bool bsd_terminationletter { get; set; } // thư thanh lý
        public string bsd_terminationletter_format { get { return BoolToStringData.GetStringByBool(bsd_terminationletter); } }
        public bool bsd_specialcontractprintingapproval { get; set; } // duyệt in đặc biệt
        public string bsd_specialcontractprintingapproval_format { get { return BoolToStringData.GetStringByBool(bsd_specialcontractprintingapproval); } }

        public DateTime? _bsd_approvaldateforspecialcontract; // ngày duyệt
        public DateTime? bsd_approvaldateforspecialcontract
        {
            get
            {
                if (_bsd_approvaldateforspecialcontract.HasValue)
                    return _bsd_approvaldateforspecialcontract.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_approvaldateforspecialcontract = value;
                    OnPropertyChanged(nameof(bsd_approvaldateforspecialcontract));
                }
            }
        }
        public Guid bsd_approverforspecialcontract_id { get; set; } // id người duyệt

        private string _bsd_approverforspecialcontract_name; // tên người duyệt
        public string bsd_approverforspecialcontract_name { get => _bsd_approverforspecialcontract_name; set { _bsd_approverforspecialcontract_name = value; OnPropertyChanged(nameof(bsd_approverforspecialcontract_name)); } }

        public DateTime? _bsd_dadate; // ttđc/vbtt
        public DateTime? bsd_dadate
        {
            get
            {
                if (_bsd_dadate.HasValue)
                    return _bsd_dadate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_dadate = value;
                    OnPropertyChanged(nameof(bsd_dadate));
                }
            }
        }

        public DateTime? _bsd_agreementdate; // ngày in ttđc/vbtt
        public DateTime? bsd_agreementdate
        {
            get
            {
                if (_bsd_agreementdate.HasValue)
                    return _bsd_agreementdate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_agreementdate = value;
                    OnPropertyChanged(nameof(bsd_agreementdate));
                }
            }
        }

        public DateTime? _bsd_signeddadate; // ngày ký ttđc/vbtt
        public DateTime? bsd_signeddadate
        {
            get
            {
                if (_bsd_signeddadate.HasValue)
                    return _bsd_signeddadate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_signeddadate = value;
                    OnPropertyChanged(nameof(bsd_signeddadate));
                }
            }
        }
        public string bsd_contractnumber { get; set; } // số hợp đồng
        public int bsd_contracttype { get; set; } // loại hợp đồng
        public string bsd_contracttype_format
        {
            get
            {
                if (bsd_contracttype == 100000000)
                    return Language.contract_long_term_lease_type;//"Long Term Lease";
                else if (bsd_contracttype == 100000001)
                    return Language.contract_purchase_type; //"Purchase"; //contract_purchase_type
                else
                    return "";
            }
        }
        public int bsd_contracttypedescription { get; set; } // mô tả loại hợp đồng
        public string bsd_contracttypedescription_format
        {
            get
            {
                var type = ContractTypeData.GetContractTypeById(bsd_contracttypedescription.ToString());
                if (type != null)
                    return type.Label;
                else
                    return "";
            }
        }
        public bool bsd_updatecontractdate { get; set; } // cập nhật hợp đồng
        public string bsd_updatecontractdate_format { get { return BoolToStringData.GetStringByBool(bsd_updatecontractdate); } }

        public DateTime? _bsd_contractdate; // ngày spa
        public DateTime? bsd_contractdate
        {
            get
            {
                if (_bsd_contractdate.HasValue)
                    return _bsd_contractdate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_contractdate = value;
                    OnPropertyChanged(nameof(bsd_contractdate));
                }
            }
        }

        public DateTime? _bsd_contractprinteddate; // ngày in hợp đồng
        public DateTime? bsd_contractprinteddate
        {
            get
            {
                if (_bsd_contractprinteddate.HasValue)
                    return _bsd_contractprinteddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_contractprinteddate = value;
                    OnPropertyChanged(nameof(bsd_contractprinteddate));
                }
            }
        }

        public DateTime? _bsd_signingexpired; // ngày hết hạn ký hợp đồng
        public DateTime? bsd_signingexpired
        {
            get
            {
                if (_bsd_signingexpired.HasValue)
                    return _bsd_signingexpired.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_signingexpired = value;
                    OnPropertyChanged(nameof(bsd_signingexpired));
                }
            }
        }

        public DateTime? _bsd_signedcontractdate; // ngày ký hợp đồng
        public DateTime? bsd_signedcontractdate
        {
            get
            {
                if (_bsd_signedcontractdate.HasValue)
                    return _bsd_signedcontractdate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_signedcontractdate = value;
                    OnPropertyChanged(nameof(bsd_signedcontractdate));
                }
            }
        }

        public DateTime? _bsd_bsd_uploadeddate; // ngày tải lên hợp đồng
        public DateTime? bsd_bsd_uploadeddate
        {
            get
            {
                if (_bsd_bsd_uploadeddate.HasValue)
                    return _bsd_bsd_uploadeddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_bsd_uploadeddate = value;
                    OnPropertyChanged(nameof(bsd_bsd_uploadeddate));
                }
            }
        }
        public decimal bsd_detailamount { get; set; } // giá gốc
        public string bsd_detailamount_format { get => StringFormatHelper.FormatCurrency(bsd_detailamount); }
        public decimal bsd_discount { get; set; } // chiết khấu
        public string bsd_discount_format { get => StringFormatHelper.FormatCurrency(bsd_discount); }
        public decimal bsd_packagesellingamount { get; set; } // điều kiện bàn giao
        public string bsd_packagesellingamount_format { get => StringFormatHelper.FormatCurrency(bsd_packagesellingamount); }
        public decimal bsd_totalamountlessfreight { get; set; } // giá bán thực
        public string bsd_totalamountlessfreight_format { get => StringFormatHelper.FormatCurrency(bsd_totalamountlessfreight); }
        public decimal bsd_landvaluededuction { get; set; } // giá trị QSDĐ
        public string bsd_landvaluededuction_format { get => StringFormatHelper.FormatCurrency(bsd_landvaluededuction); }
        public decimal totaltax { get; set; } // thuế
        public string totaltax_format { get => StringFormatHelper.FormatCurrency(totaltax); }
        public decimal bsd_freightamount { get; set; } // phí bảo trì
        public string bsd_freightamount_format { get => StringFormatHelper.FormatCurrency(bsd_freightamount); }
        public decimal bsd_totalamount { get; set; } // tổng tiền
        public string bsd_totalamount_format { get => StringFormatHelper.FormatCurrency(bsd_totalamount); }
        public int bsd_numberofmonthspaidmf { get; set; } // số tháng đóng phí
        public decimal bsd_managementfee { get; set; } // phí quản lý
        public string bsd_managementfee_format { get => StringFormatHelper.FormatCurrency(bsd_managementfee); }
        public int bsd_waivermanafeemonth { get; set; } // miễn giảm
        public Guid handovercondition_id { get; set; } // id điều kiện bàn giao

        public string _handovercondition_name; // tên điều kiện bàn giao
        public string handovercondition_name { get => _handovercondition_name; set { _handovercondition_name = value; OnPropertyChanged(nameof(handovercondition_name)); } }
        public Guid paymentscheme_id { get; set; } // id phương thức thanh toán

        public string _paymentscheme_name; // tên phương thức thanh toán
        public string paymentscheme_name { get => _paymentscheme_name; set { _paymentscheme_name = value; OnPropertyChanged(nameof(paymentscheme_name)); } }
        public Guid discountlist_id { get; set; } // id chiết khấu

        public string _discountlist_name; // tên chiết khấu
        public string discountlist_name { get => _discountlist_name; set { _discountlist_name = value; OnPropertyChanged(nameof(discountlist_name)); } }
        public string bsd_discounts { get; set; } // danh sách chiết khấu

        private string _interneldiscount_name;
        public string interneldiscount_name { get => _interneldiscount_name; set { _interneldiscount_name = value; OnPropertyChanged(nameof(interneldiscount_name)); } }
        public Guid discountpromotion_id { get; set; }

        private string _discountpromotion_name;
        public string discountpromotion_name { get => _discountpromotion_name; set { _discountpromotion_name = value; OnPropertyChanged(nameof(discountpromotion_name)); } }
        public string bsd_exchangediscount { get; set; }
        public string bsd_interneldiscount { get; set; }

        public decimal bsd_totalamountpaidinstallment { get; set; }
        public string bsd_totalamountpaidinstallment_format { get => StringFormatHelper.FormatCurrency(bsd_totalamountpaidinstallment); }
        public decimal bsd_totalpercent { get; set; }
        public string totalpercent_Format { get => StringFormatHelper.FormatCurrency(bsd_totalpercent); }

        public string bsd_chietkhautheopttt { get; set; }
        public string bsd_selectedchietkhaupttt { get; set; }

        public Guid unit_handoverid { get; set; }
        public Guid acceptanceid { get; set; }
        public Guid pinkbook_handoverid { get; set; }
        public bool bsd_specialhandoverunit { get; set; }
    }
}
