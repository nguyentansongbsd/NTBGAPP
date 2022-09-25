using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;

namespace PhuLongCRM.ViewModels
{
    public class QueuesDetialPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();
        public Guid QueueId { get; set; }
        public string NumPhone { get; set; }

        private ObservableCollection<ReservationListModel> _bangTinhGiaList;
        public ObservableCollection<ReservationListModel> BangTinhGiaList { get => _bangTinhGiaList; set { _bangTinhGiaList = value; OnPropertyChanged(nameof(BangTinhGiaList)); } }

        private ObservableCollection<ReservationListModel> _datCocList;
        public ObservableCollection<ReservationListModel> DatCocList { get => _datCocList; set { _datCocList = value; OnPropertyChanged(nameof(DatCocList)); } }

        private ObservableCollection<ContractModel> _hopDongList;
        public ObservableCollection<ContractModel> HopDongList { get => _hopDongList; set { _hopDongList = value; OnPropertyChanged(nameof(HopDongList)); } }

        private QueuesDetailModel _queue;
        public QueuesDetailModel Queue { get => _queue; set { _queue = value; OnPropertyChanged(nameof(Queue)); } }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private QueuesStatusCodeModel _queueStatusCode;
        public QueuesStatusCodeModel QueueStatusCode { get => _queueStatusCode; set { _queueStatusCode = value; OnPropertyChanged(nameof(QueueStatusCode)); } }
        public ObservableCollection<HoatDongListModel> list_thongtincase { get; set; }

        private string _queueProject;
        public string QueueProject { get => _queueProject; set { _queueProject = value; OnPropertyChanged(nameof(QueueProject)); } }

        private bool _showBtnHuyGiuCho;
        public bool ShowBtnHuyGiuCho { get => _showBtnHuyGiuCho; set { _showBtnHuyGiuCho = value; OnPropertyChanged(nameof(ShowBtnHuyGiuCho)); } }

        private bool _showBtnBangTinhGia;
        public bool ShowBtnBangTinhGia { get => _showBtnBangTinhGia; set { _showBtnBangTinhGia = value; OnPropertyChanged(nameof(ShowBtnBangTinhGia)); } }

        private bool _showButtons;
        public bool ShowButtons { get => _showButtons; set { _showButtons = value; OnPropertyChanged(nameof(ShowButtons)); } }

        private bool _showMoreBangTinhGia;
        public bool ShowMoreBangTinhGia { get => _showMoreBangTinhGia; set { _showMoreBangTinhGia = value; OnPropertyChanged(nameof(ShowMoreBangTinhGia)); } }

        private bool _showMoreDatCoc;
        public bool ShowMoreDatCoc { get => _showMoreDatCoc; set { _showMoreDatCoc = value; OnPropertyChanged(nameof(ShowMoreDatCoc)); } }

        private bool _showMoreHopDong;
        public bool ShowMoreHopDong { get => _showMoreHopDong; set { _showMoreHopDong = value; OnPropertyChanged(nameof(ShowMoreHopDong)); } }

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        private bool _showCare;
        public bool ShowCare { get => _showCare; set { _showCare = value; OnPropertyChanged(nameof(ShowCare)); } }

        public int PageBangTinhGia { get; set; } = 1;
        public int PageDatCoc { get; set; } = 1;
        public int PageHopDong { get; set; } = 1;
        public int PageCase { get; set; } = 1;

        public string CodeAccount = LookUpMultipleTabs.CodeAccount;

        public string CodeContact = LookUpMultipleTabs.CodeContac;

        public QueuesDetialPageViewModel()
        {
            list_thongtincase = new ObservableCollection<HoatDongListModel>();
        }

