using System;
using PhuLongCRM.ViewModels;

namespace PhuLongCRM.Models
{
    public class Provinces : BaseViewModel
    {
        private string _new_name;
        public string new_name { get { return _new_name; } set { _new_name = value; OnPropertyChanged(nameof(new_name)); } }

        private DateTime? _createdon;
        public DateTime? createdon { get { return _createdon; } set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        private string _new_id;
        public string new_id { get { return _new_id; } set { _new_id = value; OnPropertyChanged(nameof(new_id)); } }

        private string __bsd_country_value;
        public string _bsd_country_value { get { return __bsd_country_value; } set { __bsd_country_value = value; OnPropertyChanged(nameof(_bsd_country_value)); } }

        private string _bsd_priority;
        public string bsd_priority { get { return _bsd_priority; } set { _bsd_priority = value; OnPropertyChanged(nameof(bsd_priority)); } }

        private string _bsd_provincename;
        public string bsd_provincename { get { return _bsd_provincename; } set { _bsd_provincename = value; OnPropertyChanged(nameof(bsd_provincename)); } }

        private string _new_provinceid;
        public string new_provinceid { get { return _new_provinceid; } set { _new_provinceid = value; OnPropertyChanged(nameof(new_provinceid)); } }

        private string _bsd_countries;
        public string bsd_countries { get { return _bsd_countries; } set { _bsd_countries = value; OnPropertyChanged(nameof(bsd_countries)); } }

        public Provinces()
        {
            this.new_name = " ";
        }
    }
}
