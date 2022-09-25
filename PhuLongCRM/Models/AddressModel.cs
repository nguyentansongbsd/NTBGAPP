using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AddressModel
    {
        public Guid country_id { get; set; }
        public string country_name { get; set; }
        public string country_name_en { get; set; }
        public Guid province_id { get; set; }
        public string province_name { get; set; }
        public string province_name_en { get; set; }
        public Guid district_id { get; set; }
        public string district_name { get; set; }
        public string district_name_en { get; set; }
        public string lineaddress { get; set; }
        public string address { get; set; }
        public string lineaddress_en { get; set; }
        public string address_en { get; set; }
    }
}