        public async Task LoadQueue()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_bookingtime' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='opportunityid' />
                                <attribute name='bsd_queuingfee' />
                                <attribute name='budgetamount' />
                                <attribute name='description' />
                                <attribute name='bsd_nameofstaffagent' />
                                <attribute name='bsd_project' />
                                <attribute name='bsd_units' alias='_bsd_units_value'/>
                                <attribute name='bsd_phaselaunch' />
                                <attribute name='bsd_salesagentcompany' />
                                <attribute name='bsd_priorityqueue' />
                                <attribute name='bsd_prioritynumber' />
                                <attribute name='bsd_ordernumber' />
                                <attribute name='bsd_dateorder' />
                                <attribute name='statecode' />
                                <attribute name='bsd_expired' />
                                <attribute name='bsd_queuingfeepaid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='opportunityid' operator='eq' value='{QueueId}'/>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_805e44d019dbeb11bacb002248168cad'>
                                  <attribute name='bsd_name'  alias='project_name'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_ba2436e819dbeb11bacb002248168cad'>
                                    <attribute name='name'  alias='unit_name'/>
                                    <attribute name='statuscode'  alias='unit_status'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_434f5ec290d1eb11bacc000d3a80021e'>
                                    <attribute name='bsd_name' alias='account_name'/>
                                    <attribute name='accountid' alias='account_id'/>
                                    <attribute name='telephone1' alias='PhoneAccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_884f5ec290d1eb11bacc000d3a80021e'>
                                    <attribute name='bsd_fullname' alias='contact_name' />
                                    <attribute name='contactid' alias='contact_id'/>
                                    <attribute name='mobilephone' alias='PhoneContact'/>
                                </link-entity>
                                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' visible='false' link-type='outer' alias='a_485347ca19dbeb11bacb002248168cad'>
                                  <attribute name='bsd_name' alias='phaselaunch_name'/>
                                  <attribute name='bsd_phaseslaunchid' alias='bsd_phaseslaunch_id'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='bsd_salesagentcompany' visible='false' link-type='outer' alias='a_ab034cb219dbeb11bacb002248168cad'>
                                  <attribute name='bsd_name' alias='salesagentcompany_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='bsd_customerreferral' link-type='outer' alias='aa'>
                                   <attribute name='bsd_fullname' alias='customerreferral_name' />
                                   <attribute name='contactid' alias='customerreferral_id'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='bsd_collaborator' link-type='outer' alias='ab'>
                                   <attribute name='bsd_fullname' alias='collaborator_name' />
                                   <attribute name='contactid' alias='collaborator_id'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesDetailModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(data.account_name))
            {
                Customer = new OptionSet() { Val= data.account_id.ToString(), Label = data.account_name, Title= CodeAccount };
            }
            else if (!string.IsNullOrWhiteSpace(data.contact_name))
            {
                Customer = new OptionSet() { Val = data.contact_id.ToString(), Label = data.contact_name, Title = CodeContact }; ;
            }

            if (!string.IsNullOrWhiteSpace(data.PhoneAccount))
            {
                NumPhone = data.PhoneAccount;
            }
            else if (!string.IsNullOrWhiteSpace(data.PhoneContact))
            {
                NumPhone = data.PhoneContact;
            }

            if (data.unit_name != null)
            {
                QueueProject = Language.khong;
            }
            else
            {
                QueueProject = Language.co;
            }

            ShowBtnHuyGiuCho = (data.statuscode == 100000000 || data.statuscode == 100000002) ? true : false;
