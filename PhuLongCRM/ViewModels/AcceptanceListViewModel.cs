using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class AcceptanceListViewModel : ListViewBaseViewModel2<AcceptanceListModel>
    {
        public string Keyword { get; set; }
        public AcceptanceListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_acceptances";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='ne' value='2' />
                                        <filter type='or'>
                                            <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                            <condition entityname='project' attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                            <condition entityname='product' attribute='name' operator='like' value='%25{Keyword}%25' />
                                            <condition entityname='contract' attribute='name' operator='like' value='%25{Keyword}%25' />
                                        </filter>
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
            });
        }
    }
}
