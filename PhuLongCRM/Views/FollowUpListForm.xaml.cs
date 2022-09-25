using PhuLongCRM.Helper;
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
    public partial class FollowUpListForm : ContentPage
    {
        public FollowUpListFormViewModel viewModel;
        public Action<bool> OnCompleted;
        public FollowUpListForm(Guid fulid)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowUpListFormViewModel();
            this.Title = Language.cap_nhat_thong_tin_title;
            Init(fulid);
        }

        public FollowUpListForm(FollowUpModel ful)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowUpListFormViewModel();
            this.Title = btnSave.Text = Language.tao_moi_danh_sach_theo_doi_title;
            if (ful.bsd_followuplistid != Guid.Empty)
                viewModel.FULDetail = ful;
            InitCreate();
        }

        public async void Init(Guid fulid)
        {
            await viewModel.LoadFUL(fulid);
            SetPreOpen();

            if (viewModel.FULDetail != null)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_type.ToString()))
                    viewModel.Type = FollowUpType.GetFollowUpTypeById(viewModel.FULDetail.bsd_type.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_terminationtype.ToString()) && viewModel.FULDetail.bsd_terminationtype != 0)
                    viewModel.TypeTerminateletter = FollowUpTerminationType.GetFollowUpTerminationTypeById(viewModel.FULDetail.bsd_terminationtype.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_group.ToString()) && viewModel.FULDetail.bsd_group != 0)
                    viewModel.Group = FollowUpGroup.GetFollowUpGroupById(viewModel.FULDetail.bsd_group.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_takeoutmoney.ToString()) && viewModel.FULDetail.bsd_takeoutmoney != 0)
                    viewModel.TakeOutMoney = FollowUpListTakeOutMoney.GetFollowUpListTakeOutMoneyById(viewModel.FULDetail.bsd_takeoutmoney.ToString());
            }

            if (viewModel.FULDetail != null && viewModel.FULDetail.bsd_followuplistid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
        }
        public async void InitCreate()
        {
            SetPreOpen();
            await Task.Delay(1);
            if (viewModel.FULDetail != null)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_type.ToString()))
                    viewModel.Type = FollowUpType.GetFollowUpTypeById(viewModel.FULDetail.bsd_type.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_group.ToString()) && viewModel.FULDetail.bsd_group != 0)
                    viewModel.Group = FollowUpGroup.GetFollowUpGroupById(viewModel.FULDetail.bsd_group.ToString());
            }

            if (viewModel.FULDetail != null && viewModel.FULDetail.bsd_followuplistid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
        }

        public void SetPreOpen()
        {
            Lookup_Type.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListType = FollowUpType.FollowUpTypeData();
                LoadingHelper.Hide();
            };
            Lookup_TypeTerminateletter.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListTypeTerminateletter = FollowUpTerminationType.FollowUpTerminationTypeData();
                LoadingHelper.Hide();
            };
            Lookup_Group.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListGroup = FollowUpGroup.FollowUpGroupData();
                LoadingHelper.Hide();
            };
            Lookup_TakeOut.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListTakeOutMoney = FollowUpListTakeOutMoney.FollowUpListTakeOutMoneyData();
                LoadingHelper.Hide();
            };
            Lookup_PhaseLaunch.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                if (viewModel.FULDetail != null && viewModel.FULDetail.project_id != Guid.Empty)
                {
                    await viewModel.LoadPhaseLaunch(viewModel.FULDetail.project_id);
                }
                LoadingHelper.Hide();
            };
            Lookup_Meeting.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                if (viewModel.FULDetail != null && viewModel.FULDetail.bsd_followuplistid != Guid.Empty)
                {
                    await viewModel.LoadMeeting(viewModel.FULDetail.bsd_followuplistid);
                }
                LoadingHelper.Hide();
            };
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if (viewModel.FULDetail == null || string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_name))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ten);
                return;
            }
            if (viewModel.FULDetail == null || viewModel.FULDetail.bsd_date == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_tao);
                return;
            }
            if (viewModel.Type == null || string.IsNullOrWhiteSpace(viewModel.Type.Id))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai);
                return;
            }
            if (viewModel.TypeTerminateletter == null || string.IsNullOrWhiteSpace(viewModel.TypeTerminateletter.Id))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai_thanh_ly);
                return;
            }
            if (viewModel.Group == null || string.IsNullOrWhiteSpace(viewModel.Group.Id))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_nhom);
                return;
            }
            if (viewModel.TakeOutMoney == null || string.IsNullOrWhiteSpace(viewModel.TakeOutMoney.Id))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_phuong_thuc_phat);
                return;
            }
            if (viewModel.TakeOutMoney.Id == "100000000" && viewModel.Refund < 0 || viewModel.TakeOutMoney.Id == "100000000" && viewModel.FULDetail.bsd_totalamountpaid < viewModel.Refund)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_tien_hoan_lai);
                return;
            }
            if (viewModel.TakeOutMoney.Id == "100000001" && viewModel.Refund < 0 || viewModel.TakeOutMoney.Id == "100000001" && viewModel.Refund > 100)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_gia_tri_tu_0_den_100);
                return;
            }

            if (viewModel.FULDetail != null && viewModel.FULDetail.bsd_resell)
            {
                if (viewModel.PhaseLaunch == null || viewModel.PhaseLaunch.Id == Guid.Empty)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_dot_mo_ban);
                    return;
                }
            }

            LoadingHelper.Show();
            var create = await viewModel.createFUL();
            if (create)
            {
                if (FollowDetailPage.NeedToRefresh.HasValue) FollowDetailPage.NeedToRefresh = true;
                if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                await Navigation.PopAsync();
                ToastMessageHelper.ShortMessage(Language.da_tao_danh_sach_theo_doi);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.tao_moi_danh_sach_theo_doi_that_bai);
            }
            //var updated = await viewModel.updateFUL();
            //if (updated)
            //{
            //    if (FollowDetailPage.NeedToRefresh.HasValue) FollowDetailPage.NeedToRefresh = true;
            //    if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
            //    await Navigation.PopAsync();
            //    ToastMessageHelper.ShortMessage(Language.cap_nhat_danh_sach_theo_doi_thanh_cong);
            //    LoadingHelper.Hide();
            //}
            //else
            //{
            //    LoadingHelper.Hide();
            //    ToastMessageHelper.ShortMessage(Language.cap_nhat_danh_sach_theo_doi_that_bai);
            //}
        }

        private void Lookup_TakeOut_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.TakeOutMoney == null) return;

            lb_so_tien.IsVisible = true;
            entry_so_tien.IsVisible = true;

            if (viewModel.TakeOutMoney.Id == "100000000") //refund
            {
                lb_so_tien.Text = Language.hoan_tien;
            }
            else if (viewModel.TakeOutMoney.Id == "100000001")
            {
                lb_so_tien.Text = Language.tien_phat_thanh_ly;
            }
            viewModel.Refund = 0;
            entry_so_tien_Unfocused(null, null);
        }

        private void entry_so_tien_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel.TakeOutMoney == null) return;

            if (string.IsNullOrWhiteSpace(entry_so_tien.Text))
                viewModel.Refund = 0;

            if (viewModel.TakeOutMoney.Id == "100000000") //refund
            {
                if (viewModel.FULDetail.bsd_totalamountpaid < viewModel.Refund || viewModel.Refund < 0)
                {
                    ToastMessageHelper.ShortMessage(Language.hoan_tien_phai_nho_hon_tien_da_thanh_toan);
                    return;
                }   
                viewModel.FULDetail.bsd_totalforfeitureamount_new = viewModel.FULDetail.bsd_totalamountpaid - viewModel.Refund;
                viewModel.FULDetail.bsd_totalforfeitureamount_format = StringFormatHelper.FormatCurrency(viewModel.FULDetail.bsd_totalforfeitureamount_new) + " đ";
            }
            else if (viewModel.TakeOutMoney.Id == "100000001")
            {
                if (viewModel.Refund > 100 || viewModel.Refund < 0)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_nhap_gia_tri_tu_0_den_100);
                    return;
                }
                viewModel.FULDetail.bsd_totalforfeitureamount_new = (viewModel.FULDetail.bsd_depositfee * viewModel.Refund) / 100;
                viewModel.FULDetail.bsd_totalforfeitureamount_format = StringFormatHelper.FormatCurrency(viewModel.FULDetail.bsd_totalforfeitureamount_new) + " đ";
            }
        }
    }
}