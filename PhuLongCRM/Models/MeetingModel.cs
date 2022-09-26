using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class MeetingModel : BaseViewModel
    {
        public Guid activityid { get; set; }

        private string _subject;
        public string subject { get => _subject; set { _subject = value; OnPropertyChanged(nameof(subject)); } }

        private string _description;
        public string description { get => _description; set { _description = value; OnPropertyChanged(nameof(description)); } }

        public DateTime? _scheduledstart;
        public DateTime? scheduledstart
        {
            get => this._scheduledstart;
            set
            {
                _scheduledstart = value;
                OnPropertyChanged(nameof(scheduledstart));
            }
        }
        public TimeSpan _timeStart { get; set; }
        public TimeSpan timeStart
        {
            get => this._timeStart;
            set
            {
                if (_timeStart != value)
                {
                    _timeStart = value;
                    OnPropertyChanged(nameof(timeStart));
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

        public TimeSpan _timeEnd;
        public TimeSpan timeEnd
        {
            get => this._timeEnd;
            set
            {
                if (_timeEnd != value)
                {
                    _timeEnd = value;
                    OnPropertyChanged(nameof(timeEnd));
                }
            }
        }
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public int scheduleddurationminutes { get; set; }
        public OptionSet _durationValue;
        public OptionSet durationValue
        {
            get => _durationValue;
            set
            {
                if (_durationValue != value)
                {
                    _durationValue = value;
                    OnPropertyChanged(nameof(durationValue));
                }
            }
        }
        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid lead_id { get; set; }
        public string lead_name { get; set; }

        public Guid queue_id { get; set; }
        public string queue_name { get; set; }

        public Guid user_id { get; set; }
        public string user_name { get; set; }
        public LookUp Lead
        {
            set
            {
                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 3
                    };
                }
            }
        }
        public LookUp Contact
        {
            set
            {

                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 1
                    };
                }
            }
        }
        public LookUp Account
        {
            set
            {
                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 2
                    };

                }

            }
        }

        // Regarding (liên quan đến)
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
        private string _location;
        public string location { get => _location; set { _location = value; OnPropertyChanged(nameof(location)); } }

        private bool _isalldayevent;
        public bool isalldayevent { get => _isalldayevent; set { _isalldayevent = value; OnPropertyChanged(nameof(isalldayevent)); } }

        public int statecode { get; set; }
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }

        public bool _editable;
        public bool editable
        {
            get => _editable;
            set
            {
                if (_editable != value)
                {
                    _editable = value;
                    OnPropertyChanged(nameof(editable));
                }
            }
        }

        private string _required;
        public string required { get => _required; set { _required = value; OnPropertyChanged(nameof(required)); } }

        private string _optional;
        public string optional { get => _optional; set { _optional = value; OnPropertyChanged(nameof(optional)); } }

        public string bsd_collectiontype { get; set; }
    }
}
