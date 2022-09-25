using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class FollowUpListPageViewModel : ListViewBaseViewModel2<FollowUpModel>
    {
        public string Keyword { get; set; }
        public FollowUpListPageViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_followuplists";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_followuplist'>
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_type' />
                                    <attribute name='bsd_followuplistcode' />
                                    <attribute name='bsd_followuplistid' />
                                    <attribute name='bsd_expiredate' />
                                    <order attribute='createdon' descending='true' /> 
                                    <filter type='and'>
                                        <filter type='or'>
                                          <condition attribute='bsd_unitsname' operator='like' value='%25{Keyword}%25' />
                                          <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                          <condition attribute='bsd_followuplistcode' operator='like' value='%25{Keyword}%25' />
                                          <condition entityname='customer_reservation' attribute='customeridname' operator='like' value='%25{Keyword}%25' />
                                          <condition entityname='customer_optionentry' attribute='customeridname' operator='like' value='%25{Keyword}%25' />
                                        </filter>
                                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='customer_reservation'>
                                        <attribute name='name' alias='name_reservation'/>
                                        <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='aa'>
                                            <attribute name='bsd_name' alias='account_name_oe'/>
                                        </link-entity>
                                        <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ab'>
                                            <attribute name='bsd_fullname' alias='contact_name_oe'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='customer_optionentry'>
                                        <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ad'>
                                            <attribute name='name' alias='account_name_re'/>
                                        </link-entity>
                                        <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ae'>
                                            <attribute name='bsd_fullname' alias='contact_name_re'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer' alias='af'>
                                      <attribute name='name' alias='bsd_units'/>     
                                    </link-entity>
                                  </entity>
                                </fetch>";
            });
        }
    }
}
