
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class MandatorySecondaryModel : ContentView
    {
        public Guid bsd_mandatorysecondaryid { get; set; }

        private string _bsd_name;
        public string bsd_name { get { return _bsd_name; } set { _bsd_name = value; OnPropertyChanged(nameof(bsd_name)); } }
        private DateTime? _createdon;
        public DateTime? createdon
        {
            get => _createdon;
            set
            {
                if (value.HasValue)
                { _createdon = value.Value.ToLocalTime();
                    OnPropertyChanged(nameof(createdon));
                }
            }
        }
        public int statuscode { get; set; }
        public string statuscode_format { get { return statuscode != 0 ? MandatorySecondaryStatusCodeData.GetMandatorySecondaryStatusById(statuscode.ToString())?.Name : null; } }
        public string statuscode_color { get { return statuscode != 0 ? MandatorySecondaryStatusCodeData.GetMandatorySecondaryStatusById(statuscode.ToString())?.Background : "#808080"; } }
        public string ownerid { get; set; }

        public string _bsd_jobtitlevn;
        public string bsd_jobtitlevn { get { return _bsd_jobtitlevn; } set { _bsd_jobtitlevn = value; OnPropertyChanged(nameof(bsd_jobtitlevn)); } }

        public string _bsd_jobtitleen;
        public string bsd_jobtitleen { get { return _bsd_jobtitleen; } set { _bsd_jobtitleen = value; OnPropertyChanged(nameof(bsd_jobtitleen)); } }

        private DateTime? _bsd_effectivedateto;
        public DateTime? bsd_effectivedateto { get { return _bsd_effectivedateto; }
            set { if (value.HasValue) { _bsd_effectivedateto = value.Value.ToLocalTime(); OnPropertyChanged(nameof(bsd_effectivedateto)); } } }

        private DateTime? _bsd_effectivedatefrom;
        public DateTime? bsd_effectivedatefrom
        {
            get { return _bsd_effectivedatefrom; }
            set { if (value.HasValue) { _bsd_effectivedatefrom = value.Value.ToLocalTime(); OnPropertyChanged(nameof(bsd_effectivedatefrom)); } }
        }
        public Guid _bsd_developeraccount_value { get; set; }

        private string _bsd_developeraccount;
        public string bsd_developeraccount { get { return _bsd_developeraccount; } set { _bsd_developeraccount = value; OnPropertyChanged(nameof(bsd_developeraccount)); } }

        private string _bsd_developeraccount_name;
        public string bsd_developeraccount_name { get { return _bsd_developeraccount_name; } set { _bsd_developeraccount_name = value; OnPropertyChanged(nameof(bsd_developeraccount_name)); } }
        public string bsd_contact_name { get; set; }
        public string bsd_contacmobilephone { get; set; }
        public string bsd_contactaddress { get; set; }        
        public string _bsd_contact_value { get; set; }
        public string statuscode_title { get; set; }
        public Guid bsd_contactid { get; set; } 

        private string _bsd_descriptionsvn;
        public string bsd_descriptionsvn { get { return _bsd_descriptionsvn; } set { _bsd_descriptionsvn = value; OnPropertyChanged(nameof(bsd_descriptionsvn)); } }

        private string _bsd_descriptionsen;
        public string bsd_descriptionsen { get { return _bsd_descriptionsen; } set { _bsd_descriptionsen = value; OnPropertyChanged(nameof(bsd_descriptionsen)); } }
        public bool is_employee { get; set; }
        public Guid bsd_employeeid { get; set; }

    }
}
