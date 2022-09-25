using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class UnitInfoModel
    {
        public string bsd_units { get; set; } // Units Number
        public string name { get; set; } // Units Code
        public string productnumber { get; set; } // Unit Description - mã can hộ dự án
        public string productid { get; set; }
        public int statuscode { get; set; }
        public Guid _bsd_projectcode_value { get; set; }
        public Guid bsd_project_id { get; set; } // khong su dung trong form unit , su dung trong form queue
        public string bsd_project_name { get; set; }

        public Guid bsd_block_id { get; set; } // khong su dung trong form unit , su dung trong form queue
        public string bsd_block_name { get; set; }

        public Guid bsd_floor_id { get; set; } // khong su dung trong form unit , su dung trong form queue
        public string bsd_floor_name { get; set; }

        public Guid _bsd_phaseslaunchid_value { get; set; }
        public Guid bsd_phaseslaunch_id { get; set; }// khong su dung trong form unit , su dung trong form queue
        public string bsd_phaseslaunch_name { get; set; } // khong su dung trong form unit , su dung trong form queue

        public Guid pricelist_id { get; set; }// khong su dung trong form unit , su dung trong form queue
        public string pricelist_name { get; set; } // khong su dung trong form unit , su dung trong form queue
        public decimal bsd_depositamount { get; set; } // Deposit Amount
        public string bsd_depositamount_format { get => StringFormatHelper.FormatCurrency(bsd_depositamount); }
        public decimal bsd_queuingfee { get; set; } // Queuing Amount
        public string bsd_queuingfee_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfee); }
        public Guid bsd_unittype_value { get; set; }
        public string bsd_unittype_name { get; set; }

        public bool bsd_vippriority { get; set; }
        public string bsd_vippriority_format
        {
            get => bsd_vippriority == true ? Language.co : Language.khong;// "Yes" : "No";
        }

        // thong tin dien tich
        public decimal bsd_areavariance { get; set; } // Biên độ diện tích cho phép
        public string bsd_areavariance_format { get => StringFormatHelper.FormatCurrency(bsd_areavariance); }
        public decimal bsd_constructionarea { get; set; } // diện tích xây dựng
        public string bsd_constructionarea_format { get => StringFormatHelper.FormatPercent(bsd_constructionarea); }
        public decimal bsd_netsaleablearea { get; set; } // diện tích sử dụng 
        public string bsd_netsaleablearea_format { get => StringFormatHelper.FormatCurrency(bsd_netsaleablearea); }

        // thong itn gia
        public decimal price { get; set; } // Giá bán
        public string price_format { get => StringFormatHelper.FormatCurrency(price); }

        public decimal bsd_landvalueofunit { get; set; } // đơn giá giá trị đất
        public string bsd_landvalueofunit_format { get => StringFormatHelper.FormatCurrency(bsd_landvalueofunit); }
        public decimal bsd_landvalue { get; set; } // giá trị đất
        public string bsd_landvalue_format { get => StringFormatHelper.FormatCurrency(bsd_landvalue); }
        public decimal bsd_maintenancefeespercent { get; set; } // phần trăm phí bảo trì
        public string bsd_maintenancefeespercent_format { get => StringFormatHelper.FormatPercent(bsd_maintenancefeespercent); }
        public decimal bsd_maintenancefees { get; set; } // tiền phí bảo trị
        public string bsd_maintenancefees_format { get => StringFormatHelper.FormatCurrency(bsd_maintenancefees); }
        public decimal bsd_taxpercent { get; set; } // phằn trăm thuế
        public string bsd_taxpercent_format { get => StringFormatHelper.FormatPercent(bsd_taxpercent); }
        public decimal bsd_vat { get; set; } // tiền tuế
        public string bsd_vat_format { get => StringFormatHelper.FormatCurrency(bsd_vat); }
        public decimal bsd_totalprice { get; set; } // tiền tuế

        public string bsd_direction { get; set; }
        public string bsd_viewphulong { get; set; }


        // bàn giao

        public DateTime bsd_estimatehandoverdate { get; set; } // ngày dự kiến bàn giao.
        public int bsd_numberofmonthspaidmf { get; set; } // số tháng tính phí quản lý.
        public decimal bsd_managementamountmonth { get; set; }// đơn giá tính phí quản lý (tháng/m2)
        public string bsd_managementamountmonth_format { get => StringFormatHelper.FormatCurrency(bsd_managementamountmonth); }
        public decimal bsd_handovercondition { get; set; } // Điều kiện bàn giao %
        public string bsd_handovercondition_format { get => StringFormatHelper.FormatPercent(bsd_handovercondition); }
        public Guid event_id { get; set; }
        public bool is_event { get { if (event_id != Guid.Empty) return true; else return false; } }
    }
}
