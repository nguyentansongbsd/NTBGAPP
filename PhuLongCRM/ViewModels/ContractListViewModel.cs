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
    public class ContractListViewModel : ListViewBaseViewModel2<ContractModel>
    {
        public ObservableCollection<OptionSet> FiltersStatus { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<OptionSet> FiltersProject { get; set; } = new ObservableCollection<OptionSet>();

        public List<string> _filterStatus;
        public List<string> FilterStatus { get => _filterStatus; set { _filterStatus = value; OnPropertyChanged(nameof(FilterStatus)); } }
        public OptionSet _filterProject;
        public OptionSet FilterProject { get => _filterProject; set { _filterProject = value; OnPropertyChanged(nameof(FilterProject)); } }

        public string Keyword { get; set; }
        public ContractListViewModel()
        {
            string attibute = string.Empty;
            attibute = UserLogged.IsLoginByUserCRM ? "bsd_saleby" : "bsd_employee";
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
                    project = $@"<condition attribute='bsd_project' operator='eq' value='{FilterProject.Val}' />";
                }
                else
                {
                    project = null;
                }

                //<condition attribute='statuscode' operator='not-in'>
                //                            <value>4</value>
                //                            <value>3</value>
                //                            <value>100003</value>
                //                            <value>1</value>
                //                            <value>100002</value>
                //                            <value>2</value>
                //                        </condition>

                EntityName = "salesorders";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='salesorder'>
                                    <attribute name='name' />
                                    <attribute name='customerid' />
                                    <attribute name='statuscode' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_unitnumber' alias='unit_id'/>
                                    <attribute name='bsd_project' alias='project_id'/>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <attribute name='bsd_contractnumber' />
                                    <order attribute='bsd_project' descending='true' />
                                    <filter type='and'>
                                        {project}
                                        {status}
                                        <condition attribute='statuscode' operator='in'>
                                            <value>100000005</value>
                                            <value>100000009</value>
                                            <value>100000004</value>
                                            <value>100000011</value>
                                            <value>100000012</value>
                                            <value>100001</value>
                                        </condition>
                                        <condition attribute = '{attibute}' operator= 'eq' value = '{UserLogged.Id}' />  
                                        <filter type='or'>      
                                            <condition attribute='customeridname' operator='like' value ='%25{Keyword}%25' />          
                                            <condition attribute='bsd_projectname' operator='like' value ='%25{Keyword}%25' />              
                                            <condition attribute='bsd_unitnumbername' operator='like' value ='%25{Keyword}%25' />             
                                            <condition attribute='ordernumber' operator='like' value ='%25{Keyword}%25' />
                                            <condition attribute='bsd_optionno' operator='like' value ='%25{Keyword}%25' />
                                            <condition attribute='bsd_contractnumber' operator='like' value ='%25{Keyword}%25' />
                                        </filter >                  
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
            }); 
        }

        public void LoadStatus()
        {
            if (FiltersStatus != null && FiltersStatus.Count == 0)
            {
                FiltersStatus.Add(new OptionSet("-1", Language.tat_ca));
                var list = ContractStatusCodeData.ContractStatusData();
                foreach (var item in list)
                {
                    //if (item.Id != "4" && item.Id != "3" && item.Id != "100003" && item.Id != "1" && item.Id != "100002" && item.Id != "2")
                    if (item.Id == "100001" || item.Id == "100000012" || item.Id == "100000011" || item.Id == "100000004" || item.Id == "100000009" || item.Id == "100000005")
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
