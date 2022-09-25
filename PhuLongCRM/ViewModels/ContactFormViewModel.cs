using Newtonsoft.Json;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class ContactFormViewModel : BaseViewModel
    {
        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }

        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }

        private OptionSet _singleLocalization;
        public OptionSet singleLocalization { get => _singleLocalization; set { _singleLocalization = value; OnPropertyChanged(nameof(singleLocalization)); } }

        public ObservableCollection<LookUp> list_account_lookup { get; set; }

        private LookUp _account;
        public LookUp Account { get => _account; set { _account = value; OnPropertyChanged(nameof(Account)); } }

        public ObservableCollection<LookUp> list_lookup { get; set; }

        private List<OptionSet> _contactTypes;
        public List<OptionSet> ContactTypes { get => _contactTypes; set { _contactTypes = value; OnPropertyChanged(nameof(ContactTypes)); } }

        public OptionSet _contactType;
        public OptionSet ContactType { get => _contactType; set { _contactType = value; OnPropertyChanged(nameof(ContactType)); } }

        private List<OptionSet> _customerStatusReasons;
        public List<OptionSet> CustomerStatusReasons { get => _customerStatusReasons; set { _customerStatusReasons = value;OnPropertyChanged(nameof(CustomerStatusReasons)); } }

        private OptionSet _customerStatusReason;
        public OptionSet CustomerStatusReason { get => _customerStatusReason; set { _customerStatusReason = value;OnPropertyChanged(nameof(CustomerStatusReason)); } }      
        public ObservableCollection<OptionSet> GenderOptions { get; set; }
        public ObservableCollection<OptionSet> LocalizationOptions { get; set; }

        private string IMAGE_CMND_FOLDER = "Contact_CMND";
        private string frontImage_name;
        private string behindImage_name;
        public string checkCMND { get; set; }

        private bool _isOfficial;
        public bool IsOfficial { get => _isOfficial; set { _isOfficial = value; OnPropertyChanged(nameof(IsOfficial)); } }

        private AddressModel _address1;
        public AddressModel Address1 { get => _address1; set { _address1 = value; OnPropertyChanged(nameof(Address1)); } }
        //Address2 // địa chỉ thường trú
        private AddressModel _address2;
        public AddressModel Address2 { get => _address2; set { _address2 = value; OnPropertyChanged(nameof(Address2)); } }

        private AddressModel _addressCopy;
        public AddressModel AddressCopy { get => _addressCopy; set { _addressCopy = value; OnPropertyChanged(nameof(AddressCopy)); } }
        public ContactFormViewModel()
        {
            singleContact = new ContactFormModel();
            IsOfficial = true;
            list_lookup = new ObservableCollection<LookUp>();
            list_account_lookup = new ObservableCollection<LookUp>();
            GenderOptions = new ObservableCollection<OptionSet>();
            LocalizationOptions = new ObservableCollection<OptionSet>();

            ContactTypes = ContactTypeData.ContactTypes();
        }

        public async Task LoadOneContact(String id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='fullname' />
                                <attribute name='statuscode' />
                                <attribute name='ownerid' />
                                <attribute name='mobilephone' />
                                <attribute name='jobtitle' />
                                <attribute name='bsd_identitycardnumber' />
                                <attribute name='gendercode' />
                                <attribute name='emailaddress1' />
                                <attribute name='createdon' />
                                <attribute name='birthdate' />
                                <attribute name='address1_composite' />
                                <attribute name='bsd_fullname' />
                                <attribute name='contactid' />
                                <attribute name='telephone1' />
                                <attribute name='parentcustomerid' />
                                <attribute name='bsd_province' alias='_bsd_province_value'/>
                                <attribute name='bsd_placeofissuepassport' />
                                <attribute name='bsd_placeofissue' />
                                <attribute name='bsd_permanentprovince' alias='_bsd_permanentprovince_value'/>
                                <attribute name='bsd_permanentdistrict' alias='_bsd_permanentdistrict_value'/>
                                <attribute name='bsd_permanentcountry' alias='_bsd_permanentcountry_value'/>
                                <attribute name='bsd_permanentaddress1' />
                                <attribute name='bsd_permanentaddress' />
                                <attribute name='bsd_passport' />
                                <attribute name='bsd_localization' />
                                <attribute name='bsd_jobtitlevn' />
                                <attribute name='bsd_issuedonpassport' />
                                <attribute name='bsd_housenumberstreet' />
                                <attribute name='bsd_district' alias='_bsd_district_value'/>
                                <attribute name='bsd_dategrant' />
                                <attribute name='bsd_country' alias='_bsd_country_value'/>
                                <attribute name='bsd_postalcode' />
                                <attribute name='bsd_contactaddress' />
                                <attribute name='bsd_identitycard' />
                                <attribute name='bsd_identitycarddategrant' />
                                <attribute name='bsd_placeofissueidentitycard' />
                                <attribute name='bsd_customercode' />
                                    <link-entity name='account' from='accountid' to='parentcustomerid' visible='false' link-type='outer' alias='aa'>
                                          <attribute name='accountid' alias='_parentcustomerid_value' />
                                          <attribute name='bsd_name' alias='parentcustomerid_label' />
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_country_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_country_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_province_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_province_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_district_label'/>                                      
                                        <attribute name='bsd_nameen'  alias='bsd_district_en'/>
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_permanentcountry' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_permanentcountry_label'/>                                      
                                        <attribute name='bsd_nameen'  alias='bsd_permanentcountry_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_permanentprovince' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_permanentprovince_label'/>                                        
                                        <attribute name='bsd_nameen'  alias='bsd_permanentprovince_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_permanentdistrict' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_permanentdistrict_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_permanentdistrict_en'/>
                                    </link-entity>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                     <condition attribute='contactid' operator='eq' value='" + id + @"' />
                                    </filter>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result == null || result.value == null)
            {
                return;
            }

            var tmp = result.value.FirstOrDefault();
            tmp.bsd_fullname = tmp.fullname;
            this.singleContact = tmp;

            checkCMND = tmp.bsd_identitycardnumber;

            if (singleContact.statuscode == "100000000")
                IsOfficial = false;
            else
                IsOfficial = true;

            Address1 = new AddressModel
            {
                country_id = singleContact._bsd_country_value,
                country_name = !string.IsNullOrWhiteSpace(singleContact.bsd_country_en) && UserLogged.Language == "en" ? singleContact.bsd_country_en : singleContact.bsd_country_label,
                country_name_en = singleContact.bsd_country_en,
                province_id = singleContact._bsd_province_value,
                province_name = !string.IsNullOrWhiteSpace(singleContact.bsd_province_en) && UserLogged.Language == "en" ? singleContact.bsd_province_en : singleContact.bsd_province_label,
                province_name_en = singleContact.bsd_province_en,
                district_id = singleContact._bsd_district_value,
                district_name = !string.IsNullOrWhiteSpace(singleContact.bsd_district_en) && UserLogged.Language == "en" ? singleContact.bsd_district_en : singleContact.bsd_district_label,
                district_name_en = singleContact.bsd_district_en,
                address = singleContact.bsd_contactaddress,
                lineaddress = singleContact.bsd_housenumberstreet
            };

            Address2 = new AddressModel
            {
                country_id = singleContact._bsd_permanentcountry_value,
                country_name = !string.IsNullOrWhiteSpace(singleContact.bsd_permanentcountry_en) && UserLogged.Language == "en" ? singleContact.bsd_permanentcountry_en : singleContact.bsd_permanentcountry_label,
                country_name_en = singleContact.bsd_permanentcountry_en,
                province_id = singleContact._bsd_permanentprovince_value,
                province_name = !string.IsNullOrWhiteSpace(singleContact.bsd_permanentprovince_en) && UserLogged.Language == "en" ? singleContact.bsd_permanentprovince_en : singleContact.bsd_permanentprovince_label,
                province_name_en = singleContact.bsd_permanentprovince_en,
                district_id = singleContact._bsd_permanentdistrict_value,
                district_name = !string.IsNullOrWhiteSpace(singleContact.bsd_permanentdistrict_en) && UserLogged.Language == "en" ? singleContact.bsd_permanentdistrict_en : singleContact.bsd_permanentdistrict_label,
                district_name_en = singleContact.bsd_permanentdistrict_en,
                address = singleContact.bsd_permanentaddress1,
                lineaddress = singleContact.bsd_permanentaddress
            };
            await GetImageCMND();        
        }

        public async Task<Boolean> updateContact(ContactFormModel contact)
        {
            string path = "/contacts(" + contact.contactid + ")";
            var content = await this.getContent(contact);

            CrmApiResponse result = await CrmHelper.PatchData(path, content);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Guid> createContact(ContactFormModel contact)
        {
            string path = "/contacts";
            contact.contactid = Guid.NewGuid();
            var content = await this.getContent(contact);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return contact.contactid;
            }
            else
            {
                return new Guid();
            }

        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        {
            var result = await CrmHelper.SetNullLookupField("contacts", contactId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent(ContactFormModel contact)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["contactid"] = contact.contactid;
            data["lastname"] = contact.bsd_fullname;
            data["firstname"] = "";
            data["bsd_fullname"] = contact.bsd_fullname;
            data["emailaddress1"] = contact.emailaddress1;
            data["birthdate"] = contact.birthdate.HasValue ? (DateTime.Parse(contact.birthdate.ToString()).ToUniversalTime()).ToString("yyyy-MM-dd") : null;
            data["mobilephone"] = contact.mobilephone;//.Contains("-") ? contact.mobilephone.Replace("+","").Replace("-",""): contact.mobilephone;
            data["gendercode"] = contact.gendercode;
            if (checkCMND != contact.bsd_identitycardnumber)
            {
                data["bsd_identitycardnumber"] = contact.bsd_identitycardnumber;
            }
            data["bsd_localization"] = contact.bsd_localization;
            data["bsd_dategrant"] = contact.bsd_dategrant.HasValue ? (DateTime.Parse(contact.bsd_dategrant.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;
            data["bsd_placeofissue"] = contact.bsd_placeofissue;
            if (!string.IsNullOrWhiteSpace(contact.bsd_passport))
            {
                data["bsd_passport"] = contact.bsd_passport;
            }
            data["bsd_issuedonpassport"] = contact.bsd_issuedonpassport.HasValue ? (DateTime.Parse(contact.bsd_issuedonpassport.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
            data["bsd_jobtitlevn"] = contact.bsd_jobtitlevn;
            data["telephone1"] = string.IsNullOrWhiteSpace(contact.telephone1)? "+84": contact.telephone1;//!string.IsNullOrWhiteSpace(contact.telephone1) && contact.telephone1.Contains("-") ? contact.telephone1.Replace("+", "").Replace("-", "") : string.IsNullOrWhiteSpace(contact.telephone1)? "+84": contact.telephone1
            data["statuscode"] = this.CustomerStatusReason?.Val;
            data["bsd_identitycard"] = contact.bsd_identitycard;
            data["bsd_identitycarddategrant"] = contact.bsd_identitycarddategrant.HasValue ? (DateTime.Parse(contact.bsd_identitycarddategrant.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["bsd_placeofissueidentitycard"] = contact.bsd_placeofissueidentitycard;
            //  data["bsd_housenumberstreet"] = contact.bsd_housenumberstreet;
            //  data["bsd_contactaddress"] = contact.bsd_contactaddress;
            //  data["bsd_diachi"] = contact.bsd_diachi;
            ////  data["bsd_postalcode"] = contact.bsd_postalcode;
            //  data["bsd_housenumber"] = contact.bsd_housenumberstreet;

            //  data["bsd_permanentaddress1"] = contact.bsd_permanentaddress1;
            //  data["bsd_diachithuongtru"] = contact.bsd_diachithuongtru;
            //  data["bsd_permanenthousenumber"] = contact.bsd_permanentaddress;
            //  data["bsd_permanentaddress"] = contact.bsd_permanentaddress;

            data["bsd_type"] = this.ContactType.Val;

            if(Address1 != null)
            {
                if (!string.IsNullOrWhiteSpace(Address1.lineaddress))
                    data["bsd_housenumberstreet"] = Address1.lineaddress;
                if (!string.IsNullOrWhiteSpace(Address1.lineaddress_en))
                    data["bsd_housenumber"] = Address1.lineaddress_en;
                if (!string.IsNullOrWhiteSpace(Address1.address))
                    data["bsd_contactaddress"] = Address1.address;
                if (!string.IsNullOrWhiteSpace(Address1.address_en))
                    data["bsd_diachi"] = Address1.address_en;
            }

            if (Address2 != null)
            {
                if (!string.IsNullOrWhiteSpace(Address2.lineaddress))
                    data["bsd_permanentaddress"] = Address2.lineaddress;
                if (!string.IsNullOrWhiteSpace(Address2.lineaddress_en))
                    data["bsd_permanenthousenumber"] = Address2.lineaddress_en;
                if (!string.IsNullOrWhiteSpace(Address2.address))
                    data["bsd_permanentaddress1"] = Address2.address;
                if (!string.IsNullOrWhiteSpace(Address2.address_en))
                    data["bsd_diachithuongtru"] = Address2.address_en;
            }

            if (contact._parentcustomerid_value == null)
            {
                await DeletLookup("parentcustomerid_account", contact.contactid);
            }
            else
            {
                data["parentcustomerid_account@odata.bind"] = "/accounts(" + contact._parentcustomerid_value + ")"; /////Lookup Field

            }

            if (Address1 == null || Address1.country_id == Guid.Empty)
            {
                await DeletLookup("bsd_country", contact.contactid);
            }
            else
            {
                data["bsd_country@odata.bind"] = "/bsd_countries(" + Address1.country_id + ")"; /////Lookup Field
            }

            if (Address1 == null || Address1.province_id == Guid.Empty)
            {
                await DeletLookup("bsd_province", contact.contactid);
            }
            else
            {
                data["bsd_province@odata.bind"] = "/new_provinces(" + Address1.province_id + ")"; /////Lookup Field
            }

            if (Address1 == null || Address1.district_id == Guid.Empty)
            {
                await DeletLookup("bsd_district", contact.contactid);
            }
            else
            {
                data["bsd_district@odata.bind"] = "/new_districts(" + Address1.district_id + ")"; /////Lookup Field
            }

            if (Address2 == null || Address2.country_id == Guid.Empty)
            {
                await DeletLookup("bsd_permanentcountry", contact.contactid);
            }
            else
            {
                data["bsd_permanentcountry@odata.bind"] = "/bsd_countries(" + Address2.country_id + ")"; /////Lookup Field
            }

            if (Address2 == null || Address2.province_id == Guid.Empty)
            {
                await DeletLookup("bsd_permanentprovince", contact.contactid);
            }
            else
            {
                data["bsd_permanentprovince@odata.bind"] = "/new_provinces(" + Address2.province_id + ")"; /////Lookup Field
            }

            if (Address2 == null || Address2.district_id == Guid.Empty)
            {
                await DeletLookup("bsd_permanentdistrict", contact.contactid);
            }
            else
            {
                data["bsd_permanentdistrict@odata.bind"] = "/new_districts(" + Address2.district_id + ")"; /////Lookup Field
            }
            if (UserLogged.Id != null && !UserLogged.IsLoginByUserCRM)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty && !UserLogged.IsLoginByUserCRM)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        public async Task LoadAccountsLookup()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='account'>
                                        <attribute name='name' alias='Name'/>
                                        <attribute name='accountid' alias='Id'/>
                                        <filter type='and'>
                                          <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='" + UserLogged.Id + @"' />
                                        </filter>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("accounts", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                x.Detail = "Account";
                list_account_lookup.Add(x);
            }
        }

        public async Task<bool> CheckCMND(string identitycardnumber, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='bsd_identitycardnumber' operator='eq' value='" + identitycardnumber + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckPassport(string bsd_passport, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='bsd_passport' operator='eq' value='" + bsd_passport + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckCCCD(string idcard, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='bsd_idcard' operator='eq' value='" + idcard + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckEmail(string email, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='emailaddress1' operator='eq' value='" + email + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task PostCMND()
        {
            GetTokenResponse getTokenResponse = await LoginHelper.getSharePointToken();
            var client = BsdHttpClient.Instance();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);

            var frontImage_name = singleContact.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_front.jpg";
            var behindImage_name = singleContact.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_behind.jpg";
            var folder = StringFormatHelper.ReplaceNameContact(singleContact.bsd_fullname) + "_" + singleContact.contactid.ToString().Replace("-", "").ToUpper();

            var url_front = $"https://graph.microsoft.com/v1.0/drives/{Config.OrgConfig.Graph_ContactID}/root:/{folder}/{frontImage_name}:/content";
            if (singleContact.bsd_mattruoccmnd_base64 != null)
            {
                byte[] arrByteFront = Convert.FromBase64String(singleContact.bsd_mattruoccmnd_base64);
                HttpContent content = new ByteArrayContent(arrByteFront);
                using (var response1 = client.PutAsync(url_front, content))
                {
                    if (response1.Result != null)
                    {
                    }
                }
            }

            var url_behind = $"https://graph.microsoft.com/v1.0/drives/{Config.OrgConfig.Graph_ContactID}/root:/{folder}/{behindImage_name}:/content";
            if (singleContact.bsd_matsaucmnd_base64 != null)
            {
                byte[] arrByteFront = Convert.FromBase64String(singleContact.bsd_matsaucmnd_base64);
                HttpContent content = new ByteArrayContent(arrByteFront);
                using (var response1 = client.PutAsync(url_behind, content))
                {
                    if (response1.Result != null)
                    {
                    }
                }
            }
        }
        public async Task GetImageCMND()
        {
            if (this.singleContact.contactid != Guid.Empty)
            {
                var frontImage_name = this.singleContact.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_front.jpg";
                var behindImage_name = this.singleContact.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_behind.jpg";

                GetTokenResponse getTokenResponse = await LoginHelper.getSharePointToken();
                var client = BsdHttpClient.Instance();
                string name_folder = StringFormatHelper.ReplaceNameContact(singleContact.bsd_fullname) + "_" + singleContact.contactid.ToString().Replace("-", "").ToUpper();
                string fileListUrl = $"https://graph.microsoft.com/v1.0/drives/{Config.OrgConfig.Graph_ContactID}/root:/{name_folder}:/children?$select=name,eTag";
                var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RetrieveMultipleApiResponse<SharePointGraphModel>>(body);

                    if (result == null || result.value.Any() == false)
                    {
                        return;
                    }
                    List<SharePointGraphModel> list = result.value;
                    foreach (var item in list)
                    {
                        if (item.name.Contains("_front.jpg"))
                        {
                            var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_ContactID}/items/{item.id}/driveItem/thumbnails");
                            if (urlVideo != null)
                            {
                                string url = urlVideo.value.SingleOrDefault().large.url;
                                singleContact.bsd_mattruoccmnd_source = url;
                            }
                        }
                        else if (item.name.Contains("_behind.jpg"))
                        {
                            var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_ContactID}/items/{item.id}/driveItem/thumbnails");
                            if (urlVideo != null)
                            {
                                string url = urlVideo.value.SingleOrDefault().large.url;
                                singleContact.bsd_matsaucmnd_source = url;
                            }
                        }
                    }
                }
            }
        }
    }
}