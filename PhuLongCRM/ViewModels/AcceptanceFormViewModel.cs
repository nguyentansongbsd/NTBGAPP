using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class AcceptanceFormViewModel : BaseViewModel
    {
        private AcceptanceModel _acceptance;
        public AcceptanceModel Acceptance { get => _acceptance; set { _acceptance = value; OnPropertyChanged(nameof(Acceptance)); } }

        private List<OptionSet> _typeResults;
        public List<OptionSet> TypeResults { get => _typeResults; set { _typeResults = value; OnPropertyChanged(nameof(TypeResults)); } }

        private OptionSet _typeResult;
        public OptionSet TypeResult { get => _typeResult; set { _typeResult = value; OnPropertyChanged(nameof(TypeResult)); } }

        private List<OptionSet> _installments;
        public List<OptionSet> Installments { get => _installments; set { _installments = value; OnPropertyChanged(nameof(Installments)); } }

        private OptionSet _installment;
        public OptionSet Installment { get => _installment; set { _installment = value; OnPropertyChanged(nameof(Installment)); } }

        private bool _isUpdate;
        public bool IsUpdate { get => _isUpdate; set { _isUpdate = value; OnPropertyChanged(nameof(IsUpdate)); } }
        public AcceptanceFormViewModel()
        {
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
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_contract' link-type='outer'>
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
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='contacts'>                
                                        <attribute name='fullname' alias='contact_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='accounts'>              
                                        <attribute name='bsd_name' alias='account_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AcceptanceModel>>("bsd_acceptances", fetchXml);
            if (result == null || result.value.Count == 0) return;
            Acceptance = result.value.SingleOrDefault();
            // loại kq
            TypeResult = !string.IsNullOrWhiteSpace(Acceptance.bsd_typeresult) ? AcceptanceTypeResult.GetAcceptanceTypeResultById(Acceptance.bsd_typeresult) : null;
            if (Acceptance.installment_id != Guid.Empty && !string.IsNullOrWhiteSpace(Acceptance.installment_name))
                Installment = new OptionSet { Val = Acceptance.installment_id.ToString(), Label = Acceptance.installment_name };
            if(Acceptance.bsd_actualacceptancedate.HasValue)
                Acceptance.bsd_actualacceptancedate = Acceptance.bsd_actualacceptancedate.Value.ToLocalTime();
            if (Acceptance.bsd_reacceptancedate.HasValue)
                Acceptance.bsd_reacceptancedate = Acceptance.bsd_reacceptancedate.Value.ToLocalTime();

            if (Acceptance.bsd_typeresult == "100000000" || Acceptance.bsd_typeresult == "100000003")
                IsUpdate = true;
            else
                IsUpdate = false;
        }
        public async Task LoadInstallment()
        {
            if (Acceptance == null || Acceptance.installment_id == Guid.Empty) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                    <entity name='bsd_paymentschemedetail'>
                                        <attribute name='bsd_paymentschemedetailid' alias='Val' />
                                        <attribute name='bsd_name' alias='Label' />
                                        <order attribute='bsd_name' descending='false' />
                                        <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' link-type='inner'>
                                            <link-entity name='bsd_paymentschemedetail' from='bsd_paymentscheme' to='bsd_paymentschemeid' link-type='inner'>
                                                <filter type='and'>
                                                    <condition attribute='bsd_paymentschemedetailid' operator='eq' value='{Acceptance.installment_id}' />
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_paymentschemedetails", fetchXml);
            if (result == null || result.value.Count == 0) return;
            Installments = result.value;
        }
        public async Task<CrmApiResponse> Update()
        {
            string path = "/bsd_acceptances(" + Acceptance.bsd_acceptanceid + ")";
            var content = await getContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;

        }
        public async Task<Boolean> DeletLookup(string fieldName, Guid id)
        {
            var result = await CrmHelper.SetNullLookupField("bsd_acceptances", id, fieldName);
            return result.IsSuccess;
        }
        public async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            if (TypeResult != null)
                data["bsd_typeresult"] = TypeResult.Val;

            if (Acceptance.bsd_actualacceptancedate.HasValue)
                data["bsd_actualacceptancedate"] =  Acceptance.bsd_actualacceptancedate.Value.ToUniversalTime();
            data["bsd_expense"] = Acceptance.bsd_expense;

            if (Installment != null)
            {
                data["bsd_installment@odata.bind"] = "/bsd_paymentschemedetails(" + Installment.Val + ")";
            }
            else
                await DeletLookup("bsd_installment", Acceptance.bsd_acceptanceid);

            if (Acceptance.bsd_reacceptancedate.HasValue)
                data["bsd_reacceptancedate"] = Acceptance.bsd_reacceptancedate.Value.ToUniversalTime(); ;
            data["bsd_repairtimeday"] = Acceptance.bsd_repairtimeday;
            data["bsd_remark"] = Acceptance.bsd_remark;
            return data;
        }
    }
}
