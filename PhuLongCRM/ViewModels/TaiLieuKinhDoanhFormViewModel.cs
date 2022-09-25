using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class TaiLieuKinhDoanhFormViewModel : BaseViewModel
    {
        public Guid salesliteratureid { get; set; }

        public ObservableCollection<ListInforUnitTLKD> list_thongtinunit { get; set; }
        public ObservableCollection<ListInforDoithucanhtranhTLKD> list_thongtinduancanhtranh { get; set; }
        private ObservableCollection<SalesLiteratureItemListModel> _list_salesliteratureitem;
        public ObservableCollection<SalesLiteratureItemListModel> list_salesliteratureitem { get => _list_salesliteratureitem; set { _list_salesliteratureitem = value; OnPropertyChanged(nameof(list_salesliteratureitem)); } }

        private TaiLieuKinhDoanhFormModel _TaiLieuKinhDoanhFormModel;
        public TaiLieuKinhDoanhFormModel TaiLieuKinhDoanh { get => _TaiLieuKinhDoanhFormModel; set { _TaiLieuKinhDoanhFormModel = value; OnPropertyChanged(nameof(TaiLieuKinhDoanh)); } }

        private bool _showMoreThongTinUnit;
        public bool ShowMoreThongTinUnit { get => _showMoreThongTinUnit; set { _showMoreThongTinUnit = value; OnPropertyChanged(nameof(ShowMoreThongTinUnit)); } }

        private bool _showMoreDuAnCanhTranh;
        public bool ShowMoreDuAnCanhTranh { get => _showMoreDuAnCanhTranh; set { _showMoreDuAnCanhTranh = value; OnPropertyChanged(nameof(ShowMoreDuAnCanhTranh)); } }

        private bool _showMoreTaiLieu;
        public bool ShowMoreTaiLieu { get => _showMoreTaiLieu; set { _showMoreTaiLieu = value; OnPropertyChanged(nameof(ShowMoreTaiLieu)); } }

        public int PageThongTinUnit { get; set; } = 1;
        public int PageDuAnCanhTranh { get; set; } = 1;
        public int PageTaiLieu { get; set; } = 1;

        public TaiLieuKinhDoanhFormViewModel()
        {
            list_thongtinunit = new ObservableCollection<ListInforUnitTLKD>();
            list_thongtinduancanhtranh = new ObservableCollection<ListInforDoithucanhtranhTLKD>();
            list_salesliteratureitem = new ObservableCollection<SalesLiteratureItemListModel>();
        }

        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='salesliterature'>
                                 <all-attributes/>
                                 <order attribute='name' descending='false' />
                                 <filter type='and'>
                                    <condition attribute='salesliteratureid' operator='eq' value='" + salesliteratureid + @"' />
                                 </filter>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjecttitle'/>
                                </link-entity>
                              </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaiLieuKinhDoanhFormModel>>("salesliteratures", xml);
            var data = result.value.FirstOrDefault();
            if (data == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            this.TaiLieuKinhDoanh = data;
        }

        public async Task loadUnit()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageThongTinUnit}' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='product'>
                                <attribute name='productid' />
                                <attribute name='bsd_units' />
                                <attribute name='name' />
                                <attribute name='productnumber' />
                                <attribute name='statuscode' />
                                <link-entity name='productsalesliterature' intersect='true' visible='false' to='productid' from='productid'>
                                    <link-entity name='salesliterature' to='salesliteratureid' from='salesliteratureid' alias='ab'>
                                        <filter type='and'>
                                            <condition attribute='salesliteratureid' value='" + salesliteratureid + @"' uitype='salesliterature' operator='eq'/>
                                        </filter>
                                    </link-entity>
                                </link-entity>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' >
                                  <attribute name='bsd_name' alias='bsd_projectname'/>
                                </link-entity>
                                </entity>
                             </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListInforUnitTLKD>>("products", fetch);

            var data = result.value;

            ShowMoreThongTinUnit = data.Count < 3 ? false : true;

            if (data.Any())
            {
                foreach (var item in data)
                {
                    this.list_thongtinunit.Add(item);
                }
            }
        }

        public async Task loadDoiThuCanhTranh()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDuAnCanhTranh}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='competitor'>
                                  <all-attributes/>
                                  <link-entity name='competitorsalesliterature' intersect='true' visible='false' to='competitorid' from='competitorid'>
                                      <link-entity name='salesliterature' to='salesliteratureid' from='salesliteratureid' alias='ab'>
                                          <filter type='and'>
                                              <condition attribute='salesliteratureid' value='" + salesliteratureid + @"' uitype='salesliterature' operator='eq'/>
                                          </filter>
                                      </link-entity>
                                  </link-entity>
                                </entity>
                             </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListInforDoithucanhtranhTLKD>>("competitors", fetch);

            var data = result.value;
            ShowMoreDuAnCanhTranh = data.Count < 3 ? false : true;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    this.list_thongtinduancanhtranh.Add(item);
                }
            }
        }

        public async Task loadAllSalesLiteratureIten()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageTaiLieu}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesliteratureitem'>
                                    <attribute name='title' alias='title_label' />
                                    <attribute name='modifiedon' />
                                    <attribute name='salesliteratureitemid' />
                                    <attribute name='filename' />
                                    <attribute name='documentbody' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='salesliteratureid' operator='eq' value='{salesliteratureid}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SalesLiteratureItemListModel>>("salesliteratureitems", fetch);

            var data = result.value;
            ShowMoreTaiLieu = data.Count < 3 ? false : true;

            if (data.Any())
            {
                foreach (var item in data)
                {
                    this.list_salesliteratureitem.Add(item);
                }
            }
        }
    }
}

