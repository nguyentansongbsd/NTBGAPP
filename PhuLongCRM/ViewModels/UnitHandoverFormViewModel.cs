using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;

namespace PhuLongCRM.ViewModels
{
    public class UnitHandoverFormViewModel :BaseViewModel
    {
        public Guid UnitHandoverId { get; set; }
        private UnitHandoverPageModel _unitHandover;
        public UnitHandoverPageModel UnitHandover { get => _unitHandover; set { _unitHandover = value;OnPropertyChanged(nameof(UnitHandover)); } }

        public UnitHandoverFormViewModel()
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
            if (result == null && result.value.Any() == false) return;
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

        public async Task LoadContract(ContractModel contract)
        {
            if (contract != null)
            {
                UnitHandoverPageModel item = new UnitHandoverPageModel();
                item.optionentry_id = contract.salesorderid;
                item.optionentry_name = contract.unit_name;
                item.project_id = contract.project_id;
                item.project_name = contract.project_name;
                item.unit_id = contract.unit_id;
                item.unit_name = contract.unit_name;
                item.bsd_actualnsa = contract.bsd_constructionarea;
                item.bsd_totalpaidamount = contract.totalamount;
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                        <entity name='bsd_handovernotice'>
                                            <attribute name='bsd_name' alias='Label'/>
                                            <attribute name='bsd_handovernoticeid' alias='Val'/>
                                            <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' link-type='inner' alias='aa'>
                                                <filter type='and'>
                                                    <condition attribute='salesorderid' operator='eq' value='{contract.salesorderid}' />
                                                </filter>
                                            </link-entity>
                                        </entity>
                                    </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_handovernotices", fetchXml);
                if (result != null && result.value.Any() == true);
                {
                    var data = result.value.SingleOrDefault();
                    item.handovernotice_name = data.Label;
                    item.handovernotice_id = Guid.Parse(data.Val);
                }
                UnitHandover = item;
            }
        }

        public async Task<CrmApiResponse> UpdateUnitHandover()
        {
            string path = "/bsd_handovers(" + this.UnitHandoverId + ")";

            var content = new
            {
                bsd_handoverdate = this.UnitHandover.bsd_handoverdate,
                bsd_actualnsa = this.UnitHandover.bsd_actualnsa,
                bsd_producterror = this.UnitHandover.bsd_producterror
            };
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            return result;
        }
        public async Task<CrmApiResponse> CreateUnitHandover()
        {
            string path = "/bsd_handovers";

            var content = await getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            return result;
        }
        public async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            if (UnitHandover.project_id != Guid.Empty)
            {
                data["bsd_projectid@odata.bind"] = "/bsd_projects(" + UnitHandover.project_id + ")";
            }
            if (UnitHandover.unit_id != Guid.Empty)
            {
                data["bsd_Units@odata.bind"] = "/products(" + UnitHandover.unit_id + ")";
            }
            if (UnitHandover.optionentry_id != Guid.Empty)
            {
                data["bsd_OptionEntry@odata.bind"] = "/salesorders(" + UnitHandover.optionentry_id + ")";
            }
            if (UnitHandover.handovernotice_id != Guid.Empty)
            {
                data["bsd_handovernotices@odata.bind"] = "/bsd_handovernotices(" + UnitHandover.handovernotice_id + ")";
            }
            data["bsd_handoverdate"] = UnitHandover.bsd_handoverdate;
            data["bsd_actualnsa"] = UnitHandover.bsd_actualnsa;
            data["bsd_producterror"] = UnitHandover.bsd_producterror;
            data["bsd_name"] = UnitHandover.bsd_name;
            data["bsd_handovernumber"] = UnitHandover.bsd_handovernumber;
            data["bsd_handoverid"] = Guid.NewGuid();
            return data;
        }
    }
}
