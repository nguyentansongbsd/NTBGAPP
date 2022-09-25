using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;

namespace PhuLongCRM.Helper
{
    // chỉ sử dụng cho activity meet
    public class MeetCustomerHelper
    {
        public static async Task<string> MeetCustomer(Guid activityid)
        {
            if (activityid == null || activityid == Guid.Empty) return null;

            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='appointment'>
                                    <filter type='and'>
                                        <condition attribute='activityid' operator='eq' value='{activityid}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='5' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='required_contact'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='required_account'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='required_lead'/>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<RequiredMeetModel>>("appointments", fetch);
            if (result == null || result.value.Count == 0) return null;

            var data = result.value;
            List<string> customer = new List<string>();
            foreach (var item in data)
            {
                customer.Add(item.required);
            }
            return string.Join(", ", customer);
        }
    }
}