//            ShowBtnBangTinhGia = (data.statuscode == 100000000 && !string.IsNullOrWhiteSpace(data.phaselaunch_name)) ? true : false;
            ShowButtons = (data.statuscode == 100000008 || data.statuscode == 100000009 || data.statuscode == 100000010) ? false : true; //data.statuscode == 100000008 ||
            ShowCare = (data.statuscode == 1 || data.statuscode == 4 || data.statuscode == 5 || data.statuscode == 100000003 || data.statuscode == 100000008) ? false : true;
            this.QueueStatusCode = QueuesStatusCodeData.GetQueuesById(data.statuscode.ToString());
            this.Queue = data;
        }

        public async Task<bool> CheckReserve()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' alias='Label'/>
                        <filter type='and'>
                            <condition attribute='opportunityid' operator='like'  value='{this.Queue.opportunityid}' />
                            <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                            <condition attribute='statuscode' operator='in'>
                                   <value>100000000</value>
                                   <value>100000001</value>
                                   <value>100000006</value>
                                   <value>3</value>
                                   <value>4</value>
                               </condition>
                        </filter>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("quotes", fetchXml);
            if (result == null) return false;
            if (result.value.Any() == false && this.Queue.statuscode == 100000000)
            {
                return ShowBtnBangTinhGia = (Queue.statuscode == 100000000 && !string.IsNullOrWhiteSpace(Queue.phaselaunch_name)) ? true : false;
                //return true;
            }
            return false;
        }

        public async Task LoadDanhSachBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageBangTinhGia}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' />
                        <attribute name='totalamount' />
                        <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_projectid' alias='bsd_project_id' />
                        <attribute name='quoteid' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                            <condition attribute='opportunityid' operator='like'  value='{this.Queue.opportunityid}' />
                            <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                            <filter type='or'>
                              <condition attribute='statuscode' operator='in'>
                                <value>100000007</value>
                              </condition>
                              <filter type='and'>
                                 <condition attribute='statuscode' operator='in'>
                                    <value>100000009</value>
                                    <value>6</value>
                                  </condition>
                                  <condition attribute='bsd_quotationsigneddate' operator='null' />
                              </filter>
                            </filter>
                        </filter>
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                            <attribute name='bsd_name' alias='bsd_project_name' />
                        </link-entity>
                        <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                          <attribute name='name' alias='bsd_unitno_name' />
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                          <attribute name='bsd_name' alias='purchaser_accountname' />
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                          <attribute name='bsd_fullname' alias='purchaser_contactname' />
                        </link-entity>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ShowMoreBangTinhGia = result.value.Count > 4 ? true : false;

            foreach (var item in result.value)
            {
                this.BangTinhGiaList.Add(item);
            }
        }

        public async Task LoadDanhSachDatCoc()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageDatCoc}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' />
                        <attribute name='totalamount' />
                        <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_projectid' alias='bsd_project_id' />
                        <attribute name='quoteid' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                            <condition attribute='opportunityid' operator='like'  value='{this.Queue.opportunityid}' />
                            <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                             <filter type='or'>
                               <condition attribute='statuscode' operator='in'>
                                   <value>100000000</value>
                                   <value>100000001</value>
                                   <value>100000006</value>
                                   <value>861450001</value>
                                   <value>861450002</value>
                                   <value>4</value>                
                                   <value>3</value>
                               </condition>
                               <filter type='and'>
                                   <condition attribute='statuscode' operator='in'>
                                       <value>100000009</value>
                                       <value>6</value>
                                   </condition>
                                   <condition attribute='bsd_quotationsigneddate' operator='not-null' />
                               </filter>
                             </filter>
                        </filter>
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                            <attribute name='bsd_name' alias='bsd_project_name' />
                        </link-entity>
                        <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                          <attribute name='name' alias='bsd_unitno_name' />
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                          <attribute name='bsd_name' alias='purchaser_accountname' />
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                          <attribute name='bsd_fullname' alias='purchaser_contactname' />
                        </link-entity>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ShowMoreDatCoc = result.value.Count > 4 ? true : false;

            foreach (var item in result.value)
            {
                this.DatCocList.Add(item);
            }
        }

        public async Task LoadDanhSachHopDong()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageHopDong}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='salesorder'>
                                    <attribute name='name' />
                                    <attribute name='customerid' />
                                    <attribute name='statuscode' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_unitnumber' alias='unit_id'/>
                                    <attribute name='bsd_project' alias='project_id'/>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <order attribute='bsd_project' descending='true' />
                                    <filter type='and'>                                      
                                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                        <condition attribute='opportunityid' operator='eq' value='{this.Queue.opportunityid}'/>                
                                    </filter >
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='aa'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitnumber' link-type='outer' alias='ab'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ac'>
                                        <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ad'>
                                        <attribute name='bsd_fullname' alias='contact_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.ShowMoreHopDong = result.value.Count > 4 ? true : false;
            foreach (var item in result.value)
            {
                this.HopDongList.Add(item);
            }
        }

        public async Task<bool> CheckQuote()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='statuscode' />
                                    <attribute name='quoteid' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='not-in'>
                                        <value>2</value>
                                        <value>6</value>
                                      </condition>
                                      <condition attribute='opportunityid' operator='eq' uitype='opportunity' value='{QueueId}' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesDetailModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return false;

            var data = result.value.FirstOrDefault();
            if (data.statuscode == 100000000 || data.statuscode == 100000006 || data.statuscode == 861450001 || data.statuscode == 861450002)
                return true;
            else
                return false;
        }

        public async Task updateStatusUnit()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            string fetchSalesorder = @"<fetch>
                                  <entity name='salesorder'>
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <link-entity name='product' from='productid' to='bsd_unitnumber'>
                                      <filter>
                                        <condition attribute='productid' operator='eq' value='{" + Queue._bsd_units_value + @"}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var resultSalesorder = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetchSalesorder);
            if (resultSalesorder != null && resultSalesorder.value.Count > 0)
                return;

            string fetchQuote = @"<fetch>
                                  <entity name='quote'>
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <link-entity name='product' from='productid' to='bsd_unitno'>
                                      <filter>
                                        <condition attribute='productid' operator='eq' value='{" + Queue._bsd_units_value + @"}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var resultQuote = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationDetailPageModel>>("quotes", fetchQuote);
            if (resultQuote != null && resultQuote.value.Count > 0)
            {
                if (resultQuote.value.FirstOrDefault(x => x.statuscode == 3 || x.statuscode == 861450000) != null)
                {
                    data["statecode"] = 0;
                    data["statuscode"] = 100000003;
                }
                else if (resultQuote.value.FirstOrDefault(x => x.statuscode == 4) != null)
                {
                    data["statecode"] = 0;
                    data["statuscode"] = 100000010;
                }
            }
            else
            {
                string fetchQueue = @"<fetch>
                                          <entity name='opportunity'>
                                            <attribute name='name' />
                                            <attribute name='statuscode' />
                                            <attribute name='bsd_phaselaunch' alias='_bsd_phaselaunch_value' />
                                            <filter type='and'>
                                              <condition attribute='bsd_units' operator='eq' value='{" + Queue._bsd_units_value + @"}'/>
                                            </filter>
                                          </entity>
                                        </fetch>";
                var resultQueue = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesDetailModel>>("opportunities", fetchQueue);
                if (resultQueue != null && resultQueue.value.Count > 0)
                {
                    if (resultQueue.value.FirstOrDefault(x => x.statuscode == 100000000 || x.statuscode == 100000008) != null)
                    {
                        data["statecode"] = 0;
                        data["statuscode"] = 100000004;
                    }
                    else if (resultQueue.value.FirstOrDefault(x => x.statuscode == 100000002) != null)
                    {
                        data["statecode"] = 0;
                        data["statuscode"] = 100000007;
                    }
                    else
                    {
                        var queue = resultQueue.value.FirstOrDefault(x => x.statuscode == 4 || x.statuscode == 100000007 || x.statuscode == 100000009);
                        if (queue != null && queue._bsd_phaselaunch_value != Guid.Empty)
                        {
                            data["statecode"] = 0;
                            data["statuscode"] = 100000000;
                        }
                        else if (resultQueue.value.FirstOrDefault(x => x.statuscode == 4 || x.statuscode == 100000007) != null)
                        {
                            data["statecode"] = 0;
                            data["statuscode"] = 1;
                        }
                    }
                }
            }

            string path = "/products(" + Queue._bsd_units_value + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
        }
        public async Task LoadCaseForQueue()
        {
            if (list_thongtincase != null && Queue != null && Queue.opportunityid != Guid.Empty)
            {
                await Task.WhenAll(
                    LoadActiviy(Queue.opportunityid, "task", "tasks"),
                    LoadActiviy(Queue.opportunityid, "phonecall", "phonecalls"),
                    LoadActiviy(Queue.opportunityid, "appointment", "appointments")
                );
            }
            ShowMoreCase = list_thongtincase?.Count < (3 * PageCase) ? false : true;
        }
        public async Task LoadActiviy(Guid queueID, string entity, string entitys)
        {
            string forphonecall = null;
            if (entity == "phonecall")
            {
                forphonecall = @"<link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>";
            }

            string fetch = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='{entity}'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='regardingobjectid' operator='eq' value='{queueID}' />
                                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' link-type='outer' alias= 'aaff'>
                                        <attribute name='name' alias='queue_name'/>
                                    </link-entity>
                                    {forphonecall}
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>(entitys, fetch);
            if (result == null || result.value.Count == 0) return;

            var data = result.value;
            foreach (var x in data)
            {
                list_thongtincase.Add(x);
            }
        }
    }
}
