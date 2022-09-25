using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;

namespace PhuLongCRM.ViewModels
{
    public class UserInfoPageViewModel : BaseViewModel
    {
        private ContactFormModel _contactModel;
        public ContactFormModel ContactModel { get => _contactModel; set { _contactModel = value;OnPropertyChanged(nameof(ContactModel)); } }

        public byte[] AvatarArr { get; set; }
        private string _avatar;
        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(nameof(Avatar)); } }

        private string _userName;
        public string UserName { get => _userName; set { _userName = value;OnPropertyChanged(nameof(UserName)); } }

        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(nameof(Address)); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }

        private string _oldPassword;
        public string OldPassword { get => _oldPassword; set { _oldPassword = value; OnPropertyChanged(nameof(OldPassword)); } }

        private string _newPassword;
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); } }

        private string _confirmNewPassword;
        public string ConfirmNewPassword { get => _confirmNewPassword; set { _confirmNewPassword = value; OnPropertyChanged(nameof(ConfirmNewPassword)); } }

        private string _phoneNumber;
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); } }

        private OptionSet _gender;
        public OptionSet Gender { get => _gender; set { _gender = value;OnPropertyChanged(nameof(Gender)); } }

        private List<OptionSet> _genders;
        public List<OptionSet> Genders { get => _genders; set { _genders = value; OnPropertyChanged(nameof(Genders)); } }

        private AddressModel _addressContact;
        public AddressModel AddressContact { get => _addressContact; set { _addressContact = value; OnPropertyChanged(nameof(AddressContact)); } }
        public UserInfoPageViewModel()
        {
            Password = UserLogged.Password;
            this.Genders = new List<OptionSet>() { new OptionSet("1", Language.nam), new OptionSet("2", Language.nu), new OptionSet("100000000", Language.khac) };
            this.Avatar = UserLogged.Avartar;
            this.UserName = UserLogged.User;
        }

        public async Task LoadContact()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='bsd_fullname' />
                                    <attribute name='mobilephone' />
                                    <attribute name='bsd_identitycardnumber' />
                                    <attribute name='gendercode' />
                                    <attribute name='emailaddress1' />
                                    <attribute name='createdon' />
                                    <attribute name='birthdate' />
                                    <attribute name='contactid' />
                                    <attribute name='bsd_postalcode' />
                                    <attribute name='bsd_housenumberstreet' />
                                    <attribute name='bsd_contactaddress' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.ContactId}'/>
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer' alias='a_8b5241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_countryid' alias='_bsd_country_value'/>
                                        <attribute name='bsd_countryname' alias='bsd_country_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_country_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer' alias='a_8fd440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_provinceid' alias='_bsd_province_value'/>
                                        <attribute name='new_name' alias='bsd_province_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_province_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer' alias='a_50d440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_districtid' alias='_bsd_district_value'/>
                                        <attribute name='new_name' alias='bsd_district_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_district_en'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ContactModel = result.value.SingleOrDefault();

            if (ContactModel.mobilephone != null && ContactModel.mobilephone.Contains("-"))
            {
                this.PhoneNumber = ContactModel.mobilephone.Split('-')[1].StartsWith("84") ? ContactModel.mobilephone.Split('-')[1].Replace("84", "+84-") : ContactModel.mobilephone;
            }
            else if (ContactModel.mobilephone != null && ContactModel.mobilephone.Contains("+84"))
            {
                this.PhoneNumber = ContactModel.mobilephone.Replace("+84", "+84-");
            }
            else if (ContactModel.mobilephone != null && ContactModel.mobilephone.StartsWith("84"))
            {
                this.PhoneNumber = ContactModel.mobilephone.Replace("84", "+84-");
            }
            else
            {
                this.PhoneNumber = ContactModel.mobilephone;
            }

            this.Gender = this.Genders.SingleOrDefault(x => x.Val == ContactModel.gendercode);
            AddressContact = new AddressModel
            {
                country_id = ContactModel._bsd_country_value,
                country_name = !string.IsNullOrWhiteSpace(ContactModel.bsd_country_en) && UserLogged.Language == "en" ? ContactModel.bsd_country_en : ContactModel.bsd_country_label,
                country_name_en = ContactModel.bsd_country_en,
                province_id = ContactModel._bsd_province_value,
                province_name = !string.IsNullOrWhiteSpace(ContactModel.bsd_province_en) && UserLogged.Language == "en" ? ContactModel.bsd_province_en : ContactModel.bsd_province_label,
                province_name_en = ContactModel.bsd_province_en,
                district_id = ContactModel._bsd_district_value,
                district_name = !string.IsNullOrWhiteSpace(ContactModel.bsd_district_en) && UserLogged.Language == "en" ? ContactModel.bsd_district_en : ContactModel.bsd_district_label,
                district_name_en = ContactModel.bsd_district_en,
                address = ContactModel.bsd_contactaddress,
                lineaddress = ContactModel.bsd_housenumberstreet
            };
        }

        public async Task<bool> ChangePassword()
        {
            string path = $"/bsd_employees({UserLogged.Id})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_password"] = this.ConfirmNewPassword;

            var content = data as object;
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChangeAvatar()
        {
            string path = $"/bsd_employees({UserLogged.Id})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_avatar"] = this.AvatarArr;
            
            var content = data as object;
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public async Task<bool> UpdateUserInfor()
        {
            string path = $"/contacts({UserLogged.ContactId})";
            var content = await GetContent();
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        {
            var result = await CrmHelper.SetNullLookupField("contacts", contactId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> GetContent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            //data["lastname"] = this.ContactModel.bsd_fullname;
            //data["bsd_fullname"] = this.ContactModel.bsd_fullname;
            data["emailaddress1"] = this.ContactModel.emailaddress1;
            data["mobilephone"] = PhoneNumber.Contains("+") ? PhoneNumber.Replace("+","").Replace("-",""): PhoneNumber;
            data["birthdate"] = (DateTime.Parse(this.ContactModel.birthdate.ToString()).ToLocalTime()).ToString("yyyy-MM-dd");
            data["gendercode"] = this.Gender?.Val;
            data["bsd_postalcode"] = this.ContactModel.bsd_postalcode;

            if (AddressContact != null && !string.IsNullOrWhiteSpace(AddressContact.lineaddress))
            {
                if(!string.IsNullOrWhiteSpace(AddressContact.address))
                    data["bsd_contactaddress"] = AddressContact.address;
                if (!string.IsNullOrWhiteSpace(AddressContact.lineaddress))
                    data["bsd_housenumberstreet"] = AddressContact.lineaddress;

                if (AddressContact.country_id != Guid.Empty)
                    data["bsd_country@odata.bind"] = "/bsd_countries(" + AddressContact.country_id + ")";
                else
                    await DeletLookup("bsd_country", ContactModel.contactid);

                if (AddressContact.province_id != Guid.Empty)
                    data["bsd_province@odata.bind"] = "/new_provinces(" + AddressContact.province_id + ")";
                else
                    await DeletLookup("bsd_province", ContactModel.contactid);

                if (AddressContact.district_id != Guid.Empty)
                    data["bsd_district@odata.bind"] = "/new_districts(" + AddressContact.district_id + ")";
                else
                    await DeletLookup("bsd_district", ContactModel.contactid);
            }           

            return data;
        }
    }
}
