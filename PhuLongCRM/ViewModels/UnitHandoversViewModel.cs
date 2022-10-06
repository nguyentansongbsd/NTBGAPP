using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class UnitHandoversViewModel : ListViewBaseViewModel2<UnitHandoversModel>
    {
        public ObservableCollection<OptionSet> FiltersStatus { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<OptionSet> FiltersProject { get; set; } = new ObservableCollection<OptionSet>();

        public List<string> _filterStatus;
        public List<string> FilterStatus { get => _filterStatus; set { _filterStatus = value; OnPropertyChanged(nameof(FilterStatus)); } }
        public OptionSet _filterProject;
        public OptionSet FilterProject { get => _filterProject; set { _filterProject = value; OnPropertyChanged(nameof(FilterProject)); } }

        public string Keyword { get; set; }
        public UnitHandoversViewModel()
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

                EntityName = "bsd_handovers";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_handover'>
                                    <attribute name='bsd_handoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_projectid' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                       {project}
                                       {status}
                                      <condition attribute='statuscode' operator='ne' value='2' />
                                        <filter type='or'>
                                          <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                          <condition attribute='bsd_handovernumber' operator='like' value='%25{Keyword}%25' />
                                          <condition attribute='bsd_unitsname' operator='like' value='%25{Keyword}%25' />
                                        </filter>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a_3743f43dba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_96e2d45bba81e911a83b000d3a07be23'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            });
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

        public void LoadStatus()
        {
            if (FiltersStatus != null && FiltersStatus.Count == 0)
            {
                FiltersStatus.Add(new OptionSet("-1", Language.tat_ca));
                var list = UnitHandoverStatusData.GetUnitHandoverStatus();
                foreach (var item in list)
                {
                    if (item.Id != "2")
                        FiltersStatus.Add(new OptionSet(item.Id, item.Name));
                }
            }
        }
    }
}
