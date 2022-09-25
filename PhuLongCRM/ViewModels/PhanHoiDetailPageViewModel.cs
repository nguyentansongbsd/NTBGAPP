using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class PhanHoiDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();

        private PhanHoiFormModel _case;
        public PhanHoiFormModel Case { get => _case; set { _case = value; OnPropertyChanged(nameof(Case)); } }

        public ObservableCollection<ListPhanHoiModel> _listCase;
        public ObservableCollection<ListPhanHoiModel> ListCase { get => _listCase; set { _listCase = value; OnPropertyChanged(nameof(ListCase)); } }
        public int PageCase { get; set; } = 1;

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        public bool _showButton;
        public bool ShowButton { get => _showButton; set { _showButton = value; OnPropertyChanged(nameof(ShowButton)); } }

        public bool _showFloatingButtonGroup;
        public bool ShowFloatingButtonGroup { get => _showFloatingButtonGroup; set { _showFloatingButtonGroup = value; OnPropertyChanged(nameof(ShowFloatingButtonGroup)); } }

        public string _customerName;
        public string CustomerName { get => _customerName; set { _customerName = value; OnPropertyChanged(nameof(CustomerName)); } }

        public OptionSet _caseType;
        public OptionSet CaseType { get => _caseType; set { _caseType = value; OnPropertyChanged(nameof(CaseType)); } }

        public OptionSet _origin;
        public OptionSet Origin { get => _origin; set { _origin = value; OnPropertyChanged(nameof(Origin)); } }

        public OptionSet _statusCode;
        public OptionSet StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

        private List<OptionSet> _resolutionTypes;
        public List<OptionSet> ResolutionTypes { get => _resolutionTypes; set { _resolutionTypes = value; OnPropertyChanged(nameof(ResolutionTypes)); } }

        private List<OptionSet> _billableTimes;
        public List<OptionSet> BillableTimes { get => _billableTimes; set { _billableTimes = value; OnPropertyChanged(nameof(BillableTimes)); } }
        public OptionSet ResolutionType { get; set; }
        public OptionSet BillableTime { get; set; }
        public string description { get; set; }
        public string subject { get; set; }
        public PhanHoiDetailPageViewModel()
        {
            Case = new PhanHoiFormModel();
            ListCase = new ObservableCollection<ListPhanHoiModel>();
            ResolutionTypes = new List<OptionSet>();
            BillableTimes = new List<OptionSet>();
        }

        public async Task LoadCase(Guid CaseID)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='incidentid' />
                                    <attribute name='caseorigincode' />
                                    <attribute name='statuscode' />
                                    <attribute name='statecode' />
                                    <attribute name='casetypecode' />
                                    <attribute name='description' />
                                  <order attribute='title' descending='false' />
                                  <filter type='and'>
                                      <condition attribute='incidentid' operator='eq'  value='{" + CaseID + @"}' />
                                  </filter>
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                    <attribute name='bsd_name' alias='accountName'/>
                                    <attribute name='accountid' alias='accountId'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                    <attribute name='bsd_fullname' alias='contactName'/>
                                    <attribute name='contactid' alias='contactId'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' alias='products'>
                                  <attribute name='name' alias='unitName'/>
                                </link-entity>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                    <attribute name='title' alias='subjectTitle'/>
                                    <attribute name='subjectid' alias ='subjectId'/>
                                </link-entity>
                                <link-entity name='incident' from='incidentid' to='parentcaseid' link-type='outer' alias='aa'>    
                                    <attribute name='title' alias='parentCaseTitle' />
                                </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("incidents", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(data.subjectId))
            {
                data.subjectTitle = CaseSubjectData.GetCaseSubjectById(data.subjectId).Label;
            }
            
            this.Case = data;
            if (Case.statuscode != 1)
            {
                ShowButton = true;
                ShowFloatingButtonGroup = false;
            }    
            else
            {
                ShowButton = false;
                ShowFloatingButtonGroup = true;
            }    

            if(Case.accountName != null)
            {
                CustomerName = Case.accountName;
            }    
            else if(Case.contactName != null)
            {
                CustomerName = Case.contactName;
            }
            
            CaseType = CaseTypeData.GetCaseById(Case.casetypecode);
            Origin = CaseOriginData.GetOriginById(Case.caseorigincode);
            StatusCode = CaseStatusCodeData.GetCaseStatusCodeById(Case.statuscode.ToString());
        }

        public async Task LoadListCase(Guid CaseId)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='casetypecode' />
                                  <order attribute='title' descending='false' />                               
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                  <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>                               
                                <filter type='and'>
                                    <condition attribute='parentcaseid' operator='eq' uitype='incident' value='{CaseId}' />
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='" + UserLogged.Id+@"' />
                                </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListPhanHoiModel>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreCase = false;
                return;
            }
            var data = result.value;
            if (data.Count < 3)
            {
                ShowMoreCase = false;
            }
            else
            {
                ShowMoreCase = true;
            }
            foreach (var item in data)
            {
                ListCase.Add(item);
            }
        }

        public async Task<bool> UpdateCase()
        {
            string path = $"/incidents({Case.incidentid})"; 
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
            data["statecode"] = Case.statecode;
            data["statuscode"] = Case.statuscode;
            return data;
        }

        public async Task<bool> UpdateCaseResolution()
        {
            string path = $"/incidentresolutions";
            var content = await GetContentResolution();
            CrmApiResponse apiResponse = await CrmHelper.PostData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> GetContentResolution()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = Guid.NewGuid();
            data["timespent"] = BillableTime.Val;
            data["resolutiontypecode"] = ResolutionType.Val;
            data["subject"] = subject;
            data["description"] = description;
            if (Case.incidentid != Guid.Empty)
            {
                data["incidentid@odata.bind"] = "/incidents(" + Case.incidentid + ")";
            }
            return data;
        }
    }
}
