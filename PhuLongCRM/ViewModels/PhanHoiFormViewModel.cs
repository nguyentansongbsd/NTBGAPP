using PhuLongCRM.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using System.Collections.Generic;
using PhuLongCRM.Settings;

namespace PhuLongCRM.ViewModels
{
    public class PhanHoiFormViewModel : BaseViewModel
    {
        private PhanHoiFormModel _singlePhanHoi;
        public PhanHoiFormModel singlePhanHoi { get => _singlePhanHoi; set { _singlePhanHoi = value; OnPropertyChanged(nameof(singlePhanHoi)); } }

        private List<OptionSet> _caseTypes;
        public List<OptionSet> CaseTypes { get => _caseTypes; set { _caseTypes = value; OnPropertyChanged(nameof(CaseTypes)); } }
        private List<OptionSet> _subjects;
        public List<OptionSet> Subjects { get => _subjects; set { _subjects = value; OnPropertyChanged(nameof(Subjects)); } }
        private List<OptionSet> _caseLienQuans;
        public List<OptionSet> CaseLienQuans { get => _caseLienQuans; set { _caseLienQuans = value; OnPropertyChanged(nameof(CaseLienQuans)); } }
        private List<OptionSet> _caseOrigins;
        public List<OptionSet> CaseOrigins { get => _caseOrigins; set { _caseOrigins = value; OnPropertyChanged(nameof(CaseOrigins)); } }
        private List<OptionSet> _projects;
        public List<OptionSet> Projects { get => _projects; set { _projects = value; OnPropertyChanged(nameof(Projects)); } }
        private List<OptionSet> _units;
        public List<OptionSet> Units { get => _units; set { _units = value; OnPropertyChanged(nameof(Units)); } }
        private List<OptionSet> _contacts;
        public List<OptionSet> Contacts { get => _contacts; set { _contacts = value; OnPropertyChanged(nameof(Contacts)); } }
        private List<OptionSet> _accounts;
        public List<OptionSet> Accounts { get => _accounts; set { _accounts = value; OnPropertyChanged(nameof(Accounts)); } }
        private List<OptionSet> _queues;
        public List<OptionSet> Queues { get => _queues; set { _queues = value; OnPropertyChanged(nameof(Queues)); } }
        private List<OptionSet> _quotes;
        public List<OptionSet> Quotes { get => _quotes; set { _quotes = value; OnPropertyChanged(nameof(Quotes)); } }
        private List<OptionSet> _optionEntries;
        public List<OptionSet> OptionEntries { get => _optionEntries; set { _optionEntries = value; OnPropertyChanged(nameof(OptionEntries)); } }

        private OptionSet _caseType;
        public OptionSet CaseType { get => _caseType; set { _caseType = value; OnPropertyChanged(nameof(CaseType)); } }
        private OptionSet _subject;
        public OptionSet Subject { get => _subject; set { _subject = value; OnPropertyChanged(nameof(Subject)); } }
        private OptionSet _caseLienQuan;
        public OptionSet CaseLienQuan { get => _caseLienQuan; set { _caseLienQuan = value; OnPropertyChanged(nameof(CaseLienQuan)); } }
        private OptionSet _caseOrigin;
        public OptionSet CaseOrigin { get => _caseOrigin; set { _caseOrigin = value; OnPropertyChanged(nameof(CaseOrigin)); } }
        private OptionSet _project;
        public OptionSet Project { get => _project; set { _project = value; OnPropertyChanged(nameof(Project)); } }
        private OptionSet _unit;
        public OptionSet Unit { get => _unit; set { _unit = value; OnPropertyChanged(nameof(Unit)); } }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private List<List<OptionSet>> _allItemSourceDoiTuong;
        public List<List<OptionSet>> AllItemSourceDoiTuong { get => _allItemSourceDoiTuong; set { _allItemSourceDoiTuong = value; OnPropertyChanged(nameof(AllItemSourceDoiTuong)); } }
        private List<string> _tabsDoiTuong;
        public List<string> TabsDoiTuong { get => _tabsDoiTuong; set { _tabsDoiTuong = value; OnPropertyChanged(nameof(TabsDoiTuong)); } }
        private OptionSet _doiTuong;
        public OptionSet DoiTuong { get => _doiTuong; set { _doiTuong = value; OnPropertyChanged(nameof(DoiTuong)); } }

