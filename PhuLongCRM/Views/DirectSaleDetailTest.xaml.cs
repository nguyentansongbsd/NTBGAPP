using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSaleDetailTest : ContentPage
    {
        public static bool? NeedToRefreshDirectSale = null;
        private bool RefreshDirectSale { get; set; }
        public Action<int> OnCompleted;
        private DirectSaleDetailTestViewModel viewModel;
        private Grid grid;
        private Grid gridBtn;
        private Button btnQueue;
        private Button btnQuote;
        private Label labelName;
        public DirectSaleDetailTest(DirectSaleSearchModel filter)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DirectSaleDetailTestViewModel();
            viewModel.Filter = filter;
            viewModel.CreateFilterXml();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadTotalDirectSale();
            if (viewModel.Blocks != null && viewModel.Blocks.Count != 0)
            {
                var rd = stackBlocks.Children[0] as RadBorder;
                var lb = rd.Content as Label;
                VisualStateManager.GoToState(rd, "Selected");
                VisualStateManager.GoToState(lb, "Selected");
                NumberUnitInBlock(viewModel.Blocks[0]);
                if (viewModel.Block.Floors.Count != 0)
                {
                    var floor = viewModel.Block.Floors[0];
                    floor.iShow = true;
                    await viewModel.LoadUnitByFloor(floor.bsd_floorid);
                    OnCompleted?.Invoke(0);
                }
                else
                {
                    OnCompleted?.Invoke(1);
                }
            }
            else
            {
                OnCompleted?.Invoke(2);
                return;
            }
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
        public async void Block_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Task.Delay(1);
            var blockChoosed = sender as RadBorder;
            if (blockChoosed != null)
            {
                for (int i = 0; i < stackBlocks.Children.Count; i++)
                {
                    if (stackBlocks.Children[i] == blockChoosed)
                    {
                        var rd = stackBlocks.Children[i] as RadBorder;
                        var lb = rd.Content as Label;
                        VisualStateManager.GoToState(rd, "Selected");// UI trong app.xaml
                        VisualStateManager.GoToState(lb, "Selected");// UI trong app.xaml
                    }
                    else
                    {
                        var rd = stackBlocks.Children[i] as RadBorder;
                        var lb = rd.Content as Label;
                        VisualStateManager.GoToState(rd, "Normal");// UI trong app.xaml
                        VisualStateManager.GoToState(lb, "Normal");// UI trong app.xaml
                    }
                }
                var item = (Block)(blockChoosed.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                NumberUnitInBlock(item);
            }
            LoadingHelper.Hide();
        }
        public async void NumberUnitInBlock(Block block)
        {
            if (block != null && block != viewModel.Block)
            {
                viewModel.Block = block;
                await viewModel.LoadFloor();
            }
        }
        private async void ItemFloor_Tapped(object sender, EventArgs e)
        {
            var item = sender as Grid;
            var collectionFloor = ((StackLayout)item.Parent).Children[1] as FlexLayout;
            if (collectionFloor != null)
            {
                LoadingHelper.Show();
                var floor = (Floor)(item.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                if (floor != null)
                {
                    if (floor.Units.Count == 0)
                        await viewModel.LoadUnitByFloor(floor.bsd_floorid);
                    //BindableLayout.SetItemsSource(collectionFloor, floor.Units);
                    floor.iShow = !floor.iShow;
                }
                LoadingHelper.Hide();
            }
            //(((RadBorder)((StackLayout)item.Parent).Parent).Parent as ViewCell).ForceUpdateSize();
        }
        private async void UnitItem_Tapped(object sender, EventArgs e)
        {
            var unitId = (Guid)((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            await LoadUnit(unitId);
            CreatePopupUnit(unitId);
            PopupUnit.IsVisible = true;
        }
        private async Task LoadUnit(Guid unitId)
        {
            LoadingHelper.Show();
            await viewModel.LoadUnitById(unitId);
            LoadingHelper.Hide();
        }
        private void NumberUnit_Tapped(object sender, EventArgs e)
        {
            var item = (string)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item == "1")
                PopupHover.ShowHover(Language.chuan_bi);
            else if (item == "2")
                PopupHover.ShowHover(Language.san_sang);
            else if (item == "3")
                PopupHover.ShowHover(Language.dat_cho);
            else if (item == "4")
                PopupHover.ShowHover(Language.giu_cho);
            else if (item == "5")
                PopupHover.ShowHover(Language.dat_coc);
            else if (item == "6")
                PopupHover.ShowHover(Language.dong_y_chuyen_coc);
            else if (item == "7")
                PopupHover.ShowHover(Language.da_du_tien_coc);
            else if (item == "8")
                PopupHover.ShowHover(Language.hoan_tat_dat_coc);
            else if (item == "9")
                PopupHover.ShowHover(Language.thanh_toan_dot_1);
            else if (item == "10")
                PopupHover.ShowHover(Language.da_ky_ttdc_hddc);
            else if (item == "11")
                PopupHover.ShowHover(Language.du_dieu_dien);
            else if (item == "12")
                PopupHover.ShowHover(Language.da_ban);
        }

        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e == null) return;
            var index = e.ItemIndex;
            if (index + 1 == viewModel.Block.Floors.Count)
                await viewModel.LoadFloor();
        }

        private async void ScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            //if (!(sender is ScrollView scrollView))
            //    return;

            //var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            //if (scrollingSpace > e.ScrollY)
            //    return;
            //await viewModel.LoadFloor();
            //await DisplayAlert("", "asdfasd", "ok");
            //return;
        }

        private void UnitInfor_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitInfo unitInfo = new UnitInfo(viewModel.Unit.productid);
            unitInfo.OnCompleted = async (IsSuccess) => {
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
        private void BangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ReservationForm reservationForm = new ReservationForm(viewModel.Unit.productid, null, null, null, null);
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
        private void GiuCho_Clicked(object sender, EventArgs e)
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
                    // hiện câu thông báo bên queue form
                }
            };
        }
        private void CreatePopupUnit(Guid unit_id)
        {
            if (grid == null)
            {
                grid = new Grid
                {
                    RowDefinitions = {
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                    }
                    ,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{ Width = GridLength.Auto},
                        new ColumnDefinition{ Width = GridLength.Auto},
                        new ColumnDefinition{ Width = GridLength.Auto},
                        new ColumnDefinition{ Width = new GridLength(1,GridUnitType.Star)},
                        new ColumnDefinition{ Width = GridLength.Auto},
                    }
                };
                PopupUnit.Body = grid;

                //status
                RadBorder radBorder = new RadBorder();
                radBorder.CornerRadius = 5;
                radBorder.SetBinding(RadBorder.BackgroundColorProperty, "Unit.statuscode_color");
                Label label = new Label();
                label.SetBinding(Label.TextProperty, "Unit.statuscode_format");
                label.FontSize = 14;
                label.FontAttributes = FontAttributes.Bold;
                label.TextColor = Color.White;
                label.Margin = 5;
                radBorder.Content = label;
                grid.Children.Add(radBorder);
                Grid.SetColumn(radBorder, 0);
                Grid.SetRow(radBorder, 0);

                // ten
                labelName = new Label { FontSize = 16, TextColor = Color.FromHex("#1C78C2"), FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center };
                labelName.SetBinding(Label.TextProperty, "Unit.name");
                grid.Children.Add(labelName);
                Grid.SetRow(labelName, 0);

                //vip
                BoxView boxView = new BoxView();
                boxView.HeightRequest = 20;
                boxView.WidthRequest = 1;
                boxView.BackgroundColor = Color.Gray;
                boxView.VerticalOptions = LayoutOptions.Center;
                boxView.HorizontalOptions = LayoutOptions.Center;
                boxView.SetBinding(BoxView.IsVisibleProperty, "Unit.bsd_vippriority");
                grid.Children.Add(boxView);
                Grid.SetColumn(boxView, 1);
                Grid.SetRow(boxView, 0);

                RadBorder radBorderVip = new RadBorder();
                radBorderVip.CornerRadius = 5;
                radBorderVip.BackgroundColor = Color.FromHex("#FEC93D");
                radBorderVip.SetBinding(RadBorder.IsVisibleProperty, "Unit.bsd_vippriority");
                Label labelVip = new Label();
                labelVip.FontSize = 14;
                labelVip.TextColor = Color.White;
                labelVip.Margin = 5;
                radBorderVip.Content = labelVip;

                var formattedString = new FormattedString();
                formattedString.Spans.Add(new Span { Text = "\uf005", FontFamily = "FontAwesomeSolid" });
                formattedString.Spans.Add(new Span { Text = "VIP", FontAttributes = FontAttributes.Bold });
                labelVip.FormattedText = formattedString;
                grid.Children.Add(radBorderVip);
                Grid.SetColumn(radBorderVip, 2);
                Grid.SetRow(radBorderVip, 0);

                // su kien
                var formattedStringEvent = new FormattedString();
                formattedStringEvent.Spans.Add(new Span { Text = Language.su_kien, FontAttributes = FontAttributes.Bold });
                formattedStringEvent.Spans.Add(new Span { Text = "\uf005", FontFamily = "FontAwesomeSolid" });
                Label labelEvent = new Label { FontSize = 14, TextColor = Color.FromHex("#FEC93D"), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, FormattedText = formattedStringEvent };
                labelEvent.SetBinding(Label.IsVisibleProperty, "Unit.has_event");
                grid.Children.Add(labelEvent);
                Grid.SetColumn(labelEvent, 4);
                Grid.SetRow(labelEvent, 0);

                // gia
                Label labelPrice = new Label { FontSize = 16, TextColor = Color.Red, FontAttributes = FontAttributes.Bold };
                labelPrice.SetBinding(Label.TextProperty, new Binding("Unit.price_format") { StringFormat = "{0} đ" });
                grid.Children.Add(labelPrice);
                Grid.SetColumn(labelPrice, 0);
                Grid.SetRow(labelPrice, 1);
                Grid.SetColumnSpan(labelPrice, 5);

                //dien tich su dung
                FieldListViewItem field = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.dien_tich_su_dung, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#444444") };
                field.SetBinding(FieldListViewItem.TextProperty, "Unit.bsd_netsaleablearea_format");
                grid.Children.Add(field);
                Grid.SetColumn(field, 0);
                Grid.SetRow(field, 2);
                Grid.SetColumnSpan(field, 5);

                //loai unit
                FieldListViewItem fieldUnitType = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.loai_unit, TextColor = Color.FromHex("#444444") };
                fieldUnitType.SetBinding(FieldListViewItem.TextProperty, "Unit.bsd_unittype_name");
                grid.Children.Add(fieldUnitType);
                Grid.SetColumn(fieldUnitType, 0);
                Grid.SetRow(fieldUnitType, 3);
                Grid.SetColumnSpan(fieldUnitType, 5);

                // grid huong nhin
                Grid gridhuong = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{ Width = GridLength.Auto},
                        new ColumnDefinition{ Width = GridLength.Auto}
                    }
                };

                //huong
                FieldListViewItem fieldhuong = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.huong, TextColor = Color.FromHex("#444444") };
                fieldhuong.SetBinding(FieldListViewItem.TextProperty, "Unit.bsd_direction_format");
                gridhuong.Children.Add(fieldhuong);
                Grid.SetColumn(fieldhuong, 0);
                Grid.SetRow(fieldhuong, 0);

                //huong nhin
                FieldListViewItem fieldhuongnhin = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.huong_nhin, TextColor = Color.FromHex("#444444") };
                fieldhuongnhin.SetBinding(FieldListViewItem.TextProperty, "Unit.bsd_viewphulong_format");
                gridhuong.Children.Add(fieldhuongnhin);
                Grid.SetColumn(fieldhuongnhin, 1);
                Grid.SetRow(fieldhuongnhin, 0);

                grid.Children.Add(gridhuong);
                Grid.SetColumn(gridhuong, 0);
                Grid.SetRow(gridhuong, 4);
                Grid.SetColumnSpan(gridhuong, 5);

                // btn xem thong tin
                RadBorder radBorderUnitInf = new RadBorder { CornerRadius = 10, BorderColor = Color.FromHex("#2196F3"), BorderThickness = 1, BackgroundColor = Color.White, Padding = 8, HorizontalOptions = LayoutOptions.Fill,Margin = new Thickness(0,5) };
                TapGestureRecognizer tapUnitInf = new TapGestureRecognizer();
                tapUnitInf.Tapped += UnitInfor_Clicked;
                radBorderUnitInf.GestureRecognizers.Add(tapUnitInf);

                var formattedlabelUnitInfo = new FormattedString();
                formattedlabelUnitInfo.Spans.Add(new Span { Text = Language.xem_thong_tin, FontAttributes = FontAttributes.Bold, FontSize = 15 });
                formattedlabelUnitInfo.Spans.Add(new Span { Text = "   " });
                formattedlabelUnitInfo.Spans.Add(new Span { Text = "\uf101", FontFamily = "FontAwesomeSolid", FontSize = 18 });
                Label labelUnitInfo = new Label { TextColor = Color.FromHex("#2196F3"), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FormattedText = formattedlabelUnitInfo };
                radBorderUnitInf.Content = labelUnitInfo;
                grid.Children.Add(radBorderUnitInf);
                Grid.SetColumn(radBorderUnitInf, 0);
                Grid.SetRow(radBorderUnitInf, 5);
                Grid.SetColumnSpan(radBorderUnitInf, 5);

                Label labelQueue = new Label { Text = Language.giu_cho_title, FontSize = 16, FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center, Padding = new Thickness(10, 8), BackgroundColor = Color.FromHex("#f4fafe"), TextColor = Color.FromHex("#145a92"), Margin = new Thickness(-10, 0) };
                grid.Children.Add(labelQueue);
                Grid.SetColumn(labelQueue, 0);
                Grid.SetRow(labelQueue, 6);
                Grid.SetColumnSpan(labelQueue, 5);

                //create button
                gridBtn = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(),
                        new ColumnDefinition()
                    },
                    HorizontalOptions = LayoutOptions.Fill
                };
                grid.Children.Add(gridBtn);
                Grid.SetColumn(gridBtn, 0);
                Grid.SetRow(gridBtn, 8);
                Grid.SetColumnSpan(gridBtn, 5);

                btnQueue = new Button { Text = Language.giu_cho_btn, TextColor = Color.White, BackgroundColor = (Color)Application.Current.Resources["NavigationPrimary"], CornerRadius = 10, FontAttributes = FontAttributes.Bold, HeightRequest = 45, Padding = 5 };
                btnQueue.Clicked += GiuCho_Clicked;
                gridBtn.Children.Add(btnQueue);
                btnQuote = new Button { Text = Language.bang_tinh_gia_btn, BackgroundColor = Color.White, TextColor = (Color)Application.Current.Resources["NavigationPrimary"], BorderWidth = 1, BorderColor = (Color)Application.Current.Resources["NavigationPrimary"], CornerRadius = 10, FontAttributes = FontAttributes.Bold, HeightRequest = 45, Padding = 5 };
                btnQuote.Clicked += BangTinhGia_Clicked;
                gridBtn.Children.Add(btnQuote);
            }
            QueuesControl queuesControl = new QueuesControl(unit_id);
            queuesControl.HeightRequest = 400;
            grid.Children.Add(queuesControl);
            Grid.SetColumn(queuesControl, 0);
            Grid.SetRow(queuesControl, 7);
            Grid.SetColumnSpan(queuesControl, 5);

            // hiện btn giữ chỗ availabe, queuing, preparing, booking
            if (viewModel.Unit.statuscode == 1 || viewModel.Unit.statuscode == 100000000
                || viewModel.Unit.statuscode == 100000004 || viewModel.Unit.statuscode == 100000007)
            {
                btnQueue.IsVisible = viewModel.Unit.bsd_vippriority ? false : true;
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
                btnQueue.IsVisible = false;
                viewModel.IsShowBtnBangTinhGia = false;
            }
            gridBtn.IsVisible = !viewModel.Unit.bsd_vippriority;
            btnQuote.IsVisible = viewModel.IsShowBtnBangTinhGia;

            //set button
            if (btnQueue.IsVisible == false && btnQuote.IsVisible == false)
            {
                gridBtn.IsVisible = false;
            }
            else if (btnQueue.IsVisible == true && btnQuote.IsVisible == true)
            {
                gridBtn.IsVisible = true;
                Grid.SetColumn(btnQueue, 0);
                Grid.SetColumn(btnQuote, 1);
            }
            else if (btnQueue.IsVisible == true && btnQuote.IsVisible == false)
            {
                gridBtn.IsVisible = true;
                Grid.SetColumn(btnQueue, 0);
                Grid.SetColumnSpan(btnQueue, 2);
            }
            else if (btnQueue.IsVisible == false && btnQuote.IsVisible == true)
            {
                gridBtn.IsVisible = true;
                Grid.SetColumn(btnQuote, 0);
                Grid.SetColumnSpan(btnQuote, 2);
            }
            // set vip
            if (viewModel.Unit.bsd_vippriority)
            {
                Grid.SetColumn(labelName, 3);
            }
            else
            {
                Grid.SetColumn(labelName, 1);
                Grid.SetColumnSpan(labelName, 3);
            }
        }
    }
    public class QueuesControl : BsdListView
    {
        private DataTemplate dataTemplate;
        private QueuesControlViewModel viewModel;
        public QueuesControl(Guid unit_id)
        {
            this.BindingContext = viewModel = new QueuesControlViewModel(unit_id);
            this.SetBinding(BsdListView.ItemsSourceProperty, "Data");
            this.Margin = new Thickness(-10, 0);
            this.BackgroundColor = Color.White;
            Init();
        }
        private async void Init()
        {
            await viewModel.LoadData();
            CreateItemTemplate();
        }
        public void CreateItemTemplate()
        {
            if (dataTemplate == null)
            {
                dataTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        RowDefinitions = {
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                        new RowDefinition{Height = GridLength.Auto },
                    }
                    ,
                        ColumnDefinitions =
                    {
                        new ColumnDefinition{ Width = GridLength.Auto},
                        new ColumnDefinition{ Width = new GridLength(1,GridUnitType.Star)},
                    },
                        Margin = new Thickness(0,1,0,0),
                        Padding = 10,
                        BackgroundColor = Color.White
                    };

                    //status
                    RadBorder radBorder = new RadBorder{CornerRadius = 5,VerticalOptions =LayoutOptions.Start};
                    radBorder.SetBinding(RadBorder.BackgroundColorProperty, "statuscode_color");
                    Label label = new Label();
                    label.SetBinding(Label.TextProperty, "statuscode_format");
                    label.FontSize = 14;
                    label.FontAttributes = FontAttributes.Bold;
                    label.TextColor = Color.White;
                    label.Margin = 5;
                    radBorder.Content = label;
                    grid.Children.Add(radBorder);
                    Grid.SetColumn(radBorder, 0);
                    Grid.SetRow(radBorder, 0);

                    //ten
                    Label labelName = new Label { FontSize = 15, TextColor = (Color)Application.Current.Resources["NavigationPrimary"], FontAttributes = FontAttributes.Bold, VerticalOptions=LayoutOptions.Center };

                    labelName.SetBinding(Label.TextProperty, new MultiBinding
                    {
                        Bindings = new Collection<BindingBase>
                        {
                            new Binding(){Source = Language.ma},
                            new Binding("bsd_queuenumber"),
                            new Binding("name")
                        },
                        StringFormat = "({0}: {1}) {2}"
                    });

                    grid.Children.Add(labelName);
                    Grid.SetColumn(labelName, 1);
                    Grid.SetRow(labelName, 0);

                    //khach hang
                    FieldListViewItem fieldKH = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.khach_hang, TextColor = Color.FromHex("#444444") };
                    fieldKH.SetBinding(FieldListViewItem.TextProperty, "customername");
                    grid.Children.Add(fieldKH);
                    Grid.SetColumn(fieldKH, 0);
                    Grid.SetRow(fieldKH, 1);
                    Grid.SetColumnSpan(fieldKH, 2);

                    //du an
                    StackLayout stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    FieldListViewItem fieldProject = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.du_an, TextColor = Color.FromHex("#444444") };
                    fieldProject.SetBinding(FieldListViewItem.TextProperty, "project_name");
                    stackLayout.Children.Add(fieldProject);
                    // thien chi
                    Label labelThienChi = new Label { FontSize = 15, TextColor = Color.FromHex("#444444")};
                    labelThienChi.SetBinding(Label.TextProperty, new Binding() { Source = Language.thien_chi, StringFormat = "- {0}" });
                    labelThienChi.SetBinding(Label.IsVisibleProperty, new Binding("bsd_queueforproject"));
                    stackLayout.Children.Add(labelThienChi);

                    grid.Children.Add(stackLayout);
                    Grid.SetColumn(stackLayout, 0);
                    Grid.SetRow(stackLayout, 2);
                    Grid.SetColumnSpan(stackLayout, 2);

                    // thoi gian het han
                    FieldListViewItem fieldDate = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.thoi_gian_het_han, TextColor = Color.FromHex("#444444") };
                    fieldDate.SetBinding(FieldListViewItem.TextProperty, new Binding("bsd_queuingexpired") { StringFormat = "{0:dd/MM/yyyy - HH:mm}" });
                    grid.Children.Add(fieldDate);
                    Grid.SetColumn(fieldDate, 0);
                    Grid.SetRow(fieldDate, 3);
                    Grid.SetColumnSpan(fieldDate, 2);

                    // phi_giu_cho_da_thanh_toan
                    FieldListViewItem fieldPaid = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.phi_giu_cho_da_thanh_toan, TextColor = Color.FromHex("#444444") };
                    fieldPaid.SetBinding(FieldListViewItem.TextProperty, "bsd_queuingfeepaid_format");
                    grid.Children.Add(fieldPaid);
                    Grid.SetColumn(fieldPaid, 0);
                    Grid.SetRow(fieldPaid, 4);
                    Grid.SetColumnSpan(fieldPaid, 2);
                    //da_thanh_toan_phi_giu_cho
                    FieldListViewItem fieldPaid2 = new FieldListViewItem { TitleTextColor = Color.FromHex("#444444"), Title = Language.da_thanh_toan_phi_giu_cho, TextColor = Color.Red, FontAttributes=FontAttributes.Bold };
                    fieldPaid2.SetBinding(FieldListViewItem.TextProperty, "bsd_collectedqueuingfee_format");
                    grid.Children.Add(fieldPaid2);
                    Grid.SetColumn(fieldPaid2, 0);
                    Grid.SetRow(fieldPaid2, 5);
                    Grid.SetColumnSpan(fieldPaid2, 2);

                    return new ViewCell { View = grid };
                });
            }
            this.ItemTemplate = dataTemplate;
        }
    }
    public class QueuesControlViewModel : ListViewBaseViewModel2<QueueListModel>
    {
        public QueuesControlViewModel(Guid unit_id)
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "opportunities";
                FetchXml = $@"<fetch version='1.0' count='5' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='opportunity'>
                        <attribute name='name'/>
                        <attribute name='statuscode' />
                        <attribute name='bsd_project' />
                        <attribute name='opportunityid' />
                        <attribute name='bsd_queuingfeepaid' />
                        <attribute name='bsd_collectedqueuingfee' />
                        <attribute name='bsd_queuingexpired' />
                        <attribute name='bsd_queuenumber' />
                        <attribute name='bsd_queueforproject' />
                        <order attribute='statuscode' descending='false' />
                        <filter type='and'>
                            <condition attribute='bsd_units' operator='eq' value='{unit_id}'/>
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
                      </entity>
                    </fetch>";
            });
        }
    }
}