using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class Account : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        public string bsd_housenumber { get; set; }
        public string bsd_housenumberstreet { get; set; }
        public string bsd_country_label { get; set; }
        public string bsd_country_labelen { get; set; }
        public string _bsd_country_value { get; set; }
        public string bsd_province_label { get; set; }
        public string bsd_province_labelen { get; set; }
        public string _bsd_province_value { get; set; }
        public string bsd_district_label { get; set; }
        public string bsd_district_labelen { get; set; }
        public string _bsd_district_value { get; set; }
        public double bsd_businesstype { get; set; }

        public string bsd_permanentaddress { get; set; }
        public string bsd_permanenthousenumber { get; set; }
        public string bsd_permanentcountry_label { get; set; }
        public string bsd_permanentcountry_labelen { get; set; }
        public string _bsd_permanentcountry_value { get; set; }
        public string bsd_permanentprovince_label { get; set; }
        public string bsd_permanentprovince_labelen { get; set; }
        public string _bsd_permanentprovince_value { get; set; }
        public string bsd_permanentdistrict_label { get; set; }
        public string bsd_permanentdistrict_labelen { get; set; }
        public string _bsd_permanentdistrict_value { get; set; }

        public string que_bsd_project { get; set; }
        public string que_customerid { get; set; }
        public string que_bsd_queuenumber { get; set; }
        public string que_names { get; set; }
        public string que_estimatedvalue { get; set; }
        public string que_statuscode { get; set; }
        public string que_createdon { get; set; }

        public string quo_bsd_project { get; set; }
        public string quo_bsd_unitno { get; set; }
        public string quo_bsd_quotationnumber { get; set; }
        public string quo_customerid { get; set; }
        public string quo_statuscode { get; set; }
        public string quo_totalamount { get; set; }
        public string quo_createdon { get; set; }

        public string contract_bsd_project { get; set; }
        public string contract_bsd_unitnumber { get; set; }
        public string contract_bsd_optionno { get; set; }
        public string contract_customerid { get; set; }
        public string contract_statuscode { get; set; }
        public string contract_totalamount { get; set; }
        public string contract_bsd_signingexpired { get; set; }
        public string contract_createdon { get; set; }

        public string case_title { get; set; }
        public string case_ticketnumber { get; set; }
        public string case_prioritycode { get; set; }
        public string case_caseorigincode { get; set; }
        public string case_customerid { get; set; }
        public string case_statuscode { get; set; }

        public string activitie_subject { get; set; }
        public string activitie_activitytypecode { get; set; }
        public string activitie_description { get; set; }
        public string activitie_createdon { get; set; }

        public string bsd_address { get { return _bsd_address; } set { _bsd_address = value; OnPropertyChanged(nameof(bsd_address)); } }
        private string _bsd_address;

        public string bsd_diachi { get { return _bsd_diachi; } set { _bsd_diachi = value; OnPropertyChanged(nameof(bsd_diachi)); } }
        private string _bsd_diachi;

        public string bsd_permanentaddress1 { get { return _bsd_permanentaddress1; } set { _bsd_permanentaddress1 = value; OnPropertyChanged(nameof(bsd_permanentaddress1)); } }
        private string _bsd_permanentaddress1;

        public string bsd_diachithuongtru { get { return _bsd_diachithuongtru; } set { _bsd_diachithuongtru = value; OnPropertyChanged(nameof(bsd_diachithuongtru)); } }
        private string _bsd_diachithuongtru;

        public string list_thongtinqueing { get { return _list_thongtinqueing; } set { _list_thongtinqueing = value; OnPropertyChanged(nameof(list_thongtinqueing)); } }
        private string _list_thongtinqueing;

        public string list_thongtinquotation { get { return _list_thongtinquotation; } set { _list_thongtinquotation = value; OnPropertyChanged(nameof(list_thongtinquotation)); } }
        private string _list_thongtinquotation;

        public string list_thongtincontract { get { return _list_thongtincontract; } set { _list_thongtincontract = value; OnPropertyChanged(nameof(list_thongtincontract)); } }
        private string _list_thongtincontract;

        public string list_thongtincase { get { return _list_thongtincase; } set { _list_thongtincase = value; OnPropertyChanged(nameof(list_thongtincase)); } }
        private string _list_thongtincase;

        public string list_thongtinactivitie { get { return _list_thongtinactivitie; } set { _list_thongtinactivitie = value; OnPropertyChanged(nameof(list_thongtinactivitie)); } }
        private string _list_thongtinactivitie;

        //---------------------------------------------------------------------//
        public Guid accountid { get; set; }

        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(name));
                }
            }
        }

        private string _bsd_businesstypevalue;
        public string bsd_businesstypevalue
        {
            get { return _bsd_businesstypevalue; }
            set
            {
                _bsd_businesstypevalue = value;
                OnPropertyChanged(bsd_businesstypevalue);
            }
        }

        private string _bsd_localization;
        public string bsd_localization
        {
            get { return _bsd_localization; }
            set
            {
                if (_bsd_localization != value)
                {
                    _bsd_localization = value;
                    OnPropertyChanged(nameof(bsd_localization));
                }
            }
        }

        private string _bsd_customergroup;
        public string bsd_customergroup
        {
            get { return _bsd_customergroup; }
            set
            {
                if (_bsd_customergroup != value)
                {
                    _bsd_customergroup = value;
                    OnPropertyChanged(nameof(bsd_customergroup));
                }
            }
        }

        private OptionSet _businessType;
        public OptionSet BusinessType
        {
            get { return _businessType; }
            set
            {
                _businessType = value;
                OnPropertyChanged(nameof(BusinessType));
            }
        }

        private OptionSet _localization;
        public OptionSet Localization
        {
            get { return _localization; }
            set
            {
                if (_localization != value)
                {
                    _localization = value;
                    OnPropertyChanged(nameof(Localization));
                }
            }
        }

        private OptionSet _customerGroup;
        public OptionSet CustomerGroup
        {
            get { return _customerGroup; }
            set
            {
                if (_customerGroup != value)
                {
                    _customerGroup = value;
                    OnPropertyChanged(nameof(CustomerGroup));
                }
            }
        }

        private Guid _primarycontact_id;
        public Guid primarycontact_id
        {
            get
            {
                return _primarycontact_id;
            }
            set
            {
                if (_primarycontact_id != value)
                {
                    _primarycontact_id = value;
                    OnPropertyChanged(nameof(primarycontact_id));
                }
            }
        }

        // hứng dữ liệu trả về.
        private string _primarycontact_name;
        public string primarycontact_name
        {
            get
            {
                return _primarycontact_name;
            }
            set
            {
                if (_primarycontact_name != value)
                {
                    _primarycontact_name = value;
                    OnPropertyChanged(nameof(primarycontact_name));
                }
            }
        }

        private LookUp _primaryContact;
        public LookUp PrimaryContact
        {
            get
            {
                return _primaryContact;
            }
            set
            {
                if (_primaryContact != value)
                {
                    _primaryContact = value;
                    OnPropertyChanged(nameof(PrimaryContact));
                }
            }
        }

        private LookUp _project;
        public LookUp Project
        {
            get
            {
                return _project;
            }
            set
            {
                if (_project != value)
                {
                    _project = value;
                    OnPropertyChanged(nameof(Project));
                }
            }
        }

        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get { return this.createdon.ToString("dd/MM/yyyy"); }
        }

        private string _telephone1;
        public string telephone1
        {
            get { return _telephone1; }
            set
            {
                if (_telephone1 != value)
                {
                    _telephone1 = value;
                    OnPropertyChanged(nameof(telephone1));
                }
            }
        }

        private string _fax;
        public string fax
        {
            get { return _fax; }
            set
            {
                if (_fax != value)
                {
                    _fax = value;
                    OnPropertyChanged(nameof(fax));
                }
            }
        }

        private string _websiteurl;
        public string websiteurl
        {
            get { return _websiteurl; }
            set
            {
                if (_websiteurl != value)
                {
                    _websiteurl = value;
                    OnPropertyChanged(nameof(websiteurl));
                }
            }
        }

        private string _emailaddress1;
        public string emailaddress1
        {
            get { return _emailaddress1; }
            set
            {
                if (_emailaddress1 != value)
                {
                    _emailaddress1 = value;
                    OnPropertyChanged(nameof(emailaddress1));
                }
            }
        }

        private string _bsd_companycode;
        public string bsd_companycode
        {
            get { return _bsd_companycode; }
            set
            {
                if (_bsd_companycode != value)
                {
                    _bsd_companycode = value;
                    OnPropertyChanged(nameof(bsd_companycode));
                }
            }
        }

        private bool _bsd_mandatorysecondary;
        public bool bsd_mandatorysecondary
        {
            get { return _bsd_mandatorysecondary; }
            set
            {
                if (_bsd_mandatorysecondary != value)
                {
                    _bsd_mandatorysecondary = value;
                    OnPropertyChanged(nameof(bsd_mandatorysecondary));
                }
            }
        }

        public Account()
        {
            _businessType = new OptionSet()
            {
                Val = "",
                Label = Language.chon_loai_khach_hang //"Select Business Type"
            };
            _localization = new OptionSet();
            _customerGroup = new OptionSet();
        }
    }
}
