using PhuLongCRM.Helper;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PhuLongCRM.Models;
using PhuLongCRM.ViewModels;
using FFImageLoading.Forms;
using System.Collections.ObjectModel;
using PhuLongCRM.Resources;
using System.Linq;
using Stormlion.PhotoBrowser;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitInfo : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefreshQueue = null;
        public static bool? NeedToRefreshQuotation = null;
        public static bool? NeedToRefreshReservation = null;
        private UnitInfoViewModel viewModel;

        public UnitInfo(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitInfoViewModel();
            NeedToRefreshQueue = false;
            NeedToRefreshQuotation = false;
            NeedToRefreshReservation = false;
            viewModel.UnitId = id;
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadUnit();
            await viewModel.CheckShowBtnBangTinhGia();
            
            if (viewModel.UnitInfo != null)
            {
                viewModel.StatusCode = StatusCodeUnit.GetStatusCodeById(viewModel.UnitInfo.statuscode.ToString());
                if (!string.IsNullOrWhiteSpace(viewModel.UnitInfo.bsd_direction)) 
                {
                    viewModel.Direction = DirectionData.GetDiretionById(viewModel.UnitInfo.bsd_direction);
                }

                if (!string.IsNullOrWhiteSpace(viewModel.UnitInfo.bsd_viewphulong))
                {
                    viewModel.View = ViewData.GetViewByIds(viewModel.UnitInfo.bsd_viewphulong);
                }

                if (viewModel.UnitInfo.statuscode == 1 || viewModel.UnitInfo.statuscode == 100000000 || viewModel.UnitInfo.statuscode == 100000004 || viewModel.UnitInfo.statuscode == 100000007)
                {
                    btnGiuCho.IsVisible = viewModel.UnitInfo.bsd_vippriority ? false : true;
                    if (viewModel.UnitInfo.statuscode != 1 && viewModel.IsShowBtnBangTinhGia == true)
                    {
                        viewModel.IsShowBtnBangTinhGia = true;
                    }
                    else
                    {
                        viewModel.IsShowBtnBangTinhGia = false;
                    }
                }
                else
                {
                    btnGiuCho.IsVisible = false;
                    viewModel.IsShowBtnBangTinhGia = false;
                }

                SetButton();
                //gridButton.IsVisible = !viewModel.UnitInfo.bsd_vippriority;
                // // thanh lý ket thuc hop dong
                gridButton.IsVisible = false;
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(NeedToRefreshQueue == true)
            {
                LoadingHelper.Show();
                viewModel.PageAcceptance = 1;
                viewModel.Acceptances.Clear();
                await viewModel.LoadAcceptances();
                NeedToRefreshQueue = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshQuotation == true)
            {
                LoadingHelper.Show();
                viewModel.PageUnitHandover = 1;
                if (viewModel.UnitHandovers != null)
                    viewModel.UnitHandovers.Clear();
                await viewModel.LoadUnitHandovers();
                NeedToRefreshQuotation = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshReservation == true)
            {
                LoadingHelper.Show();
                viewModel.PagePinkBooHandover = 1;
                viewModel.PinkBooHandovers.Clear();
                await viewModel.LoadPinkBooHandovers();
                NeedToRefreshReservation = false;
                LoadingHelper.Hide();
            }
            //await CrossMediaManager.Current.Stop();
        }

        public void SetButton()
        {
            gridButton = new Grid();
            if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == false)
            {
                gridButton.IsVisible = false;
            }
            gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
            if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == true)
            {
                gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumn(btnBangTinhGia, 1);
            }
            else if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == false)
            {
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumn(btnBangTinhGia, 0);
            }
            else if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == true)
            {
                Grid.SetColumn(btnBangTinhGia, 0);
                Grid.SetColumn(btnGiuCho, 0);
            }
        }

        private async void ShowMoreAcceptances_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageAcceptance++;
            await viewModel.LoadAcceptances();
            LoadingHelper.Hide();
        }

        private async void ShowMorePinkBooHandovers_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PagePinkBooHandover++;
            await viewModel.LoadPinkBooHandovers();
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachHopDong++;
            await viewModel.LoadOptoinEntry();
            LoadingHelper.Hide();
        }

        private void GiuCho_Clicked(object sender, EventArgs e)
        {
            //LoadingHelper.Show();
            //QueueForm queue = new QueueForm(viewModel.UnitId, true);
            //queue.OnCompleted = async (IsSuccess) => {
            //    if (IsSuccess)
            //    {
            //        await Navigation.PushAsync(queue);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //       // ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
            //    }
            //};
        }

        private void BangTinhGia_Clicked(object sender, EventArgs e)
        {
            //LoadingHelper.Show();
            //ReservationForm reservationForm = new ReservationForm(Guid.Parse(viewModel.UnitInfo.productid), null, null, null,null);
            //reservationForm.CheckReservation = async (isSuccess) => {
            //    if (isSuccess == 0)
            //    {
            //        await Navigation.PushAsync(reservationForm);
            //        LoadingHelper.Hide();
            //    }
            //    else if (isSuccess == 1)
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.san_pham_khong_the_tao_bang_tinh_gia);
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
            //    }
            //};
        }

        private void AcceptanceItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (AcceptanceListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            AcceptanceDetailPage page = new AcceptanceDetailPage(item.bsd_acceptanceid);
            page.OnCompleted = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(page);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }

            };
        }

        private void PinkBooHandoverItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (PinkBookHandoversModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            PinkBookHandoverPage pinkBookHandover = new PinkBookHandoverPage(item.bsd_pinkbookhandoverid);
            pinkBookHandover.OnCompleted = async (isSuccessed) =>
            {
                if (isSuccessed)
                {
                    await Navigation.PushAsync(pinkBookHandover);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private async void ShowMoreUnitHandovers_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageUnitHandover++;
            await viewModel.LoadUnitHandovers();
            LoadingHelper.Hide();
        }

        private void ItemHopDong_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ContractDetailPage contractDetail = new ContractDetailPage(itemId);
            contractDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(contractDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
        private async void OpenEvent_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Event == null)
                await viewModel.LoadDataEvent();
            ContentEvent.IsVisible = true;
        }
        private void CloseContentEvent_Tapped(object sender, EventArgs e)
        {
            ContentEvent.IsVisible = false;
        }

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    stackThongTinCanHo.IsVisible = true;
                    stackGiaoDich.IsVisible = false;
                    stackBanGiao.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    LoadingHelper.Show();
                    stackThongTinCanHo.IsVisible = false;
                    stackGiaoDich.IsVisible = false;
                    stackBanGiao.IsVisible = true;
                    LoadingHelper.Hide();
                }
                else if ((int)e.Item == 2)
                {
                    LoadingHelper.Show();
                    stackThongTinCanHo.IsVisible = false;
                    stackGiaoDich.IsVisible = true;
                    stackBanGiao.IsVisible = false;

                    if (viewModel.IsLoaded == false)
                    {
                        await Task.WhenAll(
                            viewModel.LoadAcceptances(),
                            viewModel.LoadPinkBooHandovers(),
                            viewModel.LoadUnitHandovers(),
                            viewModel.LoadOptoinEntry()
                        );
                    }
                    LoadingHelper.Hide();
                }
            }
        }

        private void UnitHandoverItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (UnitHandoversModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            UnitHandoverPage unitHandover = new UnitHandoverPage(item.bsd_handoverid);
            unitHandover.OnCompleted = async (isSuccessed) => {
                if (isSuccessed)
                {
                    await Navigation.PushAsync(unitHandover);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
        private void ItemSlider_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CollectionData)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.SharePointType == SharePointType.Image)
            {
                var img = viewModel.Photos.SingleOrDefault(x => x.URL == item.ImageSource);
                var index = viewModel.Photos.IndexOf(img);

                new PhotoBrowser()
                {
                    Photos = viewModel.Photos,
                    StartIndex = index,
                    EnableGrid = true
                }.Show();
            }
            else if (item.SharePointType == SharePointType.Video)
            {
                ShowMedia showMedia = new ShowMedia(Config.OrgConfig.SP_ProjectID, item.MediaSourceId);
                showMedia.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(showMedia);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Không lấy được video");
                    }
                };
            }
            LoadingHelper.Hide();
        }
    }
}