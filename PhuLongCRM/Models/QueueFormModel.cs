using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class QueueFormModel : BaseViewModel
    {
        public Guid opportunityid { get; set; }
        public string bsd_queuenumber { get; set; }
        public string name { get; set; }
        public decimal budgetamount { get; set; }       
        public string description { get; set; }

        /// <summary>
        /// Su dung contact_id + contact_name khi lay du lieu cua queue ve tu form update.
        /// khi tu direct sale qua thi ko su dug thong tin nay.
        /// tuong tu cho account.
        /// </summary>
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        private string _customer_name;
        public string customer_name { get => _customer_name; set { _customer_name = value; OnPropertyChanged(nameof(customer_name)); } }

        public Guid bsd_customerreferral_account_id { get; set; }
        public string bsd_customerreferral_name { get; set; }
        public Guid bsd_salesagentcompany_account_id { get; set; }
        public string bsd_salesagentcompany_name { get; set; }
        public string bsd_nameofstaffagent { get; set; }

        public Guid bsd_collaborator_contact_id { get; set; }
        public string bsd_collaborator_name { get; set; }

        public int statuscode { get; set; } // chi su dung trong form update.

        public DateTime _createdon;
        public DateTime createdon { get => _createdon.AddHours(7); set { _createdon = value; OnPropertyChanged(nameof(createdon)); } } // Thời gian đặt chỗ 

        public DateTime _bsd_queuingexpired;
        public DateTime bsd_queuingexpired { get => _bsd_queuingexpired.AddHours(7); set { _bsd_queuingexpired = value; OnPropertyChanged(nameof(bsd_queuingexpired)); } } // Thời gian đặt chỗ  // Thời gian hết hạn

        public DateTime _bsd_bookingtime;
        public DateTime bsd_bookingtime { get => _bsd_bookingtime.AddHours(7); set { _bsd_bookingtime = value; OnPropertyChanged(nameof(bsd_bookingtime)); } } // Thời gian bat dau

        public Guid bsd_project_id { get; set; }
        public string bsd_project_name { get; set; } // dự án
        public decimal bsd_bookingf { get; set; }

        public Guid bsd_phaseslaunch_id { get; set; }
        public string bsd_phaseslaunch_name { get; set; }
        public Guid bsd_discountlist_id { get; set; } // lấy về kèm theo khi lấy phaselaucn mục đích dùng để đưa qua đặt cọc.

        public Guid bsd_block_id { get; set; }
        public string bsd_block_name { get; set; }

        public Guid bsd_floor_id { get; set; }
        public string bsd_floor_name { get; set; }

        public Guid bsd_units_id { get; set; }
        public string _bsd_units_name;
        public string bsd_units_name { get => _bsd_units_name; set { _bsd_units_name = value; OnPropertyChanged(nameof(bsd_units_name)); } }
        public decimal bsd_units_queuingfee { get; set; }

        public Guid pricelist_id { get; set; }
        public string pricelist_name { get; set; }

        public decimal constructionarea { get; set; } // diện tích xây dựng , tên gốc bsd_constructionarea => đổi lại tránh trùng khi trong form update khi lấy thông tin về.

        public decimal netsaleablearea { get; set; } // diện tích sử dụng  , tên gốc bsd_netsaleablearea => đổi lại tránh trùng khi trong form update khi lấy thông tin về.

        public bool bsd_collectedqueuingfee { get; set; } // Đã nhận tiền

        private decimal _bsd_queuingfee;
        public decimal bsd_queuingfee { get => _bsd_queuingfee; set { _bsd_queuingfee = value; OnPropertyChanged(nameof(bsd_queuingfee)); } } // phí đặt chỗ
        
        private string _bsd_queuingfee_format;
        public string bsd_queuingfee_format { get => _bsd_queuingfee_format; set { _bsd_queuingfee_format = value; OnPropertyChanged(nameof(bsd_queuingfee_format)); } }
        public decimal landvalue { get; set; } // giá trị đất

        public decimal unit_price { get; set; } // Giá bán , tên gốc price => đổi lại tránh trùng khi trong form update khi lấy thông tin về.
        public int bsd_longtime { get; set; }
        public int bsd_shorttime { get; set; }
        public DateTime _queue_createdon { get; set; }
        public DateTime _queue_bsd_queuingexpired { get; set; }
        public DateTime _queue_bsd_bookingtime { get; set; }
        public int UnitStatusCode { get; set; }

        public string bsd_bookingid { get; set; }
        public string _defaultuomid_value { get; set; }
        public string _transactioncurrencyid_value { get; set; }
        public decimal bsd_taxpercent { get; set; }

        private int _bsd_ordernumber;
        public int bsd_ordernumber { get => _bsd_ordernumber; set { _bsd_ordernumber = value; OnPropertyChanged(nameof(bsd_ordernumber)); } }

        private int _bsd_priorityqueue;
        public int bsd_priorityqueue { get => _bsd_priorityqueue; set { _bsd_priorityqueue = value; OnPropertyChanged(nameof(bsd_priorityqueue)); } }

        private int _bsd_prioritynumber;
        public int bsd_prioritynumber { get => _bsd_prioritynumber; set { _bsd_prioritynumber = value; OnPropertyChanged(nameof(bsd_prioritynumber)); } }

        private DateTime _bsd_dateorder;
        public DateTime bsd_dateorder { get => _bsd_dateorder; set { _bsd_dateorder = value; OnPropertyChanged(nameof(bsd_dateorder)); } }

        private bool _bsd_expired;
        public bool bsd_expired { get => _bsd_expired; set { _bsd_expired = value; OnPropertyChanged(nameof(bsd_expired)); } }

        public string bsd_expired_format { get { return BoolToStringData.GetStringByBool(bsd_expired); } }

        public string statuscode_format { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString())?.Name; } }
        public string statuscode_color { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString())?.BackGroundColor; } }

        public bool bsd_queueforproject { get; set; }
    }
}
