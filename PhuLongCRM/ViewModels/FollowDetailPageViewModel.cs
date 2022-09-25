using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class FollowDetailPageViewModel : BaseViewModel
    {
        private FollowUpModel _followDetail;
        public FollowUpModel FollowDetail { get => _followDetail; set { _followDetail = value; OnPropertyChanged(nameof(FollowDetail)); } }

        public FollowDetailPageViewModel()
        {
            FollowDetail = new FollowUpModel();
        }
        public async Task Load(Guid id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_followuplist'>
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_followuplistcode' />
                                    <attribute name='bsd_followuplistid' />
                                    <attribute name='bsd_expiredate' />
                                    <attribute name='bsd_type' />
                                    <attribute name='bsd_terminationtype' />
                                    <attribute name='bsd_group' />
                                    <attribute name='bsd_sellingprice' />
                                    <attribute name='bsd_totalamount' />
                                    <attribute name='bsd_totalamountpaid' />
                                    <attribute name='bsd_totalforfeitureamount' />
                                    <attribute name='bsd_forfeitureamount' />
                                    <attribute name='bsd_takeoutmoney' />
                                    <attribute name='bsd_forfeiturepercent' />
                                    <attribute name='bsd_terminateletter' />
                                    <attribute name='bsd_termination' />
                                    <attribute name='bsd_resell' />
                                    <attribute name='bsd_description' />
                                    <attribute name='bsd_depositfee' />
                                    <attribute name='bsd_salecomment' />
                                    <attribute name='bsd_totalforfeitureamount_new' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                       <condition attribute='bsd_followuplistid' operator='eq' value='{id}'/>
                                    </filter>
                                    <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='customer_reservation'>
                                        <attribute name='name' alias='name_reservation'/>
                                        <attribute name='quoteid' alias='bsd_reservation_id' />
                                        <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='aa'>
                                            <attribute name='bsd_name' alias='account_name_oe'/>
                                            <attribute name='accountid' alias='account_id_oe'/>
                                        </link-entity>
                                        <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ab'>
                                            <attribute name='bsd_fullname' alias='contact_name_oe'/>
                                            <attribute name='contactid' alias='contact_id_oe'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='customer_optionentry'>
                                        <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ad'>
                                            <attribute name='name' alias='account_name_re'/>
                                            <attribute name='accountid' alias='account_id_re'/>
                                        </link-entity>
                                        <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ae'>
                                            <attribute name='bsd_fullname' alias='contact_name_re'/>
                                            <attribute name='contactid' alias='contact_id_re'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer' alias='af'>
                                      <attribute name='name' alias='bsd_units'/>    
                                      <attribute name='productid' alias='product_id' />
                                    </link-entity>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='ag'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                        <attribute name='bsd_projectid' alias='project_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='outer' alias='al'>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                    </link-entity>
                                    <link-entity name='appointment' from='activityid' to='bsd_collectionmeeting' link-type='outer' alias='ah'>
                                        <attribute name='subject' alias='bsd_collectionmeeting_subject'/>
                                        <attribute name='activityid' alias='bsd_collectionmeeting_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<FollowUpModel>>("bsd_followuplists", fetchXml);
            if (result != null)
            {
                FollowDetail = result.value.FirstOrDefault();
                if (FollowDetail.bsd_forfeitureamount != 0)
                    FollowDetail.isRefund = true;
                else
                    FollowDetail.isRefund = false;

                if (FollowDetail.bsd_forfeiturepercent != 0)
                    FollowDetail.isForfeiture = true;
                else
                    FollowDetail.isForfeiture = false;
            }
        }
    }
}
