using PhuLongCRM.Controls;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class PartyModel : BaseViewModel
    {
        public Guid partyID { get; set; }
        public int typemask { get; set; }
        public Guid contact_id { get; set; }
        public Guid account_id { get; set; }
        public Guid lead_id { get; set; }
        public string contact_name { get; set; }
        public string account_name { get; set; }
        public string lead_name { get; set; }
        public string user_name { get; set; }

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
        public string cutomer_name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(lead_name))
                    return lead_name;
                else if (!string.IsNullOrWhiteSpace(contact_name))
                    return contact_name;
                else if (!string.IsNullOrWhiteSpace(account_name))
                    return account_name;
                else
                    return user_name;
            }
        }
        public string title_code
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(lead_name))
                    return LookUpMultiSelect.CodeLead;
                else if (!string.IsNullOrWhiteSpace(contact_name))
                    return LookUpMultiSelect.CodeContac;
                else if (!string.IsNullOrWhiteSpace(account_name))
                    return LookUpMultiSelect.CodeAccount;
                else
                    return "";
            }
        }
    }
}
