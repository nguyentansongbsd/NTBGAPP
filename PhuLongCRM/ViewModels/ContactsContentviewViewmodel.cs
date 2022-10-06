using System;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class ContactsContentviewViewmodel : ListViewBaseViewModel2<ContactListModel>
    {
        public string Keyword { get; set; }
        public string KeyFilter { get; set; }
        public ContactsContentviewViewmodel()
        {
            PreLoadData = new Command(() =>
            {
                string filter = string.Empty;
                if (!string.IsNullOrWhiteSpace(KeyFilter))
                {
                    if (KeyFilter == "0")
                        filter = null;
                    else if (KeyFilter == "1")
                        filter = "<condition attribute='statuscode' operator='eq' value='100000000' />";
                    else if (KeyFilter == "2")
                        filter = "<condition attribute='statuscode' operator='eq' value='1' />";
                    else
                        filter = string.Empty;
                }
                EntityName = "contacts";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='bsd_fullname' />
                    <attribute name='mobilephone' />
                    <attribute name='birthdate' />
                    <attribute name='emailaddress1' />
                    <attribute name='bsd_contactaddress' />
                    <attribute name='createdon' />
                    <attribute name='contactid' />
                    <attribute name='statuscode' />
                    <attribute name='bsd_customercode' />
                    <order attribute='createdon' descending='true' />
                    <filter type='or'>
                        <condition attribute='bsd_fullname' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='bsd_identitycardnumber' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='bsd_identitycard' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='bsd_passport' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='mobilephone' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='emailaddress1' operator='like' value='%25{Keyword}%25' />
                        <condition attribute='bsd_customercode' operator='like' value='%25{Keyword}%25' />
                    </filter>
                    <filter type='and'>
                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                      {filter}
                    </filter>
                  </entity>
                </fetch>";
            });
        }
    }
}
