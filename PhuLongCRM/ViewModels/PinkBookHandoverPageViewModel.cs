using System;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;

namespace PhuLongCRM.ViewModels
{
    public class PinkBookHandoverPageViewModel :BaseViewModel
    {
        public Guid PinkBookHandoverId { get; set; }
        private PinkBookHandoverPageModel _pinkBookHandover;
        public PinkBookHandoverPageModel PinkBookHandover { get => _pinkBookHandover; set { _pinkBookHandover = value;OnPropertyChanged(nameof(PinkBookHandover)); } }

        public PinkBookHandoverPageViewModel()
        {
        }

        public async Task LoadPinkBookHandover()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_pinkbookhandover'>
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_handovernumber' />
                                    <attribute name='bsd_pinkbookhandoverid' />
                                    <attribute name='bsd_totalpaidamount' />
                                    <attribute name='bsd_description' />
                                    <attribute name='bsd_printdate' />
                                    <attribute name='bsd_confirmdate' />
                                    <attribute name='bsd_symbol' />
                                    <attribute name='bsd_registrationtax' />
                                    <attribute name='bsd_placeofissue' />
                                    <attribute name='bsd_pinkbookreceiptdate' />
                                    <attribute name='bsd_pinkbookarea' />
                                    <attribute name='bsd_otherhandoverdocument' />
                                    <attribute name='bsd_issuedon' />
                                    <attribute name='bsd_issuancefee' />
                                    <attribute name='bsd_inspectionfee' />
                                    <attribute name='bsd_handoverdate' />
                                    <attribute name='bsd_certificatenumber' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_pinkbookhandoverid' operator='eq' value='{PinkBookHandoverId}'/>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_26f8767ec690ec11b400000d3aa1f0ac'>
                                      <attribute name='bsd_contactfullname' alias='project_name'/>
                                      <attribute name='bsd_projectid' alias='project_id'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_91124d44c790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='unit_name'/>
                                      <attribute name='productid' alias='unit_id'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_fd36f62dc790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='optionentry_name'/>
                                      <attribute name='salesorderid' alias='optionentry_id'/>
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_printer' visible='false' link-type='outer' alias='a_f20a80e0ca90ec11b400000d3aa1f0ac'>
                                      <attribute name='fullname' alias='pinter_name' />
                                      <attribute name='systemuserid' alias='pinter_id' />
                                    </link-entity>
                                    <link-entity name='systemuser' from='systemuserid' to='bsd_confirmer' visible='false' link-type='outer' alias='a_0cb38011cb90ec11b400000d3aa1f0ac'>
                                      <attribute name='fullname' alias='confirmer_name' />
                                      <attribute name='systemuserid' alias='confirmer_id' />
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PinkBookHandoverPageModel>>("bsd_pinkbookhandovers", fetchXml);
            if (result == null && result.value.Any() == false) return;
            var data = result.value.SingleOrDefault();
            if (data.bsd_printdate.HasValue)
            {
                data.bsd_printdate = data.bsd_printdate.Value.ToLocalTime();
            }
            if (data.bsd_confirmdate.HasValue)
            {
                data.bsd_confirmdate = data.bsd_confirmdate.Value.ToLocalTime();
            }
            if (data.bsd_issuedon.HasValue)
            {
                data.bsd_issuedon = data.bsd_issuedon.Value.ToLocalTime();
            }
            if (data.bsd_handoverdate.HasValue)
            {
                data.bsd_handoverdate = data.bsd_handoverdate.Value.ToLocalTime();
            }
            if (data.bsd_pinkbookreceiptdate.HasValue)
            {
                data.bsd_pinkbookreceiptdate = data.bsd_pinkbookreceiptdate.Value.ToLocalTime();
            }
            this.PinkBookHandover = data;
        }
    }
}
