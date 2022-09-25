using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSaleDetail : ContentPage
    {
        public static bool? NeedToRefreshDirectSale = null;
        private bool RefreshDirectSale { get; set; }
        public Action<int> OnCompleted;
        private DirectSaleDetailViewModel viewModel;
        private int currentBlock = 0;

        public DirectSaleDetail()
        {
            InitializeComponent();
        }

        public DirectSaleDetail(DirectSaleSearchModel filter) //,List<Block> blocks
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DirectSaleDetailViewModel(filter);
            //viewModel.Blocks = blocks;
            NeedToRefreshDirectSale = false;
            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // giu cho thanh cong hoac huy giu cho thanh cong
            if (NeedToRefreshDirectSale == true)
            {
                await LoadUnit(viewModel.Unit.productid);
                RefreshDirectSale = true;
                NeedToRefreshDirectSale = false;
            }
        }

        public async void Init()
        {
            //viewModel.Filter.Block = viewModel.Blocks.FirstOrDefault().bsd_blockid.ToString();
            await viewModel.LoadTotalDirectSale();
             if (viewModel.DirectSaleResult.Count != 0)
            {
                SetBlocks();
                var firstBlock = viewModel.DirectSaleResult.FirstOrDefault();
                viewModel.ResetDirectSale(firstBlock);
                SaveLoadedBlock();
            }
            else
            {
                OnCompleted?.Invoke(2);
                return;
            }
            
            if (viewModel.Block.Floors.Count != 0)
            {
                currentBlock = viewModel.Blocks.FindIndex(x => x.bsd_blockid == viewModel.blockId);
                SetActiveBlock();

                var floorId = viewModel.Block.Floors.FirstOrDefault().bsd_floorid;
                await viewModel.LoadUnitByFloor(floorId);
                SetContentFloor();

                OnCompleted?.Invoke(0);
            }
            else
            {
                OnCompleted?.Invoke(1);
            }
        }

        private void SaveLoadedBlock()
        {
            foreach (var item in viewModel.Blocks)
            {
                if (item.bsd_blockid == viewModel.Block.bsd_blockid)
                {
                    item.bsd_blockid = viewModel.Block.bsd_blockid;
                    item.NumChuanBiInBlock = viewModel.Block.NumChuanBiInBlock;
                    item.NumDaBanInBlock = viewModel.Block.NumDaBanInBlock;
                    item.NumBookingInBlock = viewModel.Block.NumBookingInBlock;
                    item.NumOptionInBlock = viewModel.Block.NumOptionInBlock;
                    item.NumSignedDAInBlock = viewModel.Block.NumSignedDAInBlock;
                    item.NumQualifiedInBlock = viewModel.Block.NumQualifiedInBlock;
                    item.NumDaDuTienCocInBlock = viewModel.Block.NumDaDuTienCocInBlock;
                    item.NumDatCocInBlock = viewModel.Block.NumDatCocInBlock;
                    item.NumDongYChuyenCoInBlock = viewModel.Block.NumDongYChuyenCoInBlock;
                    item.NumGiuChoInBlock = viewModel.Block.NumGiuChoInBlock;
                    item.NumSanSangInBlock = viewModel.Block.NumSanSangInBlock;
                    item.NumThanhToanDot1InBlock = viewModel.Block.NumThanhToanDot1InBlock;
                    item.Floors = viewModel.Block.Floors;
                }
            }
        }

        private void SetBlocks()
        {
            List<Block> blocks = new List<Block>();
            foreach (var item in viewModel.DirectSaleResult)
            {
                Block block = new Block();
                block.bsd_blockid = Guid.Parse(item.ID);
                block.bsd_name = item.name;
                var arrStatus = item.stringQty.Split(',');
                block.TotalUnitInBlock = int.Parse(arrStatus[0]) + int.Parse(arrStatus[1]) + int.Parse(arrStatus[2]) + int.Parse(arrStatus[3]) + int.Parse(arrStatus[4]) + int.Parse(arrStatus[5]) + int.Parse(arrStatus[6]) + int.Parse(arrStatus[7]) + int.Parse(arrStatus[8]) + int.Parse(arrStatus[9]) + int.Parse(arrStatus[10]) + int.Parse(arrStatus[11]);
                blocks.Add(block);
            }
            viewModel.Blocks = blocks;
        }

        private void SetContentFloor(int index = 0)
        {
            var content = ((stackFloors.Children[index] as RadBorder).Content as StackLayout).Children[3] as FlexLayout;
            BindableLayout.SetItemsSource(content, viewModel.Block.Floors[index].Units);
            content.IsVisible = !content.IsVisible;
        }

        private async void ItemFloor_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = sender as RadBorder;
            int CurrentFloor = stackFloors.Children.IndexOf(item);
            FlexLayout units = ((stackFloors.Children[CurrentFloor] as RadBorder).Content as StackLayout).Children[3] as FlexLayout;
            if (viewModel.Block.Floors[CurrentFloor].Units.Count == 0)
            {
                var floorId = (Guid)(item.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                await viewModel.LoadUnitByFloor(floorId);
                BindableLayout.SetItemsSource(units, viewModel.Block.Floors[CurrentFloor].Units);
                SaveLoadedBlock();
                units.IsVisible = !units.IsVisible;
            }
            else
            {
                SetContentFloor(CurrentFloor);
            }
            LoadingHelper.Hide();
        }

        public async void Block_Tapped(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            var blockChoosed = sender as RadBorder;
            if (stackBlocks.Children.IndexOf(blockChoosed) == currentBlock) return;
            SetInActiveBlock();
            this.currentBlock = stackBlocks.Children.IndexOf(blockChoosed);
            SetActiveBlock();
            await Task.Delay(1);

            var item = (Block)(blockChoosed.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.blockId = item.bsd_blockid;

            if (item.Floors.Count == 0)
            {
                viewModel.Filter.Block = item.bsd_blockid.ToString();
                await viewModel.LoadTotalDirectSale();
                DirectSaleModel block = viewModel.DirectSaleResult.FirstOrDefault();
                if (block != null)
                {
                    viewModel.ResetDirectSale(block);

                    var floorId = viewModel.Block.Floors.FirstOrDefault().bsd_floorid;
                    await viewModel.LoadUnitByFloor(floorId);
                    SaveLoadedBlock();

                    var content = ((stackFloors.Children[0] as RadBorder).Content as StackLayout).Children[3] as FlexLayout;
                    BindableLayout.SetItemsSource(content, viewModel.Block.Floors[0].Units);
                    content.IsVisible = true;
                }
            }
            else
            {
                viewModel.Block = item;
                SetContentFloor();
            }
            
            LoadingHelper.Hide();
        }

        public void SetActiveBlock()
        {
            var radblock = (RadBorder)stackBlocks.Children[currentBlock];
            Label lblblock = (radblock.Content as Label);
            VisualStateManager.GoToState(radblock, "Active");
            VisualStateManager.GoToState(lblblock, "Active");
        }

        public void SetInActiveBlock()
        {
            var radblock = (RadBorder)stackBlocks.Children[currentBlock];
            Label lblblock = (radblock.Content as Label);
            VisualStateManager.GoToState(radblock, "InActive");
            VisualStateManager.GoToState(lblblock, "InActive");
        }

        private async void UnitItem_Tapped(object sender, EventArgs e)
        {
            var unitId = (Guid)((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            await LoadUnit(unitId);
            contentUnitInfor.IsVisible = true;
        }

        public void SetButton()
        {
            if (btnGiuCho.IsVisible == false && btnBangTinhGia.IsVisible == false)
            {
                gridButton.IsVisible = false;
            }
            else if (btnGiuCho.IsVisible == true && btnBangTinhGia.IsVisible == true)
            {
                gridButton.IsVisible = true;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumnSpan(btnGiuCho, 1);
                Grid.SetColumn(btnBangTinhGia, 1);
                Grid.SetColumnSpan(btnBangTinhGia, 1);
            }
            else if (btnGiuCho.IsVisible == true && btnBangTinhGia.IsVisible == false)
            {
                gridButton.IsVisible = true;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumnSpan(btnGiuCho, 2);
                Grid.SetColumn(btnBangTinhGia, 0);
            }
            else if (btnGiuCho.IsVisible == false && btnBangTinhGia.IsVisible == true)
            {
                gridButton.IsVisible = true;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumn(btnBangTinhGia, 0);
                Grid.SetColumnSpan(btnBangTinhGia, 2);
            }

            //gridButton = new Grid();
            //if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == false)
            //{
            //    gridButton.IsVisible = false;
            //}
            //gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
            //if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == true)
            //{
            //    gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
            //    Grid.SetColumn(btnGiuCho, 0);
            //    Grid.SetColumn(btnBangTinhGia, 1);
            //}
            //else if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == false)
            //{
            //    Grid.SetColumn(btnGiuCho, 0);
            //    Grid.SetColumn(btnBangTinhGia, 0);
            //}
            //else if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == true)
            //{
            //    Grid.SetColumn(btnBangTinhGia, 0);
            //    Grid.SetColumn(btnGiuCho, 0);
            //}
        }

        private void UnitInfor_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitInfo unitInfo = new UnitInfo(viewModel.Unit.productid);
            unitInfo.OnCompleted= async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(unitInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }

        private async void CloseUnintInfor_Tapped(object sender,EventArgs e)
        {
            LoadingHelper.Show();

            //Load lai thong tin directsale khi giu cho, huy giu cho thanh cong
            if (RefreshDirectSale==true)
            {
                //viewModel.Floors.Clear();
                viewModel.Filter.Block = viewModel.blockId.ToString();
                await viewModel.LoadTotalDirectSale();
                var currentBlock = viewModel.DirectSaleResult.SingleOrDefault(x => x.ID == viewModel.Unit.blockid.ToString());
                viewModel.ResetDirectSale(currentBlock);
                await viewModel.LoadUnitByFloor(viewModel.Unit.floorid);

                int indexFloor = viewModel.Block.Floors.IndexOf(viewModel.Block.Floors.SingleOrDefault(x => x.bsd_floorid == viewModel.Unit.floorid));
                SetContentFloor(indexFloor);
                RefreshDirectSale = false;
            }
            
            contentUnitInfor.IsVisible = false;
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueues(viewModel.Unit.productid);
            LoadingHelper.Hide();
        }

        private async void GiuCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            QueueForm queue = new QueueForm(viewModel.Unit.productid, true);
            queue.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Shell.Current.Navigation.PushAsync(queue);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                  //  ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                  // hiện câu thông báo bên queue form
                }
            };
        }

        private void BangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ReservationForm reservationForm = new ReservationForm(viewModel.Unit.productid,null,null,null,null);
            reservationForm.CheckReservation = async (isSuccess) => {
                if (isSuccess == 0)
                {
                    await Navigation.PushAsync(reservationForm);
                    LoadingHelper.Hide();
                }
                else if (isSuccess == 1)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.san_pham_khong_the_tao_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            queuesDetialPage.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(queuesDetialPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void Question_CLicked(object sender, EventArgs e)
        {
            stackQuestion.IsVisible = !stackQuestion.IsVisible;
        }

        private void CloseQuestion_Tapped(object sender, EventArgs e)
        {
            stackQuestion.IsVisible = !stackQuestion.IsVisible;
        }

        private async Task LoadUnit(Guid unitId)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho = 1;
            viewModel.QueueList.Clear();
            await Task.WhenAll(
                viewModel.LoadQueues(unitId),
                viewModel.LoadUnitById(unitId)
                );

            viewModel.UnitStatusCode = StatusCodeUnit.GetStatusCodeById(viewModel.Unit.statuscode.ToString());
            if (!string.IsNullOrWhiteSpace(viewModel.Unit.bsd_direction))
            {
                viewModel.UnitDirection = DirectionData.GetDiretionById(viewModel.Unit.bsd_direction);
            }
            else
            {
                viewModel.UnitDirection = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.Unit.bsd_viewphulong))
            {
                viewModel.UnitView = ViewData.GetViewByIds(viewModel.Unit.bsd_viewphulong);
            }
            else
            {
                viewModel.UnitView = null;
            }
            // hiện btn giữ chỗ availabe, queuing, preparing, booking
            if (viewModel.UnitStatusCode.Id == "1" || viewModel.UnitStatusCode.Id == "100000000" || viewModel.UnitStatusCode.Id == "100000004"
                || viewModel.UnitStatusCode.Id == "100000007")
            {
                btnGiuCho.IsVisible = viewModel.Unit.bsd_vippriority ? false : true;
                if (viewModel.Unit.statuscode != 1 && viewModel.IsShowBtnBangTinhGia == true)
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
            gridButton.IsVisible = !viewModel.Unit.bsd_vippriority;
            LoadingHelper.Hide();
        }

        async void ScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            //if (!(sender is ScrollView scrollView))
            //    return;

            //var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            //if (scrollingSpace > e.ScrollY)
            //    return;

            //await DisplayAlert("", "asdfasd", "ok");
            //return;
        }
    }
}
