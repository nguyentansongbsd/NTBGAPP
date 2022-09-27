using System;
using PhuLongCRM.Models;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class UnitHandoversViewModel : ListViewBaseViewModel2<UnitHandoversModel>
    {
        public string Keyword { get; set; }
        public UnitHandoversViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_handovers";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_handover'>
                                    <attribute name='bsd_handoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='ne' value='2' />
                                        <filter type='or'>
                                          <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                          <condition attribute='bsd_handovernumber' operator='like' value='%25{Keyword}%25' />
                                        </filter>
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a_3743f43dba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_96e2d45bba81e911a83b000d3a07be23'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            });
        }
    }
}
