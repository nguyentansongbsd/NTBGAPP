using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class AcceptanceListViewModel : ListViewBaseViewModel2<AcceptanceListModel>
    {
        public ObservableCollection<OptionSet> Statuss { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<OptionSet> Projects { get; set; } = new ObservableCollection<OptionSet>();

        public List<string> _status;
        public List<string> Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

        public OptionSet _project;
        public OptionSet Project { get => _project; set { _project = value; OnPropertyChanged(nameof(Project)); } }
        public string Keyword { get; set; }
        public AcceptanceListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                string project = null;
                string status = null;
                if (Status != null && Status.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(Status.Where(x => x == "-1").FirstOrDefault()))
                    {
                        string sts = string.Empty;
                        foreach (var item in Status)
                        {
                            sts += $@"<value>{item}</value>";
                        }
                        status = @"<condition attribute='statuscode' operator='in'>" + sts + "</condition>";
                    }
                }
                if (Project != null && Project.Val != "-1")
                    project = $@"<condition attribute='bsd_project' operator='eq' value='{Project.Val}' />";

                EntityName = "bsd_acceptances";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        {status}
                                        {project}
                                        <filter type='or'>
                                            <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                            <condition attribute='bsd_acceptancenumber' operator='like' value='%25{Keyword}%25' />
                                            <condition entityname='project' attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                            <condition entityname='product' attribute='name' operator='like' value='%25{Keyword}%25' />
                                            <condition entityname='contract' attribute='name' operator='like' value='%25{Keyword}%25' />
                                        </filter>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='project'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer' alias='product'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_contract' link-type='outer' alias='contract'>
                                        <attribute name='name' alias='contract_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            });
        }
        public void LoadStatus()
        {
            if (Statuss != null && Statuss.Count == 0)
            {
                Statuss.Add(new OptionSet("-1", Language.tat_ca));
                var list = AcceptanceStatus.AcceptanceStatusData();
                foreach (var item in list)
                {
                    Statuss.Add(new OptionSet(item.Id, item.Name));
                }
            }
        }
        public async Task LoadProject()
        {
            if (Projects != null)
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                    </filter>
                                  </entity>
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_projects", fetchXml);
                if (result == null || result.value.Any() == false) return;

                Projects.Add(new OptionSet("-1", Language.tat_ca));
                var data = result.value;
                foreach (var item in data)
                {
                    Projects.Add(item);
                }
            }
        }
    }
}
