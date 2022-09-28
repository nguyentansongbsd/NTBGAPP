using System;
using PhuLongCRM.Models;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class PinkBooHandoversViewModel : ListViewBaseViewModel2<PinkBookHandoversModel>
    {
        public string Keyword { get; set; }
        public PinkBooHandoversViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_pinkbookhandovers";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_pinkbookhandover'>
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_pinkbookhandoverid' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <filter type='or'>
                                            <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                            <condition attribute='bsd_handovernumber' operator='like' value='%25{Keyword}%25' />
                                        </filter>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_26f8767ec690ec11b400000d3aa1f0ac'>
                                      <attribute name='bsd_contactfullname' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_91124d44c790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_fd36f62dc790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='optionentry_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            });
        }
    }
}