        public Guid IncidentId { get; set; }

        public PhanHoiFormViewModel()
        {
            singlePhanHoi = new PhanHoiFormModel();
        }

        public async Task<bool> CreateCase()
        {
            string path = "/incidents";
            singlePhanHoi.incidentid = Guid.NewGuid();
            var content = await GetContent();
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

        public async Task<bool> UpdateCase()
        {
            string path = $"/incidents({singlePhanHoi.incidentid})";
            var content = await GetContent();
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> GetContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["incidentid"] = singlePhanHoi.incidentid.ToString();
            data["casetypecode"] = CaseType.Val;
            data["title"] = singlePhanHoi.title;
            data["caseorigincode"] = CaseOrigin != null ? CaseOrigin.Val : null;
            data["description"] = singlePhanHoi.description;
            if (Subject == null)
            {
                await DeletLookup("subjectid", singlePhanHoi.incidentid);
            }
            else
            {
                data["subjectid@odata.bind"] = "/subjects(" + Subject.Val + ")";
            }

            if (CaseLienQuan == null)
            {
                await DeletLookup("parentcaseid", singlePhanHoi.incidentid);
            }
            else
            {
                data["parentcaseid@odata.bind"] = $"incidents({CaseLienQuan.Val})";
            }

            if (Customer == null)
            {
                await DeletLookup("customerid_account", singlePhanHoi.incidentid);
                await DeletLookup("customerid_contact", singlePhanHoi.incidentid);
            }
            else if (Customer.Title == "3") // account
            {
                data["customerid_account@odata.bind"] = "/accounts(" + Customer.Val + ")";
            }
            else if (Customer.Title == "2") // contact
            {
                data["customerid_contact@odata.bind"] = "/contacts(" + Customer.Val + ")";
            }

            if (Unit == null)
            {
                await DeletLookup("productid", singlePhanHoi.incidentid);
            }
            else
            {
                data["productid@odata.bind"] = "/products(" + Unit.Val + ")";
            }

            if (UserLogged.IsLoginByUserCRM == false && UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }

            if (UserLogged.IsLoginByUserCRM == false && UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        private async Task<Boolean> DeletLookup(string fieldName, Guid IncidentId)
        {
            var result = await CrmHelper.SetNullLookupField("incidents", IncidentId, fieldName);
            return result.IsSuccess;
        }

        public async Task LoadCase()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='createdon' />
                                    <attribute name='incidentid' />
                                    <attribute name='caseorigincode' />
                                    <attribute name='casetypecode' />
                                    <attribute name='description' />
                                    <order attribute='title' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='incidentid' operator='eq'  value='{" + this.IncidentId + @"}' />
                                    </filter>
                                    <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' alias='a_e59e3e7a83d1eb11bacc000d3a80021e'>
                                      <attribute name='title' alias ='subjectTitle'/>
                                      <attribute name='subjectid' alias ='subjectId'/>
                                    </link-entity>
                                    <link-entity name='incident' from='incidentid' to='parentcaseid' visible='false' link-type='outer' alias='a_4889446e83d1eb11bacc000d3a80021e'>
                                      <attribute name='title' alias='parentCaseTitle'/>
                                      <attribute name='incidentid' alias='parentCaseId'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_c34c436283d1eb11bacc000d3a80021e'>
                                      <attribute name='bsd_name' alias='accountName'/>
                                      <attribute name='accountid' alias='accountId'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_244d436283d1eb11bacc000d3a80021e'>
                                      <attribute name='fullname' alias='contactName'/>
                                      <attribute name='contactid' alias = 'contactId' />
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' alias='a_3894417483d1eb11bacc000d3a80021e'>
                                      <attribute name='name' alias='unitName'/>
                                      <attribute name='productid' alias='unitId'/>   
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0) return;

            this.singlePhanHoi = result.value.FirstOrDefault();

            this.CaseType = CaseTypeData.GetCaseById(singlePhanHoi.casetypecode);

            if (!string.IsNullOrWhiteSpace(singlePhanHoi.caseorigincode))
            {
                this.CaseOrigin = CaseOriginData.GetOriginById(singlePhanHoi.caseorigincode);
            }

            if (!string.IsNullOrWhiteSpace(singlePhanHoi.subjectId))
            {
                this.Subject = CaseSubjectData.GetCaseSubjectById(singlePhanHoi.subjectId);
            }
            if (!string.IsNullOrWhiteSpace(singlePhanHoi.parentCaseId))
            {
                this.CaseLienQuan = new OptionSet(singlePhanHoi.parentCaseId, singlePhanHoi.parentCaseTitle);
            }
            if (!string.IsNullOrWhiteSpace(singlePhanHoi.unitId))
            {
                var projects = await LoadProjects(singlePhanHoi.unitId);
                this.Project = projects.FirstOrDefault();
                this.Unit = new OptionSet(singlePhanHoi.unitId, singlePhanHoi.unitName);
            }
            if (!string.IsNullOrWhiteSpace(singlePhanHoi.contactId))
            {
                OptionSet contact = new OptionSet();
                contact.Val = singlePhanHoi.contactId;
                contact.Label = singlePhanHoi.contactName;
                contact.Title = "1";
                this.Customer = contact;
            }
            if (!string.IsNullOrWhiteSpace(singlePhanHoi.accountId))
            {
                OptionSet account = new OptionSet();
                account.Val = singlePhanHoi.accountId;
                account.Label = singlePhanHoi.accountName;
                account.Title = "2";
                this.Customer = account;
            }
        }

        public async Task LoadSubjects()
        {
            string fetchXml = @"<fetch version='1.0'  output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='subject'>      
                                    <attribute name='title' alias ='Label'/>
                                    <attribute name='subjectid' alias ='Val'/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("subjects", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Subjects = result.value;
            foreach(var item in Subjects)
            {
                item.Label = CaseSubjectData.GetCaseSubjectById(item.Val).Label;
            }
        }

        public async Task LoadCaseLienQuan(string notContantIncidentId = null)
        {
            string codition = string.Empty;
            codition = !string.IsNullOrWhiteSpace(notContantIncidentId) ? $@"<condition attribute='incidentid' operator='ne' uitype='incident' value='{notContantIncidentId}' />" : null;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' alias ='Label'/>
                                    <attribute name='incidentid' alias= 'Val'/>
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                       {codition}
                                       <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.CaseLienQuans = result.value;
        }

        public async Task LoadContacts()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' alias='Label'/>
                                    <attribute name='contactid' alias = 'Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("contacts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = "1";
                this.Contacts.Add(item);
            }
        }

