using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhanHoiDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private Guid CaseId;
        PhanHoiDetailPageViewModel viewModel;
        public static bool? NeedToRefresh = null;
        public PhanHoiDetailPage(Guid id)
        {
            InitializeComponent();
            LoadingHelper.Show();
            CaseId = id;
            BindingContext = viewModel = new PhanHoiDetailPageViewModel();
            centerModalUpdateCase.Body.BindingContext = viewModel;
            NeedToRefresh = false;
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(CaseId);
            SetPreOpen();
            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_phan_hoi, "FontAwesomeRegular", "\uf273", null, CancelCase));
            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.giai_quyet_phan_hoi, "FontAwesomeRegular", "\uf274", null, CompletedCase));
            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.cap_nhat, "FontAwesomeRegular", "\uf044", null,Update));

            if (viewModel.Case.incidentid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadCase(CaseId);
                LoadingHelper.Hide();
                NeedToRefresh = false;
            }    
        }

        public void SetPreOpen()
        {
            Lookup_ResolutionType.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ResolutionTypes = CaseStatusCodeData.GetCaseStatusCodeByIds(new List<string>() {"5","1000"});
                LoadingHelper.Hide();
            };
            Lookup_BillableTime.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.BillableTimes = CaseBillableTime.CaseBillableTimeData();
                LoadingHelper.Hide();
            };
        }

        // tab thong tin
        private async Task LoadDataThongTin(Guid Id)
        {
            if (Id != Guid.Empty && viewModel.Case.incidentid == Guid.Empty)
            {
                await viewModel.LoadCase(Id);
            }
        }

        // phan hoi lien quan
        private async Task LoadDataPhanHoiLienQuan(Guid Id)
        {
            if (viewModel.ListCase != null && viewModel.ListCase.Count <= 0)
            {
                await viewModel.LoadListCase(Id);
            }
        }

        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadListCase(CaseId);
            LoadingHelper.Hide();
        }

        private void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            PhanHoiForm newPage = new PhanHoiForm(viewModel.Case.incidentid);
            newPage.CheckPhanHoi = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(newPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private async void CancelCase(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool confirm = await DisplayAlert(Language.xac_nhan_huy_phan_hoi, Language.ban_co_muon_huy_phan_hoi_nay_khong, Language.co, Language.khong);
            if (confirm)
            {
                viewModel.Case.statecode = 2;
                viewModel.Case.statuscode = 6;
                if (await viewModel.UpdateCase())
                {
                    await viewModel.LoadCase(viewModel.Case.incidentid);
                    if (ListPhanHoi.NeedToRefresh.HasValue) ListPhanHoi.NeedToRefresh = true;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                }
            }
            else
            {
                LoadingHelper.Hide();
            }
        }

        private async void CompletedCase(object sender, EventArgs e)
        {
            await centerModalUpdateCase.Show();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await centerModalUpdateCase.Hide();
        }

        private async void Confirm_Clicked(object sender, EventArgs e)
        {
            if(viewModel.ResolutionType == null || viewModel.ResolutionType.Val == string.Empty)
            {
                ToastMessageHelper.ShortMessage(Language.chua_chon_huong_giai_quyet);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.subject))
            {
                ToastMessageHelper.ShortMessage(Language.chua_nhap_phuong_an);
                return;
            }

            if (viewModel.BillableTime == null || viewModel.BillableTime.Val == string.Empty)
            {
                ToastMessageHelper.ShortMessage(Language.chua_chon_thoi_gian_co_the_thanh_toan);
                return;
            }

            if (await viewModel.UpdateCaseResolution())
            {
                await viewModel.LoadCase(viewModel.Case.incidentid);
                if (ListPhanHoi.NeedToRefresh.HasValue) ListPhanHoi.NeedToRefresh = true;
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.phan_hoi_da_duoc_giai_quyet);
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
            await centerModalUpdateCase.Hide();
        }

        private async void MoLaiPhanHoi_Clicked(object sender, EventArgs e)
        {
            viewModel.Case.statecode = 0;
            viewModel.Case.statuscode = 1;
            if (await viewModel.UpdateCase())
            {
                await viewModel.LoadCase(viewModel.Case.incidentid);
                if (ListPhanHoi.NeedToRefresh.HasValue) ListPhanHoi.NeedToRefresh = true;
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.da_mo_lai_phan_hoi);
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
        }

        private void Customer_Tapped(object sender, EventArgs e)
        {
            if(viewModel.Case != null)
            {
                LoadingHelper.Show();
                if (!string.IsNullOrWhiteSpace(viewModel.Case.accountId))
                {
                    AccountDetailPage newPage = new AccountDetailPage(Guid.Parse(viewModel.Case.accountId));
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
                else if (!string.IsNullOrWhiteSpace(viewModel.Case.contactId))
                {
                    ContactDetailPage newPage = new ContactDetailPage(Guid.Parse(viewModel.Case.contactId));
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
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

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    TabThongTin.IsVisible = true;
                    TabPhanHoiLienQuan.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    await LoadDataPhanHoiLienQuan(CaseId);
                    TabThongTin.IsVisible = false;
                    TabPhanHoiLienQuan.IsVisible = true;
                }
            }
        }
    }
}