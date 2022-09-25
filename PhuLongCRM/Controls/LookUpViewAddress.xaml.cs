using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookUpViewAddress : StackLayout
    {
        public event EventHandler<LookUpChangeEvent> SelectedItemChange;

        public event EventHandler CloseEvent;

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpViewAddress), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(AddressModel), typeof(LookUpViewAddress), null, BindingMode.TwoWay, propertyChanged: SelectedItemChang);

        public AddressModel SelectedItem { get => (AddressModel)GetValue(SelectedItemProperty); set { SetValue(SelectedItemProperty, value); }}
        public BottomModal bottomModal { get; set; }

        public static readonly BindableProperty list_country_lookupProperty = BindableProperty.Create(nameof(list_country_lookup), typeof(ObservableCollection<LookUp>), typeof(LookUpViewAddress), null, BindingMode.TwoWay, null);
        public ObservableCollection<LookUp> list_country_lookup { get => (ObservableCollection<LookUp>)GetValue(list_country_lookupProperty); set { SetValue(list_country_lookupProperty, value); } }

        public static readonly BindableProperty list_province_lookupProperty = BindableProperty.Create(nameof(list_province_lookup), typeof(ObservableCollection<LookUp>), typeof(LookUpViewAddress), null, BindingMode.TwoWay, null);
        public ObservableCollection<LookUp> list_province_lookup { get => (ObservableCollection<LookUp>)GetValue(list_province_lookupProperty); set { SetValue(list_province_lookupProperty, value); } }

        public static readonly BindableProperty list_district_lookupProperty = BindableProperty.Create(nameof(list_district_lookup), typeof(ObservableCollection<LookUp>), typeof(LookUpViewAddress), null, BindingMode.TwoWay, null);
        public ObservableCollection<LookUp> list_district_lookup { get => (ObservableCollection<LookUp>)GetValue(list_district_lookupProperty); set { SetValue(list_district_lookupProperty, value); } }

        public static readonly BindableProperty CountryProperty = BindableProperty.Create(nameof(Country), typeof(Models.LookUp), typeof(LookUpViewAddress), null, BindingMode.TwoWay,propertyChanged:CountryChang);
        public Models.LookUp Country { get => (Models.LookUp)GetValue(CountryProperty); set { SetValue(CountryProperty, value); } }

        public static readonly BindableProperty ProvinceProperty = BindableProperty.Create(nameof(Province), typeof(Models.LookUp), typeof(LookUpViewAddress), null, BindingMode.TwoWay, propertyChanged: ProvinceChang);
        public Models.LookUp Province { get => (Models.LookUp)GetValue(ProvinceProperty); set { SetValue(ProvinceProperty, value); } }

        public static readonly BindableProperty DistrictProperty = BindableProperty.Create(nameof(District), typeof(Models.LookUp), typeof(LookUpViewAddress), null, BindingMode.TwoWay);
        public Models.LookUp District { get => (Models.LookUp)GetValue(DistrictProperty); set { SetValue(DistrictProperty, value); } }
       
        public LookUpViewAddress()
        {
            InitializeComponent();
            this.BindingContext = this;
            lookUpCountry.BindingContext = this;
            lookUpProvince.BindingContext = this;
            lookUpDistrict.BindingContext = this;
            lineAddress.BindingContext = this;

            lookUpCountry.SetBinding(LookUp.SelectedItemProperty, "Country");
            lookUpCountry.SetBinding(LookUp.ItemsSourceProperty, "list_country_lookup");
            lookUpProvince.SetBinding(LookUp.SelectedItemProperty, "Province");
            lookUpProvince.SetBinding(LookUp.ItemsSourceProperty, "list_province_lookup");
            lookUpDistrict.SetBinding(LookUp.SelectedItemProperty, "District");
            lookUpDistrict.SetBinding(LookUp.ItemsSourceProperty, "list_district_lookup");
            lineAddress.SetBinding(EntryNoneBorder.TextProperty, "SelectedItem.lineaddress");

            lookUpCountry.BottomModal = bottomModal;
            lookUpDistrict.BottomModal = bottomModal;
            lookUpProvince.BottomModal = bottomModal;
            OnPreOpen();

            SelectedItem = new AddressModel();
            SelectedItem.lineaddress = "";

            list_country_lookup = new ObservableCollection<LookUp>();
            list_province_lookup = new ObservableCollection<LookUp>();
            list_district_lookup = new ObservableCollection<LookUp>();
        }

        private void OnPreOpen()
        {
            lookUpCountry.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await LoadCountryForLookup();
                LoadingHelper.Hide();
            };
        }
        private async void CountryAddress_Changed(object sender, LookUpChangeEvent e)
        {
            await LoadProvincesForLookup();
        }
        private async void StateProvinceAddress_Changed(object sender, LookUpChangeEvent e)
        {
            await LoadDistrictForLookup();
        }
        private void CloseAddress_Clicked(object sender, EventArgs e)
        {
            CloseEvent?.Invoke(this, null);
        }
        //public async Task ShowAddress()
        //{
        //    if (centerModal == null || bottomModal == null) return;
        //    Init();
        //    await centerModal.Show();
        //}
        private async void ConfirmAddress_Clicked(object sender, EventArgs e)
        {
            if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.lineaddress))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_nha_duong_phuong);
                return;
            }

            if (District != null && District.Id != Guid.Empty)
            {
                SelectedItem.district_name = District.Name;
                SelectedItem.district_id = District.Id;
            }
            else
            {
                SelectedItem.district_name = null;
                SelectedItem.district_id = Guid.Empty;
            }
            if (Province != null && Province.Id != Guid.Empty)
            {
                SelectedItem.province_name = Province.Name;
                SelectedItem.province_id = Province.Id;
            }
            else
            {
                SelectedItem.province_name = null;
                SelectedItem.province_id = Guid.Empty;
            }
            if (Country != null && Country.Id != Guid.Empty)
            {
                SelectedItem.country_name = Country.Name;
                SelectedItem.country_id = Country.Id;
            }
            else
            {
                SelectedItem.country_name = null;
                SelectedItem.country_id = Guid.Empty;
            }
            SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
            CloseEvent?.Invoke(this, null);
        }
        public async Task LoadCountryForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_priority' descending='false' />
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_countries", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                list_country_lookup.Add(x);
            }
        }
        public async Task LoadCountryByName()
        {
            if (SelectedItem != null && !string.IsNullOrWhiteSpace(SelectedItem.country_name) && SelectedItem.country_id != Guid.Empty)
            {
                string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_countryname' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_countryname' operator='eq' value='" + SelectedItem.country_name + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhuLongCRM.Models.LookUp>>("bsd_countries", fetch);
                if (result != null && result.value.Count > 0)
                {
                    Country = result.value.FirstOrDefault();
                    SelectedItem.country_name = Country.Name;
                    SelectedItem.country_id = Country.Id;
                }
            }
        }
        public async Task LoadProvincesForLookup()
        {
            if (SelectedItem == null || SelectedItem.country_id == Guid.Empty) return;
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_country' operator='eq' value='" + SelectedItem.country_id + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_provinces", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                list_province_lookup.Add(x);
            }
        }
        public async Task LoadProvinceByName()
        {
            if (SelectedItem != null && !string.IsNullOrWhiteSpace(SelectedItem.province_name) && SelectedItem.country_id != Guid.Empty)
            {
                string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_provincename' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_country' operator='eq' value='" + SelectedItem.country_id + @"' />
                                        <condition attribute='bsd_provincename' operator='eq' value='" + SelectedItem.province_name + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhuLongCRM.Models.LookUp>>("new_provinces", fetch);
                if (result != null && result.value.Count > 0)
                {
                    Province = result.value.FirstOrDefault();
                    SelectedItem.province_name = Province.Name;
                    SelectedItem.province_id = Province.Id;
                }
            }
        }
        public async Task LoadDistrictForLookup()
        {
            if (SelectedItem == null || SelectedItem.province_id == Guid.Empty) return;
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='new_province' operator='eq' value='" + SelectedItem.province_id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_districts", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                list_district_lookup.Add(x);
            }
        }
        public async Task LoadDistrictByName()
        {
            if (SelectedItem != null && !string.IsNullOrWhiteSpace(SelectedItem.district_name) && SelectedItem.province_id != Guid.Empty)
            {
                string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                    <condition attribute='new_province' operator='eq' value='" + SelectedItem.province_id + @"' />
                                    <condition attribute='new_name' operator='eq' value='" + SelectedItem.district_name + @"' />
                                </filter>
                              </entity>
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhuLongCRM.Models.LookUp>>("new_districts", fetch);
                if (result != null && result.value.Count > 0)
                {
                    District = result.value.FirstOrDefault();
                    SelectedItem.district_name = District.Name;
                    SelectedItem.district_id = District.Id;
                }
            }
        }
        private static void ProvinceChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUpViewAddress control = (LookUpViewAddress)bindable;
            control.Province = null;
            control.District = null;
        }
        private static void CountryChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUpViewAddress control = (LookUpViewAddress)bindable;
            control.District = null;
        }
        private static void SelectedItemChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUpViewAddress control = (LookUpViewAddress)bindable;
            control.SetItem();
        }
        private void SetItem()
        {
            if (SelectedItem == null) return;
            if (SelectedItem.district_id != Guid.Empty)
                District = new Models.LookUp { Id = SelectedItem.district_id, Name = SelectedItem.district_name };
            else
                District = null;

            if (SelectedItem.province_id != Guid.Empty)
                Province = new Models.LookUp { Id = SelectedItem.province_id, Name = SelectedItem.province_name };
            else
                Province = null;

            if (SelectedItem.country_id != Guid.Empty)
                Country = new Models.LookUp { Id = SelectedItem.country_id, Name = SelectedItem.country_name };
            else
                Country = null;
        }
        public Grid Footer()
        {
            Grid grid = new Grid();
            grid.Padding = 10;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Button btnClose = new Button();
            btnClose.Text = Language.dong;
            btnClose.BackgroundColor = Color.White;
            btnClose.TextColor = (Color)App.Current.Resources["NavigationPrimary"];
            btnClose.BorderColor = (Color)App.Current.Resources["NavigationPrimary"];
            btnClose.CornerRadius = 10;
            btnClose.BorderWidth = 1;
            btnClose.HeightRequest = 40;
            btnClose.Clicked += CloseAddress_Clicked;
            grid.Children.Add(btnClose);
            Grid.SetColumn(btnClose, 0);
            Grid.SetRow(btnClose, 0);

            Button btnSave = new Button();
            btnSave.Text = Language.luu;
            btnSave.SetBinding(Button.TextProperty, "SelectedItem.lineaddress");
            btnSave.BackgroundColor = (Color)App.Current.Resources["NavigationPrimary"];
            btnSave.TextColor = Color.White;
            btnSave.BorderColor = (Color)App.Current.Resources["NavigationPrimary"];
            btnSave.CornerRadius = 10;
            btnSave.BorderWidth = 1;
            btnSave.HeightRequest = 40;
            btnSave.Clicked += ConfirmAddress_Clicked;
            grid.Children.Add(btnSave);
            Grid.SetColumn(btnSave, 1);
            Grid.SetRow(btnSave, 0);

            return grid;
        }
    }
}