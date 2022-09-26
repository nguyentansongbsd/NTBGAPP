using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class AcceptanceDetailPageViewModel : BaseViewModel
    {
        private AcceptanceModel _acceptance;
        public AcceptanceModel Acceptance { get => _acceptance; set { _acceptance = value; OnPropertyChanged(nameof(Acceptance)); } }
        public AcceptanceDetailPageViewModel()
        {
        }
        public async Task LoadAcceptance(Guid id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                       <condition attribute='bsd_acceptanceid' operator='eq' value='{id}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='project'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer' alias='product'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_contract' link-type='outer' alias='contract'>
                                        <attribute name='name' alias='contract_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AcceptanceModel>>("bsd_acceptances", fetchXml);
            if (result == null || result.value.Count == 0) return;
            Acceptance = result.value.SingleOrDefault();
        }
    }
}
