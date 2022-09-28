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
    public class AcceptanceDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();

        private AcceptanceModel _acceptance;
        public AcceptanceModel Acceptance { get => _acceptance; set { _acceptance = value; OnPropertyChanged(nameof(Acceptance)); } }

        private string _systemuser;
        public string Systemuser { get => _systemuser; set { _systemuser = value; OnPropertyChanged(nameof(Systemuser)); } }

        private DateTime _dateTimeNow;
        public DateTime DateTimeNow { get => _dateTimeNow; set { _dateTimeNow = value; OnPropertyChanged(nameof(DateTimeNow)); } }
        public AcceptanceDetailPageViewModel()
        {
            DateTimeNow = DateTime.Now;
            Systemuser = UserLogged.ManagerName;
        }
        public async Task LoadAcceptance(Guid id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_acceptancenumber' />
                                    <attribute name='bsd_acceptancetype' />
                                    <attribute name='bsd_typeresult' />
                                    <attribute name='bsd_actualacceptancedate' />
                                    <attribute name='bsd_expense' />
                                    <attribute name='bsd_reacceptancedate' />
                                    <attribute name='bsd_repairtimeday' />
                                    <attribute name='bsd_remark' />
                                    <attribute name='bsd_printedate' />
                                    <attribute name='bsd_confirmdate' />
                                    <attribute name='bsd_cancelleddate' />
                                    <attribute name='bsd_cancelledreason' />
                                    <attribute name='bsd_deactivereason' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                       <condition attribute='bsd_acceptanceid' operator='eq' value='{id}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer'>
                                        <attribute name='bsd_projectid' alias='project_id'/>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer'>
                                        <attribute name='productid' alias='unit_id'/>
                                        <attribute name='name' alias='unit_name'/>
                                        <attribute name='productnumber' alias='unit_code'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_contract' link-type='outer'>
                                        <attribute name='salesorderid' alias='contract_id'/>
                                        <attribute name='name' alias='contract_name'/>
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_printer' link-type='outer' >
                                        <attribute name='fullname' alias='printer_name' />
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_canceller' link-type='outer'>
                                        <attribute name='fullname' alias='canceller_name' />
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_confirmer' link-type='outer'>
                                        <attribute name='fullname' alias='confirmer_name' />
                                    </link-entity>
                                    <link-entity name='bsd_acceptancenotices' from='bsd_acceptancenoticesid' to='bsd_acceptancenotices' link-type='outer'>
                                        <attribute name='bsd_name' alias='acceptance_noti_name' />
                                    </link-entity>
                                    <link-entity name='bsd_paymentschemedetail' from='bsd_paymentschemedetailid' to='bsd_installment' link-type='outer'>
                                        <attribute name='bsd_paymentschemedetailid' alias='installment_id' />
                                        <attribute name='bsd_name' alias='installment_name' />
                                        <attribute name='bsd_ordernumber' alias='installment_number' />
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='contacts'>                
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='accounts'>              
                                        <attribute name='bsd_name' alias='account_name'/>
                                        <attribute name='accountid' alias='account_id'/>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AcceptanceModel>>("bsd_acceptances", fetchXml);
            if (result == null || result.value.Count == 0) return;
            Acceptance = result.value.SingleOrDefault();
            if (Acceptance.bsd_actualacceptancedate.HasValue)
                Acceptance.bsd_actualacceptancedate = Acceptance.bsd_actualacceptancedate.Value.ToLocalTime();
            if (Acceptance.bsd_reacceptancedate.HasValue)
                Acceptance.bsd_reacceptancedate = Acceptance.bsd_reacceptancedate.Value.ToLocalTime();
        }
        public async Task<CrmApiResponse> CancelAcceptance()
        {
            string path = "/bsd_acceptances(" + Acceptance.bsd_acceptanceid + ")";

            IDictionary<string, object> content = new Dictionary<string, object>();
            content["statuscode"] = "100000002";
            content["bsd_cancelledreason"] = Acceptance.bsd_cancelledreason;
            content["bsd_cancelleddate"] = DateTimeNow.ToUniversalTime(); ;
            content["bsd_canceller@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";

            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;

        }
        public async Task<CrmApiResponse> ConfirmInformation()
        {
            string path = "/bsd_acceptances(" + Acceptance.bsd_acceptanceid + ")";

            IDictionary<string, object> content = new Dictionary<string, object>();
            if(Acceptance.bsd_expense > 0)
                content["statuscode"] = "100000000";
            else
                content["statuscode"] = "100000001";

            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;

        }
        public async Task<bool> CloseInformation()
        {
            string path = "/bsd_acceptances(" + Acceptance.bsd_acceptanceid + ")";

            IDictionary<string, object> content = new Dictionary<string, object>();
            content["statuscode"] = "100000003";
            content["bsd_confirmer@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            content["bsd_confirmdate"] = DateTimeNow.ToUniversalTime();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                if (Acceptance.bsd_typeresult == "100000000" || Acceptance.bsd_typeresult == "100000003")
                {
                   bool result_contract_unit = await UpdateContract_Unit();
                    if (result_contract_unit)
                        return true;
                    else
                        return false;
                }
                else if (Acceptance.bsd_typeresult == "100000002")
                {
                    bool result_acceptanceNotice = await CreateAcceptanceNotice();
                    if (result_acceptanceNotice)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<bool> UpdateContract_Unit()
        {
            IDictionary<string, object> content_contract = new Dictionary<string, object>();
            content_contract["bsd_acceptanceinformation@odata.bind"] = "/bsd_acceptances(" + Acceptance.bsd_acceptanceid + ")";
            content_contract["bsd_acceptance"] = true;

            CrmApiResponse result_contract = await CrmHelper.PatchData($@"/salesorders({Acceptance.contract_id})", content_contract);

            IDictionary<string, object> content_unit = new Dictionary<string, object>();
            content_unit["bsd_acceptance"] = true;

            CrmApiResponse result_unit = await CrmHelper.PatchData($@"/products({Acceptance.unit_id})", content_unit);

            if (result_contract.IsSuccess == true && result_unit.IsSuccess == true)
                return true;
            else
            {
                return false;
            }
        }
        private async Task<bool> CreateAcceptanceNotice()
        {
            string path = "/bsd_acceptancenoticeses";

            IDictionary<string, object> content = new Dictionary<string, object>();
            content["statecode"] = "0";
            content["bsd_name"] = $"Thông báo nghiệm thu lần (2+) của căn hộ <{Acceptance.unit_code}>";
            content["bsd_date"] = DateTimeNow.ToUniversalTime(); ;
            content["bsd_project@odata.bind"] = "/bsd_projects(" + Acceptance.project_id + ")";
            content["bsd_units@odata.bind"] = "/products(" + Acceptance.unit_id + ")";
            content["bsd_contract@odata.bind"] = "/salesorders(" + Acceptance.contract_id + ")";
            content["bsd_installment@odata.bind"] = "/bsd_paymentschemedetails(" + Acceptance.installment_id + ")";
            if (Acceptance.contact_id != Guid.Empty)
                content["bsd_customer_contact@odata.bind"] = "/contacts(" + Acceptance.contact_id + ")";
            else if (Acceptance.account_id != Guid.Empty)
                content["bsd_customer_account@odata.bind"] = "/accounts(" + Acceptance.account_id + ")";
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if(result.IsSuccess)
            {
                if (Acceptance.bsd_expense > 0)
                {
                    bool result_mics = await CreateMics();
                    if (result_mics)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        private async Task<bool> CreateMics()
        {
            string path = "/bsd_miscellaneouses";

            IDictionary<string, object> content = new Dictionary<string, object>();
            content["statuscode"] = "1";
            content["bsd_name"] = "Phí phát sinh sau nghiệm thu";
            content["bsd_duedate"] = DateTimeNow.ToUniversalTime();
            content["bsd_amount"] = Acceptance.bsd_expense;
            content["bsd_project@odata.bind"] = "/bsd_projects(" + Acceptance.project_id + ")";
            content["bsd_units@odata.bind"] = "/products(" + Acceptance.unit_id + ")";
            content["bsd_optionentry@odata.bind"] = "/salesorders(" + Acceptance.contract_id + ")";
            content["bsd_installment@odata.bind"] = "/bsd_paymentschemedetails(" + Acceptance.installment_id + ")";
            content["bsd_installmentnumber"] = Acceptance.installment_number;
            content["bsd_acceptance@odata.bind"] = "/bsd_acceptances(" + Acceptance.bsd_acceptanceid + ")";
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            return result.IsSuccess;
        }
    }
}
