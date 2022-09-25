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
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class ReservationListViewModel : ListViewBaseViewModel2<ReservationListModel>
    {
        public ObservableCollection<OptionSet> FiltersStatus { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<OptionSet> FiltersProject { get; set; } = new ObservableCollection<OptionSet>();

        public List<string> _filterStatus;
        public List<string> FilterStatus { get => _filterStatus; set { _filterStatus = value; OnPropertyChanged(nameof(FilterStatus)); } }

        public OptionSet _filterProject;
        public OptionSet FilterProject { get => _filterProject; set { _filterProject = value; OnPropertyChanged(nameof(FilterProject)); } }
        public string Keyword { get; set; }
        public ReservationListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                string project = null;
                string status = null;
                if (FilterStatus != null && FilterStatus.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(FilterStatus.Where(x => x == "-1").FirstOrDefault()))
                    {
                        string sts = string.Empty;
                        foreach (var item in FilterStatus)
                        {
                            sts += $@"<value>{item}</value>";
                        }
                        status = @"<condition attribute='statuscode' operator='in'>" + sts + "</condition>";
                    }
                    else
                    {
                        status = null;
                    }
                }
                else
                {
                    status = null;
                }
                if (FilterProject != null && FilterProject.Val != "-1")
                {
                    project = $@"<condition attribute='bsd_projectid' operator='eq' value='{FilterProject.Val}' />";
                }
                else
                {
                    project = null;
                }
                EntityName = "quotes";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' />
                        <attribute name='totalamount' />
                        <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_projectid' alias='bsd_project_id' />
                        <attribute name='quoteid' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                            <filter type='or'>
                              <condition attribute='customeridname' operator='like' value='%25{Keyword}%25' />
                              <condition attribute='bsd_projectidname' operator='like' value='%25{Keyword}%25' />
                              <condition attribute='bsd_quotationnumber' operator='like' value='%25{Keyword}%25' />
                              <condition attribute='name' operator='like' value='%25{Keyword}%25' />
                            </filter>
                            <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                            <filter type='or'>
                              <condition attribute='statuscode' operator='in'>
                                <value>100000007</value>
                              </condition>
                              <filter type='and'>
                                 <condition attribute='statuscode' operator='in'>
                                    <value>100000009</value>
                                    <value>6</value>
                                  </condition>
                                  <condition attribute='bsd_quotationsigneddate' operator='null' />
                              </filter>
                            </filter>
                        {status}
                        {project}
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
            });
        }
        public void LoadStatus()
        {
            if (FiltersStatus != null && FiltersStatus.Count == 0)
            {
                FiltersStatus.Add(new OptionSet("-1", Language.tat_ca));
                var list = QuoteStatusCodeData.GetQuoteByIds("100000007,6,100000009");
                foreach (var item in list)
                {
                    FiltersStatus.Add(new OptionSet(item.Id, item.Name));
                }
            }
        }

        public async Task LoadProject()
        {
            if (FiltersProject != null)
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='Val'/>
                                    <attribute name='bsd_projectcode'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='createdon' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                    </filter>
                                  </entity>
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_projects", fetchXml);
                if (result == null || result.value.Any() == false) return;

                FiltersProject.Add(new OptionSet("-1", Language.tat_ca));
                var data = result.value;
                foreach (var item in data)
                {
                    FiltersProject.Add(item);
                }
            }
        }
    }
}
