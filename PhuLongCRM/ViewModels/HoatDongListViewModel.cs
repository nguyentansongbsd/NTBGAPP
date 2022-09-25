using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class HoatDongListViewModel : ListViewBaseViewModel2<HoatDongListModel>
    {
        public string Keyword { get; set; }
        public HoatDongListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "activitypointers";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' count='15' page='{Page}'>
              <entity name='activitypointer'>
                <attribute name='subject' />
                <attribute name='scheduledstart' />
                <attribute name='regardingobjectid' />
                <attribute name='prioritycode' />
                <attribute name='scheduledend' />
                <attribute name='activitytypecode' />
                <attribute name='instancetypecode' />
                <attribute name='community' />
                <attribute name='createdon' />
                <attribute name='statecode' />
                <attribute name='ownerid' />
                <attribute name='activityid' />
                <order attribute='createdon' descending='true' />
                <order attribute='scheduledend' descending='true' />
                <filter type='and'>
                  <condition attribute='statecode' operator='in'>
                    <value>0</value>
                    <value>3</value>
                    <value>1</value>
                    <value>2</value>
                  </condition>
                  <condition attribute='isregularactivity' operator='eq' value='1' />
                </filter>
                <filter type='and'>
                   <condition attribute='subject' operator='like' value='%{Keyword}%' />
                </filter>
                <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='aa'>
                  <filter type='and'>
                    <condition attribute='partyid' operator='eq-userid' />
                  </filter>
                </link-entity>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='accounts'>
                  <attribute name='bsd_name' alias='accounts_bsd_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                  <attribute name='fullname' alias='contact_bsd_fullname'/>
                </link-entity>
                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='leads'>
                  <attribute name='fullname' alias='lead_fullname'/>
                </link-entity>
                <link-entity name='bsd_systemsetup' from='bsd_systemsetupid' to='regardingobjectid' visible='false' link-type='outer' alias='users'>
                    <attribute name='bsd_name' alias='systemsetup_bsd_name'/>
                </link-entity>
                <link-entity name='systemuser' from='systemuserid' to='owninguser' visible='false' link-type='outer' alias='owners'>
                  <attribute name='fullname' alias='owners_fullname'/>
                </link-entity>
              </entity>
            </fetch>";
            });
        }
    }
}
