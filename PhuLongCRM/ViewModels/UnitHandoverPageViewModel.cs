using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;

namespace PhuLongCRM.ViewModels
{
    public class UnitHandoverPageViewModel :BaseViewModel
    {
        public Guid UnitHandoverId { get; set; }
        private UnitHandoverPageModel _unitHandover;
        public UnitHandoverPageModel UnitHandover { get => _unitHandover;set { _unitHandover = value;OnPropertyChanged(nameof(UnitHandover)); } }

        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();

        private DateTime? _confirmDate;
        public DateTime? ConfirmDate { get => _confirmDate; set { _confirmDate = value;OnPropertyChanged(nameof(ConfirmDate)); } }

        private ObservableCollection<UnitSpecificationDetailModelGroup> unitSpecificationDetails;
        public ObservableCollection<UnitSpecificationDetailModelGroup> UnitSpecificationDetails { get => unitSpecificationDetails; set { unitSpecificationDetails = value; OnPropertyChanged(nameof(UnitSpecificationDetails)); } }

        private UnitSpecificationDetailModel unitSpecificationDetail;
        public UnitSpecificationDetailModel UnitSpecificationDetail { get => unitSpecificationDetail; set { unitSpecificationDetail = value; OnPropertyChanged(nameof(UnitSpecificationDetail)); } }
        public int Page { get; set; } = 1;

        private bool _showMore;
        public bool ShowMore { get => _showMore; set { _showMore = value; OnPropertyChanged(nameof(ShowMore)); } }
        public UnitHandoverPageViewModel()
        {
        }

        public async Task LoadUnitHandover()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_handover'>
                                    <attribute name='bsd_handoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_handovernumber' />
                                    <attribute name='bsd_description' />
                                    <attribute name='bsd_totalpaidamount' />
                                    <attribute name='bsd_handoverformprintdate' />
                                    <attribute name='bsd_confirmdate' />
                                    <attribute name='bsd_cancelledreason' />
                                    <attribute name='bsd_cancelleddate' />
                                    <attribute name='bsd_estimatehandoverdate' />
                                    <attribute name='bsd_actualnsa' />
                                    <attribute name='bsd_handoverdate' />
                                    <attribute name='bsd_producterror' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_handoverid' operator='eq' value='{UnitHandoverId}'/>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a_3743f43dba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                      <attribute name='bsd_projectid' alias='project_id'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_96e2d45bba81e911a83b000d3a07be23'>
                                        <attribute name='name' alias='unit_name'/>
                                        <attribute name='productid' alias='unit_id'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_a2c3c967ba81e911a83b000d3a07be23'>
                                        <attribute name='name' alias='optionentry_name'/>
                                        <attribute name='salesorderid' alias='optionentry_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_handovernotice' from='bsd_handovernoticeid' to='bsd_handovernotices' visible='false' link-type='outer' alias='a_dec00e1936f4eb1194ef00224856b174'>
                                       <attribute name='bsd_name' alias='handovernotice_name'/>
                                        <attribute name='bsd_handovernoticeid' alias='handovernotice_id'/>
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_pinter' visible='false' link-type='outer' alias='a_fab2a996378fec11b400000d3a082a92'>
                                        <attribute name='fullname' alias='pinter_name'/>
                                        <attribute name='systemuserid' alias='pinter_id'/>
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_confirmer' visible='false' link-type='outer' alias='a_36c37db5378fec11b400000d3a082a92'>
                                        <attribute name='fullname' alias='confirmer_name'/>
                                        <attribute name='systemuserid' alias='confirmer_id'/>
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_canceller' visible='false' link-type='outer' alias='a_1dc906f0388fec11b400000d3a082a92'>
                                        <attribute name='fullname' alias='canceller_name'/>
                                        <attribute name='systemuserid' alias='canceller_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch> ";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitHandoverPageModel>>("bsd_handovers", fetchXml);
            if (result == null || result.value.Any() == false) return;
            var data = result.value.SingleOrDefault();
            if (data.bsd_handoverformprintdate.HasValue)
            {
                data.bsd_handoverformprintdate = data.bsd_handoverformprintdate.Value.ToLocalTime();
            }
            if (data.bsd_confirmdate.HasValue)
            {
                data.bsd_confirmdate = data.bsd_confirmdate.Value.ToLocalTime();
            }
            if (data.bsd_cancelleddate.HasValue)
            {
                data.bsd_cancelleddate = data.bsd_cancelleddate.Value.ToLocalTime();
            }
            if (data.bsd_estimatehandoverdate.HasValue)
            {
                data.bsd_estimatehandoverdate = data.bsd_estimatehandoverdate.Value.ToLocalTime();
            }
            if (data.bsd_handoverdate.HasValue)
            {
                data.bsd_handoverdate = data.bsd_handoverdate.Value.ToLocalTime();
            }
            
