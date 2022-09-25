using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class TaskFormModel : BaseViewModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string description { get; set; }

        public DateTime? _scheduledstart;
        public DateTime? scheduledstart
        {
            get => this._scheduledstart;
            set
            {
                if (value.HasValue)
                {
                    _scheduledstart = value;
                    OnPropertyChanged(nameof(scheduledstart));
                }
            }
        }

        public DateTime? _scheduledend;
        public DateTime? scheduledend
        {
            get => this._scheduledend;
            set
            {
                if (value.HasValue)
                {
                    _scheduledend = value;
                    OnPropertyChanged(nameof(scheduledend));
                }
            }
        }     

        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public Guid account_id { get; set; }
        public string account_name { get; set; }
        public Guid lead_id { get; set; }
        public string lead_name { get; set; }
        public Guid queue_id { get; set; }
        public string queue_name { get; set; }

        private CustomerLookUp _customer;
        public CustomerLookUp Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }
        public int statecode { get; set; } 
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }

    }
}
