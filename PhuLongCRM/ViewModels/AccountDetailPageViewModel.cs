using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhuLongCRM.ViewModels
{
    public class AccountDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();
        public ObservableCollection<OptionSet> BusinessTypeOptions { get; set; }

        private string _businessTypes;
        public string BusinessTypes { get => _businessTypes; set { _businessTypes = value; OnPropertyChanged(nameof(BusinessTypes)); } }

        private OptionSet _localization;
        public OptionSet Localization { get => _localization; set { _localization = value; OnPropertyChanged(nameof(Localization)); } }

        private AccountFormModel _singleAccount;
        public AccountFormModel singleAccount { get => _singleAccount; set { _singleAccount = value; OnPropertyChanged(nameof(singleAccount)); } }

        private ContactFormModel _PrimaryContact;
        public ContactFormModel PrimaryContact { get => _PrimaryContact; set { if (_PrimaryContact != value) { this._PrimaryContact = value; OnPropertyChanged(nameof(PrimaryContact)); } } }        

        public ObservableCollection<QueueFormModel> list_thongtinqueing { get; set; }

        public ObservableCollection<ReservationListModel> list_thongtinquotation { get; set; } = new ObservableCollection<ReservationListModel>();
        public ObservableCollection<ContractModel> list_thongtincontract { get; set; }
        public ObservableCollection<ActivityListModel> Cares { get; set; }

        public ObservableCollection<MandatorySecondaryModel> _list_MandatorySecondary;
        public ObservableCollection<MandatorySecondaryModel> list_MandatorySecondary { get => _list_MandatorySecondary; set { _list_MandatorySecondary = value; OnPropertyChanged(nameof(list_MandatorySecondary)); } }

        public int PageQueueing { get; set; } = 1;
        public int PageQuotation { get; set; } = 1;
        public int PageContract { get; set; } = 1;
        public int PageCase { get; set; } = 1;
        public int PageMandatory { get; set; } = 1;

        private bool _showMoreQueueing;
        public bool ShowMoreQueueing { get => _showMoreQueueing; set { _showMoreQueueing = value; OnPropertyChanged(nameof(ShowMoreQueueing)); } }

        private bool _showMoreQuotation;
        public bool ShowMoreQuotation { get => _showMoreQuotation; set { _showMoreQuotation = value; OnPropertyChanged(nameof(ShowMoreQuotation)); } }

        private bool _showMoreContract;
        public bool ShowMoreContract { get => _showMoreContract; set { _showMoreContract = value; OnPropertyChanged(nameof(ShowMoreContract)); } }

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        private bool _showMoreMandatory;
        public bool ShowMoreMandatory { get => _showMoreMandatory; set { _showMoreMandatory = value; OnPropertyChanged(nameof(ShowMoreMandatory)); } }

        private MandatorySecondaryModel _mandatorySecondary;
        public MandatorySecondaryModel MandatorySecondary { get => _mandatorySecondary; set { _mandatorySecondary = value; OnPropertyChanged(nameof(MandatorySecondary)); } }

        private bool _isLoadMore;
        public bool isLoadMore { get => _isLoadMore; set { _isLoadMore = value; OnPropertyChanged(nameof(isLoadMore)); } }

        public string CodeAccount = LookUpMultipleTabs.CodeAccount;

        public AccountDetailPageViewModel()
        {
            BusinessTypeOptions = new ObservableCollection<OptionSet>();
            BusinessTypeOptions.Add(new OptionSet("100000000", Language.khach_hang));
            BusinessTypeOptions.Add(new OptionSet("100000001", Language.doi_tac));
            BusinessTypeOptions.Add(new OptionSet("100000002", Language.dai_ly));
            BusinessTypeOptions.Add(new OptionSet("100000003", Language.chu_dau_tu));

            list_thongtinqueing = new ObservableCollection<QueueFormModel>();
            list_thongtincontract = new ObservableCollection<ContractModel>();
            Cares = new ObservableCollection<ActivityListModel>();
            list_MandatorySecondary = new ObservableCollection<MandatorySecondaryModel>();
            MandatorySecondary = new MandatorySecondaryModel();
            isLoadMore = false;
        }

        //tab thong tin
        public async Task LoadOneAccount(string accountid)
        {
            singleAccount = new AccountFormModel();//Phu long k co field bsd_businesstypesys <attribute name='bsd_businesstypesys' />
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='primarycontactid' />
                                <attribute name='telephone1' />
                                <attribute name='bsd_rocnumber2' />
                                <attribute name='bsd_rocnumber1' />
                                <attribute name='websiteurl' />
                                <attribute name='bsd_vatregistrationnumber' />
                                <attribute name='bsd_incorporatedate' />
                                <attribute name='bsd_hotlines' />
                                <attribute name='bsd_generalledgercompanynumber' />
                                <attribute name='fax' />
                                <attribute name='emailaddress1' />
                                <attribute name='bsd_email2' />
                                <attribute name='bsd_placeofissue' />
                                <attribute name='bsd_issuedon' />
                                <attribute name='bsd_permanentaddress1' />
                                <attribute name='bsd_groupgstregisttationnumber' />
                                <attribute name='statuscode' />
                                <attribute name='ownerid' />
                                <attribute name='createdon' />
                                <attribute name='address1_composite' alias='bsd_address' />
                                <attribute name='bsd_companycode' />
                                <attribute name='bsd_registrationcode' />
                                <attribute name='bsd_accountnameother' />
                                <attribute name='bsd_localization' />
                                <attribute name='bsd_name' />
                                <attribute name='name' />
                                <attribute name='accountid' />
                                <attribute name='bsd_postalcode' />
                                <attribute name='bsd_housenumberstreet' />
                                <attribute name='bsd_businesstype' />
                                <attribute name='bsd_customercode' />
                                <attribute name='bsd_imageqrcode' />
                                <order attribute='createdon' descending='true' />
                                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='primarycontactname'/>
                                        <attribute name='mobilephone' alias='primarycontacttelephohne'/>
                                        <attribute name='bsd_contactaddress' alias='primarycontactaddress'/>
                                        <attribute name='bsd_permanentaddress1' alias='primarycontactpermanentaddress'/>
                                    </link-entity>
                                    <filter type='and'>
                                      <condition attribute='accountid' operator='eq' value='" + accountid + @"' />
                                    </filter>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' link-type='outer' alias='af' >
                                        <attribute name='new_name' alias='district_name' />                                       
                                    </link-entity>
                                     <link-entity name='new_province' from='new_provinceid' to='bsd_province' link-type='outer' alias='ag'>
                                        <attribute name='new_name' alias='province_name' />                                       
                                    </link-entity>
                                   <link-entity name='bsd_country' from='bsd_countryid' to='bsd_nation' link-type='outer' alias='as'>
                                        <attribute name='bsd_name' alias='country_name' />                                      
                                    </link-entity>
                              </entity>
                            </fetch>"; 
            //<link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' visible='false' link-type='outer' alias='a_cf81d7378befeb1194ef000d3a81fcba'>
            //                      <attribute name='bsd_employeeid' alias='employee_id'/>
            //                      <attribute name='bsd_name' alias='employee_name'/>
            //                    </link-entity> // bị lỗi khi load theo user crm nhưng k thấy sài
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }

            singleAccount = tmp;
            singleAccount.bsd_address = LoadAddress(tmp.bsd_address);
            singleAccount.bsd_permanentaddress1 = LoadAddress(tmp.bsd_permanentaddress1);
            PrimaryContact = new ContactFormModel()
            {
                contactid = tmp._primarycontactid_value
                ,
                bsd_fullname = tmp.primarycontactname
                ,
                mobilephone = tmp.primarycontacttelephohne
                ,
                bsd_contactaddress = LoadAddress(tmp.primarycontactaddress)
                ,
                bsd_permanentaddress1 = LoadAddress(tmp.primarycontactpermanentaddress)
            };
        }

        public void GetTypeById(string loai)
        {
            if (loai != string.Empty)
            {
                List<string> listType = new List<string>();
                var ids = singleAccount.bsd_businesstype.Split(',').ToList();
                foreach (var item in ids)
                {
                    OptionSet optionSet = BusinessTypeOptions.Single(x => x.Val == item);
                    listType.Add(optionSet.Label);
                }
                BusinessTypes = string.Join(", ", listType);
            }
            else
                BusinessTypes = Language.khach_hang;
        }

        private string LoadAddress(string address)
        {
            if (address == null)
            { return null; }
            var address_composite = address.Split('\n');
            return string.Join(", ", address_composite);
        }

        //tab giao dich
        public async Task LoadDSQueueingAccount(Guid accountid)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageQueueing}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' />
                                <attribute name='createdon' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='opportunityid' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_queueforproject' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='parentaccountid' operator='eq' uitype='account' value='{accountid}' />
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                </filter>  
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_edc3f143ba81e911a83b000d3a07be23'>
                                    <attribute name='bsd_name' alias='bsd_project_name'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='parentaccountid' visible='false' link-type='outer' alias='a_87ea9a00777ee911a83b000d3a07fbb4'>
                                    <attribute name='name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='parentcontactid' visible='false' link-type='outer' alias='a_8eea9a00777ee911a83b000d3a07fbb4'>
                                    <attribute name='bsd_fullname' alias='contact_name'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_5025d361ba81e911a83b000d3a07be23'>
                                    <attribute name='name' alias='bsd_units_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetchXml);
            if (result == null)
            {
                return;
            }
            var data = result.value;
            ShowMoreQueueing = data.Count < 3 ? false : true;
            foreach (var item in data)
            {
                QueueFormModel queue = new QueueFormModel();
                queue = item;
                if (!string.IsNullOrWhiteSpace(item.contact_name))
                {
                    queue.customer_name = item.contact_name;
                }
                else if (!string.IsNullOrWhiteSpace(item.account_name))
                {
                    queue.customer_name = item.account_name;
                }
                list_thongtinqueing.Add(item);
            }
        }

        public async Task LoadDSQuotationAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageQuotation}' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='quote'>
                                <attribute name='name' />
                                <attribute name='totalamount' />
                                <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_projectid' alias='bsd_project_id' />
                                <attribute name='quoteid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='customerid' operator='eq' value='{accountid}' />
                                  <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    <filter type='or'>
                                       <condition attribute='statuscode' operator='in'>
                                           <value>100000000</value>
                                            <value>100000001</value>
                                            <value>100000006</value>
                                            <value>861450001</value>
                                            <value>861450002</value>
                                            <value>4</value>                
                                            <value>3</value>
                                            <value>100000007</value>
                                       </condition>
                                       <filter type='and'>
                                           <condition attribute='statuscode' operator='in'>
                                               <value>100000009</value>
                                               <value>6</value>
                                           </condition>
                                           <condition attribute='bsd_quotationsigneddate' operator='not-null' />
                                       </filter>
                                     </filter>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                                    <attribute name='bsd_name' alias='bsd_project_name' />
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                                  <attribute name='name' alias='bsd_unitno_name' />
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                                  <attribute name='bsd_name' alias='purchaser_accountname' />
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                                  <attribute name='bsd_fullname' alias='purchaser_contactname' />
                                </link-entity>
                              </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", fetch);
            if (result == null || result.value.Any() == false) return;
            var data = result.value;
            ShowMoreQuotation = data.Count < 5 ? false : true;
            List<ReservationListModel> aa = new List<ReservationListModel>();
            foreach (var item in data)
            {
                list_thongtinquotation.Add(item);
            }
        }

        public async Task LoadDSContractAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageContract}' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='salesorder'>
                                    <attribute name='name' />
                                    <attribute name='customerid' />
                                    <attribute name='statuscode' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_unitnumber' alias='unit_id'/>
                                    <attribute name='bsd_project' alias='project_id'/>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <order attribute='bsd_project' descending='true' />
                                    <filter type='and'>                                      
                                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                        <condition attribute='customerid' operator='eq' value='{accountid}' />               
                                    </filter >
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='aa'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitnumber' link-type='outer' alias='ab'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ac'>
                                        <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ad'>
                                        <attribute name='bsd_fullname' alias='contact_name'/>
                                    </link-entity>
                                </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetch);
            if (result == null || result.value.Any() == false) return;
            var data = result.value;
            ShowMoreContract = data.Count < 3 ? false : true;
            foreach (var item in data)
            {
                list_thongtincontract.Add(item);
            }
        }

        public async Task LoadCase()
        {
            if (Cares == null)
                Cares = new ObservableCollection<ActivityListModel>();
            if (singleAccount == null || singleAccount.accountid == Guid.Empty) return;
            string fetch = $@"<fetch version='1.0' count='5' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='activitypointer'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' />
                                    <filter type='and'>
                                        <condition attribute='activitytypecode' operator='in'>
                                            <value>4212</value>
                                            <value>4210</value>
                                            <value>4201</value>
                                        </condition>
	                                    <filter type='or'>
                                            <condition entityname='meet' attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                            <condition entityname='task' attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                            <condition entityname='phonecall' attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                        </filter>
                                        <condition attribute='regardingobjectid' operator='eq' value='{singleAccount.accountid}' />
                                    </filter>
                                    <link-entity name='appointment' from='activityid' to='activityid' alias='meet' link-type='outer'>
                                        <attribute name='requiredattendees' />
                                    </link-entity>
                                    <link-entity name='task' from='activityid' to='activityid' alias='task' link-type='outer'>
                                        <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer'>
                                            <attribute name='bsd_name' alias='task_account_name'/>
                                        </link-entity>
                                        <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer'>
                                            <attribute name='fullname' alias='task_contact_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                            <attribute name='fullname' alias='task_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='phonecall' from='activityid' to='activityid' alias='phonecall' link-type='outer'>
                                        <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer'>
                                            <filter type='and'>
                                                <condition attribute='participationtypemask' operator='eq' value='2' />
                                            </filter>
                                            <link-entity name='contact' from='contactid' to='partyid' link-type='outer'>
                                                <attribute name='fullname' alias='phonecall_contact_name'/>
                                            </link-entity>
                                            <link-entity name='account' from='accountid' to='partyid' link-type='outer'>
                                                <attribute name='bsd_name' alias='phonecall_account_name'/>
                                            </link-entity>
                                            <link-entity name='lead' from='leadid' to='partyid' link-type='outer'>
                                                <attribute name='fullname' alias='phonecall_lead_name'/>
                                            </link-entity>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivityListModel>>("activitypointers", fetch);
            if (result != null && result.value.Count > 0)
            {
                foreach (var item in result.value)
                {
                    if (item.activitytypecode == "appointment")
                        item.customer = await MeetCustomerHelper.MeetCustomer(item.activityid);
                    else if (item.activitytypecode == "task")
                        item.customer = item.task__customer;
                    else if (item.activitytypecode == "phonecall")
                        item.customer = item.phonecall_customer;
                    Cares.Add(item);
                }
            }
            ShowMoreCase = Cares.Count < (5 * PageCase) ? false : true;
        }

        // tab nguoi uy quyyen
        public async Task Load_List_Mandatory_Secondary(string accountid)
        {
            string fetchxml = $@"<fetch version='1.0' count='10' page='{PageMandatory}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_mandatorysecondary'>
                                    <attribute name='bsd_mandatorysecondaryid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='statuscode' />
                                    <attribute name='ownerid' />
                                    <attribute name='bsd_jobtitlevn' />
                                    <attribute name='bsd_jobtitleen' />
                                    <attribute name='bsd_effectivedateto' />
                                    <attribute name='bsd_effectivedatefrom' />
                                    <attribute name='bsd_descriptionsen' />
                                    <attribute name='bsd_descriptionsvn' />
                                    <attribute name='bsd_developeraccount' />
                                    <attribute name='bsd_contact' />
                                    <order attribute='statuscode' descending='false' />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='contact' from='contactid' to='bsd_contact' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='bsd_contact_name'/>
                                        <attribute name='mobilephone' alias='bsd_contacmobilephone'/>
                                        <attribute name='bsd_contactaddress' alias='bsd_contactaddress'/>
                                    </link-entity>         
                                    <link-entity name='account' from='accountid' to='bsd_developeraccount' link-type='inner' alias='aa'>
                                        <filter type='and'>
                                            <condition attribute='bsd_mandatorysecondary' operator='eq' value='1' />
                                        </filter>
                                        <attribute name='bsd_name' alias='bsd_developeraccount_name' />
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='bsd_developeraccount' operator='eq' value='{accountid}' />
                                        <condition attribute='statuscode' operator='eq' value='100000000' />
                                    </filter>
                                  </entity>
                                </fetch>"; // 100000000 == Applying
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MandatorySecondaryModel>>("bsd_mandatorysecondaries", fetchxml);
            if (result == null || result.value.Count == 0) return;
            foreach (var x in result.value)
            {
                list_MandatorySecondary.Add(x);
            }
        }

        public async Task<bool> DeleteMandatory_Secondary(MandatorySecondaryModel Mandatory)
        {
            if (Mandatory != null)
            {
                var deleteResponse = await CrmHelper.DeleteRecord($"/bsd_mandatorysecondaries({Mandatory.bsd_mandatorysecondaryid})");
                if (deleteResponse.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // Save qrcode
        public async Task<bool> SaveQRCode(string qrCode)
        {
            string path = "/accounts(" + this.singleAccount.accountid + ")";
            object content = new
            {
                bsd_imageqrcode = qrCode,
            };

            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
                return true;
            else
                return false;
        }
    }
}
