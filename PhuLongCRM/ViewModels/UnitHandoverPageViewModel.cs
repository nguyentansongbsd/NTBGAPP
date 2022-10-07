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
    }
}
