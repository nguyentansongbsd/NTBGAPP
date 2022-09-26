using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class ActivityPopupContentViewViewModel : BaseViewModel
    {
        public PhoneCellModel _phoneCall;
        public PhoneCellModel PhoneCall { get => _phoneCall; set { _phoneCall = value; OnPropertyChanged(nameof(PhoneCall)); } }

        public TaskFormModel _task;
        public TaskFormModel Task { get => _task; set { _task = value; OnPropertyChanged(nameof(Task)); } }

        public MeetingModel _meet;
        public MeetingModel Meet { get => _meet; set { _meet = value; OnPropertyChanged(nameof(Meet)); } }

        public OptionSet _collectionType;
        public OptionSet CollectionType { get => _collectionType; set { _collectionType = value; OnPropertyChanged(nameof(CollectionType)); } }

        private StatusCodeModel _activityStatusCode;
        public StatusCodeModel ActivityStatusCode { get => _activityStatusCode; set { _activityStatusCode = value; OnPropertyChanged(nameof(ActivityStatusCode)); } }

        public bool _showGridButton;
        public bool ShowGridButton { get => _showGridButton; set { _showGridButton = value; OnPropertyChanged(nameof(ShowGridButton)); } }

        private string _activityType;
        public string ActivityType { get => _activityType; set { _activityType = value; OnPropertyChanged(nameof(ActivityType)); } }

        public string CodeCompleted = "completed";

        public string CodeCancel = "cancel";

        public string CodeAccount = "3";

        public string CodeContac = "2";

        public string CodeLead = "1";

        public List<OptionSet> MeetRequired { get; set; }

        public ActivityPopupContentViewViewModel()
        {

        }

        public async Task loadPhoneCall(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='phonecall'>
                <attribute name='subject' />
                <attribute name='statecode' />
                <attribute name='prioritycode' />
                <attribute name='scheduledend' alias='scheduledend' />
                <attribute name='createdby' />
                <attribute name='regardingobjectid' />
                <attribute name='activityid' />
                <attribute name='statuscode' />
                <attribute name='scheduledstart' alias='scheduledstart' />
                <attribute name='actualdurationminutes' />
                <attribute name='description' />
                <attribute name='activitytypecode' />
                <attribute name='phonenumber' />
                <order attribute='subject' descending='false' />
                <filter type='and'>
                    <condition attribute='activityid' operator='eq' uitype='phonecall' value='" + id + @"' />
                </filter>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9b4f4019bdc24dd79b1858c2d087a27d'>
                    <attribute name='accountid' alias='account_id' />                  
                    <attribute name='bsd_name' alias='account_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_66b6d0af970a40c9a0f42838936ea5ce'>
                    <attribute name='contactid' alias='contact_id' />                  
                    <attribute name='fullname' alias='contact_name'/>
                </link-entity>
                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_fb87dbfd8304e911a98b000d3aa2e890'>
                    <attribute name='leadid' alias='lead_id'/>                  
                    <attribute name='fullname' alias='lead_name'/>
                </link-entity>
                <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' link-type='outer' alias='ab'>
                    <attribute name='opportunityid' alias='queue_id'/>                  
                    <attribute name='name' alias='queue_name'/>
                </link-entity>
            </entity>
          </fetch>";
            //"<link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='outer' alias='aa'>
            //        <attribute name='bsd_name' alias='user_name'/>
            //        <attribute name='bsd_employeeid' alias='user_id'/>
            //    </link-entity>"

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhoneCellModel>>("phonecalls", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            PhoneCall = data;

            if (data.scheduledend != null && data.scheduledstart != null)
            {
                PhoneCall.scheduledend = data.scheduledend.Value.ToLocalTime();
                PhoneCall.scheduledstart = data.scheduledstart.Value.ToLocalTime();
            }

            if (PhoneCall.contact_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.contact_name
                };
            }
            else if (PhoneCall.account_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.account_name
                };
            }
            else if (PhoneCall.lead_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.lead_name
                };
            }
            else if (PhoneCall.queue_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.queue_name
                };
            }


            if (PhoneCall.statecode == 0 || PhoneCall.statecode == 3)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }
        public async Task loadFromTo(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' >
                                    <entity name='phonecall' >
                                        <attribute name='subject' />
                                        <attribute name='activityid' />
                                        <order attribute='subject' descending='false' />
                                        <filter type='and' >
                                            <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                        </filter>
                                        <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab' >
                                            <attribute name='partyid' alias='partyID'/>
                                            <attribute name='participationtypemask' alias='typemask' />
                                            <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount' >
                                                <attribute name='bsd_name' alias='account_name'/>
                                                <attribute name='accountid' alias='account_id'/>
                                            </link-entity>
                                            <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact' >
                                                <attribute name='fullname' alias='contact_name'/>
                                                <attribute name='contactid' alias='contact_id'/>
                                            </link-entity>
                                            <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead' >
                                                <attribute name='fullname' alias='lead_name'/>
                                                <attribute name='leadid' alias='lead_id'/>
                                            </link-entity>
                                            <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser' >
                                                <attribute name='fullname' alias='user_name'/>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("phonecalls", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.typemask == 1) // from call
                    {
                        // hiển thị thông tin là employee khi ở dưới app
                        if (!string.IsNullOrWhiteSpace(UserLogged.ContactName))
                            PhoneCall.call_from = UserLogged.ContactName;
                    }
                    else if (item.typemask == 2) // to call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            PhoneCall.call_to = item.contact_name;
                            PhoneCall.callto_contact_id = item.contact_id;
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            PhoneCall.call_to = item.account_name;
                            PhoneCall.callto_account_id = item.account_id;
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            PhoneCall.call_to = item.lead_name;
                            PhoneCall.callto_lead_id = item.lead_id;
                        }
                    }
                }
            }

        }

        public async Task<bool> UpdateStatusPhoneCall(string update)
        {
            if (update == CodeCompleted)
            {
                PhoneCall.statecode = 1;
                PhoneCall.statuscode = 2;
            }
            else if (update == CodeCancel)
            {
                PhoneCall.statecode = 2;
                PhoneCall.statuscode = 3;
            }

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = PhoneCall.statecode;
            data["statuscode"] = PhoneCall.statuscode;

            string path = "/phonecalls(" + PhoneCall.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task loadTask(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='task'>
                                <attribute name='subject' />
                                <attribute name='statecode' />
                                <attribute name='scheduledend' />
                                <attribute name='activityid' />
                                <attribute name='statuscode' />
                                <attribute name='scheduledstart' />
                                <attribute name='description' />
                                <order attribute='subject' descending='false' />
                                <filter type='and' >
                                    <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                </filter>
                                <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ah'>
    	                            <attribute name='accountid' alias='account_id' />                  
    	                            <attribute name='bsd_name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='ai'>
	                                <attribute name='contactid' alias='contact_id' />                  
                                    <attribute name='fullname' alias='contact_name'/>
                                </link-entity>
                                <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='aj'>
	                                <attribute name='leadid' alias='lead_id'/>                  
                                    <attribute name='fullname' alias='lead_name'/>
                                </link-entity>
                                <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' link-type='outer' alias='ab'>
                                    <attribute name='opportunityid' alias='queue_id'/>                  
                                    <attribute name='name' alias='queue_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaskFormModel>>("tasks", fetch);
            if (result == null || result.value == null) return;
            var data = result.value.FirstOrDefault();
            Task = data;
            if (data.scheduledend != null && data.scheduledstart != null)
            {
                Task.scheduledstart = data.scheduledstart.Value.ToLocalTime();
                Task.scheduledend = data.scheduledend.Value.ToLocalTime();
            } 

            if (Task.contact_id != Guid.Empty)
            {
                Task.Customer = new CustomerLookUp
                {
                    Name = Task.contact_name
                };
            }
            else if (Task.account_id != Guid.Empty)
            {
                Task.Customer = new CustomerLookUp
                {
                    Name = Task.account_name
                };
            }
            else if (Task.lead_id != Guid.Empty)
            {
                Task.Customer = new CustomerLookUp
                {
                    Name = Task.lead_name
                };
            }
            else if (Task.queue_id != Guid.Empty)
            {
                Task.Customer = new CustomerLookUp
                {
                    Name = Task.queue_name
                };
            }

            if (Task.statecode == 0 || Task.statecode == 3)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task<bool> UpdateStatusTask(string update)
        {
            if (update == CodeCompleted)
            {
                Task.statecode = 1;
            }
            else if (update == CodeCancel)
            {
                Task.statecode = 2;
            }

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = Task.statecode;

            string path = "/tasks(" + Task.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task loadMeet(Guid id)
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
            Meet = data;

            this.CollectionType = CollectionTypeData.GetCollectionTypeById(data.bsd_collectiontype);
            if (data.scheduledend != null && data.scheduledstart != null)
            {
                Meet.scheduledend = data.scheduledend.Value.ToLocalTime();
                Meet.scheduledstart = data.scheduledstart.Value.ToLocalTime();
            }

            if (Meet.contact_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.contact_name
                };
            }
            else if (Meet.account_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.account_name
                };
            }
            else if (Meet.lead_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.lead_name
                };
            }
            else if (Meet.queue_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.queue_name
                };
            }

            if (Meet.statecode == 0 || Meet.statecode == 3)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task loadFromToMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
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
                                        <attribute name='accountid' alias='account_id'/>
                                      </link-entity>
                                      <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                      </link-entity>
                                      <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead'>
                                        <attribute name='fullname' alias='lead_name'/>
                                        <attribute name='leadid' alias='lead_id'/>
                                      </link-entity>
                                      <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser'>
                                        <attribute name='fullname' alias='user_name'/>
                                      </link-entity>
                                    </link-entity>
                                </entity>
                              </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                MeetRequired = new List<OptionSet>();
                List<string> optional = new List<string>();
                foreach (var item in data)
                {
                    if (item.typemask == 5) // from call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            MeetRequired.Add(new OptionSet { Val = item.contact_id.ToString(), Label = item.contact_name, Title = CodeContac });
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            MeetRequired.Add(new OptionSet { Val = item.account_id.ToString(), Label = item.account_name, Title = CodeAccount });
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            MeetRequired.Add(new OptionSet { Val = item.lead_id.ToString(), Label = item.lead_name, Title = CodeLead });
                        }
                    }
                    else if (item.typemask == 6) // to call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            optional.Add(item.contact_name);
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            optional.Add(item.account_name);
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            optional.Add(item.lead_name);
                        }
                    }
                }
                Meet.optional = string.Join(", ", optional);
            }

        }

        public async Task<bool> UpdateStatusMeet(string update)
        {
            if (update == CodeCompleted)
            {
                Meet.statecode = 1;
            }
            else if (update == CodeCancel)
            {
                Meet.statecode = 2;
            }

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = Meet.statecode;

            string path = "/appointments(" + Meet.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
