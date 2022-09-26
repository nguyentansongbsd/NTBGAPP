using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class UnitSpecificationListViewModel : ListViewBaseViewModel2<UnitSpecificationListModel>
    {
        public string Keyword { get; set; }
        public UnitSpecificationListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_unitsspecifications";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_unitsspecification'>
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_typeofunitspec' />
                                    <attribute name='bsd_unitsspecificationid' />
                                    <order attribute='bsd_project' descending='false' />
                                    <filter type='or'>
                                        <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                        <condition entityname='project' attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                        <condition entityname='unittype' attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='project'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' link-type='outer' alias='unittype'>
                                        <attribute name='bsd_name' alias='unittype_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            });
        }

    }
}
