using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using PhuLongCRM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class MeetingViewModel : BaseViewModel
    {
        private MeetingModel _meetingModel;
        public MeetingModel MeetingModel { get => _meetingModel; set { if (_meetingModel != value) { _meetingModel = value; OnPropertyChanged(nameof(MeetingModel)); } } }

        private List<OptionSetFilter> _leadsLookUpRequired;
        public List<OptionSetFilter> LeadsLookUpRequired { get => _leadsLookUpRequired; set { _leadsLookUpRequired = value; OnPropertyChanged(nameof(LeadsLookUpRequired)); } }

        private List<OptionSetFilter> _contactsLookUpRequired;
        public List<OptionSetFilter> ContactsLookUpRequired { get => _contactsLookUpRequired; set { _contactsLookUpRequired = value; OnPropertyChanged(nameof(ContactsLookUpRequired)); } }

        private List<OptionSetFilter> _accountsLookUpRequired;
        public List<OptionSetFilter> AccountsLookUpRequired { get => _accountsLookUpRequired; set { _accountsLookUpRequired = value; OnPropertyChanged(nameof(AccountsLookUpRequired)); } }

        private List<List<OptionSetFilter>> _allsLookUpRequired;
        public List<List<OptionSetFilter>> AllsLookUpRequired { get => _allsLookUpRequired; set { _allsLookUpRequired = value; OnPropertyChanged(nameof(AllsLookUpRequired)); } }

        private List<OptionSetFilter> _leadsLookUpOptional;
        public List<OptionSetFilter> LeadsLookUpOptional { get => _leadsLookUpOptional; set { _leadsLookUpOptional = value; OnPropertyChanged(nameof(LeadsLookUpOptional)); } }

        private List<OptionSetFilter> _contactsLookUpOptional;
        public List<OptionSetFilter> ContactsLookUpOptional { get => _contactsLookUpOptional; set { _contactsLookUpOptional = value; OnPropertyChanged(nameof(ContactsLookUpOptional)); } }

        private List<OptionSetFilter> _accountsLookUpOptional;
        public List<OptionSetFilter> AccountsLookUpOptional { get => _accountsLookUpOptional; set { _accountsLookUpOptional = value; OnPropertyChanged(nameof(AccountsLookUpOptional)); } }

        private List<List<OptionSetFilter>> _allsLookUpOptional;
        public List<List<OptionSetFilter>> AllsLookUpOptional { get => _allsLookUpOptional; set { _allsLookUpOptional = value; OnPropertyChanged(nameof(AllsLookUpOptional)); } }

        public List<string> Tabs { get; set; }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private OptionSet _collectionType;
        public OptionSet CollectionType { get => _collectionType; set { _collectionType = value; OnPropertyChanged(nameof(CollectionType)); } }

        private List<OptionSetFilter> _required;
        public List<OptionSetFilter> Required { get => _required; set { _required = value; OnPropertyChanged(nameof(Required)); } }

        private List<OptionSetFilter> _optional;
        public List<OptionSetFilter> Optional { get => _optional; set { _optional = value; OnPropertyChanged(nameof(Optional)); } }

        private List<OptionSet> _collectionTypes;
        public List<OptionSet> CollectionTypes { get => _collectionTypes; set { _collectionTypes = value; OnPropertyChanged(nameof(CollectionTypes)); } }

        public string CodeAccount = LookUpMultipleTabs.CodeAccount;
        public string CodeContac = LookUpMultipleTabs.CodeContac;
        public string CodeLead = LookUpMultipleTabs.CodeLead;
        //public string CodeQueue = QueuesDetialPage.CodeQueue;

        public bool _showButton;
        public bool ShowButton { get => _showButton; set { _showButton = value; OnPropertyChanged(nameof(ShowButton)); } }

        public int PageLead = 1;
        public int PageContact = 1;
        public int PageAccount = 1;

        private OptionSet _customerMapping;
        public OptionSet CustomerMapping { get => _customerMapping; set { _customerMapping = value; OnPropertyChanged(nameof(CustomerMapping)); } }
        public MeetingViewModel()
        {
            MeetingModel = new MeetingModel();
            AllsLookUpRequired = new List<List<OptionSetFilter>>();
            AllsLookUpOptional = new List<List<OptionSetFilter>>();
            Tabs = new List<string>();
            ShowButton = true;
        }

        public async Task loadDataMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='appointment'>
                      <attribute name='subject' />
                      <attribute name='statecode' />
                      <attribute name='createdby' />
                      <attribute name='statuscode' />
                      <attribute name='scheduledstart' />
                      <attribute name='scheduledend' />
                      <attribute name='scheduleddurationminutes' />
                      <attribute name='isalldayevent' />
                      <attribute name='location' />
                      <attribute name='activityid' />
                      <attribute name='description' />
                      <attribute name='bsd_collectiontype' />
                      <order attribute='createdon' descending='true' />
                      <filter type='and'>
                          <condition attribute='activityid' operator='eq' uitype='appointment' value='" + id + @"' />
                      </filter>               
                      <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                        <attribute name='contactid' alias='contact_id' />                  
                        <attribute name='fullname' alias='contact_name'/>
                      </link-entity>
                      <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='accounts'>
                          <attribute name='accountid' alias='account_id' />                  
                          <attribute name='bsd_name' alias='account_name'/>
                      </link-entity>
                      <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='leads'>
                          <attribute name='leadid' alias='lead_id'/>                  
                          <attribute name='fullname' alias='lead_name'/>
                      </link-entity>
                    <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' link-type='outer' alias='ab'>
                        <attribute name='opportunityid' alias='queue_id'/>                  
                        <attribute name='name' alias='queue_name'/>
                    </link-entity>
                  </entity>
                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MeetingModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();

            MeetingModel.activityid = data.activityid;
            MeetingModel.subject = data.subject;
            MeetingModel.statecode = data.statecode;
            MeetingModel.statuscode = data.statuscode;
            if (data.scheduledend != null && data.scheduledstart != null)
            {
                MeetingModel.scheduledstart = data.scheduledstart.Value.ToLocalTime();
                MeetingModel.scheduledend = data.scheduledend.Value.ToLocalTime();
            }

            MeetingModel.description = data.description;
            MeetingModel.location = data.location;
            MeetingModel.isalldayevent = data.isalldayevent;
            MeetingModel.scheduleddurationminutes = data.scheduleddurationminutes;
            MeetingModel.bsd_collectiontype = data.bsd_collectiontype;

            if (data.contact_id != Guid.Empty)
            {
                Customer = new OptionSetFilter
                {
                    Title = CodeContac,
                    Val = data.contact_id.ToString(),
                    Label = data.contact_name
                };
            }
            else if (data.account_id != Guid.Empty)
            {
                Customer = new OptionSetFilter
                {
                    Title = CodeAccount,
                    Val = data.account_id.ToString(),
                    Label = data.account_name
                };
            }
            else if (data.lead_id != Guid.Empty)
            {
                Customer = new OptionSetFilter
                {
                    Title = CodeLead,
                    Val = data.lead_id.ToString(),
                    Label = data.lead_name
                };
            }
            else if (data.queue_id != Guid.Empty)
            {
                Customer = new OptionSetFilter
                {
                    //Title = CodeQueue,
                    Val = data.queue_id.ToString(),
                    Label = data.queue_name
                };
            }

            if (MeetingModel.statecode == 0 || MeetingModel.statecode == 3)
                ShowButton = true;
            else
                ShowButton = false;

            string xml_fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                  <entity name='appointment'>
                      <attribute name='subject' />
                      <attribute name='createdon' />
                      <attribute name='activityid' />
                      <order attribute='createdon' descending='false' />
                      <filter type='and'>
                          <condition attribute='activityid' operator='eq' value='" + id + @"' />
                      </filter>
                      <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab'>
                          <attribute name='partyid' alias='partyID'/>
                          <attribute name='participationtypemask' alias='typemask'/>
                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount'>
                          <attribute name='bsd_name' alias='account_name'/>
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact'>
                          <attribute name='fullname' alias='contact_name'/>
                        </link-entity>
                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead'>
                          <attribute name='fullname' alias='lead_name'/>
                        </link-entity>
                        <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser'>
                          <attribute name='fullname' alias='user_name'/>
                        </link-entity>
                      </link-entity>
                  </entity>
                </fetch>";
            var _result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", xml_fetch);
            if (_result == null || _result.value == null)
                return;
            var _data = _result.value;
            if (_data.Any())
            {
                List<OptionSetFilter> requiredIds = new List<OptionSetFilter>();
                List<OptionSetFilter> optionalIds = new List<OptionSetFilter>();
                foreach (var item in _data)
                {
                    if (item.typemask == 5)
                    {
                        requiredIds.Add(new OptionSetFilter { Val = item.partyID.ToString(), Label = item.cutomer_name, Title = item.title_code, Selected = true });
                    }
                    else if (item.typemask == 6)
                    {
                        optionalIds.Add(new OptionSetFilter { Val = item.partyID.ToString(), Label = item.cutomer_name, Title = item.title_code, Selected = true });
                    }
                }
                Optional = optionalIds;
                Required = requiredIds;
            }
        }

        public async Task<List<PartyModel>> loadDataParty(Guid id)
        {
            string xml_fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                  <entity name='appointment'>
                      <attribute name='subject' />
                      <attribute name='createdon' />
                      <attribute name='activityid' />
                      <order attribute='createdon' descending='false' />
                      <filter type='and'>
                          <condition attribute='activityid' operator='eq' value='" + id + @"' />
                      </filter>
                      <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab'>
                          <attribute name='partyid' alias='partyID'/>
                          <attribute name='participationtypemask' alias='typemask'/>
                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount'>
                          <attribute name='bsd_name' alias='account_name'/>
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact'>
                          <attribute name='fullname' alias='contact_name'/>
                        </link-entity>
                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead'>
                          <attribute name='fullname' alias='lead_name'/>
                        </link-entity>
                        <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser'>
                          <attribute name='fullname' alias='user_name'/>
                        </link-entity>
                      </link-entity>
                  </entity>
                </fetch>";
            var _result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", xml_fetch);
            if (_result == null || _result.value == null)
                return null;
            return _result.value;
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid id)
        {
            var result = await CrmHelper.SetNullLookupField("appointments", id, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = MeetingModel.activityid;
            data["subject"] = MeetingModel.subject;
            data["isalldayevent"] = MeetingModel.isalldayevent;
            data["location"] = MeetingModel.location;
            data["description"] = MeetingModel.description ?? "";
            data["scheduledstart"] = MeetingModel.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = MeetingModel.scheduledend.Value.ToUniversalTime();
            data["actualdurationminutes"] = MeetingModel.scheduleddurationminutes;
            data["statecode"] = MeetingModel.statecode;
            data["statuscode"] = MeetingModel.statuscode;
            data["bsd_collectiontype"] = this.CollectionType.Val;

            if (Customer != null && Customer.Selected == false) // selected = flase là customer từ page khác queue
            {
                if (Customer.Title == CodeLead)
                {
                    data["regardingobjectid_lead_appointment@odata.bind"] = "/leads(" + Customer.Val + ")";
                }
                else if (Customer.Title == CodeContac)
                {
                    data["regardingobjectid_contact_appointment@odata.bind"] = "/contacts(" + Customer.Val + ")";
                }
                else if (Customer.Title == CodeAccount)
                {
                    data["regardingobjectid_account_appointment@odata.bind"] = "/accounts(" + Customer.Val + ")";
                }
            }
            else
            {
                await DeletLookup("regardingobjectid_contact_appointment", MeetingModel.activityid);
                await DeletLookup("regardingobjectid_account_appointment", MeetingModel.activityid);
                await DeletLookup("regardingobjectid_lead_appointment", MeetingModel.activityid);
            }

            List<object> arrayMeeting = new List<object>();

            if (CustomerMapping != null)
            {
                IDictionary<string, object> item_required = new Dictionary<string, object>();
                if (CustomerMapping.Title == CodeContac)
                {
                    item_required["partyid_contact@odata.bind"] = "/contacts(" + CustomerMapping.Val + ")";
                    item_required["participationtypemask"] = 5;
                    arrayMeeting.Add(item_required);

                    data["regardingobjectid_contact_appointment@odata.bind"] = "/contacts(" + CustomerMapping.Val + ")";
                }
                else if (CustomerMapping.Title == CodeAccount)
                {
                    item_required["partyid_account@odata.bind"] = "/accounts(" + CustomerMapping.Val + ")";
                    item_required["participationtypemask"] = 5;
                    arrayMeeting.Add(item_required);

                    data["regardingobjectid_account_appointment@odata.bind"] = "/accounts(" + CustomerMapping.Val + ")";
                }
                else if (CustomerMapping.Title == CodeLead)
                {
                    item_required["partyid_lead@odata.bind"] = "/leads(" + CustomerMapping.Val + ")";
                    item_required["participationtypemask"] = 5;
                    arrayMeeting.Add(item_required);

                    data["regardingobjectid_lead_appointment@odata.bind"] = "/leads(" + CustomerMapping.Val + ")";
                }
                //else if (CustomerMapping.Title == CodeQueue)
                //{
                //    if (Customer != null && Customer.Selected == true) // selected = true là required từ queue
                //    {
                //        if (Customer.Title == CodeContac) // khách hàng từ queue k có lead
                //        {
                //            item_required["partyid_contact@odata.bind"] = "/contacts(" + Customer.Val + ")";
                //            item_required["participationtypemask"] = 5;
                //            arrayMeeting.Add(item_required);
                //        }
                //        else if (Customer.Title == CodeAccount)
                //        {
                //            item_required["partyid_account@odata.bind"] = "/accounts(" + Customer.Val + ")";
                //            item_required["participationtypemask"] = 5;
                //            arrayMeeting.Add(item_required);
                //        }
                //    }
                //    data["regardingobjectid_opportunity_appointment@odata.bind"] = "/opportunities(" + CustomerMapping.Val + ")";
                //}
            }
            if (Required != null)
            {
                foreach (var item in Required)
                {
                    IDictionary<string, object> item_required = new Dictionary<string, object>();
                    if (item.Title == CodeContac)
                    {
                        item_required["partyid_contact@odata.bind"] = "/contacts(" + item.Val + ")";
                        item_required["participationtypemask"] = 5;
                        arrayMeeting.Add(item_required);
                    }
                    else if (item.Title == CodeAccount)
                    {
                        item_required["partyid_account@odata.bind"] = "/accounts(" + item.Val + ")";
                        item_required["participationtypemask"] = 5;
                        arrayMeeting.Add(item_required);
                    }
                    else if (item.Title == CodeLead)
                    {
                        item_required["partyid_lead@odata.bind"] = "/leads(" + item.Val + ")";
                        item_required["participationtypemask"] = 5;
                        arrayMeeting.Add(item_required);
                    }
                    else if (item.Selected == true)
                    {
                        item_required["partyid_systemuser@odata.bind"] = "/systemusers(" + item.Val + ")";
                        item_required["participationtypemask"] = 5;
                        arrayMeeting.Add(item_required);
                    }
                }
            }
            if (Optional != null)
            {
                foreach (var item in Optional)
                {
                    IDictionary<string, object> item_optional = new Dictionary<string, object>();
                    if (item.Title == CodeContac)
                    {
                        item_optional["partyid_contact@odata.bind"] = "/contacts(" + item.Val + ")";
                        item_optional["participationtypemask"] = 6;
                        arrayMeeting.Add(item_optional);
                    }
                    else if (item.Title == CodeAccount)
                    {
                        item_optional["partyid_account@odata.bind"] = "/accounts(" + item.Val + ")";
                        item_optional["participationtypemask"] = 6;
                        arrayMeeting.Add(item_optional);
                    }
                    else if (item.Title == CodeLead)
                    {
                        item_optional["partyid_lead@odata.bind"] = "/leads(" + item.Val + ")";
                        item_optional["participationtypemask"] = 6;
                        arrayMeeting.Add(item_optional);
                    }
                    else if (item.Selected == true)
                    {
                        item_optional["partyid_systemuser@odata.bind"] = "/systemusers(" + item.Val + ")";
                        item_optional["participationtypemask"] = 6;
                        arrayMeeting.Add(item_optional);
                    }
                }
            }
            data["appointment_activity_parties"] = arrayMeeting;

            if (UserLogged.IsLoginByUserCRM == false && UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            if (UserLogged.IsLoginByUserCRM == false && UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee_Appointment@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            return data;
        }

        public async Task<bool> createMeeting()
        {
            MeetingModel.activityid = Guid.NewGuid();
            MeetingModel.statecode = 0;
            MeetingModel.statuscode = 1;
            var actualdurationminutes = Math.Round((MeetingModel.scheduledend.Value - MeetingModel.scheduledstart.Value).TotalMinutes);
            MeetingModel.scheduleddurationminutes = int.Parse(actualdurationminutes.ToString());
            string path = "/appointments";
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateMeeting(Guid meetingid)
        {
            var actualdurationminutes = Math.Round((MeetingModel.scheduledend.Value - MeetingModel.scheduledstart.Value).TotalMinutes);
            MeetingModel.scheduleddurationminutes = int.Parse(actualdurationminutes.ToString());
            string path = "/appointments(" + meetingid + ")";
            var content = await this.getContent();
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
    }
}
