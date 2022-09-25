using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AccountListModel
    {
        public Guid accountid { get; set; }

        public string bsd_name { get; set; }
        public string bsd_registrationcode { get; set; }
        public string bsd_vatregistrationnumber { get; set; }
        public string bsd_companycode { get; set; }
        public string primarycontact_name { get; set; }
        public string telephone1 { get; set; }
        public string telephone1_format
        {
            get
            {
                if (telephone1 != null && telephone1.Contains("-"))
                {
                    return telephone1.Split('-')[1].StartsWith("84") ? telephone1.Replace("84", "+84-") : telephone1;
                }
                else if (telephone1 != null && telephone1.Contains("+84"))
                {
                    return telephone1.Replace("+84", "+84-");
                }
                else if (telephone1 != null && telephone1.StartsWith("84"))
                {
                    return telephone1.Replace("84", "+84-");
                }
                else
                {
                    return telephone1;
                }
            }
        }
        public string bsd_postalcode { get; set; }
        public string bsd_housenumberstreet { get; set; }
        public string district_name { get; set; }
        public string province_name { get; set; }
        public string country_name { get; set; }

        public string bsd_address
        {
            get
            {
                List<string> address = new List<string>();
                if (!string.IsNullOrWhiteSpace(bsd_housenumberstreet))
                {
                    address.Add(bsd_housenumberstreet);
                }
                if (!string.IsNullOrWhiteSpace(district_name))
                {
                    address.Add(district_name);
                }
                if (!string.IsNullOrWhiteSpace(province_name))
                {
                    address.Add(province_name);
                }
                if (!string.IsNullOrWhiteSpace(bsd_postalcode))
                {
                    address.Add(bsd_postalcode);
                }
                if (!string.IsNullOrWhiteSpace(country_name))
                {
                    address.Add(country_name);
                }

                return string.Join(", ", address);
            }
        }

        public string statuscode { get; set; }
        public string statuscode_format { get { return CustomerStatusCodeData.GetCustomerStatusCodeById(statuscode.ToString())?.Name; } }
        public string statuscode_color { get { return CustomerStatusCodeData.GetCustomerStatusCodeById(statuscode.ToString())?.Background; } }
        public string bsd_customercode { get; set; } // mã khách hàng
    }
}
