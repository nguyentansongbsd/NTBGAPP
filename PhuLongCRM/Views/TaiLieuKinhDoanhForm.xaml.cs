using PhuLongCRM.Config;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using PhuLongCRM.Controls;
using Xamarin.Essentials;
using PhuLongCRM.IServices;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaiLieuKinhDoanhForm : ContentPage
    {
        public Action<bool> CheckTaiLieuKinhDoanh;
        public TaiLieuKinhDoanhFormViewModel viewModel;

        public TaiLieuKinhDoanhForm(Guid literatureid)
        {
            InitializeComponent();
            BindingContext = viewModel = new TaiLieuKinhDoanhFormViewModel();
            viewModel.salesliteratureid = literatureid;
            Init();          
        }

        private async void Init()
        {
            await Task.WhenAll(viewModel.loadData(), viewModel.loadUnit(), viewModel.loadDoiThuCanhTranh());
            
            if (viewModel.TaiLieuKinhDoanh != null)
            {
                CheckTaiLieuKinhDoanh(true);
            }
            else
            {
                CheckTaiLieuKinhDoanh(false);
            }
        }
        
        private async void PdfFile_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                if (await Permissions.CheckStatusAsync<Permissions.StorageRead>() != PermissionStatus.Granted && await Permissions.CheckStatusAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
                {
                    var readPermision = await PermissionHelper.RequestPermission<Permissions.StorageRead>("Thư Viện", "PhuLongCRM cần quyền truy cập vào thư viện", PermissionStatus.Granted);
                    var writePermision = await PermissionHelper.RequestPermission<Permissions.StorageWrite>("Thư Viện", "PhuLongCRM cần quyền truy cập vào thư viện", PermissionStatus.Granted);
                }
                if (await Permissions.CheckStatusAsync<Permissions.StorageRead>() == PermissionStatus.Granted && await Permissions.CheckStatusAsync<Permissions.StorageWrite>() == PermissionStatus.Granted)
                {
                    LoadingHelper.Show();
                    var item = (SalesLiteratureItemListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                    byte[] arr = Convert.FromBase64String(item.documentbody);
                    await DependencyService.Get<IPDFSaveAndOpen>().SaveAndView(item.filename, arr);
                    LoadingHelper.Hide();
                }
            }
            catch(Exception ex)
            {
                LoadingHelper.Hide();
            }
            
        }

        private async void ShowMoreThongTinUnit_Clicked(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageThongTinUnit++;
            await viewModel.loadUnit();
            LoadingHelper.Hide();
        }

        private async void ShowMoreDuAnCanhTranh_Clicked(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDuAnCanhTranh++;
            await viewModel.loadDoiThuCanhTranh();
            LoadingHelper.Hide();
        }

        private async void ShowMoreTaiLieu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageTaiLieu++;
            await viewModel.loadAllSalesLiteratureIten();
            LoadingHelper.Hide();
        }

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    ThongTin.IsVisible = true;
                    ThongTinTaiLieu.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    if (viewModel.list_salesliteratureitem != null && viewModel.list_salesliteratureitem.Count <= 0)
                    {
                        LoadingHelper.Show();
                        await viewModel.loadAllSalesLiteratureIten();
                        LoadingHelper.Hide();
                    }
                    ThongTin.IsVisible = false;
                    ThongTinTaiLieu.IsVisible = true;
                }
            }
        }

        private void GoToUnit_Tapped(object sender, EventArgs e)
        {
            var id = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            LoadingHelper.Show();
            UnitInfo unit = new UnitInfo(id);
            unit.OnCompleted = async (isSuccess) => {
                if (isSuccess)
                {
                    await Navigation.PushAsync(unit);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
    }
}
