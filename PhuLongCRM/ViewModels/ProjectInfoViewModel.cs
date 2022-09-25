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
using Xamarin.Forms;
namespace PhuLongCRM.ViewModels
{
    public class ProjectInfoViewModel : BaseViewModel
    {
        public ObservableCollection<CollectionData> Collections { get; set; } = new ObservableCollection<CollectionData>();
        public ObservableCollection<CollectionData> PdfFiles { get; set; } = new ObservableCollection<CollectionData>();

        public List<Photo> Photos { get; set; }
        private bool _showCollections = false;
        public bool ShowCollections { get => _showCollections; set { _showCollections = value; OnPropertyChanged(nameof(ShowCollections)); } }

        private int _totalMedia;
        public int TotalMedia { get => _totalMedia; set { _totalMedia = value; OnPropertyChanged(nameof(TotalMedia)); } }

        private int _totalPhoto;
        public int TotalPhoto { get => _totalPhoto; set { _totalPhoto = value; OnPropertyChanged(nameof(TotalPhoto)); } }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<ChartModel> unitChartModels { get; set; }
        public ObservableCollection<ChartModel> UnitChart { get; set; } = new ObservableCollection<ChartModel>();

        private ObservableCollection<QueuesModel> _listGiuCho;
        public ObservableCollection<QueuesModel> ListGiuCho { get => _listGiuCho; set { _listGiuCho = value; OnPropertyChanged(nameof(ListGiuCho)); } }

        private EventModel _event;
        public EventModel Event { get => _event; set { _event = value; OnPropertyChanged(nameof(Event)); } }

        private ProjectInfoModel _project;
        public ProjectInfoModel Project
        {
            get => _project;
            set
            {
                if (_project != value)
                { _project = value; OnPropertyChanged(nameof(Project)); }
            }
        }

        private OptionSet _projectType;
        public OptionSet ProjectType { get => _projectType; set { _projectType = value; OnPropertyChanged(nameof(ProjectType)); } }

        private OptionSet _propertyUsageType;
        public OptionSet PropertyUsageType { get => _propertyUsageType; set { _propertyUsageType = value; OnPropertyChanged(nameof(PropertyUsageType)); } }

        private OptionSet _handoverCoditionMinimum;
        public OptionSet HandoverCoditionMinimum { get => _handoverCoditionMinimum; set { _handoverCoditionMinimum = value; OnPropertyChanged(nameof(HandoverCoditionMinimum)); } }

        private bool _isShowBtnGiuCho;
        public bool IsShowBtnGiuCho { get => _isShowBtnGiuCho; set { _isShowBtnGiuCho = value; OnPropertyChanged(nameof(IsShowBtnGiuCho)); } }

