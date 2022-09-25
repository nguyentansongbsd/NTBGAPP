using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class AccountContentViewViewModel : ListViewBaseViewModel2<AccountListModel>
    {
        public string Keyword { get; set; }
        public string KeyFilter { get; set; }
        public AccountContentViewViewModel()
        {
            PreLoadData = new Command(() =>
            {
                string filter = string.Empty;
                if (!string.IsNullOrWhiteSpace(KeyFilter))
                {
                    if (KeyFilter == "1")
                        filter = "<condition attribute='statuscode' operator='eq' value='100000000' />";
                    else if (KeyFilter == "2")
                        filter = "<condition attribute='statuscode' operator='eq' value='1' />";
                    else
                        filter = string.Empty;
                }
                EntityName = "accounts";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='bsd_name' />
                    <attribute name='statuscode' />
                    <attribute name='telephone1' />
                    <attribute name='accountid' />
                    <attribute name='address1_composite' alias='bsd_address' />
                    <attribute name='bsd_postalcode' />
                    <attribute name='bsd_housenumberstreet' />
                    <attribute name='bsd_customercode' />
                    <order attribute='createdon' descending='true' />
                    <filter type='or'>
                        <condition attribute='name' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='telephone1' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='bsd_registrationcode' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='bsd_customercode' operator='like' value='%25{Keyword}%25' />
                    </filter>
                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='a'>
                         <attribute name='bsd_fullname' alias='primarycontact_name' />
                    </link-entity>
                    <link-entity name='new_district' from='new_districtid' to='bsd_district' link-type='outer' alias='af' >
                         <attribute name='new_name' alias='district_name' />                                       
                    </link-entity>
                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' link-type='outer' alias='ag'>
                         <attribute name='new_name' alias='province_name' />                                       
                    </link-entity>
                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_nation' link-type='outer' alias='as'>
                        <attribute name='bsd_name' alias='country_name' />                                      
                    </link-entity>
                    <filter type='and'>
                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                        {filter}
                       </filter>
                  </entity>
                </fetch>";
            });
        }
    }
}
