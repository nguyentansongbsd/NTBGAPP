using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<ActivitiModel> Activities { get; set; } = new ObservableCollection<ActivitiModel>();

        public ObservableCollection<ChartModel> AcceptanceTotalExpense { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> AcceptanceNumTotal { get; set; } = new ObservableCollection<ChartModel>();

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

        #region Hoat dong
        private StatusCodeModel _activityStatusCode;
        public StatusCodeModel ActivityStatusCode { get => _activityStatusCode; set { _activityStatusCode = value; OnPropertyChanged(nameof(ActivityStatusCode)); } }
        private string _activityType;
        public string ActivityType { get => _activityType; set { _activityType = value; OnPropertyChanged(nameof(ActivityType)); } }
        public bool _showGridButton;
        public bool ShowGridButton { get => _showGridButton; set { _showGridButton = value; OnPropertyChanged(nameof(ShowGridButton)); } }
        private DateTime? _scheduledStartTask;
        public DateTime? ScheduledStartTask { get => _scheduledStartTask; set { _scheduledStartTask = value; OnPropertyChanged(nameof(ScheduledStartTask)); } }
        private DateTime? _scheduledEndTask;
        public DateTime? ScheduledEndTask { get => _scheduledEndTask; set { _scheduledEndTask = value; OnPropertyChanged(nameof(ScheduledEndTask)); } }
        public string CodeCompleted = "completed";
        public string CodeCancel = "cancel";
        #endregion

        #region Nghiem thu
        private int _totalAcceptancing;
        public int TotalAcceptanceing { get => _totalAcceptancing; set { _totalAcceptancing = value; OnPropertyChanged(nameof(TotalAcceptanceing)); } }
        private int _totalAcceptanced;
        public int TotalAcceptanced { get => _totalAcceptanced; set { _totalAcceptanced = value; OnPropertyChanged(nameof(TotalAcceptanced)); } }
        #endregion

        #region Ban giao sp
        private int _totalUnitHandovering;
        public int TotalUnitHandovering { get => _totalUnitHandovering; set { _totalUnitHandovering = value; OnPropertyChanged(nameof(TotalUnitHandovering)); } }
        private int _totalUnitHandovered;
        public int TotalUnitHandovered { get => _totalUnitHandovered; set { _totalUnitHandovered = value; OnPropertyChanged(nameof(TotalUnitHandovered)); } }
        #endregion

        #region Ban giao GCN
        private int _totalPinkBookHandovering;
        public int TotalPinkBookHandovering { get => _totalPinkBookHandovering; set { _totalPinkBookHandovering = value; OnPropertyChanged(nameof(TotalPinkBookHandovering)); } }
        private int _totalPinkBookHandovered;
        public int TotalPinkBookHandovered { get => _totalPinkBookHandovered; set { _totalPinkBookHandovered = value; OnPropertyChanged(nameof(TotalPinkBookHandovered)); } }
        #endregion

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await RefreshDashboard();
            IsRefreshing = false;
        });

        public DashboardViewModel()
        {
        }

        public async Task LoadAcceptances()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name'/>
                                    <attribute name='statuscode'/>
                                    <attribute name='bsd_expense'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='in'>
                                        <value>100000000</value>
                                        <value>100000001</value>
                                        <value>1</value>
                                        <value>100000003</value>
                                      </condition>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_f1ec0b1f36f4eb1194ef00224856b174'>
                                      <attribute name='bsd_projectcode' alias='projectcode' />
                                      <attribute name='bsd_projectid' alias='projectid'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AcceptanceListModel>>("bsd_acceptances", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.TotalAcceptanceing = result.value.Where(x => x.statuscode == "100000000" || x.statuscode == "100000001" || x.statuscode == "1").Count(); // sts = Confirmed Information,Confirmed Acceptance,Active : nhung sts dang nghiem thu
            this.TotalAcceptanced = result.value.Where(x => x.statuscode == "100000003").Count();// sts = Closed : sts Da nghiem thu

            List<AcceptanceListModel> listGroupByProject = result.value.GroupBy(x => x.projectid).Select(y => y.First()).ToList();

            foreach (var item in listGroupByProject)
            {
                this.AcceptanceTotalExpense.Add(new ChartModel() { Category = item.projectcode, Value = TotalAMonth(result.value.Where(x => x.projectid == item.projectid).Select(x => x.bsd_expense).Sum()) });
                this.AcceptanceNumTotal.Add(new ChartModel() { Category = item.projectcode, Value = result.value.Where(x => x.projectid == item.projectid).Count() });
            }
        }

        public async Task LoadUnitHandovers()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_handover'>
                                    <attribute name='bsd_handoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='in'>
                                        <value>1</value>
                                        <value>100000002</value>
                                        <value>100000001</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitHandoversModel>>("bsd_handovers", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.TotalUnitHandovering = result.value.Where(x => x.statuscode == "1").Count(); //sts = Active : Dang ban giao sp
            this.TotalUnitHandovered = result.value.Where(x => x.statuscode == "100000001").Count(); //sts = Handover : Da ban giao sp
        }

        public async Task LoadPinkBookHandovers()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_pinkbookhandover'>
                                    <attribute name='bsd_pinkbookhandoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='in'>
                                        <value>1</value>
                                        <value>100000000</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PinkBookHandoversModel>>("bsd_pinkbookhandovers", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.TotalPinkBookHandovering = result.value.Where(x => x.statuscode == "1").Count();// sts = Active : Dang ban giao gcn
            this.TotalPinkBookHandovered = result.value.Where(x => x.statuscode == "100000000").Count();// sts = Handed over certificate : Da ban giao gcn
        }

        private double TotalAMonth(double total)
        {
            if (total > 0 && total.ToString().Length > 9)
            {
                var _currency = total.ToString().Substring(0, total.ToString().Length - 9);
                return double.Parse(_currency);
            }
            else
            {
                return 0;
            }
        }

        public async Task LoadTasks()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='task'>
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='activitytypecode' />
                                    <attribute name='createdon' />
                                    <order attribute='scheduledstart' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statecode' operator='eq' value='0' />
                                      <condition attribute='scheduledstart' operator='today' />
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_48f82b1a8ad844bd90d915e7b3c4f263'>
                                          <attribute name='fullname' alias='contact_name'/>
                                          <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9cdbdceab5ee4a8db875050d455757bd'>
                                          <attribute name='accountid' alias='account_id'/>
                                          <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_0a67d7c87cd1eb11bacc000d3a80021e'>
                                          <attribute name='leadid' alias='lead_id'/>
                                          <attribute name='lastname' alias='lead_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("tasks", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.scheduledstart = item.scheduledstart.ToLocalTime();
                item.scheduledend = item.scheduledend.ToLocalTime();
                if (!string.IsNullOrWhiteSpace(item.contact_name))
                {
                    item.customer = item.contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.account_name))
                {
                    item.customer = item.account_name;
                }
                if (!string.IsNullOrWhiteSpace(item.lead_name))
                {
                    item.customer = item.lead_name;
                }

                this.Activities.Add(item);
            }
        }

        public async Task LoadMettings()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='appointment'>
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='activitytypecode' />   
                                    <attribute name='createdon' />
                                    <order attribute='scheduledstart' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statecode' operator='in'>
                                            <value>0</value>
                                            <value>3</value>
                                        </condition>
                                      <condition attribute='scheduledstart' operator='today' />
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_48f82b1a8ad844bd90d915e7b3c4f263'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9cdbdceab5ee4a8db875050d455757bd'>
                                          <attribute name='accountid' alias='account_id'/>
                                          <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_0a67d7c87cd1eb11bacc000d3a80021e'>
                                          <attribute name='leadid' alias='lead_id'/>
                                          <attribute name='lastname' alias='lead_name'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='5' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_accounts_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("appointments", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                var meet = Activities.FirstOrDefault(x => x.activityid == item.activityid);
                if (meet != null)
                {
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        string new_customer = ", " + item.callto_contact_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_accounts_name))
                    {
                        string new_customer = ", " + item.callto_accounts_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        string new_customer = ", " + item.callto_lead_name;
                        meet.customer += new_customer;
                    }
                }
                else
                {
                    item.scheduledstart = item.scheduledstart.ToLocalTime();
                    item.scheduledend = item.scheduledend.ToLocalTime();
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        item.customer = item.callto_contact_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_accounts_name))
                    {
                        item.customer = item.callto_accounts_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        item.customer = item.callto_lead_name;
                    }
                    this.Activities.Add(item);
                }
            }
        }

        public async Task LoadPhoneCalls()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='phonecall'>
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='activitytypecode' />
                                    <attribute name='createdon' />
                                    <order attribute='scheduledstart' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statecode' operator='eq' value='0' />
                                      <condition attribute='scheduledstart' operator='today' />
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_48f82b1a8ad844bd90d915e7b3c4f263'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9cdbdceab5ee4a8db875050d455757bd'>
                                          <attribute name='accountid' alias='account_id'/>
                                          <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_0a67d7c87cd1eb11bacc000d3a80021e'>
                                          <attribute name='leadid' alias='lead_id'/>
                                          <attribute name='lastname' alias='lead_name'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_accounts_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("phonecalls", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.scheduledstart = item.scheduledstart.ToLocalTime();
                item.scheduledend = item.scheduledend.ToLocalTime();
                if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                {
                    item.customer = item.callto_contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_accounts_name))
                {
                    item.customer = item.callto_accounts_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                {
                    item.customer = item.callto_lead_name;
                }

                this.Activities.Add(item);
            }
        }

        public async Task RefreshDashboard()
        {
            this.Activities.Clear();
            this.AcceptanceTotalExpense.Clear();
            this.AcceptanceNumTotal.Clear();
            this.TotalAcceptanceing = 0;
            this.TotalAcceptanced = 0;
            this.TotalUnitHandovering = 0;
            this.TotalUnitHandovered = 0;
            this.TotalPinkBookHandovering = 0;
            this.TotalPinkBookHandovered = 0;

            await Task.WhenAll(
                 this.LoadAcceptances(),
                 this.LoadUnitHandovers(),
                 this.LoadPinkBookHandovers(),
                 this.LoadTasks(),
                 this.LoadMettings(),
                 this.LoadPhoneCalls()
                );
        }
    }

    public class CountChartModel
    {
        public string group { get; set; }
        public int count { get; set; }
    }
    public class CountChartCommissionModel : CountChartModel
    {
        public decimal totalamountpaid_sum { get; set; }
        public decimal totalamount_sum { get; set; }
        public string group_sts { get; set; }
    }
    public class DashboardChartModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal CommissionTotal { get; set; }
        public string CommissionStatus { get; set; }
        public string opportunity_count { get; set; }
        public string month { get; set; }
    }
    public class CommissionTransaction
    {
        public Guid bsd_commissiontransactionid { get; set; }
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }
        public decimal bsd_totalamountpaid { get; set; }
        public decimal bsd_totalamount { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        public int statuscode_calculator { get; set; }
    }
}
