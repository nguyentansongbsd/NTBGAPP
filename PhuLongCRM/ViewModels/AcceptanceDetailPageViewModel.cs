using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;

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

        private ObservableCollection<UnitSpecificationDetailModelGroup> unitSpecificationDetails;
        public ObservableCollection<UnitSpecificationDetailModelGroup> UnitSpecificationDetails { get => unitSpecificationDetails; set { unitSpecificationDetails = value; OnPropertyChanged(nameof(UnitSpecificationDetails)); } }

        private UnitSpecificationDetailModel unitSpecificationDetail;
        public UnitSpecificationDetailModel UnitSpecificationDetail { get => unitSpecificationDetail; set { unitSpecificationDetail = value; OnPropertyChanged(nameof(UnitSpecificationDetail)); } }
        public int Page { get; set; } = 1;

        private bool _showMore;
        public bool ShowMore { get => _showMore; set { _showMore = value; OnPropertyChanged(nameof(ShowMore)); } }
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
            var data = new
            {
                input = "2btn_Cancel",
                input2 = Acceptance.bsd_cancelledreason
            };
            CrmApiResponse result = await CrmHelper.PostData($"/bsd_acceptances({Acceptance.bsd_acceptanceid})//Microsoft.Dynamics.CRM.bsd_Action_Acceptance", data);
            return result;

        }
        public async Task<CrmApiResponse> ConfirmInformation()
        {
            var data = new
            {
                input = "1btn_ConfirmAcceptance"
            };
            CrmApiResponse result =  await CrmHelper.PostData($"/bsd_acceptances({Acceptance.bsd_acceptanceid})//Microsoft.Dynamics.CRM.bsd_Action_Acceptance", data);
            return result;
        }
        public async Task<CrmApiResponse> CloseInformation()
        {
            var data = new
            {
                input = "3btn_CloseAcceptance"
            };
            CrmApiResponse result = await CrmHelper.PostData($"/bsd_acceptances({Acceptance.bsd_acceptanceid})//Microsoft.Dynamics.CRM.bsd_Action_Acceptance", data);
            return result;
        }
        public async Task LoadUnitSpecificationDetails()
        {
            // thử để push
            if (UnitSpecificationDetails == null)
                UnitSpecificationDetails = new ObservableCollection<UnitSpecificationDetailModelGroup>();

            //string fetch = $@"<fetch count='15' page='{Page}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                    <entity name='bsd_unitsspecificationdetails'>
            //                        <attribute name='bsd_typeofroomvn' />
            //                        <attribute name='bsd_itemvn' />
            //                        <attribute name='bsd_details' />
            //                        <attribute name='bsd_typeno' />
            //                        <attribute name='bsd_unitsspecificationdetailsid' />
            //                        <order attribute='bsd_typeno' descending='false' />
            //                        <filter type='and'>
            //                          <condition attribute='bsd_typeno' operator='not-null' />
            //                        </filter>
            //                        <link-entity name='bsd_unitsspecification' from='bsd_unitsspecificationid' to='bsd_unitsspecification' link-type='inner'>
            //                            <link-entity name='salesorder' from='bsd_unitsspecification' to='bsd_unitsspecificationid' link-type='inner'>
            //                                <link-entity name='bsd_acceptance' from='bsd_contract' to='salesorderid' link-type='inner'>
            //                                    <filter type='and'>
            //                                        <condition attribute='bsd_acceptanceid' operator='eq' value='{Acceptance.bsd_acceptanceid}' />
            //                                    </filter>
            //                                </link-entity>
            //                            </link-entity>
            //                        </link-entity>
            //                    </entity>
            //                </fetch>";
            string fetch = $@"<fetch count='5' page='{Page}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_unitsspecificationdetails'>
                                    <attribute name='bsd_typeofroomvn' />
                                    <attribute name='bsd_itemvn' />
                                    <attribute name='bsd_details' />
                                    <attribute name='bsd_typeno' />
                                    <attribute name='bsd_unitsspecificationdetailsid' />
                                    <order attribute='bsd_typeno' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_typeno' operator='not-null' />
                                    </filter>
                                    <link-entity name='bsd_unitsspecification' from='bsd_unitsspecificationid' to='bsd_unitsspecification' link-type='inner'>
<filter type='and'>
                                      <condition attribute='bsd_unitsspecificationid' operator='eq' value='7d73726d-18dc-ec11-bb3c-00224859cf1f' />
                                    </filter>                                        
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitSpecificationDetailModel>>("bsd_unitsspecificationdetailses", fetch);
            if (result != null && result.value.Count > 0)
            {
                foreach (var item in result.value)
                {
                    var last_item = UnitSpecificationDetails.LastOrDefault();
                    if (last_item != null && item.bsd_typeofroomvn == last_item.group)
                        last_item.source.Add(item);
                    else
                    {
                        ObservableCollection<UnitSpecificationDetailModel> source = new ObservableCollection<UnitSpecificationDetailModel>();
                        source.Add(item);
                        UnitSpecificationDetails.Add(new UnitSpecificationDetailModelGroup(item.bsd_typeofroomvn, source));
                    }
                }
            }
            ShowMore = result.value.Count == 5 ? true : false;
        }
        public async Task<CrmApiResponse> UpdateUnitSpecificationDetail(UnitSpecificationDetailModel item)
        {
            string path = "/bsd_unitsspecificationdetailses(" + item.bsd_unitsspecificationdetailsid + ")";
            IDictionary<string, object> content = new Dictionary<string, object>();
            //content["bsd_repairtimeday"] = item.bsd_typeno;
            //content["bsd_remark"] = item.bsd_typeno;
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;
        }
        public async Task LoadUnitSpecificationDetail( Guid id)
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_unitsspecificationdetails'>
                                    <attribute name='bsd_typeofroomvn' />
                                    <attribute name='bsd_itemvn' />
                                    <attribute name='bsd_details' />
                                    <attribute name='bsd_typeno' />
                                    <attribute name='bsd_unitsspecificationdetailsid' />
                                    <order attribute='bsd_typeno' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_unitsspecificationdetailsid' operator='eq' value='{id}' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitSpecificationDetailModel>>("bsd_unitsspecificationdetailses", fetch);
            if (result != null && result.value.Count > 0)
            {
                UnitSpecificationDetail = result.value.FirstOrDefault();
            }
        }
    }
}
