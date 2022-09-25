using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class FollowUpListFormViewModel : BaseViewModel
    {
        private FollowUpModel _fulDetail;
        public FollowUpModel FULDetail { get => _fulDetail; set { _fulDetail = value; OnPropertyChanged(nameof(FULDetail)); } }

        private List<StatusCodeModel> _listType;
        public List<StatusCodeModel> ListType { get => _listType; set { _listType = value; OnPropertyChanged(nameof(ListType)); } }

        private StatusCodeModel _type;
        public StatusCodeModel Type { get => _type; set { _type = value; OnPropertyChanged(nameof(Type)); } }

        private List<StatusCodeModel> _listTypeTerminateletter;
        public List<StatusCodeModel> ListTypeTerminateletter { get => _listTypeTerminateletter; set { _listTypeTerminateletter = value; OnPropertyChanged(nameof(ListTypeTerminateletter)); } }

        private StatusCodeModel _typeTerminateletter;
        public StatusCodeModel TypeTerminateletter { get => _typeTerminateletter; set { _typeTerminateletter = value; OnPropertyChanged(nameof(TypeTerminateletter)); } }

        private List<StatusCodeModel> _listGroup;
        public List<StatusCodeModel> ListGroup { get => _listGroup; set { _listGroup = value; OnPropertyChanged(nameof(ListGroup)); } }

        private StatusCodeModel _group;
        public StatusCodeModel Group { get => _group; set { _group = value; OnPropertyChanged(nameof(Group)); } }

        private List<StatusCodeModel> _listTakeOutMoney;
        public List<StatusCodeModel> ListTakeOutMoney { get => _listTakeOutMoney; set { _listTakeOutMoney = value; OnPropertyChanged(nameof(ListTakeOutMoney)); } }

        private StatusCodeModel _takeOutMoney;
        public StatusCodeModel TakeOutMoney { get => _takeOutMoney; set { _takeOutMoney = value; OnPropertyChanged(nameof(TakeOutMoney)); } }

        private decimal _refund;
        public decimal Refund { get => _refund; set { _refund = value; OnPropertyChanged(nameof(Refund)); } }

        private List<LookUp> _listPhaseLaunch;
        public List<LookUp> ListPhaseLaunch { get => _listPhaseLaunch; set { _listPhaseLaunch = value; OnPropertyChanged(nameof(ListPhaseLaunch)); } }

        private LookUp _phaseLaunch;
        public LookUp PhaseLaunch { get => _phaseLaunch; set { _phaseLaunch = value; OnPropertyChanged(nameof(PhaseLaunch)); } }

        private List<LookUp> _listMeeting;
        public List<LookUp> ListMeeting { get => _listMeeting; set { _listMeeting = value; OnPropertyChanged(nameof(ListMeeting)); } }

        private LookUp _meeting;
        public LookUp Meeting { get => _meeting; set { _meeting = value; OnPropertyChanged(nameof(Meeting)); } }
        public FollowUpListFormViewModel()
        {
            ListType = new List<StatusCodeModel>();
            ListTypeTerminateletter = new List<StatusCodeModel>();
            ListGroup = new List<StatusCodeModel>();
            ListTakeOutMoney = new List<StatusCodeModel>();
            ListPhaseLaunch = new List<LookUp>();
            ListMeeting = new List<LookUp>();
        }

        public async Task LoadFUL(Guid fulid)
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
                                    <attribute name='bsd_depositfee' />
                                    <attribute name='bsd_description' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                       <condition attribute='bsd_followuplistid' operator='eq' value='{fulid}'/>
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
                                            <attribute name='salesorderid' alias='bsd_optionentry_id'/>
                                            <attribute name='name' alias='name_optionentry' />
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
                                        <attribute name='bsd_projectcode' alias='project_code'/>
                                        <attribute name='bsd_projectid' alias='project_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='outer' alias='al'>
                                        <attribute name='bsd_phaseslaunchid' alias='phaseslaunch_id'/>                                        
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                    </link-entity>
                                    <link-entity name='appointment' from='activityid' to='bsd_collectionmeeting' link-type='outer' alias='ah'>
                                        <attribute name='subject' alias='bsd_collectionmeeting_subject'/>
                                        <attribute name='activityid' alias='bsd_collectionmeeting_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<FollowUpModel>>("bsd_followuplists", fetchXml);
            if (result == null || result.value.Count == 0)
                return;
            FULDetail = result.value.FirstOrDefault();

            if (FULDetail.phaseslaunch_id != Guid.Empty)
            {
                PhaseLaunch = new LookUp { Id = FULDetail.phaseslaunch_id, Name = FULDetail.phaseslaunch_name };
            }

            if (FULDetail.bsd_collectionmeeting_id != Guid.Empty)
            {
                Meeting = new LookUp { Id = FULDetail.bsd_collectionmeeting_id, Name = FULDetail.bsd_collectionmeeting_subject };
            }

            if (FULDetail.bsd_takeoutmoney == 100000000)
                Refund = FULDetail.bsd_forfeitureamount;
            else if (FULDetail.bsd_takeoutmoney == 100000001)
                Refund = FULDetail.bsd_forfeiturepercent;

            if (string.IsNullOrWhiteSpace(FULDetail.bsd_name))
            {
                string name = "";
                if (!string.IsNullOrWhiteSpace(FULDetail.bsd_units))
                    name += FULDetail.bsd_units;
                if (!string.IsNullOrWhiteSpace(FULDetail.project_code))
                    name += "-" + FULDetail.project_code;

                if (!string.IsNullOrWhiteSpace(FULDetail.name_reservation))
                    name += "-" + FULDetail.name_reservation;
                else if (!string.IsNullOrWhiteSpace(FULDetail.name_optionentry))
                    name += "-" + FULDetail.name_optionentry;

                if (!string.IsNullOrWhiteSpace(FULDetail.account_name_re))
                    name += "-" + FULDetail.account_name_re;
                else if (!string.IsNullOrWhiteSpace(FULDetail.contact_name_re))
                    name += "-" + FULDetail.contact_name_re;
                else if (!string.IsNullOrWhiteSpace(FULDetail.account_name_oe))
                    name += "-" + FULDetail.account_name_oe;
                else if (!string.IsNullOrWhiteSpace(FULDetail.contact_name_oe))
                    name += "-" + FULDetail.contact_name_oe;

                if (Group != null && !string.IsNullOrWhiteSpace(Group.Id))
                    name += "-" + Group.Name;
                FULDetail.bsd_name = name;
            }
        }

        public async Task LoadPhaseLaunch(Guid projectid)
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_phaseslaunch'>
                                <attribute name='bsd_name' alias='Name'/>
                                <attribute name='bsd_phaseslaunchid' alias='Id'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectid' operator='eq' value='{projectid}' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_phaseslaunchs", fetch);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            foreach (var x in result.value)
            {
                ListPhaseLaunch.Add(x);
            }
        }

        public async Task LoadMeeting(Guid fulid)
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='appointment'>
                                    <attribute name='subject' alias='Name'/>
                                    <attribute name='activityid' alias='Id' />
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{fulid}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{fulid}' />
                                        </filter>
                                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("appointments", fetch);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            foreach (var x in result.value)
            {
                ListMeeting.Add(x);
            }
        }

        public async Task<Boolean> updateFUL()
        {
            string path = "/bsd_followuplists(" + FULDetail.bsd_followuplistid + ")";
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<Boolean> createFUL()
        {
            string path = "/bsd_followuplists";
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid FULid)
        {
            var result = await CrmHelper.SetNullLookupField("bsd_followuplists", FULid, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_name"] = FULDetail.bsd_name;
            data["bsd_type"] = Type.Id;
            data["bsd_terminationtype"] = TypeTerminateletter.Id;

            if (Group != null && !string.IsNullOrWhiteSpace(Group.Id))
                data["bsd_group"] = Group.Id;

            data["bsd_takeoutmoney"] = TakeOutMoney.Id;

            if (TakeOutMoney.Id == "100000000")
            {
                data["bsd_forfeitureamount"] = Refund;
                data["bsd_forfeiturepercent"] = 0;
            }
            else if (TakeOutMoney.Id == "100000001")
            {
                data["bsd_forfeiturepercent"] = Refund;
                data["bsd_forfeitureamount"] = 0;
            }
            data["bsd_totalforfeitureamount_new"] = FULDetail.bsd_totalforfeitureamount_new;

            data["bsd_terminateletter"] = FULDetail.bsd_terminateletter;
            data["bsd_termination"] = FULDetail.bsd_termination;
            data["bsd_resell"] = FULDetail.bsd_resell;

            if (FULDetail.bsd_resell) //bsd_phaselaunch
                data["bsd_PhaseLaunch@odata.bind"] = "/bsd_phaseslaunchs(" + PhaseLaunch.Id + ")";
            else
                await DeletLookup("bsd_PhaseLaunch", FULDetail.bsd_followuplistid);

            if (Meeting != null && Meeting.Id != Guid.Empty)
                data["bsd_collectionmeeting@odata.bind"] = "/appointments(" + Meeting.Id + ")";
            else
                await DeletLookup("bsd_collectionmeeting", FULDetail.bsd_followuplistid);

            data["bsd_description"] = FULDetail.bsd_description;
            data["bsd_salecomment"] = FULDetail.bsd_salecomment;

            data["bsd_followuplistid"] = FULDetail.bsd_followuplistid;
            if (FULDetail.project_id != Guid.Empty)
            {
                data["bsd_project@odata.bind"] = "/bsd_projects(" + FULDetail.project_id + ")";
            }
            data["bsd_date"] = FULDetail.bsd_date;

            data["bsd_reservation@odata.bind"] = "/quotes(" + FULDetail.bsd_reservation_id + ")";
            data["bsd_depositfee"] = FULDetail.bsd_depositfee;
            if (FULDetail.product_id != Guid.Empty)
            {
                data["bsd_Units@odata.bind"] = "/products(" + FULDetail.product_id + ")";
            }
            if (UserLogged.Id != Guid.Empty && !UserLogged.IsLoginByUserCRM)
            {
                data["bsd_Employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty && !UserLogged.IsLoginByUserCRM)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            data["bsd_sellingprice"] = FULDetail.bsd_sellingprice;
            data["bsd_totalamount"] = FULDetail.bsd_totalamount;
            data["bsd_totalamountpaid"] = FULDetail.bsd_totalamountpaid;
            data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies(2366fb85-b881-e911-a83b-000d3a07be23)"; // Don vi tien te mac dinh la "đ"
            return data;
        }
    }
}
