using Newtonsoft.Json;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class UnitInfoViewModel : BaseViewModel
    {
        private List<CollectionData> _collections;
        public List<CollectionData> Collections { get => _collections; set { _collections = value; OnPropertyChanged(nameof(Collections)); } }

        public List<Photo> Photos;

        private int _totalMedia;
        public int TotalMedia { get => _totalMedia; set { _totalMedia = value; OnPropertyChanged(nameof(TotalMedia)); } }

        private int _totalPhoto;
        public int TotalPhoto { get => _totalPhoto; set { _totalPhoto = value; OnPropertyChanged(nameof(TotalPhoto)); } }

        private bool _showCollections;
        public bool ShowCollections { get => _showCollections; set { _showCollections = value; OnPropertyChanged(nameof(ShowCollections)); } }
        public Guid UnitId { get; set; }

        private ObservableCollection<UnitHandoversModel> _unitHandovers;
        public ObservableCollection<UnitHandoversModel> UnitHandovers { get => _unitHandovers; set { _unitHandovers = value; OnPropertyChanged(nameof(UnitHandovers)); } }

        public ObservableCollection<AcceptanceListModel> _acceptances;
        public ObservableCollection<AcceptanceListModel> Acceptances { get => _acceptances; set { _acceptances = value; OnPropertyChanged(nameof(Acceptances)); } }
        public ObservableCollection<PinkBookHandoversModel> PinkBooHandovers { get; set; } = new ObservableCollection<PinkBookHandoversModel>();
        public ObservableCollection<ContractModel> list_danhsachhopdong { get; set; } = new ObservableCollection<ContractModel>();

        private UnitInfoModel _unitInfo;
        public UnitInfoModel UnitInfo { get => _unitInfo; set { _unitInfo = value; OnPropertyChanged(nameof(UnitInfo)); } }

        private OptionSet _diretion;
        public OptionSet Direction { get => _diretion; set { _diretion = value; OnPropertyChanged(nameof(Direction)); } }

        private string _view;
        public string View { get => _view; set { _view = value; OnPropertyChanged(nameof(View)); } }

        private StatusCodeModel _statusCode;
        public StatusCodeModel StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

        private bool _showMoreAcceptances;
        public bool ShowMoreAcceptances { get => _showMoreAcceptances; set { _showMoreAcceptances = value; OnPropertyChanged(nameof(ShowMoreAcceptances)); } }

        private bool _showMorePinkBooHandover;
        public bool ShowMorePinkBooHandover { get => _showMorePinkBooHandover; set { _showMorePinkBooHandover = value; OnPropertyChanged(nameof(ShowMorePinkBooHandover)); } }

        private bool _showMoreDanhSachHopDong;
        public bool ShowMoreDanhSachHopDong { get => _showMoreDanhSachHopDong; set { _showMoreDanhSachHopDong = value; OnPropertyChanged(nameof(ShowMoreDanhSachHopDong)); } }

        private bool _isShowBtnBangTinhGia;
        public bool IsShowBtnBangTinhGia { get => _isShowBtnBangTinhGia; set { _isShowBtnBangTinhGia = value; OnPropertyChanged(nameof(IsShowBtnBangTinhGia)); } }

        private bool _showMoreUnitHandovers;
        public bool ShowMoreUnitHandovers { get => _showMoreUnitHandovers; set { _showMoreUnitHandovers = value; OnPropertyChanged(nameof(ShowMoreUnitHandovers)); } }

        public int PageDanhSachHopDong { get; set; } = 1;
        public int PagePinkBooHandover { get; set; } = 1;
        public int PageAcceptance { get; set; } = 1;
        public int PageUnitHandover { get; set; } = 1;

        public bool IsLoaded { get; set; } = false;

        public bool IsVip { get; set; } = false;

        private EventModel _event;
        public EventModel Event { get => _event; set { _event = value; OnPropertyChanged(nameof(Event)); } }

        public UnitInfoViewModel()
        {
            Acceptances = new ObservableCollection<AcceptanceListModel>();
            UnitHandovers = new ObservableCollection<UnitHandoversModel>();
            Photos = new List<Photo>();
            Collections = new List<CollectionData>();
        }

        public async Task LoadUnit()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='product'>
                <attribute name='productid' />
                <attribute name='bsd_units' />
                <attribute name='name' />
                <attribute name='productnumber' />
                <attribute name='bsd_queuingfee' />
                <attribute name='bsd_depositamount' />
                <attribute name='statuscode' />
                <attribute name='bsd_areavariance' />
                <attribute name='bsd_constructionarea' />
                <attribute name='bsd_netsaleablearea' />
                <attribute name='price' />
                <attribute name='bsd_landvalueofunit' />
                <attribute name='bsd_landvalue' />
                <attribute name='bsd_maintenancefeespercent' />
                <attribute name='bsd_maintenancefees' />
                <attribute name='bsd_taxpercent'/>
                <attribute name='bsd_vat' />
                <attribute name='bsd_estimatehandoverdate' />
                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementamountmonth' />
                <attribute name='bsd_handovercondition' />
                <attribute name='bsd_direction' />
                <attribute name='bsd_vippriority' />
                <attribute name='bsd_viewphulong' />
                <attribute name='bsd_authenticationmanagementfee'/>
                <attribute name='bsd_acceptance' />
                <attribute name='bsd_handoverdate' />
                <attribute name='bsd_opdate' />
                <attribute name='bsd_submitpinkbookdate' />
                <attribute name='bsd_confirmdocument' />
                <attribute name='bsd_pinkbooknumber' />
                <attribute name='bsd_pinkbookreceiptdate' />
                <attribute name='bsd_complete' />
                <attribute name='bsd_phaseslaunchid' alias='bsd_phaseslaunch_id' />
                <filter type='and'>
                  <condition attribute='productid' operator='eq' uitype='product' value='" + UnitId.ToString() + @"' />
                </filter>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_a77d98e66ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_project_name' />
                </link-entity>
                <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='a_4d73a1e06ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_floor_name' />
                </link-entity>
                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' visible='false' link-type='outer' alias='a_290ca3da6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_block_name' />
                </link-entity>
                <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' visible='false' link-type='outer' alias='a_493690ec6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name'  alias='bsd_unittype_name'/>
                  <attribute name='bsd_unittypeid' alias='bsd_unittype_value'/>
                </link-entity>
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='outer' alias='ac'>
                  <link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='outer' alias='ad'>
                    <attribute name='bsd_eventid' alias='event_id' />
                    <filter type='and'>
                        <condition attribute='statuscode' operator='eq' value='100000000' />
                        <condition attribute='bsd_eventid' operator='not-null' />
                    </filter>
                  </link-entity>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitInfoModel>>("products", fetchXml);
            if (result == null || result.value.Count == 0) return;
            UnitInfo = result.value.FirstOrDefault();
            await LoadAllCollection();
        }

        public async Task LoadAcceptances()
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageAcceptance}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <condition attribute='bsd_units' operator='eq' value='{UnitInfo.productid}' />
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

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AcceptanceListModel>>("bsd_acceptances", fetch);
            if (result == null || result.value.Count == 0) return;

            IsLoaded = true;
            var data = result.value;
            ShowMoreAcceptances = data.Count < 5 ? false : true;

            foreach (var item in data)
            {
                Acceptances.Add(item);
            }
        }

        public async Task LoadUnitHandovers()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageUnitHandover}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_handover'>
                                    <attribute name='bsd_handoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <condition attribute='bsd_units' operator='eq' value='{UnitInfo.productid}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a_3743f43dba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_96e2d45bba81e911a83b000d3a07be23'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitHandoversModel>>("bsd_handovers", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ShowMoreUnitHandovers = result.value.Count > 4 ? true : false;

            foreach (var item in result.value)
            {
                this.UnitHandovers.Add(item);
            }
        }

        public async Task LoadPinkBooHandovers()
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PagePinkBooHandover}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_pinkbookhandover'>
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_pinkbookhandoverid' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <condition attribute='bsd_units' operator='eq' value='{UnitInfo.productid}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_26f8767ec690ec11b400000d3aa1f0ac'>
                                      <attribute name='bsd_contactfullname' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_91124d44c790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_fd36f62dc790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='optionentry_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PinkBookHandoversModel>>("bsd_pinkbookhandovers", fetch);
            if (result == null) return;
            IsLoaded = true;
            var data = result.value;
            ShowMorePinkBooHandover = data.Count < 5 ? false : true;

            foreach (var x in data)
            {
                PinkBooHandovers.Add(x);
            }
        }

        public async Task LoadOptoinEntry()
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageDanhSachHopDong}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' />
                                    <attribute name='customerid' />
                                    <attribute name='statuscode' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_unitnumber' alias='unit_id'/>
                                    <attribute name='bsd_project' alias='project_id'/>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <attribute name='bsd_contractnumber' />
                                    <order attribute='bsd_project' descending='true' />
                                    <filter type='and'>                                      
                                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                        <condition attribute='bsd_unitnumber' operator='eq' value='{UnitInfo.productid}'/>                
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
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetch);
            if (result == null) return;

            IsLoaded = true;
            var data = result.value;
            ShowMoreDanhSachHopDong = data.Count < 5 ? false : true;

            foreach (var x in data)
            {
                list_danhsachhopdong.Add(x);
            }
        }
        public async Task CheckShowBtnBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                  <entity name='bsd_phaseslaunch'>
                                    <attribute name='bsd_name' />
                                    <link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='inner'>
                                      <filter type='and'>
                                        <condition attribute='bsd_enddate' operator='on-or-after' value='{DateTime.Now.ToString("yyyy-MM-dd")}' />
                                        <condition attribute='bsd_startdate' operator='on-or-before' value='{DateTime.Now.ToString("yyyy-MM-dd")}' />
                                        <condition attribute='statuscode' operator='eq' value='100000000' />
                                      </filter>
                                    </link-entity>
                                    <link-entity name='product' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='inner'>
                                      <filter type='and'>
                                        <condition attribute='productid' operator='eq' value='{UnitId}' />
                                        <condition attribute='statuscode' operator='in'>
                                          <value>100000000</value>
                                          <value>100000004</value>
                                        </condition>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLanchModel>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            if(result.value.Count > 0)
                IsShowBtnBangTinhGia = true;
            else
                IsShowBtnBangTinhGia = false;
        }

        public async Task LoadAllCollection()
        {
            if (UnitInfo != null)
            {
                GetTokenResponse getTokenResponse = await LoginHelper.getSharePointToken();
                var client = BsdHttpClient.Instance();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // load unit type
                string name_folder_unitype = UnitInfo.bsd_unittype_name + "_" + UnitInfo.bsd_unittype_value.ToString().Replace("-", "").ToUpper();
                string fileUrl1 = $"https://graph.microsoft.com/v1.0/drives/{Config.OrgConfig.Graph_UnitTypeID}/root:/{name_folder_unitype}:/children?$select=name,eTag";
                var request_unitype = new HttpRequestMessage(HttpMethod.Get, fileUrl1);
                var response_unitype = await client.SendAsync(request_unitype);
                if (response_unitype.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response_unitype.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RetrieveMultipleApiResponse<SharePointGraphModel>>(body);

                    List<SharePointGraphModel> list = result.value;
                    var videos = list.Where(x => x.type == "video").ToList();
                    var images = list.Where(x => x.type == "image").ToList();
                    this.TotalMedia += videos.Count;
                    this.TotalPhoto += images.Count;

                    foreach (var item in videos)
                    {
                        var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_UnitTypeID}/items/{item.id}/driveItem/thumbnails");
                        if (urlVideo != null)
                        {
                            string url = urlVideo.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                            Collections.Add(new CollectionData { Id = item.id, MediaSourceId = item.id, ImageSource = url, SharePointType = SharePointType.Video, Index = TotalMedia });
                        }
                    }
                    foreach (var item in images)
                    {
                        var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_UnitTypeID}/items/{item.id}/driveItem/thumbnails");
                        if (urlVideo != null)
                        {
                            string url = urlVideo.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                            this.Photos.Add(new Photo { URL = url });
                            Collections.Add(new CollectionData { Id = item.id, MediaSourceId = null, ImageSource = url, SharePointType = SharePointType.Image, Index = TotalMedia });
                        }
                    }
                }

                //load hinh anh unit
                string name_folder = UnitInfo.name.Replace(".", "-") + "_" + UnitInfo.productid.ToString().Replace("-", "").ToUpper();
                string fileListUrl = $"https://graph.microsoft.com/v1.0/drives/{Config.OrgConfig.Graph_UnitID}/root:/{name_folder}:/children?$select=name,eTag";
                var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);
                var response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RetrieveMultipleApiResponse<SharePointGraphModel>>(body);

                    List<SharePointGraphModel> list = result.value;
                    var videos = list.Where(x => x.type == "video").ToList();
                    var images = list.Where(x => x.type == "image").ToList();
                    this.TotalMedia += videos.Count;
                    this.TotalPhoto += images.Count;

                    foreach (var item in videos)
                    {
                        var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_UnitID}/items/{item.id}/driveItem/thumbnails");
                        if (urlVideo != null)
                        {
                            string url = urlVideo.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                            Collections.Add(new CollectionData { Id = item.id, MediaSourceId = item.id, ImageSource = url, SharePointType = SharePointType.Video, Index = TotalMedia });
                        }
                    }
                    foreach (var item in images)
                    {
                        var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_UnitID}/items/{item.id}/driveItem/thumbnails");
                        if (urlVideo != null)
                        {
                            string url = urlVideo.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                            this.Photos.Add(new Photo { URL = url });
                            Collections.Add(new CollectionData { Id = item.id, MediaSourceId = null, ImageSource = url, SharePointType = SharePointType.Image, Index = TotalMedia });
                        }
                    }
                }
                if (Collections.Count > 0)
                    ShowCollections = true;
                else
                    ShowCollections = false;
            }
        }

        public async Task LoadDataEvent()
        {
            if (UnitInfo == null || UnitInfo.event_id == Guid.Empty) return;

            string FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_event'>
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_eventcode' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_eventid' />
                                    <order attribute='bsd_eventcode' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000000' />
                                      <condition attribute='bsd_eventid' operator='eq' value='{UnitInfo.event_id}' />
                                    </filter>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='outer' alias='ab'>
                                      <attribute name='bsd_name' alias='bsd_phaselaunch_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventModel>>("bsd_events", FetchXml);
            if (result == null || result.value.Any() == false) return;
            var data = result.value.FirstOrDefault();
            Event = data;
            if (data.bsd_startdate != null && data.bsd_enddate != null)
            {
                Event.bsd_startdate = data.bsd_startdate.Value.ToLocalTime();
                Event.bsd_enddate = data.bsd_enddate.Value.ToLocalTime();
            }
        }
    }
}
