using System;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class ContactFormModel : BaseViewModel
    {
        private string _fullname;
        public string fullname { get { return _fullname; } set { _fullname = value; OnPropertyChanged(nameof(fullname)); } }

        private string _bsd_fullname;
        public string bsd_fullname { get { return _bsd_fullname; } set { _bsd_fullname = value; OnPropertyChanged(nameof(bsd_fullname)); } }

        private string _bsd_firstname;
        public string bsd_firstname { get { return _bsd_firstname; } set { _bsd_firstname = value; OnPropertyChanged(nameof(_bsd_firstname)); } }

        private string _firstname;
        public string firstname { get => _firstname; set { _firstname = value; OnPropertyChanged(nameof(firstname)); } }

        private string _bsd_lastname;
        public string bsd_lastname { get { return _bsd_lastname; } set { _bsd_lastname = value; OnPropertyChanged(nameof(bsd_lastname)); } }

        private string _lastname;
        public string lastname { get => _lastname; set { _lastname = value; OnPropertyChanged(nameof(lastname)); } }

        private string _emailaddress1;
        public string emailaddress1 { get { return _emailaddress1; } set { _emailaddress1 = value; OnPropertyChanged(nameof(emailaddress1)); } }

        private string _jobtitle;
        public string jobtitle { get { return _jobtitle; } set { _jobtitle = value; OnPropertyChanged(nameof(jobtitle)); } }

        private DateTime? _birthdate;
        public DateTime? birthdate
        {
            get { return _birthdate; }
            set
            {
                if (value.HasValue) { _birthdate = value.Value.ToLocalTime(); } else { _birthdate = null; }
                OnPropertyChanged(nameof(birthdate));
            }
        }

        private string _mobilephone;
        public string mobilephone { get => _mobilephone; set { _mobilephone = value; OnPropertyChanged(nameof(mobilephone)); } }
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

        private string _createdon;
        public string createdon { get { return _createdon; } set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        private string _ownerid;
        public string ownerid { get { return _ownerid; } set { _ownerid = value; OnPropertyChanged(nameof(ownerid)); } }

        private string _gendercode;
        public string gendercode { get { return _gendercode; } set { _gendercode = value; OnPropertyChanged(nameof(gendercode)); } }

        private string _bsd_identitycardnumber;
        public string bsd_identitycardnumber { get { return _bsd_identitycardnumber; } set { _bsd_identitycardnumber = value; OnPropertyChanged(nameof(bsd_identitycardnumber)); } }

        private string _statuscode;
        public string statuscode { get { return _statuscode; } set { _statuscode = value; OnPropertyChanged(nameof(statuscode)); } }

        private string _bsd_contactaddress;
        public string bsd_contactaddress { get { return _bsd_contactaddress; } set { _bsd_contactaddress = value; OnPropertyChanged(nameof(bsd_contactaddress)); } }

        private Guid _contactid;
        public Guid contactid { get { return _contactid; } set { _contactid = value; OnPropertyChanged(nameof(contactid)); } }

        private string _statecode;
        public string statecode { get { return _statecode; } set { _statecode = value; OnPropertyChanged(nameof(statecode)); } }

        private string _bsd_type;
        public string bsd_type { get { return _bsd_type; } set { _bsd_type = value; OnPropertyChanged(nameof(bsd_type)); } }

        private string _bsd_localization;
        public string bsd_localization { get { return _bsd_localization; } set { _bsd_localization = value; OnPropertyChanged(nameof(bsd_localization)); } }

        private bool _bsd_haveprotector;
        public bool bsd_haveprotector { get { return _bsd_haveprotector; } set { _bsd_haveprotector = value; bsd_haveprotector_lable = value ? "Có" : "Không"; OnPropertyChanged(nameof(bsd_haveprotector)); } }

        private string _bsd_haveprotector_lable;
        public string bsd_haveprotector_lable { get { if (_bsd_haveprotector_lable == null) { _bsd_haveprotector_lable = "Không"; }; return _bsd_haveprotector_lable; } set { _bsd_haveprotector_lable = value; OnPropertyChanged(nameof(bsd_haveprotector_lable)); } }

        private string __bsd_protecter_value;
        public string _bsd_protecter_value { get { return __bsd_protecter_value; } set { __bsd_protecter_value = value; OnPropertyChanged(nameof(_bsd_protecter_value)); } }

        private string _bsd_protecter_label;
        public string bsd_protecter_label { get { return _bsd_protecter_label; } set { _bsd_protecter_label = value; OnPropertyChanged(nameof(bsd_protecter_label)); } }

        private DateTime? _bsd_dategrant;
        public DateTime? bsd_dategrant { get { return _bsd_dategrant; } set { _bsd_dategrant = value; OnPropertyChanged(nameof(bsd_dategrant)); } }

        private string _bsd_placeofissue;
        public string bsd_placeofissue { get { return _bsd_placeofissue; } set { _bsd_placeofissue = value; OnPropertyChanged(nameof(bsd_placeofissue)); } }

        private string _bsd_passport;
        public string bsd_passport { get { return _bsd_passport; } set { _bsd_passport = value; OnPropertyChanged(nameof(bsd_passport)); } }

        private DateTime? _bsd_issuedonpassport;
        public DateTime? bsd_issuedonpassport
        {
            get { return _bsd_issuedonpassport; }
            set
            {
                if (value.HasValue) { _bsd_issuedonpassport = value.Value.ToLocalTime(); } else { _bsd_issuedonpassport = null; }
                OnPropertyChanged(nameof(bsd_issuedonpassport));
            }
        }

        private string _bsd_placeofissuepassport;
        public string bsd_placeofissuepassport { get { return _bsd_placeofissuepassport; } set { _bsd_placeofissuepassport = value; OnPropertyChanged(nameof(bsd_placeofissuepassport)); } }

        private string _bsd_jobtitlevn;
        public string bsd_jobtitlevn { get { return _bsd_jobtitlevn; } set { _bsd_jobtitlevn = value; OnPropertyChanged(nameof(bsd_jobtitlevn)); } }

        private string _bsd_taxcode;
        public string bsd_taxcode { get { return _bsd_taxcode; } set { _bsd_taxcode = value; OnPropertyChanged(nameof(bsd_taxcode)); } }

        private string __parentcustomerid_value;
        public string _parentcustomerid_value { get { return __parentcustomerid_value; } set { __parentcustomerid_value = value; OnPropertyChanged(nameof(_parentcustomerid_value)); } }

        private string _parentcustomerid_label_contact;
        public string parentcustomerid_label_contact { get { return _parentcustomerid_label_contact; } set { _parentcustomerid_label_contact = value; if (value != null) { parentcustomerid_label_account = null; parentcustomerid_label = value; }; OnPropertyChanged(nameof(parentcustomerid_label_contact)); } }

        private string _parentcustomerid_label_account;
        public string parentcustomerid_label_account { get { return _parentcustomerid_label_account; } set { _parentcustomerid_label_account = value; if (value != null) { parentcustomerid_label_contact = null; parentcustomerid_label = value; }; OnPropertyChanged(nameof(parentcustomerid_label_account)); } }

        private string _parentcustomerid_label;
        public string parentcustomerid_label { get { return _parentcustomerid_label; } set { _parentcustomerid_label = value; OnPropertyChanged(nameof(parentcustomerid_label)); } }

        private string _bsd_email2;
        public string bsd_email2 { get { return _bsd_email2; } set { _bsd_email2 = value; OnPropertyChanged(nameof(bsd_email2)); } }

        private string _telephone1;
        public string telephone1 { get { return _telephone1; } set { _telephone1 = value; OnPropertyChanged(nameof(telephone1)); } }

        private string _fax;
        public string fax { get { return _fax; } set { _fax = value; OnPropertyChanged(nameof(fax)); } }

        private string __bsd_projectinterest_value;
        public string _bsd_projectinterest_value { get { return __bsd_projectinterest_value; } set { __bsd_projectinterest_value = value; OnPropertyChanged(nameof(_bsd_projectinterest_value)); } }

        private string _bsd_totaltransaction;
        public string bsd_totaltransaction { get { return _bsd_totaltransaction; } set { _bsd_totaltransaction = value; OnPropertyChanged(nameof(bsd_totaltransaction)); } }

        private string _bsd_customergroup;
        public string bsd_customergroup { get { return _bsd_customergroup; } set { _bsd_customergroup = value; OnPropertyChanged(nameof(bsd_customergroup)); } }

        private string _bsd_diachi;
        public string bsd_diachi { get => _bsd_diachi; set { _bsd_diachi = value; OnPropertyChanged(nameof(bsd_diachi)); } }

        private string _bsd_housenumberstreet;
        public string bsd_housenumberstreet { get { return _bsd_housenumberstreet; } set { _bsd_housenumberstreet = value; OnPropertyChanged(nameof(bsd_housenumberstreet)); } }

        private string _bsd_housenumber;
        public string bsd_housenumber { get { return _bsd_housenumber; } set { _bsd_housenumber = value; OnPropertyChanged(nameof(bsd_housenumber)); } }

        private Guid __bsd_country_value;
        public Guid _bsd_country_value { get { return __bsd_country_value; } set { __bsd_country_value = value; OnPropertyChanged(nameof(_bsd_country_value)); } }

        private string _bsd_country_label;
        public string bsd_country_label { get { return _bsd_country_label; } set { _bsd_country_label = value; OnPropertyChanged(nameof(bsd_country_label)); } }

        private string _bsd_country_en;
        public string bsd_country_en { get => _bsd_country_en; set { _bsd_country_en = value; OnPropertyChanged(nameof(bsd_country_en)); } }

        private Guid __bsd_province_value;
        public Guid _bsd_province_value { get { return __bsd_province_value; } set { __bsd_province_value = value; OnPropertyChanged(nameof(_bsd_province_value)); } }

        private string _bsd_province_label;
        public string bsd_province_label { get { return _bsd_province_label; } set { _bsd_province_label = value; OnPropertyChanged(nameof(bsd_province_label)); } }

        private string _bsd_province_en;
        public string bsd_province_en { get { return _bsd_province_en; } set { _bsd_province_en = value; OnPropertyChanged(nameof(bsd_province_en)); } }

        private Guid __bsd_district_value;
        public Guid _bsd_district_value { get { return __bsd_district_value; } set { __bsd_district_value = value; OnPropertyChanged(nameof(_bsd_district_value)); } }

        private string _bsd_district_label;
        public string bsd_district_label { get { return _bsd_district_label; } set { _bsd_district_label = value; OnPropertyChanged(nameof(bsd_district_label)); } }

        private string _bsd_district_en;
        public string bsd_district_en { get { return _bsd_district_en; } set { _bsd_district_en = value; OnPropertyChanged(nameof(bsd_district_en)); } }

        private string _bsd_permanentaddress1;
        public string bsd_permanentaddress1 { get { return _bsd_permanentaddress1; } set { _bsd_permanentaddress1 = value; OnPropertyChanged(nameof(bsd_permanentaddress1)); } }

        private string _bsd_diachithuongtru;
        public string bsd_diachithuongtru { get => _bsd_diachithuongtru; set { _bsd_diachithuongtru = value; OnPropertyChanged(nameof(bsd_diachithuongtru)); } }

        private Guid __bsd_permanentcountry_value;
        public Guid _bsd_permanentcountry_value { get { return __bsd_permanentcountry_value; } set { __bsd_permanentcountry_value = value; OnPropertyChanged(nameof(_bsd_permanentcountry_value)); } }

        private string _bsd_permanentcountry_label;
        public string bsd_permanentcountry_label { get { return _bsd_permanentcountry_label; } set { _bsd_permanentcountry_label = value; OnPropertyChanged(nameof(bsd_permanentcountry_label)); } }

        private string _bsd_permanentcountry_en;
        public string bsd_permanentcountry_en { get { return _bsd_permanentcountry_en; } set { _bsd_permanentcountry_en = value; OnPropertyChanged(nameof(bsd_permanentcountry_en)); } }

        private Guid __bsd_permanentprovince_value;
        public Guid _bsd_permanentprovince_value { get { return __bsd_permanentprovince_value; } set { __bsd_permanentprovince_value = value; OnPropertyChanged(nameof(_bsd_permanentprovince_value)); } }

        private string _bsd_permanentprovince_label;
        public string bsd_permanentprovince_label { get { return _bsd_permanentprovince_label; } set { _bsd_permanentprovince_label = value; OnPropertyChanged(nameof(bsd_permanentprovince_label)); } }

        private string _bsd_permanentprovince_en;
        public string bsd_permanentprovince_en { get { return _bsd_permanentprovince_en; } set { _bsd_permanentprovince_en = value; OnPropertyChanged(nameof(bsd_permanentprovince_en)); } }

        private Guid __bsd_permanentdistrict_value;
        public Guid _bsd_permanentdistrict_value { get { return __bsd_permanentdistrict_value; } set { __bsd_permanentdistrict_value = value; OnPropertyChanged(nameof(_bsd_permanentdistrict_value)); } }

        private string _bsd_permanentdistrict_label;
        public string bsd_permanentdistrict_label { get { return _bsd_permanentdistrict_label; } set { _bsd_permanentdistrict_label = value; OnPropertyChanged(nameof(bsd_permanentdistrict_label)); } }

        private string _bsd_permanentdistrict_en;
        public string bsd_permanentdistrict_en { get { return _bsd_permanentdistrict_en; } set { _bsd_permanentdistrict_en = value; OnPropertyChanged(nameof(bsd_permanentdistrict_en)); } }

        private string _bsd_permanentaddress;
        public string bsd_permanentaddress { get { return _bsd_permanentaddress; } set { _bsd_permanentaddress = value; OnPropertyChanged(nameof(bsd_permanentaddress)); } }

        private string _bsd_permanenthousenumber;
        public string bsd_permanenthousenumber { get { return _bsd_permanenthousenumber; } set { _bsd_permanenthousenumber = value; OnPropertyChanged(nameof(bsd_permanenthousenumber)); } }

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
        public bool bsd_tieuchi_hethonganninh { get => _bsd_tieuchi_hethonganninh; set { _bsd_tieuchi_hethonganninh = value; OnPropertyChanged(nameof(bsd_tieuchi_hethonganninh)); } }

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
        public bool bsd_tieuchi_ganbenhvien { get => _bsd_tieuchi_ganbenhvien; set { _bsd_tieuchi_ganbenhvien = value; OnPropertyChanged(nameof(bsd_tieuchi_ganbenhvien)); } }

        private bool _bsd_tieuchi_dientichcanho;
        public bool bsd_tieuchi_dientichcanho { get => _bsd_tieuchi_dientichcanho; set { _bsd_tieuchi_dientichcanho = value; OnPropertyChanged(nameof(bsd_tieuchi_dientichcanho)); } }

        private bool _bsd_tieuchi_thietkenoithatcanho;
        public bool bsd_tieuchi_thietkenoithatcanho { get => _bsd_tieuchi_thietkenoithatcanho; set { _bsd_tieuchi_thietkenoithatcanho = value; OnPropertyChanged(nameof(bsd_tieuchi_thietkenoithatcanho)); } }

        private bool _bsd_tieuchi_tangdepcanhodep;
        public bool bsd_tieuchi_tangdepcanhodep { get => _bsd_tieuchi_tangdepcanhodep; set { _bsd_tieuchi_tangdepcanhodep = value; OnPropertyChanged(nameof(bsd_tieuchi_tangdepcanhodep)); } }

        private bool _bsd_dientich_3060m2;
        public bool bsd_dientich_3060m2 { get => _bsd_dientich_3060m2; set { _bsd_dientich_3060m2 = value; OnPropertyChanged(nameof(bsd_dientich_3060m2)); } }

        private bool _bsd_dientich_6080m2;
        public bool bsd_dientich_6080m2 { get => _bsd_dientich_6080m2; set { _bsd_dientich_6080m2 = value; OnPropertyChanged(nameof(bsd_dientich_6080m2)); } }

        private bool _bsd_dientich_80100m2;
        public bool bsd_dientich_80100m2 { get => _bsd_dientich_80100m2; set { _bsd_dientich_80100m2 = value; OnPropertyChanged(nameof(bsd_dientich_80100m2)); } }

        private bool _bsd_dientich_100120m2;
        public bool bsd_dientich_100120m2 { get => _bsd_dientich_100120m2; set { _bsd_dientich_100120m2 = value; OnPropertyChanged(nameof(bsd_dientich_100120m2)); } }

        private bool _bsd_dientich_lonhon120m2;
        public bool bsd_dientich_lonhon120m2 { get => _bsd_dientich_lonhon120m2; set { _bsd_dientich_lonhon120m2 = value; OnPropertyChanged(nameof(bsd_dientich_lonhon120m2)); } }

        private bool _bsd_quantam_datnen;
        public bool bsd_quantam_datnen { get => _bsd_quantam_datnen; set { _bsd_quantam_datnen = value; OnPropertyChanged(nameof(bsd_quantam_datnen)); } }

        private bool _bsd_quantam_canho;
        public bool bsd_quantam_canho { get => _bsd_quantam_canho; set { _bsd_quantam_canho = value; OnPropertyChanged(nameof(bsd_quantam_canho)); } }

        private bool _bsd_quantam_bietthu;
        public bool bsd_quantam_bietthu { get => _bsd_quantam_bietthu; set { _bsd_quantam_bietthu = value; OnPropertyChanged(nameof(bsd_quantam_bietthu)); } }

        private bool _bsd_quantam_khuthuongmai;
        public bool bsd_quantam_khuthuongmai { get => _bsd_quantam_khuthuongmai; set { _bsd_quantam_khuthuongmai = value; OnPropertyChanged(nameof(bsd_quantam_khuthuongmai)); } }

        private bool _bsd_quantam_nhapho;
        public bool bsd_quantam_nhapho { get => _bsd_quantam_nhapho; set { _bsd_quantam_nhapho = value; OnPropertyChanged(nameof(bsd_quantam_nhapho)); } }

        private string _bsd_nmsinh;
        public string bsd_nmsinh { get { return _bsd_nmsinh; } set { _bsd_nmsinh = value; OnPropertyChanged(nameof(bsd_nmsinh)); } }

        private bool _bsd_loingysinh;
        public bool bsd_loingysinh { get { return _bsd_loingysinh; } set { _bsd_loingysinh = value; OnPropertyChanged(nameof(bsd_loingysinh)); } }

        private string _bsd_identitycard;
        public string bsd_identitycard { get { return _bsd_identitycard; } set { _bsd_identitycard = value; OnPropertyChanged(nameof(bsd_identitycard)); } }

        private DateTime? _bsd_identitycarddategrant;
        public DateTime? bsd_identitycarddategrant
        {
            get { return _bsd_identitycarddategrant; }
            set
            {
                if (value.HasValue) { _bsd_identitycarddategrant = value.Value.ToLocalTime(); } else { _bsd_identitycarddategrant = null; }
                OnPropertyChanged(nameof(bsd_identitycarddategrant));
            }
        }

        private string _bsd_placeofissueidentitycard;
        public string bsd_placeofissueidentitycard { get { return _bsd_placeofissueidentitycard; } set { _bsd_placeofissueidentitycard = value; OnPropertyChanged(nameof(bsd_placeofissueidentitycard)); } }

        private decimal? _bsd_birthdate;
        public decimal? bsd_birthdate { get { return _bsd_birthdate; } set { _bsd_birthdate = value; OnPropertyChanged(nameof(bsd_birthdate)); } }

        private decimal? _bsd_birthmonth;
        public decimal? bsd_birthmonth { get { return _bsd_birthmonth; } set { _bsd_birthmonth = value; OnPropertyChanged(nameof(bsd_birthmonth)); } }

        private decimal? _bsd_birthyear;
        public decimal? bsd_birthyear { get { return _bsd_birthyear; } set { _bsd_birthyear = value; OnPropertyChanged(nameof(bsd_birthyear)); } }

        //private string _bsd_mattruoccmnd;
        //public string bsd_mattruoccmnd { get => _bsd_mattruoccmnd; set { _bsd_mattruoccmnd = value; OnPropertyChanged(nameof(bsd_mattruoccmnd));
        //        byte[] arr = Convert.FromBase64String(value);
        //        bsd_mattruoccmnd_source = ImageSource.FromStream(() => new System.IO.MemoryStream(arr));
        //    } }

        //private string _bsd_matsaucmnd;
        //public string bsd_matsaucmnd { get => _bsd_matsaucmnd; set { _bsd_matsaucmnd = value; OnPropertyChanged(nameof(bsd_matsaucmnd));
        //    byte[] arr = Convert.FromBase64String(value);
        //    bsd_matsaucmnd_source = ImageSource.FromStream(() => new System.IO.MemoryStream(arr));
        //} }

        private string _bsd_mattruoccmnd_base64;
        public string bsd_mattruoccmnd_base64
        {
            get => _bsd_mattruoccmnd_base64; set
            {
                _bsd_mattruoccmnd_base64 = value; OnPropertyChanged(nameof(bsd_mattruoccmnd_base64));
                byte[] arr = Convert.FromBase64String(value);
                bsd_mattruoccmnd_source = ImageSource.FromStream(() => new System.IO.MemoryStream(arr));
            }
        }

        private string _bsd_matsaucmnd_base64;
        public string bsd_matsaucmnd_base64
        {
            get => _bsd_matsaucmnd_base64; set
            {
                _bsd_matsaucmnd_base64 = value; OnPropertyChanged(nameof(bsd_matsaucmnd_base64));
                byte[] arr = Convert.FromBase64String(value);
                bsd_matsaucmnd_source = ImageSource.FromStream(() => new System.IO.MemoryStream(arr));
            }
        }

        private ImageSource _bsd_mattruoccmnd_source;
        public ImageSource bsd_mattruoccmnd_source { get => _bsd_mattruoccmnd_source; set { _bsd_mattruoccmnd_source = value; OnPropertyChanged(nameof(bsd_mattruoccmnd_source)); } }

        private ImageSource _bsd_matsaucmnd_source;
        public ImageSource bsd_matsaucmnd_source { get => _bsd_matsaucmnd_source; set { _bsd_matsaucmnd_source = value; OnPropertyChanged(nameof(bsd_matsaucmnd_source)); } }

        private string _bsd_postalcode;
        public string bsd_postalcode { get { return _bsd_postalcode; } set { _bsd_postalcode = value; OnPropertyChanged(nameof(bsd_postalcode)); } }

        public Guid employee_id { get; set; }
        public string bsd_customercode { get; set; }
        public string statuscode_format { get { return statuscode != null ? CustomerStatusCodeData.GetCustomerStatusCodeById(statuscode.ToString())?.Name : null; } }
        public string statuscode_color { get { return statuscode != null ? CustomerStatusCodeData.GetCustomerStatusCodeById(statuscode.ToString())?.Background : null; } }

        private string _bsd_qrcode;
        public string bsd_qrcode { get => _bsd_qrcode; set { _bsd_qrcode = value; OnPropertyChanged(nameof(bsd_qrcode)); } }
        public Guid leadid_originated { get; set; }// lead originated id
    }
}