        private int _numUnit = 0;
        public int NumUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(NumUnit)); } }

        private int _soGiuCho = 0;
        public int SoGiuCho { get => _soGiuCho; set { _soGiuCho = value; OnPropertyChanged(nameof(SoGiuCho)); } }

        private int _soDatCoc = 0;
        public int SoDatCoc { get => _soDatCoc; set { _soDatCoc = value; OnPropertyChanged(nameof(SoDatCoc)); } }

        private int _soHopDong = 0;
        public int SoHopDong { get => _soHopDong; set { _soHopDong = value; OnPropertyChanged(nameof(SoHopDong)); } }

        private int _soBangTinhGia = 0;
        public int SoBangTinhGia { get => _soBangTinhGia; set { _soBangTinhGia = value; OnPropertyChanged(nameof(SoBangTinhGia)); } }

        private bool _showMoreBtnGiuCho;
        public bool ShowMoreBtnGiuCho { get => _showMoreBtnGiuCho; set { _showMoreBtnGiuCho = value; OnPropertyChanged(nameof(ShowMoreBtnGiuCho)); } }

        private bool _isHasEvent;
        public bool IsHasEvent { get => _isHasEvent; set { _isHasEvent = value; OnPropertyChanged(nameof(IsHasEvent)); } }

        private int _chuanBi = 0;
        public int ChuanBi { get=>_chuanBi; set { _chuanBi = value;OnPropertyChanged(nameof(ChuanBi)); } }
        private int _sanSang = 0;
        public int SanSang { get => _sanSang; set { _sanSang = value;OnPropertyChanged(nameof(SanSang)); } }
        private int _giuCho = 0;
        public int GiuCho { get => _giuCho; set { _giuCho = value;OnPropertyChanged(nameof(GiuCho)); } }
        private int _datCoc = 0;
        public int DatCoc { get=>_datCoc; set { _datCoc = value;OnPropertyChanged(nameof(DatCoc)); } }
        private int _dongYChuyenCoc = 0;
        public int DongYChuyenCoc { get=>_dongYChuyenCoc; set { _dongYChuyenCoc = value;OnPropertyChanged(nameof(DongYChuyenCoc)); } }
        private int _daDuTienCoc = 0;
        public int DaDuTienCoc { get=>_daDuTienCoc; set { _daDuTienCoc = value;OnPropertyChanged(nameof(DaDuTienCoc)); } }
        private int _thanhToanDot1 = 0;
        public int ThanhToanDot1 { get => _thanhToanDot1 ; set { _thanhToanDot1 = value;OnPropertyChanged(nameof(ThanhToanDot1)); } }
        private int _daBan = 0;
        public int DaBan { get=>_daBan; set { _daBan = value;OnPropertyChanged(nameof(DaBan)); } }
        private int _booking = 0;
        public int Booking { get=>_booking; set { _booking = value;OnPropertyChanged(nameof(Booking)); } }
        private int _option = 0;
        public int Option { get=>_option; set { _option = value;OnPropertyChanged(nameof(Option)); } }
        private int _signedDA = 0;
        public int SignedDA { get=>_signedDA; set { _signedDA = value;OnPropertyChanged(nameof(SignedDA)); } }

        private int _qualified = 0;
        public int Qualified { get=>_qualified; set { _qualified = value; OnPropertyChanged(nameof(Qualified)); } }

        public bool IsLoadedGiuCho { get; set; }

        public int PageListGiuCho = 1;

        private ImageSource _ImageSource;
        public ImageSource ImageSource { get => _ImageSource; set { _ImageSource = value; OnPropertyChanged(nameof(ImageSource)); } }

        private StatusCodeModel _statusCode;
        public StatusCodeModel StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

        public bool IsHasPhasesLaunch { get; set; }

        public ProjectInfoViewModel()
        {
            ListGiuCho = new ObservableCollection<QueuesModel>();
        }

        public async Task LoadData()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_projectid' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_name' />
                                <attribute name='createdon' />
                                <attribute name='bsd_address' />
                                <attribute name='bsd_projecttype' />
                                <attribute name='bsd_propertyusagetype' />
                                <attribute name='bsd_depositpercentda' />
                                <attribute name='bsd_estimatehandoverdate' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_maintenancefeespercent' />
                                <attribute name='bsd_numberofmonthspaidmf' />
                                <attribute name='bsd_managementamount' />
                                <attribute name='bsd_bookingfee' />
                                <attribute name='bsd_depositamount' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_queuesperunit' />
                                <attribute name='bsd_unitspersalesman' />
                                <attribute name='bsd_queueunitdaysaleman' />
                                <attribute name='bsd_longqueuingtime' />
                                <attribute name='bsd_shortqueingtime' />
                                <attribute name='bsd_projectslogo'/>
                                <attribute name='bsd_queueproject'/>
                                <attribute name='bsd_printagreement'/>
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                                </filter>
                                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_8924f6d5b214e911a97f000d3aa04914'>
                                  <attribute name='bsd_name' alias='bsd_investor_name' />
                                  <attribute name='accountid' alias='bsd_investor_id' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectInfoModel>>("bsd_projects", FetchXml);
            if (result == null || result.value.Any() == false) return;
            Project = result.value.FirstOrDefault();
            this.StatusCode = ProjectStatusCodeData.GetProjectStatusCodeById(Project.statuscode);
            //await LoadAllCollection();
        }

        public async Task CheckEvent()
        {
            // ham check su kien hide/show cua du an (show khi du an dang trong thoi gian dien ra su kien, va trang thai la "Approved")
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_event'>
                                <attribute name='createdon' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_startdate' />
                                <attribute name='bsd_enddate' />
                                <attribute name='bsd_eventid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='eq' value='100000000' />
                                  <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{ProjectId}' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventModel>>("bsd_events", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value;
            foreach (var item in data)
            {
                if (item.bsd_startdate < DateTime.Now && item.bsd_enddate > DateTime.Now)
                {
                    IsHasEvent = true;
                    return;
                }
            }
        }

        public async Task LoadThongKe()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                  <entity name='product'>
                                    <attribute name='statuscode' groupby='true' alias='group'/>
                                    <attribute name='productid' aggregate='count' alias='count'/>
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='not-in'>
                                            <value>0</value>
                                        </condition>
                                        <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='{ProjectId}'/>
                                      </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CountChartModel>>("products", fetchXml);
            if (result == null || result.value.Any() == false)
            {
                IsShowBtnGiuCho = true;
                unitChartModels = new List<ChartModel>()
                {
                    new ChartModel {Category ="Giữ chỗ",Value=1},
                    new ChartModel { Category = "Đặt cọc", Value = 1 },
                    new ChartModel {Category ="Đồng ý chuyển cọc",Value=1 },
                    new ChartModel { Category = "Đã đủ tiền cọc", Value = 1 },
                    new ChartModel { Category = "Option", Value = 1 },
                    new ChartModel {Category ="Thanh toán đợt 1",Value=1},
                    new ChartModel { Category = "Signed D.A", Value = 1 },
                    new ChartModel { Category = "Qualified", Value = 1 },
                    new ChartModel { Category = "Đã bán", Value =  1},
                    new ChartModel {Category ="Chuẩn bị", Value=1},
                    new ChartModel { Category = "Sẵn sàng", Value = 1 },
                    new ChartModel { Category = "Booking", Value = 1 },
                };
                foreach (var item in unitChartModels)
                {
                    UnitChart.Add(item);
                }
            }
            else
            {
                IsShowBtnGiuCho = false;
                var data = result.value;
                foreach (var item in data)
                {
                    if (item.group == "1")
                        ChuanBi = item.count;
                    else if (item.group == "100000000")
                        SanSang = item.count;
                    else if (item.group == "100000004")
                        GiuCho = item.count;
                    else if (item.group == "100000006")
                        DatCoc = item.count;
                    else if (item.group == "100000005")
                        DongYChuyenCoc = item.count;
                    else if (item.group == "100000003")
                        DaDuTienCoc = item.count;
                    else if (item.group == "100000001")
                        ThanhToanDot1 = item.count;
                    else if (item.group == "100000002")
                        DaBan = item.count;
                    else if (item.group == "100000007")
                        Booking = item.count;
                    else if (item.group == "100000010")
                        Option = item.count;
                    else if (item.group == "100000009")
                        SignedDA = item.count;
                    else if (item.group == "100000008")
                        Qualified = item.count;
                    NumUnit += item.count;
                }

                unitChartModels = new List<ChartModel>()
                {
                    new ChartModel {Category ="Giữ chỗ",Value=GiuCho},
                    new ChartModel { Category = "Đặt cọc", Value = DatCoc },
                    new ChartModel {Category ="Đồng ý chuyển cọc",Value=DongYChuyenCoc },
                    new ChartModel { Category = "Đã đủ tiền cọc", Value = DaDuTienCoc },
                    new ChartModel { Category = "Option", Value = Option },
                    new ChartModel {Category ="Thanh toán đợt 1",Value=ThanhToanDot1},
                    new ChartModel { Category = "Signed D.A", Value = SignedDA },
                    new ChartModel { Category = "Qualified", Value = Qualified },
                    new ChartModel { Category = "Đã bán", Value =  DaBan},
                    new ChartModel {Category ="Chuẩn bị", Value=ChuanBi},
                    new ChartModel { Category = "Sẵn sàng", Value = SanSang },
                    new ChartModel { Category = "Booking", Value = Booking },
                };
                foreach (var item in unitChartModels)
                {
                    UnitChart.Add(item);
                }
            }
        }

        public async Task LoadThongKeGiuCho()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                  <entity name='opportunity'>
                                    <attribute name='name' aggregate='count' alias='count'/>
                                    <filter type='and'>
                                      <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{ProjectId}' />
                                      <condition attribute='statuscode' operator='in'>
                                        <value>100000000</value>
                                        <value>100000002</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CountChartModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            SoGiuCho = result.value.FirstOrDefault().count;
        }
        public async Task LoadThongKeHopDong()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                              <entity name='salesorder'>
                                <attribute name='name' aggregate='count' alias='count'/>
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='ne' value='100000006' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ad'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value ='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CountChartModel>>("salesorders", fetchXml);
            if (result == null || result.value.Any() == false) return;

            SoHopDong = result.value.FirstOrDefault().count;
        }
        public async Task LoadThongKeBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                              <entity name='quote'>
                                <attribute name='name' aggregate='count' alias='count'/>
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='eq' value='100000007' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ae'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CountChartModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;
            SoBangTinhGia = result.value.FirstOrDefault().count;
        }
        public async Task LoadThongKeDatCoc()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                              <entity name='quote'>
                                <attribute name='name' aggregate='count' alias='count'/>
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='in'>
                                    <value>100000000</value>
                                    <value>861450001</value>
                                    <value>861450002</value>
                                    <value>100000006</value>
                                    <value>3</value>
                                    <value>861450000</value>
                                  </condition>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ae'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CountChartModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;
            SoDatCoc = result.value.FirstOrDefault().count;
        }
        public async Task LoadGiuCho()
        {
            IsLoadedGiuCho = true;
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageListGiuCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' alias='customer_id'/>
                                <attribute name='bsd_bookingtime' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='opportunityid' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_queueforproject' />
                                <order attribute='bsd_bookingtime' descending='false' />
                                <filter type='and'>
                                    <condition attribute='bsd_project' operator='eq' value='{ProjectId}' />
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}'/>
                                  <condition attribute='statuscode' operator='in'>
                                    <value>100000002</value>
                                    <value>100000000</value>
                                  </condition>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_edc3f143ba81e911a83b000d3a07be23'>
                                    <attribute name='bsd_name' alias='project_name'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='parentaccountid' visible='false' link-type='outer' alias='a_87ea9a00777ee911a83b000d3a07fbb4'>
                                    <attribute name='name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='parentcontactid' visible='false' link-type='outer' alias='a_8eea9a00777ee911a83b000d3a07fbb4'>
                                    <attribute name='bsd_fullname' alias='contact_name'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_5025d361ba81e911a83b000d3a07be23'>
                                    <attribute name='name' alias='bsd_units_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            List<QueuesModel> data = result.value;
            ShowMoreBtnGiuCho = data.Count < 10 ? false : true;
            foreach (var item in data)
            {
                ListGiuCho.Add(item);
            }
        }
        public async Task LoadAllCollection()
        {
            if (ProjectId != null)
            {
                GetTokenResponse getTokenResponse = await LoginHelper.getSharePointToken();
                var client = BsdHttpClient.Instance();
                string name_folder = ProjectName + "_" + ProjectId.ToString().Replace("-", "");
                string fileListUrl = $"https://graph.microsoft.com/v1.0/drives/{Config.OrgConfig.Graph_ProjectID}/root:/{name_folder}:/children?$select=name,eTag";
                var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RetrieveMultipleApiResponse<SharePointGraphModel>>(body);

                    if (result == null || result.value.Any() == false)
                    {
                        ShowCollections = false;
                        return;
                    }
                    ShowCollections = true;
                    Photos = new List<Photo>();
                    List<SharePointGraphModel> list = result.value;
                    var videos = list.Where(x => x.type == "video").ToList();
                    var images = list.Where(x => x.type == "image").ToList();
                    var pdfs = list.Where(x => x.type == "pdf").ToList();
                    this.TotalMedia = videos.Count;
                    this.TotalPhoto = images.Count;

                    await Task.WhenAll(GetVideos(videos), GetImages(images), GetPdfs(pdfs));
                }
            }
        }
        public async Task LoadDataEvent()
        {
            if (ProjectId == Guid.Empty) return;

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
                                      <condition attribute='bsd_project' operator='eq' value='{ProjectId}' />
                                    </filter>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='outer' alias='ab'>
                                      <attribute name='bsd_name' alias='bsd_phaselaunch_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventModel>>("bsd_events", FetchXml);
            if (result == null || result.value.Any() == false) return;
            Event = result.value.FirstOrDefault();
            if (Event.bsd_startdate.HasValue && Event.bsd_enddate.HasValue)
            {
                Event.bsd_startdate = Event.bsd_startdate.Value.ToLocalTime();
                Event.bsd_enddate = Event.bsd_enddate.Value.ToLocalTime();
            }
        }
        public async Task CheckPhasesLaunch()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_phaseslaunch'>
                                    <attribute name='bsd_phaseslaunchid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_projectid' operator='eq' value='{this.ProjectId}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_phaseslaunchs", fetchXml);
            if (result != null && result.value.Count > 0)
                this.IsHasPhasesLaunch = true;
            else
                this.IsHasPhasesLaunch = false;
        }

        private async static Task<T> LoadFiles<T>(string url) where T : class
        {
            var result = await CrmHelper.RetrieveImagesSharePoint<T>(url);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private async Task GetVideos(List<SharePointGraphModel> data)
        {
            foreach (var item in data)
            {
                var result = await LoadFiles<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_ProjectID}/items/{item.id}/driveItem/thumbnails");
                if (result != null)
                {
                    string url = result.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                    this.Collections.Add(new CollectionData { Id = item.id, MediaSourceId = item.id, ImageSource = url, SharePointType = SharePointType.Video, Index = TotalMedia });
                }
            }
        }

        private async Task GetImages(List<SharePointGraphModel> data)
        {
            foreach (var item in data)
            {
                var result = await LoadFiles<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SP_ProjectID}/items/{item.id}/driveItem/thumbnails");
                if (result != null)
                {
                    string url = result.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                    this.Photos.Add(new Photo { URL = url });
                    this.Collections.Add(new CollectionData { Id = item.id, MediaSourceId = null, ImageSource = url, SharePointType = SharePointType.Image, Index = TotalMedia });
                }
            }
        }

        private async Task GetPdfs(List<SharePointGraphModel> data)
        {
            foreach (var item in data)
            {
                var result = await LoadFiles<GrapDownLoadUrlModel>($"{Config.OrgConfig.SP_ProjectID}/items/{item.id}/driveItem");
                if (result != null)
                {
                    string url = result.MicrosoftGraphDownloadUrl;
                    this.PdfFiles.Add(new CollectionData { Id = item.id, UrlPdfFile = url, PdfName = item.name });
                }
            }
        }
    }
}