            this.UnitHandover = data;
        }

        public async Task<CrmApiResponse> CancelHandover()
        {
            string path = "/bsd_handovers(" + this.UnitHandoverId + ")";
            var content = await GetContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;
        }

        public async Task<CrmApiResponse> ConfirmHandover()
        {
            string path = "/bsd_handovers(" + this.UnitHandoverId + ")";
            var content = await GetContentConfirmHandover();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;
        }

        private async Task<object> GetContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statuscode"] = "100000003"; // sts = cancel
            data["bsd_cancelleddate"] = DateTime.Now.ToUniversalTime();
            data["bsd_cancelledreason"] = this.UnitHandover.bsd_cancelledreason;
            data["bsd_canceller@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            return data;
        }

        private async Task<object> GetContentConfirmHandover()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statuscode"] = "100000001";// sts = handover
            if (this.ConfirmDate.HasValue)
            {
                data["bsd_confirmdate"] = this.ConfirmDate.Value.Date.ToUniversalTime();
            }
            data["bsd_confirmer@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            return data;
        }
        public async Task LoadUnitSpecificationDetails()
        {
            // thử để push
            if (UnitSpecificationDetails == null)
                UnitSpecificationDetails = new ObservableCollection<UnitSpecificationDetailModelGroup>();

            string fetch = $@"<fetch count='15' page='{Page}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_unitsspecificationdetails'>
                                    <attribute name='bsd_typeofroomvn' />
                                    <attribute name='bsd_itemvn' />
                                    <attribute name='bsd_details' />
                                    <attribute name='bsd_typeno' />
                                    <attribute name='bsd_no' />
                                    <attribute name='bsd_unitsspecificationdetailsid' />
                                    <order attribute='bsd_typeno' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_typeno' operator='not-null' />
                                    </filter>
                                    <link-entity name='bsd_unitsspecification' from='bsd_unitsspecificationid' to='bsd_unitsspecification' link-type='inner' alias='aa'>
                                        <link-entity name='salesorder' from='bsd_unitsspecification' to='bsd_unitsspecificationid' link-type='inner' alias='ab'>
                                            <link-entity name='bsd_handover' from='bsd_optionentry' to='salesorderid' link-type='inner' alias='ac'>
                                                <filter type='and'>
                                                    <condition attribute='bsd_handoverid' operator='eq' value='{UnitHandoverId}'/>
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";
            //            string fetch = $@"<fetch count='5' page='{Page}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                                <entity name='bsd_unitsspecificationdetails'>
            //                                    <attribute name='bsd_typeofroomvn' />
            //                                    <attribute name='bsd_itemvn' />
            //                                    <attribute name='bsd_details' />
            //                                    <attribute name='bsd_typeno' />
            //<attribute name='bsd_no' />
            //                                    <attribute name='bsd_unitsspecificationdetailsid' />
            //                                    <order attribute='bsd_typeno' descending='false' />
            //                                    <filter type='and'>
            //                                      <condition attribute='bsd_typeno' operator='not-null' />
            //                                    </filter>
            //                                    <link-entity name='bsd_unitsspecification' from='bsd_unitsspecificationid' to='bsd_unitsspecification' link-type='inner'>
            //                                        <filter type='and'>
            //                                          <condition attribute='bsd_unitsspecificationid' operator='eq' value='7d73726d-18dc-ec11-bb3c-00224859cf1f' />
            //                                        </filter>                                        
            //                                    </link-entity>
            //                                </entity>
            //                            </fetch>";
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
        public async Task LoadUnitSpecificationDetail(Guid id)
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
        public async Task<CrmApiResponse> ConfirmDocument()
        {
            var data = new
            {
                input = "1btn_Approve"
            };
            CrmApiResponse result = await CrmHelper.PostData($"/bsd_handovers({UnitHandoverId})//Microsoft.Dynamics.CRM.bsd_Action_ConfirmDocumentForPinbook", data);
            return result;
        }
    }
}
