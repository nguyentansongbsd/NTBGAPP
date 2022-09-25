using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class LeadFormModel : BaseViewModel
    {
        private string _companyname;
        public string companyname { get { return _companyname; } set { _companyname = value; OnPropertyChanged(nameof(companyname)); } }

        private string _subject;
        public string subject { get { return _subject; } set { _subject = value; OnPropertyChanged(nameof(subject)); } }

        private string _statuscode;
        public string statuscode { get { return _statuscode; } set { _statuscode = value; OnPropertyChanged(nameof(statecode)); } }
        public string statuscode_format { get { return statuscode != null ? LeadStatusCodeData.GetLeadStatusCodeById(statuscode)?.Name : null; } } 
        public string statuscode_color { get { return statuscode != null ? LeadStatusCodeData.GetLeadStatusCodeById(statuscode)?.Background : "#808080"; } }

        private string _statecode;
        public string statecode { get { return _statecode; } set { _statecode = value; OnPropertyChanged(nameof(statecode)); } }

        private string _mobilephone;
        public string mobilephone { get { return _mobilephone; } set { _mobilephone = value; OnPropertyChanged(nameof(mobilephone)); } }

        private string _new_gender;
        public string new_gender { get { return _new_gender; } set { _new_gender = value; OnPropertyChanged(nameof(new_gender)); } }

        private string _emailaddress1;
        public string emailaddress1 { get { return _emailaddress1; } set { _emailaddress1 = value; OnPropertyChanged(nameof(emailaddress1)); } }

        private string _createdon;
        public string createdon { get { return _createdon; } set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        private Guid _leadid;
        public Guid leadid { get { return _leadid; } set { _leadid = value; OnPropertyChanged(nameof(leadid)); } }

        private string _telephone1;
        public string telephone1 { get { return _telephone1;} set { _telephone1 = value; OnPropertyChanged(nameof(telephone1)); } }

        private DateTime? _new_birthday;
        public DateTime? new_birthday
        {
            get { return _new_birthday; }
            set {
                _new_birthday = value;
                OnPropertyChanged(nameof(new_birthday)); }
        }

        private string _websiteurl;
        public string websiteurl { get { return _websiteurl; } set { _websiteurl = value; OnPropertyChanged(nameof(websiteurl)); } }

        private string _address1_composite;
        public string address1_composite { get { return _address1_composite; } set { _address1_composite = value; OnPropertyChanged(nameof(address1_composite)); } }

        private decimal? _bsd_diemdanhgia;
        public decimal? bsd_diemdanhgia { get => _bsd_diemdanhgia; set { _bsd_diemdanhgia = value;  OnPropertyChanged(nameof(bsd_diemdanhgia)); } }
        public string bsd_diemdanhgia_format => bsd_diemdanhgia.HasValue ? string.Format("{0:#,0.#}", bsd_diemdanhgia.Value) : "0";

        private string _bsd_danhgiaid;
        public string bsd_danhgiaid { get { return _bsd_danhgiaid; } set { _bsd_danhgiaid = value; OnPropertyChanged(nameof(bsd_danhgiaid)); } }

        private string _bsd_danhgiadiem;
        public string bsd_danhgiadiem { get { return _bsd_danhgiadiem; } set { _bsd_danhgiadiem = value; OnPropertyChanged(nameof(bsd_danhgiadiem)); } }

        private string __bsd_topic_value;
        public string _bsd_topic_value { get { return __bsd_topic_value; } set { __bsd_topic_value = value; OnPropertyChanged(nameof(_bsd_topic_value)); } }

        private string _bsd_topic_label;
        public string bsd_topic_label { get { return _bsd_topic_label; } set { _bsd_topic_label = value; OnPropertyChanged(nameof(bsd_topic_label)); } }

        private string _lastname;
        public string lastname { get { return _lastname; } set { _lastname = value; OnPropertyChanged(nameof(lastname)); } }

        private string _fullname;
        public string fullname { get { return _fullname; } set { _fullname = value; OnPropertyChanged(nameof(fullname)); } }

        private string _firstname;
        public string firstname { get { return _firstname; } set { _firstname = value; OnPropertyChanged(nameof(firstname)); } }

        private string _jobtitle;
        public string jobtitle { get { return _jobtitle; } set { _jobtitle = value; OnPropertyChanged(nameof(jobtitle)); } }

        private string _address1_line1;
        public string address1_line1 { get { return _address1_line1; } set { _address1_line1 = value; OnPropertyChanged(nameof(address1_line1)); } }

        private string _address1_line2;
        public string address1_line2 { get { return _address1_line2; } set { _address1_line2 = value; OnPropertyChanged(nameof(address1_line2)); } }

        private string _address1_line3;
        public string address1_line3 { get { return _address1_line3; } set { _address1_line3 = value; OnPropertyChanged(nameof(address1_line3)); } }

        private string _address1_city;
        public string address1_city { get { return _address1_city; } set { _address1_city = value; OnPropertyChanged(nameof(address1_city)); } }

        private string _address11_stateorprovince;
        public string address1_stateorprovince { get { return _address11_stateorprovince; } set { _address11_stateorprovince = value; OnPropertyChanged(nameof(address1_stateorprovince)); } }

        private string _address1_postalcode;
        public string address1_postalcode { get { return _address1_postalcode; } set { _address1_postalcode = value; OnPropertyChanged(nameof(address1_postalcode)); } }

        private string _address1_country;
        public string address1_country { get { return _address1_country; } set { _address1_country = value; OnPropertyChanged(nameof(address1_country)); } }

        private string _description;
        public string description { get { return _description; } set { _description = value; OnPropertyChanged(nameof(description)); } }

        private string _industrycode;
        public string industrycode { get { return _industrycode; } set { _industrycode = value; industrycode_notnull = value == null ? false : true; OnPropertyChanged(nameof(industrycode)); } }

        private bool _industrycode_notnull;
        public bool industrycode_notnull { get { return _industrycode_notnull; } set { _industrycode_notnull = value; OnPropertyChanged(nameof(industrycode_notnull)); } }

        //private decimal? _revenue;
        //public decimal? revenue { get { return _revenue; } set { _revenue = value; OnPropertyChanged(nameof(revenue)); } }

        public decimal? revenue { get; set; }
        public string revenue_format { get => StringFormatHelper.FormatCurrency(revenue); }
        public string numberofemployees { get; set; }

        private string _sic;
        public string sic { get { return _sic; } set { _sic = value; OnPropertyChanged(nameof(sic)); } }

        private string __transactioncurrencyid_value;
        public string _transactioncurrencyid_value { get { return __transactioncurrencyid_value; } set { __transactioncurrencyid_value = value; OnPropertyChanged(nameof(_transactioncurrencyid_value)); } }

        private string _transactioncurrencyid_label;
        public string transactioncurrencyid_label { get { return _transactioncurrencyid_label; } set { _transactioncurrencyid_label = value; OnPropertyChanged(nameof(transactioncurrencyid_label)); } }

        private String _bsd_multiselect;
        public String bsd_multiselect { get { return _bsd_multiselect; } set { _bsd_multiselect = value; OnPropertyChanged(nameof(bsd_multiselect)); } }

        private string __campaignid_value;
        public string _campaignid_value { get { return __campaignid_value; } set { __campaignid_value = value; OnPropertyChanged(nameof(_campaignid_value)); } }

        private string _campaignid_label;
        public string campaignid_label { get { return _campaignid_label; } set { _campaignid_label = value; OnPropertyChanged(nameof(campaignid_label)); } }


        private string _donotsendmmValue;
        public string donotsendmmValue { get { return _donotsendmmValue; } set { _donotsendmmValue = value; OnPropertyChanged(nameof(donotsendmmValue)); } }

        private bool _donotsendmm;
        public bool donotsendmm { get { return _donotsendmm; } set { _donotsendmm = value; donotsendmmValue = value ? Language.khong_gui : Language.gui; OnPropertyChanged(nameof(donotsendmm)); } }

        private DateTime? _lastusedincampaign;
        public DateTime? lastusedincampaign { get { return _lastusedincampaign; }
            set { if (value.HasValue) { _lastusedincampaign = value.Value.ToLocalTime(); } else { _lastusedincampaign = null; }
                OnPropertyChanged(nameof(lastusedincampaign)); } }

        private bool _bsd_tieuchi_vitri;
        public bool bsd_tieuchi_vitri { get { return _bsd_tieuchi_vitri; } set { _bsd_tieuchi_vitri = value; OnPropertyChanged(nameof(bsd_tieuchi_vitri)); } }

        private bool _bsd_tieuchi_phuongthucthanhtoan;
        public bool bsd_tieuchi_phuongthucthanhtoan { get { return _bsd_tieuchi_phuongthucthanhtoan; } set { _bsd_tieuchi_phuongthucthanhtoan = value; OnPropertyChanged(nameof(bsd_tieuchi_phuongthucthanhtoan)); } }

        private bool _bsd_tieuchi_giacanho;
        public bool bsd_tieuchi_giacanho { get { return _bsd_tieuchi_giacanho; } set { _bsd_tieuchi_giacanho = value; OnPropertyChanged(nameof(bsd_tieuchi_giacanho)); } }

        private bool _bsd_tieuchi_nhadautuuytin;
        public bool bsd_tieuchi_nhadautuuytin { get { return _bsd_tieuchi_nhadautuuytin; } set { _bsd_tieuchi_nhadautuuytin = value; OnPropertyChanged(nameof(bsd_tieuchi_nhadautuuytin)); } }

        private bool _bsd_tieuchi_moitruongsong;
        public bool bsd_tieuchi_moitruongsong { get { return _bsd_tieuchi_moitruongsong; } set { _bsd_tieuchi_moitruongsong = value; OnPropertyChanged(nameof(bsd_tieuchi_moitruongsong)); } }

        private bool _bsd_tieuchi_baidauxe;
        public bool bsd_tieuchi_baidauxe { get { return _bsd_tieuchi_baidauxe; } set { _bsd_tieuchi_baidauxe = value; OnPropertyChanged(nameof(bsd_tieuchi_baidauxe)); } }

        private bool _bsd_tieuchi_hethonganninh;
        public bool bsd_tieuchi_hethonganninh { get { return _bsd_tieuchi_hethonganninh; } set { _bsd_tieuchi_hethonganninh = value; OnPropertyChanged(nameof(bsd_tieuchi_hethonganninh)); } }

        private bool _bsd_tieuchi_huongcanho;
        public bool bsd_tieuchi_huongcanho { get { return _bsd_tieuchi_huongcanho; } set { _bsd_tieuchi_huongcanho = value; OnPropertyChanged(nameof(bsd_tieuchi_huongcanho)); } }

        private bool _bsd_tieuchi_hethongcuuhoa;
        public bool bsd_tieuchi_hethongcuuhoa { get { return _bsd_tieuchi_hethongcuuhoa; } set { _bsd_tieuchi_hethongcuuhoa = value; OnPropertyChanged(nameof(bsd_tieuchi_hethongcuuhoa)); } }

        private bool _bsd_tieuchi_nhieutienich;
        public bool bsd_tieuchi_nhieutienich { get { return _bsd_tieuchi_nhieutienich; } set { _bsd_tieuchi_nhieutienich = value; OnPropertyChanged(nameof(bsd_tieuchi_nhieutienich)); } }

        private bool _bsd_tieuchi_ganchosieuthi;
        public bool bsd_tieuchi_ganchosieuthi { get { return _bsd_tieuchi_ganchosieuthi; } set { _bsd_tieuchi_ganchosieuthi = value; OnPropertyChanged(nameof(bsd_tieuchi_ganchosieuthi)); } }

        private bool _bsd_tieuchi_gantruonghoc;
        public bool bsd_tieuchi_gantruonghoc { get { return _bsd_tieuchi_gantruonghoc; } set { _bsd_tieuchi_gantruonghoc = value; OnPropertyChanged(nameof(bsd_tieuchi_gantruonghoc)); } }

        private bool _bsd_tieuchi_ganbenhvien;
        public bool bsd_tieuchi_ganbenhvien { get { return _bsd_tieuchi_ganbenhvien; } set { _bsd_tieuchi_ganbenhvien = value; OnPropertyChanged(nameof(bsd_tieuchi_ganbenhvien)); } }

        private bool _bsd_tieuchi_dientichcanho;
        public bool bsd_tieuchi_dientichcanho { get { return _bsd_tieuchi_dientichcanho; } set { _bsd_tieuchi_dientichcanho = value; OnPropertyChanged(nameof(bsd_tieuchi_dientichcanho)); } }

        private bool _bsd_tieuchi_thietkenoithatcanho;
        public bool bsd_tieuchi_thietkenoithatcanho { get { return _bsd_tieuchi_thietkenoithatcanho; } set { _bsd_tieuchi_thietkenoithatcanho = value; OnPropertyChanged(nameof(bsd_tieuchi_thietkenoithatcanho)); } }

        private bool _bsd_tieuchi_tangcanhodep;
        public bool bsd_tieuchi_tangcanhodep { get { return _bsd_tieuchi_tangcanhodep; } set { _bsd_tieuchi_tangcanhodep = value; OnPropertyChanged(nameof(bsd_tieuchi_tangcanhodep)); } }

        private bool _bsd_dientich_3060m2;
        public bool bsd_dientich_3060m2 { get { return _bsd_dientich_3060m2; } set { _bsd_dientich_3060m2 = value; OnPropertyChanged(nameof(bsd_dientich_3060m2)); } }

        private bool _bsd_dientich_6080m2;
        public bool bsd_dientich_6080m2 { get { return _bsd_dientich_6080m2; } set { _bsd_dientich_6080m2 = value; OnPropertyChanged(nameof(bsd_dientich_6080m2)); } }

        private bool _bsd_dientich_80100m2;
        public bool bsd_dientich_80100m2 { get { return _bsd_dientich_80100m2; } set { _bsd_dientich_80100m2 = value; OnPropertyChanged(nameof(bsd_dientich_80100m2)); } }

        private bool _bsd_dientich_100120m2;
        public bool bsd_dientich_100120m2 { get { return _bsd_dientich_100120m2; } set { _bsd_dientich_100120m2 = value; OnPropertyChanged(nameof(bsd_dientich_100120m2)); } }

        private bool _bsd_dientich_lonhon120m2;
        public bool bsd_dientich_lonhon120m2 { get { return _bsd_dientich_lonhon120m2; } set { _bsd_dientich_lonhon120m2 = value; OnPropertyChanged(nameof(bsd_dientich_lonhon120m2)); } }

        private bool _bsd_quantam_datnen;
        public bool bsd_quantam_datnen { get { return _bsd_quantam_datnen; } set { _bsd_quantam_datnen = value; OnPropertyChanged(nameof(bsd_quantam_datnen)); } }

        private bool _bsd_quantam_canho;
        public bool bsd_quantam_canho { get { return _bsd_quantam_canho; } set { _bsd_quantam_canho = value; OnPropertyChanged(nameof(bsd_quantam_canho)); } }

        private bool _bsd_quantam_bietthu;
        public bool bsd_quantam_bietthu { get { return _bsd_quantam_bietthu; } set { _bsd_quantam_bietthu = value; OnPropertyChanged(nameof(bsd_quantam_bietthu)); } }

        private bool _bsd_quantam_khuthuongmai;
        public bool bsd_quantam_khuthuongmai { get { return _bsd_quantam_khuthuongmai; } set { _bsd_quantam_khuthuongmai = value; OnPropertyChanged(nameof(bsd_quantam_khuthuongmai)); } }

        private bool _bsd_quantam_nhapho;
        public bool bsd_quantam_nhapho { get { return _bsd_quantam_nhapho; } set { _bsd_quantam_nhapho = value; OnPropertyChanged(nameof(bsd_quantam_nhapho)); } }

        private string _bsd_identitycardnumber;
        public string bsd_identitycardnumber { get { return _bsd_identitycardnumber; } set { _bsd_identitycardnumber = value; OnPropertyChanged(nameof(bsd_identitycardnumber)); } }

        private GridLength _showQualifyButton;
        public GridLength showQualifyButton { get { return _showQualifyButton; } set { _showQualifyButton = value; OnPropertyChanged(nameof(showQualifyButton)); } }

        public bool checkLeadsRating(String id)
        {
            return bsd_danhgiaid == null ? false : this.getList(bsd_danhgiaid).FirstOrDefault(x => x == id) != null;
        }
        public bool checkMultiSelect(String id)
        {
            return bsd_multiselect == null ? false : this.getList(bsd_multiselect).FirstOrDefault(x => x == id) != null;
        }

        public List<String> getList(String str)
        {
            return str == null ? new List<string>() : str.Split(',').ToList();
        }

      //  public string bsd_contactaddress { get; set; }
        public string bsd_diachi { get; set; }
       // public string bsd_housenumberstreet { get; set; }
        public string bsd_housenumber { get; set; }
        public string _bsd_country_value { get; set; }
        public string bsd_country_label { get; set; }
        public string bsd_country_en { get; set; }
        public string _bsd_province_value { get; set; }
        public string bsd_province_label { get; set; }
        public string bsd_province_en { get; set; }
        public string _bsd_district_value { get; set; }
        public string bsd_district_label { get; set; }
        public string bsd_district_en { get; set; }
        public int leadqualitycode { get; set; }
        public Guid contactid { get; set; }
        public string leadsourcecode { get; set; }
        public string bsd_customercode { get; set; }
        public string bsd_customergroup { get; set; }
        public string bsd_typeofidcard { get; set; }

        private string _bsd_identitycardnumberid;
        public string bsd_identitycardnumberid { get=>_bsd_identitycardnumberid; set { _bsd_identitycardnumberid = value;OnPropertyChanged(nameof(bsd_identitycardnumberid)); } }
        public string bsd_area { get; set; }
        public string bsd_placeofissue { get; set; }
        public string bsd_registrationcode { get; set; }

        private DateTime? _bsd_dategrant;
        public DateTime? bsd_dategrant { get => _bsd_dategrant; set { _bsd_dategrant = value; OnPropertyChanged(nameof(bsd_dategrant)); } }

        //địa chỉ liên ljac
        public Guid bsd_country_id { get; set; }
        public Guid bsd_province_id { get; set; }
        public Guid bsd_district_id { get; set; }
        public string bsd_housenumberstreet { get; set; }
        public string bsd_contactaddress { get; set; }
        public string bsd_country_name { get; set; }
        public string bsd_province_name { get; set; }
        public string bsd_district_name { get; set; }
        public string bsd_country_name_en { get; set; }
        public string bsd_province_name_en { get; set; }
        public string bsd_district_name_en { get; set; }

        //địa chỉ thường trú
        public Guid bsd_permanentcountry_id { get; set; }
        public Guid bsd_permanentprovince_id { get; set; }
        public Guid bsd_permanentdistrict_id { get; set; }
        public string bsd_permanentaddress { get; set; }
        public string bsd_permanentaddress1 { get; set; }
        public string bsd_permanentcountry_name { get; set; }
        public string bsd_permanentprovince_name { get; set; }
        public string bsd_permanentdistrict_name { get; set; }
        public string bsd_permanentcountry_name_en { get; set; }
        public string bsd_permanentprovince_name_en { get; set; }
        public string bsd_permanentdistrict_name_en { get; set; }

        //địa chỉ công ty
        public Guid bsd_accountcountry_id { get; set; }
        public Guid bsd_accountprovince_id { get; set; }
        public Guid bsd_accountdistrict_id { get; set; }
        public string bsd_account_housenumberstreetwardvn { get; set; }
        public string bsd_accountaddressvn { get; set; }
        public string bsd_accountcountry_name { get; set; }
        public string bsd_accountprovince_name { get; set; }
        public string bsd_accountdistrict_name { get; set; }
        public string bsd_accountcountry_name_en { get; set; }
        public string bsd_accountprovince_name_en { get; set; }
        public string bsd_accountdistrict_name_en { get; set; }
        public Guid account_id { get; set; }
        public Guid contact_id { get; set; }

        private string _bsd_qrcode;
        public string bsd_qrcode { get=>_bsd_qrcode; set { _bsd_qrcode = value;OnPropertyChanged(nameof(bsd_qrcode)); } }
        public string mobilephone_format
        {
            get
            {
                if (mobilephone != null && mobilephone.Contains("-"))
                {
                    return mobilephone.Split('-')[1].StartsWith("84") ? mobilephone.Replace("84", "+84-") : mobilephone;
                }
                else if (mobilephone != null && mobilephone.Contains("+84"))
                {
                    return mobilephone.Replace("+84", "+84-");
                }
                else if (mobilephone != null && mobilephone.StartsWith("84"))
                {
                    return mobilephone.Replace("84", "+84-");
                }
                else
                {
                    return mobilephone;
                }
            }
        }
    }
}