        public async Task LoadAccounts()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='accountid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = "2";
                this.Accounts.Add(item);
            }
        }

        public async Task<List<OptionSet>> LoadProjects(string filterByUnitId = null)
        {
            string codition = string.Empty;
            codition = !string.IsNullOrWhiteSpace(filterByUnitId) ? $@"<link-entity name='product' from='bsd_projectcode' to='bsd_projectid' link-type='inner' alias='ab'>
                                                                          <filter type='and'>   
                                                                            <condition attribute='productid' operator='eq' uitype='product' value='{filterByUnitId}' />
                                                                          </filter>
                                                                        </link-entity>" : null;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='Val' />
                                    <attribute name='bsd_name' alias='Label' />
                                    <order attribute='createdon' descending='false' />
                                    {codition}
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_projects", fetchXml);
            if (result == null || result.value.Count == 0) return null;
            return result.value;
        }

        public async Task LoadUnits()
        {
            if (Project == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='productid' alias='Val'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectcode' operator='eq'  uitype='bsd_project' value='{Project.Val}' />
                                </filter>
                              </entity>
                            </fetch>";
            var resutl = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("products", fetchXml);
            if (resutl == null || resutl.value.Count == 0) return;
            this.Units = resutl.value;
        }

        public async Task LoadQueues()
        {
            if (Customer == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='opportunityid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                        <filter type='or'>
                                          <condition attribute='parentaccountid' operator='eq' uitype='account' value='{Customer.Val}' />
                                          <condition attribute='parentcontactid' operator='eq' uitype='contact' value='{Customer.Val}' />
                                        </filter>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("opportunities", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Queues = result.value;
        }

        public async Task LoadQuotes()
        {
            if (Customer == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='quoteid' alias='Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='customerid' operator='eq' value='{Customer.Val}' />
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value ='{UserLogged.Id}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("quotes", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Quotes = result.value;
        }

        public async Task LoadOptionEntries()
        {
            if (Customer == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='salesorderid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='customerid' operator='eq' value='{Customer.Val}' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("salesorders", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.OptionEntries = result.value;
        }

    }
}

