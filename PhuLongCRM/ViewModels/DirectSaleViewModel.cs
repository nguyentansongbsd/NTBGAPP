using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class DirectSaleViewModel : BaseViewModel
    {
        public ObservableCollection<ProjectList> Projects { get; set; } = new ObservableCollection<ProjectList>();
        public ObservableCollection<OptionSet> PhasesLaunchs { get; set; } = new ObservableCollection<OptionSet>();
        
        private List<OptionSetFilter> _viewOptions;
        public List<OptionSetFilter> ViewOptions { get => _viewOptions; set { _viewOptions = value;OnPropertyChanged(nameof(ViewOptions)); } }

        private List<Block> _blocks;
        public List<Block> Blocks { get => _blocks; set { _blocks = value; OnPropertyChanged(nameof(Blocks)); } }

        private List<OptionSetFilter> _directionOptions;
        public List<OptionSetFilter> DirectionOptions { get=>_directionOptions; set { _directionOptions = value;OnPropertyChanged(nameof(DirectionOptions)); } }

        private List<string> _selectedDirections;
        public List<string> SelectedDirections { get => _selectedDirections; set { _selectedDirections = value; OnPropertyChanged(nameof(SelectedDirections)); } }

        private List<string> _SelectedViews;
        public List<string> SelectedViews { get => _SelectedViews; set { _SelectedViews = value; OnPropertyChanged(nameof(SelectedViews)); } }

        private List<OptionSetFilter> _unitStatusOptions;
        public List<OptionSetFilter> UnitStatusOptions { get=>_unitStatusOptions; set { _unitStatusOptions = value;OnPropertyChanged(nameof(UnitStatusOptions)); } }

        private List<string> _selectedUnitStatus;
        public List<string> SelectedUnitStatus { get => _selectedUnitStatus; set { _selectedUnitStatus = value; OnPropertyChanged(nameof(SelectedUnitStatus)); } }

        private OptionSet _phasesLaunch;
        public OptionSet PhasesLaunch { get => _phasesLaunch; set { _phasesLaunch = value; OnPropertyChanged(nameof(PhasesLaunch)); } }

        private List<NetAreaDirectSaleModel> _netAreas;
        public List<NetAreaDirectSaleModel> NetAreas { get=>_netAreas; set { _netAreas = value; OnPropertyChanged(nameof(NetAreas)); } }

        private NetAreaDirectSaleModel _netArea;
        public NetAreaDirectSaleModel NetArea { get => _netArea; set { _netArea = value; OnPropertyChanged(nameof(NetArea)); } }

        private List<PriceDirectSaleModel> _prices;
        public List<PriceDirectSaleModel> Prices { get => _prices; set { _prices = value; OnPropertyChanged(nameof(Prices)); } }

        private PriceDirectSaleModel _price;
        public PriceDirectSaleModel Price { get => _price; set { _price = value; OnPropertyChanged(nameof(Price)); } }

        private string _unitCode;
        public string UnitCode { get => _unitCode; set { _unitCode = value; OnPropertyChanged(nameof(UnitCode)); } }

        private ProjectList _project;
        public ProjectList Project
        {
            get => _project;
            set
            {
                if (_project != value)
                {
                    PhasesLaunch = null;
                    PhasesLaunchs.Clear();
                    _project = value;
                    OnPropertyChanged(nameof(Project));
                    
                }
            }
        }

        private bool? _isEvent =false;
        public bool? IsEvent
        {
            get => _isEvent;
            set
            {
                if (_isEvent != value)
                {
                    this._isEvent = value;
                    OnPropertyChanged(nameof(IsEvent));
                }
            }
        }

        public DirectSaleViewModel()
        {
        }

        public async Task LoadProject()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid'/>
                                    <attribute name='bsd_projectcode'/>
                                    <attribute name='bsd_name'/>
                                    <attribute name='createdon' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                    </filter>
                                  </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("bsd_projects", fetchXml);
            if (result == null || result.value.Any() == false) return;

             var data = result.value;
            foreach (var item in data)
            {
                Projects.Add(item);
            }
        }

        public async Task LoadPhasesLanch()
        {
            if (Project == null) return;
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='bsd_phaseslaunch'>
                        <attribute name='bsd_name' alias='Label' />
                        <attribute name='bsd_phaseslaunchid' alias='Val' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                          <condition attribute='statecode' operator='eq' value='0' />
                          <condition attribute='statuscode' operator='eq' value='100000000' />
                          <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + Project.bsd_projectid + @"' />
                        </filter>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value;
            foreach (var item in data)
            {
                PhasesLaunchs.Add(item);
            }
        }

        public async Task LoadBlocks()
        {
            string conditionPhaselaunch = string.Empty;
            conditionPhaselaunch = this.PhasesLaunch != null ? $@"<link-entity name='product' from='bsd_blocknumber' to='bsd_blockid' link-type='inner' alias='al'>
                                      <filter type='and'>
                                        <condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='{this.PhasesLaunch.Val}' />
                                      </filter>
                                    </link-entity>" : "";

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                  <entity name='bsd_block'>
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_blockid' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{this.Project.bsd_projectid}' />
                                    </filter>
                                    {conditionPhaselaunch}
                                  </entity>
                                </fetch>";

            var block_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Block>>("bsd_blocks", fetchXml);
            if (block_result == null || block_result.value.Count == 0) return;

            this.Blocks = block_result.value;
        }
    }
}
