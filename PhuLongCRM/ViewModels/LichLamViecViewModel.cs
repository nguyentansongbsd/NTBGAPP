using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class LichLamViecViewModel : BaseViewModel
    {
        public ObservableCollection<CalendarEvent> lstEvents { get; set; }
        private ObservableCollection<CalendarEvent> _selectedDateEvents;
        public ObservableCollection<CalendarEvent> selectedDateEvents { get=>_selectedDateEvents; set { _selectedDateEvents = value;OnPropertyChanged(nameof(selectedDateEvents)); } }
        private ObservableCollection<Grouping<DateTime, CalendarEvent>> _selectedDateEventsGrouped;
        public ObservableCollection<Grouping<DateTime, CalendarEvent>> selectedDateEventsGrouped { get =>_selectedDateEventsGrouped; set { _selectedDateEventsGrouped = value;OnPropertyChanged(nameof(selectedDateEventsGrouped)); } }

        public PhoneCellModel _phoneCall;
        public PhoneCellModel PhoneCall { get => _phoneCall; set { _phoneCall = value; OnPropertyChanged(nameof(PhoneCall)); } }
        public TaskFormModel _task;
        public TaskFormModel Task { get => _task; set { _task = value; OnPropertyChanged(nameof(Task)); } }
        public MeetingModel _meet;
        public MeetingModel Meet { get => _meet; set { _meet = value; OnPropertyChanged(nameof(Meet)); } }

        public bool _showGridButton;
        public bool ShowGridButton { get => _showGridButton; set { _showGridButton = value; OnPropertyChanged(nameof(ShowGridButton)); } }

        private StatusCodeModel _activityStatusCode;
        public StatusCodeModel ActivityStatusCode { get => _activityStatusCode; set { _activityStatusCode = value; OnPropertyChanged(nameof(ActivityStatusCode)); } }

        private string _activityType;
        public string ActivityType { get => _activityType; set { _activityType = value; OnPropertyChanged(nameof(ActivityType)); } }

        private DateTime? _scheduledStartTask;
        public DateTime? ScheduledStartTask { get => _scheduledStartTask; set { _scheduledStartTask = value; OnPropertyChanged(nameof(ScheduledStartTask)); } }

        private DateTime? _scheduledEndTask;
        public DateTime? ScheduledEndTask { get => _scheduledEndTask; set { _scheduledEndTask = value; OnPropertyChanged(nameof(ScheduledEndTask)); } }

        private DateTime? _selectedDate;
        public DateTime? selectedDate { get => _selectedDate; set { _selectedDate = value; DayLabel = ""; OnPropertyChanged(nameof(selectedDate)); } }

        public string CodeCompleted = "completed";
        public string CodeCancel = "cancel";

        private string _DayLabel;
        public string DayLabel
        {
            get
            {
                return _DayLabel;
            }
            set
            {
                if (this.selectedDate.HasValue)
                {
                    var date = selectedDate.Value;
                    var result = string.Format("{0}, {1} - {2}", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek),date.Day, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month));

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        result = result.ToUpper();
                    }

                    _DayLabel = result;
                }
                OnPropertyChanged(nameof(DayLabel));
            }
        }

        public string entity { get; set; }
        public string EntityName { get; set; }

        public LichLamViecViewModel()
        {
            PhoneCall = new PhoneCellModel();
            Task = new TaskFormModel();
            Meet = new MeetingModel();

            lstEvents = new ObservableCollection<CalendarEvent>();
            selectedDateEvents = new ObservableCollection<CalendarEvent>();
            selectedDateEventsGrouped = new ObservableCollection<Grouping<DateTime, CalendarEvent>>();

            selectedDate = DateTime.Today;
        }

        public void reset()
        {
            lstEvents.Clear();
            selectedDate = null;
            selectedDateEvents.Clear();
            selectedDateEventsGrouped.Clear();
        }

        public async Task loadAllActivities()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='{entity}'>
                                    <attribute name='activitytypecode' />
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart'/>
                                    <attribute name='scheduledend' />
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='isregularactivity' operator='eq' value='1' />
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer'>
                                        <attribute name='fullname' alias='regardingobjectid_label_contact'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer'>
                                        <attribute name='bsd_name' alias='regardingobjectid_label_account'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer'>
                                        <attribute name='fullname' alias='regardingobjectid_label_lead'/>
                                    </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListActivitiesAcc>>(EntityName, fetch);
            if (result == null || result.value.Count == 0) return;
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    lstEvents.Add(new CalendarEvent(item));
                }
            }
        }

        public void UpdateSelectedEvents(DateTime value)
        {
            this.selectedDateEvents.Clear();
            foreach (CalendarEvent item in this.lstEvents)
            {
                var date = value;
                var recurrenceRule = item.RecurrenceRule;
                if ((recurrenceRule == null && item.StartDate.CompareTo(date) >= 0 && item.StartDate.CompareTo(date.AddDays(1)) < 0) 
                    || (recurrenceRule == null && item.EndDate.CompareTo(date) >= 0 && item.EndDate.CompareTo(date.AddDays(1)) < 0) 
                    || (recurrenceRule == null && item.StartDate.CompareTo(date) < 0 && item.EndDate.CompareTo(date.AddDays(1)) >= 0) 
                    || (recurrenceRule != null && recurrenceRule.Pattern.GetOccurrences(date, date, date).Any()))
                {
                    this.selectedDateEvents.Add(item);
                }
            }
        }

        public void UpdateSelectedEventsForWeekView(DateTime value)
        {
            this.selectedDateEvents.Clear();
            this.selectedDateEventsGrouped.Clear();

            var dayOfWeek = (int)value.DayOfWeek;
            for(int i = 1; i < 8; i++)
            {
                var currentDayOfWeek = i;
                var balance = currentDayOfWeek - dayOfWeek;
                var currentDate = value.AddDays(balance).Date;
                var checkHasValue = false;

                foreach (CalendarEvent item in this.lstEvents)
                {
                    var date = currentDate;
                    var recurrenceRule = item.RecurrenceRule;
                    if ((recurrenceRule == null && item.StartDate.CompareTo(date) >= 0 && item.StartDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.EndDate.CompareTo(date) >= 0 && item.EndDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.StartDate.CompareTo(date) < 0 && item.EndDate.CompareTo(date.AddDays(1)) >= 0)
                        || (recurrenceRule != null && recurrenceRule.Pattern.GetOccurrences(date, date, date).Any()))
                    {
                        CalendarEvent newItem = new CalendarEvent(item.Activity) { dateForGroupingWeek = date };
                        this.selectedDateEvents.Add(newItem);
                        checkHasValue = true;
                    }
                }

                if (!checkHasValue)
                {
                    selectedDateEvents.Add(new CalendarEvent() { dateForGroupingWeek = currentDate, activitytype_label = Language.khong_co_hoat_dong });
                }

            }

            var sorted = from eventCalendar in selectedDateEvents
                         group eventCalendar by eventCalendar.dateForGroupingWeek.Value into dateGrouped
                         select new Grouping<DateTime, CalendarEvent>(dateGrouped.Key, dateGrouped);
            this.selectedDateEventsGrouped = new ObservableCollection<Grouping<DateTime, CalendarEvent>>(sorted);
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
            </entity>
          </fetch>";

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

            if (PhoneCall.statecode == 0)
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
                        PhoneCall.call_from = item.user_name;
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
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaskFormModel>>("tasks", fetch);
            if (result == null || result.value == null) return;
            Task = result.value.FirstOrDefault();

            this.ScheduledStartTask = Task.scheduledstart.Value.ToLocalTime();
            this.ScheduledEndTask = Task.scheduledend.Value.ToLocalTime();

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

            if (Task.statecode == 0)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task loadMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='appointment'>
                                <attribute name='subject' />
                                <attribute name='statecode' />
                                <attribute name='createdby' />
                                <attribute name='statuscode' />
                                <attribute name='requiredattendees' />
                                <attribute name='prioritycode' />
                                <attribute name='scheduledstart' />
                                <attribute name='scheduledend' />
                                <attribute name='scheduleddurationminutes' />
                                <attribute name='bsd_mmeetingformuploaded' />
                                <attribute name='optionalattendees' />
                                <attribute name='isalldayevent' />
                                <attribute name='location' />
                                <attribute name='activityid' />
                                <attribute name='description' />
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
                            </entity>
                          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MeetingModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            Meet = data;

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

            if (Meet.statecode == 0)
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
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                List<string> required = new List<string>();
                List<string> optional = new List<string>();
                foreach (var item in data)
                {
                    if (item.typemask == 5) // from call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            required.Add(item.contact_name);
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            required.Add(item.account_name);
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            required.Add(item.lead_name);
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
                Meet.required = string.Join(", ", required);
                Meet.optional = string.Join(", ", optional);
            }
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
