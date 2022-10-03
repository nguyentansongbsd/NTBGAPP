using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class UnitSpecificationDetailPageViewModel : BaseViewModel
    {
        private UnitSpecificationModel _unitSpec;
        public UnitSpecificationModel UnitSpec { get => _unitSpec; set { _unitSpec = value; OnPropertyChanged(nameof(UnitSpec)); } }

        private ObservableCollection<UnitSpecificationDetailModel> unitSpecificationDetails;
        public ObservableCollection<UnitSpecificationDetailModel> UnitSpecificationDetails { get => unitSpecificationDetails; set { unitSpecificationDetails = value; OnPropertyChanged(nameof(UnitSpecificationDetails)); } }
        public int Page { get; set; } = 1;

        private bool _showMore;
        public bool ShowMore { get => _showMore; set { _showMore = value; OnPropertyChanged(nameof(ShowMore)); } }
        public UnitSpecificationDetailPageViewModel()
        {
        }
        public async Task LoadUnitSpec(Guid id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_unitsspecification'>
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_typeofunitspec' />
                                    <attribute name='bsd_descriptionen' />
                                    <attribute name='bsd_descriptionvn' />
                                    <attribute name='bsd_unitsspecificationid' />
                                    <order attribute='bsd_project' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_unitsspecificationid' operator='eq' value='{id}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='project'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' link-type='outer' alias='unittype'>
                                        <attribute name='bsd_name' alias='unittype_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitSpecificationModel>>("bsd_unitsspecifications", fetchXml);
            if (result == null || result.value.Count == 0) return;
            UnitSpec = result.value.SingleOrDefault();
            await LoadUnitSpecificationDetails();
        }
        public async Task LoadUnitSpecificationDetails()
        {
            if (UnitSpecificationDetails == null)
                UnitSpecificationDetails = new ObservableCollection<UnitSpecificationDetailModel>();
            if (UnitSpec == null || UnitSpec.bsd_unitsspecificationid == Guid.Empty) return;
            string fetch = $@"<fetch count='5' page='{Page}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_unitsspecificationdetails'>
                                    <attribute name='bsd_typeofroomvn' />
                                    <attribute name='bsd_itemvn' />
                                    <attribute name='bsd_details' />
                                    <attribute name='bsd_typeno' />
                                    <attribute name='bsd_unitsspecificationdetailsid' />
                                    <order attribute='bsd_typeno' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_unitsspecification' operator='eq' value='{UnitSpec.bsd_unitsspecificationid}'/>
                                      <condition attribute='bsd_typeno' operator='not-null' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitSpecificationDetailModel>>("bsd_unitsspecificationdetailses", fetch);
            if (result != null && result.value.Count > 0)
            {
                foreach (var item in result.value)
                {
                    UnitSpecificationDetails.Add(item);
                }
            }
            ShowMore = result.value.Count == 5 ? true : false;
        }
    }
}
