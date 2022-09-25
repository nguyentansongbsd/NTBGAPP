using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueForm : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefresh;
        public QueueFormViewModel viewModel;
        public Guid QueueId;
        private bool from;
        public QueueForm(Guid unitId, bool fromDirectSale) // Direct Sales (add)
        {
            InitializeComponent();   
            Init();
            viewModel.UnitId = unitId;
            from = fromDirectSale;
            Create();
        }

        public void Init()
        {          
            this.BindingContext = viewModel = new QueueFormViewModel();
            NeedToRefresh = false;
            SetPreOpen();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                Lookup_KhachHang.Refresh();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        public async void SetPreOpen()
        {
            lookUpDaiLy.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSalesAgentCompany();
                LoadingHelper.Hide();
            };
            lookUpCollaborator.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCollaboratorLookUp();
                LoadingHelper.Hide();
            };
            lookUpCustomerReferral.PreOpenAsync = async () =>
             {
                 LoadingHelper.Show();
                 await viewModel.LoadCustomerReferralLookUp();
                 LoadingHelper.Hide();
             };
        }

        public async void Create()
        {
            this.Title = btnSave.Text = Language.tao_giu_cho;
            btnSave.Clicked += Create_Clicked; ;
            viewModel.isUpdate = false;
            if (from)
            {
                await viewModel.LoadFromUnit(viewModel.UnitId);
                if(viewModel.QueueFormModel.bsd_units_id == null)
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }    
                string res = await viewModel.createQueueDraft(false, viewModel.UnitId);
                topic.Text = viewModel.QueueFormModel.bsd_units_name;
                if (viewModel.QueueFormModel.bsd_units_id != Guid.Empty && viewModel.idQueueDraft != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(res);
                }
            }
            else
            {
                await viewModel.LoadFromProject(viewModel.UnitId);
                if (viewModel.QueueFormModel.bsd_project_id == null)
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_du_an);
                }
                string res = await viewModel.createQueueDraft(true, viewModel.UnitId);
                topic.Text = viewModel.QueueFormModel.bsd_project_name +" - "+ DateTime.Now.ToString("dd/MM/yyyy");
                if (viewModel.QueueFormModel.bsd_project_id != Guid.Empty && viewModel.idQueueDraft != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(res);
                }
            }            
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            btnSave.Text = Language.dang_tao_giu_cho;
            await SaveData(null);
        }

        private async Task SaveData(string id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.QueueFormModel.name))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de_giu_cho);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (viewModel.Customer == null || string.IsNullOrWhiteSpace(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_khach_hang);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (from)
            {
                if (!await viewModel.SetQueueTime())// chỉ kiểm tra kh cho giữ chỗ sản phẩm
                {
                    ToastMessageHelper.ShortMessage(Language.khach_hang_da_tham_gia_giu_cho_cho_san_pham_nay);
                    LoadingHelper.Hide();
                    btnSave.Text = Language.tao_giu_cho;
                    return;
                }
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.DailyOption != null && viewModel.DailyOption.Id != Guid.Empty && viewModel.DailyOption.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_khac_dai_ly_ban_hang);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.Collaborator != null && viewModel.Collaborator.Id != Guid.Empty && viewModel.Collaborator.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_khac_cong_tac_vien);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.CustomerReferral != null && viewModel.CustomerReferral.Id != Guid.Empty && viewModel.CustomerReferral.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_khac_khach_hang_gioi_thieu);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            var created = await viewModel.UpdateQueue(viewModel.idQueueDraft);
            if (created)
            {
                if (ProjectInfo.NeedToRefreshQueue.HasValue) ProjectInfo.NeedToRefreshQueue = true;
                if (ProjectInfo.NeedToRefreshNumQueue.HasValue) ProjectInfo.NeedToRefreshNumQueue = true;
                if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                if (UnitInfo.NeedToRefreshQueue.HasValue) UnitInfo.NeedToRefreshQueue = true;
                if (Dashboard.NeedToRefreshQueue.HasValue) Dashboard.NeedToRefreshQueue = true;
                if (QueueList.NeedToRefresh.HasValue) QueueList.NeedToRefresh = true;
                await Navigation.PopAsync();       
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                if (!string.IsNullOrWhiteSpace(viewModel.Error_update_queue))
                    ToastMessageHelper.ShortMessage(viewModel.Error_update_queue);
                else
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
        }

        private void lookUpDaiLy_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if(viewModel.DailyOption != null && viewModel.DailyOption.Id != Guid.Empty)
            {
                viewModel.Collaborator = null;
                viewModel.CustomerReferral = null;
            }    
        }

        private void lookUpCollaborator_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.Collaborator != null && viewModel.Collaborator.Id != Guid.Empty)
            {
                viewModel.DailyOption = null;
                viewModel.CustomerReferral = null;
            }
        }

        private void lookUpCustomerReferral_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.CustomerReferral != null && viewModel.CustomerReferral.Id != Guid.Empty)
            {
                viewModel.DailyOption = null;
                viewModel.Collaborator = null;
            }
        }

    }
}