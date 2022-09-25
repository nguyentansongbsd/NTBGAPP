using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookUpAddress : Grid
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpAddress), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(AddressModel), typeof(LookUpAddress), null, BindingMode.TwoWay, propertyChanged: SelectedItemChang);

        public AddressModel SelectedItem { get => (AddressModel)GetValue(SelectedItemProperty); set { SetValue(SelectedItemProperty, value); } }
        public BottomModal BottomModal { get; set; }
        public CenterModal CenterModal { get; set; }

        public static readonly BindableProperty list_country_lookupProperty = BindableProperty.Create(nameof(list_country_lookup), typeof(ObservableCollection<Models.LookUp>), typeof(LookUpAddress), null, BindingMode.TwoWay, null);
        public ObservableCollection<Models.LookUp> list_country_lookup { get => (ObservableCollection<Models.LookUp>)GetValue(list_country_lookupProperty); set { SetValue(list_country_lookupProperty, value); } }

        public static readonly BindableProperty list_province_lookupProperty = BindableProperty.Create(nameof(list_province_lookup), typeof(ObservableCollection<Models.LookUp>), typeof(LookUpAddress), null, BindingMode.TwoWay, null);
        public ObservableCollection<Models.LookUp> list_province_lookup { get => (ObservableCollection<Models.LookUp>)GetValue(list_province_lookupProperty); set { SetValue(list_province_lookupProperty, value); } }

        public static readonly BindableProperty list_district_lookupProperty = BindableProperty.Create(nameof(list_district_lookup), typeof(ObservableCollection<Models.LookUp>), typeof(LookUpAddress), null, BindingMode.TwoWay, null);
        public ObservableCollection<Models.LookUp> list_district_lookup { get => (ObservableCollection<Models.LookUp>)GetValue(list_district_lookupProperty); set { SetValue(list_district_lookupProperty, value); } }

        public static readonly BindableProperty CountryProperty = BindableProperty.Create(nameof(Country), typeof(Models.LookUp), typeof(LookUpAddress), null, BindingMode.TwoWay);
        public Models.LookUp Country { get => (Models.LookUp)GetValue(CountryProperty); set { SetValue(CountryProperty, value); } }

        public static readonly BindableProperty ProvinceProperty = BindableProperty.Create(nameof(Province), typeof(Models.LookUp), typeof(LookUpAddress), null, BindingMode.TwoWay);
        public Models.LookUp Province { get => (Models.LookUp)GetValue(ProvinceProperty); set { SetValue(ProvinceProperty, value); } }

        public static readonly BindableProperty DistrictProperty = BindableProperty.Create(nameof(District), typeof(Models.LookUp), typeof(LookUpAddress), null, BindingMode.TwoWay);
        public Models.LookUp District { get => (Models.LookUp)GetValue(DistrictProperty); set { SetValue(DistrictProperty, value); } }

        public static readonly BindableProperty LineAddressProperty = BindableProperty.Create(nameof(LineAddress), typeof(string), typeof(LookUpAddress), null, BindingMode.TwoWay);
        public string LineAddress { get => (string)GetValue(LineAddressProperty); set { SetValue(LineAddressProperty, value); } }

        public static readonly BindableProperty AddressProperty = BindableProperty.Create(nameof(Address), typeof(string), typeof(LookUpAddress), null, BindingMode.TwoWay);
        public string Address { get => (string)GetValue(AddressProperty); set { SetValue(AddressProperty, value); } }
        private StackLayout stackLayoutMain { get; set; }
        private Grid gridFooter { get; set; }

        public static readonly BindableProperty AddressCopyProperty = BindableProperty.Create(nameof(AddressCopy), typeof(AddressModel), typeof(LookUpAddress), null, BindingMode.TwoWay, propertyChanged: AddressCopyChang);

        private static void AddressCopyChang(BindableObject bindable, object oldValue, object newValue)
        {
            LookUpAddress control = (LookUpAddress)bindable;
            if (newValue == null)
            {
                control.BtnCopy.IsVisible = false;
                return;
            }
            if (!control.root)
                control.BtnCopy.IsVisible = true;
            else
                control.BtnCopy.IsVisible = false;
            //control.BtnCopy.SetBinding(RadBorder.IsVisibleProperty, new Binding("AddressCopy") { Source = control, Converter = new Converters.NullToHideConverter() });
        }

        public AddressModel AddressCopy { get => (AddressModel)GetValue(AddressCopyProperty); set { SetValue(AddressCopyProperty, value); } }

        public static readonly BindableProperty EnableCopyAddressProperty = BindableProperty.Create(nameof(EnableCopyAddress), typeof(bool), typeof(LookUpAddress), false, BindingMode.TwoWay);
        public bool EnableCopyAddress { get => (bool)GetValue(EnableCopyAddressProperty); set => SetValue(EnableCopyAddressProperty, value); }
        private bool root;
        public LookUpAddress()
        {
            InitializeComponent();
            this.Entry.BindingContext = this;
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, "Placeholder");
            this.Entry.SetBinding(EntryNoneBorder.TextProperty, "Address");
            this.BtnClear.SetBinding(Button.IsVisibleProperty, new Binding("Address") { Source = this, Converter = new Converters.NullToHideConverter() });
            //this.BtnCopy.SetBinding(RadBorder.IsVisibleProperty, new Binding("AddressCopy") { Source = this, Converter = new Converters.NullToHideConverter() });

            list_country_lookup = new ObservableCollection<Models.LookUp>();
            list_province_lookup = new ObservableCollection<Models.LookUp>();
            list_district_lookup = new ObservableCollection<Models.LookUp>();
        }

        public void Clear_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = null;
            Country = Province = District = null;
            Address = LineAddress = null;
            if (root && AddressCopy != null)
            {
                AddressCopy = null;
                root = false;
            }
        }
        public void HideClearButton()
        {
            BtnClear.IsVisible = false;
        }
        public async void OpenLookUp_Tapped(object sender, EventArgs e)
        {
            await OpenModal();
        }

        public async Task OpenModal()
        {
            if (BottomModal == null || CenterModal == null) return;
            if (stackLayoutMain == null)
            {
                setLookUp();
                Footer();
                CenterModal.Body = stackLayoutMain;
                CenterModal.Footer = gridFooter;
                CenterModal.Title = Placeholder;
            }
            await CenterModal.Show();
        }

        private async void LookUpViewAddress_CloseEvent(object sender, EventArgs e)
        {
            await CenterModal.Hide();
        }
        private void setLookUp()
        {
            stackLayoutMain = new StackLayout();
            stackLayoutMain.Padding = 10;

            FormLabel lbCountry = new FormLabel();
            lbCountry.Text = Language.quoc_gia;
            stackLayoutMain.Children.Add(lbCountry);

            LookUp lookUpCountry = new LookUp();
            lookUpCountry.BindingContext = this;
            lookUpCountry.SetBinding(LookUp.ItemsSourceProperty, "list_country_lookup");
            lookUpCountry.SetBinding(LookUp.SelectedItemProperty, "Country");
            lookUpCountry.BottomModal = BottomModal;
            lookUpCountry.Placeholder = Language.chon_quoc_gia;
           // lookUpCountry.SetBinding(LookUp.NameDipslayProperty, "Name");
            lookUpCountry.NameDisplay = "Name";
            stackLayoutMain.Children.Add(lookUpCountry);
            lookUpCountry.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await LoadCountryForLookup();
                LoadingHelper.Hide();
            };
            lookUpCountry.SelectedItemChange += LookUpCountry_SelectedItemChange;

            FormLabel lbProvince = new FormLabel();
            lbProvince.Text = Language.tinh_thanh;
            stackLayoutMain.Children.Add(lbProvince);

            LookUp lookUpProvince = new LookUp();
            lookUpProvince.BindingContext = this;
            lookUpProvince.SetBinding(LookUp.ItemsSourceProperty, "list_province_lookup");
            lookUpProvince.SetBinding(LookUp.SelectedItemProperty, "Province");
            lookUpProvince.BottomModal = BottomModal;
            lookUpProvince.Placeholder = Language.chon_tinh_thanh;
        //    lookUpProvince.SetBinding(LookUp.NameDipslayProperty, "Name");
            lookUpProvince.NameDisplay = "Name";
            stackLayoutMain.Children.Add(lookUpProvince);
            lookUpProvince.SelectedItemChange += LookUpProvince_SelectedItemChange;

            FormLabel lbDistrict = new FormLabel();
            lbDistrict.Text = Language.quan_huyen;
            stackLayoutMain.Children.Add(lbDistrict);

            LookUp lookUpDistrict = new LookUp();
            lookUpDistrict.BindingContext = this;
            lookUpDistrict.SetBinding(LookUp.ItemsSourceProperty, "list_district_lookup");
            lookUpDistrict.SetBinding(LookUp.SelectedItemProperty, "District");
            lookUpDistrict.BottomModal = BottomModal;
            lookUpDistrict.Placeholder = Language.chon_quan_huyen;
          //  lookUpDistrict.SetBinding(LookUp.NameDipslayProperty, "Name");
            lookUpDistrict.NameDisplay = "Name";
            stackLayoutMain.Children.Add(lookUpDistrict);

            FormLabelRequired lbLineAddress = new FormLabelRequired();
            lbLineAddress.Text = Language.so_nha_duong_phuong;
            stackLayoutMain.Children.Add(lbLineAddress);

            MainEntry lineaddress = new MainEntry();
            lineaddress.BindingContext = this;
            lineaddress.Placeholder = Language.nhap_so_nha_duong_phuong;
            lineaddress.SetBinding(MainEntry.TextProperty, "LineAddress");
            stackLayoutMain.Children.Add(lineaddress);
        }
        private async void LookUpProvince_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            District = null;
            list_district_lookup.Clear();
            await LoadDistrictForLookup();
        }
        private async void LookUpCountry_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            District = null;
            Province = null;
            list_province_lookup.Clear();
            list_district_lookup.Clear();
            await LoadProvincesForLookup();
        }
        public async Task LoadCountryForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Models.LookUp>>("bsd_countries", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                if (UserLogged.Language == "en" && !string.IsNullOrWhiteSpace(x.Detail))
                    x.Name = x.Detail;
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
                    if (UserLogged.Language == "en" && !string.IsNullOrWhiteSpace(Country.Detail))
                        Country.Name = Country.Detail;
                    SelectedItem.country_name = Country.Name;
                    SelectedItem.country_id = Country.Id;
                }
            }
        }
        public async Task LoadProvincesForLookup()
        {
            if (Country == null || Country.Id == Guid.Empty) return;
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_country' operator='eq' value='" + Country.Id + @"' />
                                        <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Models.LookUp>>("new_provinces", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                if (UserLogged.Language == "en" && !string.IsNullOrWhiteSpace(x.Detail))
                    x.Name = x.Detail;
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
                    if (UserLogged.Language == "en" && !string.IsNullOrWhiteSpace(Province.Detail))
                        Province.Name = Province.Detail;
                    SelectedItem.province_name = Province.Name;
                    SelectedItem.province_id = Province.Id;
                }
            }
        }
        public async Task LoadDistrictForLookup()
        {
            if (Province == null || Province.Id == Guid.Empty) return;
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                    <condition attribute='new_province' operator='eq' value='" + Province.Id + @"' />
                                    <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Models.LookUp>>("new_districts", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                if (UserLogged.Language == "en" && !string.IsNullOrWhiteSpace(x.Detail))
                    x.Name = x.Detail;
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
                    if (UserLogged.Language == "en" && !string.IsNullOrWhiteSpace(District.Detail))
                        District.Name = District.Detail;
                    SelectedItem.district_name = District.Name;
                    SelectedItem.district_id = District.Id;
                }
            }
        }
        private static void SelectedItemChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUpAddress control = (LookUpAddress)bindable;
            control.SetItem();
        }
        private async void SetItem()
        {
            if (SelectedItem == null) return;
            if (SelectedItem.district_id != Guid.Empty || !string.IsNullOrWhiteSpace(SelectedItem.district_name))
                District = new Models.LookUp { Id = SelectedItem.district_id, Name = SelectedItem.district_name, Detail = SelectedItem.district_name_en };
            if (SelectedItem.province_id != Guid.Empty || !string.IsNullOrWhiteSpace(SelectedItem.province_name))
            {
                Province = new Models.LookUp { Id = SelectedItem.province_id, Name = SelectedItem.province_name, Detail = SelectedItem.province_name_en };
                await LoadDistrictForLookup();
            }
            if (SelectedItem.country_id != Guid.Empty || !string.IsNullOrWhiteSpace(SelectedItem.country_name))
            {
                Country = new Models.LookUp { Id = SelectedItem.country_id, Name = SelectedItem.country_name, Detail = SelectedItem.country_name_en };
                await LoadProvincesForLookup();
            }
            if (!string.IsNullOrWhiteSpace(SelectedItem.lineaddress))
                LineAddress = SelectedItem.lineaddress;
            if (!string.IsNullOrWhiteSpace(SelectedItem.address))
                Address = SelectedItem.address;
        }
        public void Footer()
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
            btnClose.FontAttributes = FontAttributes.Bold;
            grid.Children.Add(btnClose);
            Grid.SetColumn(btnClose, 0);
            Grid.SetRow(btnClose, 0);

            Button btnSave = new Button();
            btnSave.Text = Language.luu;
            btnSave.BackgroundColor = (Color)App.Current.Resources["NavigationPrimary"];
            btnSave.TextColor = Color.White;
            btnSave.BorderColor = (Color)App.Current.Resources["NavigationPrimary"];
            btnSave.CornerRadius = 10;
            btnSave.BorderWidth = 1;
            btnSave.HeightRequest = 40;
            btnSave.Clicked += ConfirmAddress_Clicked;
            btnSave.FontAttributes = FontAttributes.Bold;
            grid.Children.Add(btnSave);
            Grid.SetColumn(btnSave, 1);
            Grid.SetRow(btnSave, 0);
            gridFooter = grid;
        }
        private async void CloseAddress_Clicked(object sender, EventArgs e)
        {
            await CenterModal.Hide();
        }
        private async void ConfirmAddress_Clicked(object sender, EventArgs e)
        {
            List<string> _address = new List<string>();
            List<string> _address_en = new List<string>();
            if (string.IsNullOrWhiteSpace(LineAddress))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_nha_duong_phuong);
                return;
            }
            else
            {
                SelectedItem = new AddressModel();
                SelectedItem.lineaddress = LineAddress;
                SelectedItem.lineaddress_en = LineAddress;
                _address.Add(SelectedItem.lineaddress);
                if (!string.IsNullOrWhiteSpace(SelectedItem.lineaddress_en))
                    _address_en.Add(SelectedItem.lineaddress_en);
            }

            if (District != null && District.Id != Guid.Empty)
            {
                SelectedItem.district_name = District.Name;
                SelectedItem.district_id = District.Id;
                _address.Add(SelectedItem.district_name);
                if (!string.IsNullOrWhiteSpace(District.Detail))
                    _address_en.Add(District.Detail);
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
                _address.Add(SelectedItem.province_name);
                if (!string.IsNullOrWhiteSpace(Province.Detail))
                    _address_en.Add(Province.Detail);
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
                _address.Add(SelectedItem.country_name);
                if (!string.IsNullOrWhiteSpace(Country.Detail))
                    _address_en.Add(Country.Detail);
            }
            else
            {
                SelectedItem.country_name = null;
                SelectedItem.country_id = Guid.Empty;
            }
            Address = SelectedItem.address = string.Join(", ", _address);
            SelectedItem.address_en = string.Join(", ", _address_en);
            if (EnableCopyAddress == true)
            {
                if (AddressCopy == null)
                    root = true;
                if (root)
                {
                    AddressCopy = new AddressModel
                    {
                        country_id = SelectedItem.country_id,
                        country_name = SelectedItem.country_name,
                        country_name_en = SelectedItem.country_name_en,
                        province_id = SelectedItem.province_id,
                        province_name = SelectedItem.province_name,
                        province_name_en = SelectedItem.province_name_en,
                        district_id = SelectedItem.district_id,
                        district_name = SelectedItem.district_name,
                        district_name_en = SelectedItem.district_name_en,
                        address = SelectedItem.address,
                        address_en = SelectedItem.address_en,
                        lineaddress = SelectedItem.lineaddress,
                        lineaddress_en = SelectedItem.lineaddress_en
                    };
                }
            }
            await CenterModal.Hide();
        }

        private void CopyAddress_Tapped(object sender, EventArgs e)
        {
            if (AddressCopy != null)
                SelectedItem = new AddressModel
                {
                    country_id = AddressCopy.country_id,
                    country_name = AddressCopy.country_name,
                    country_name_en = AddressCopy.country_name_en,
                    province_id = AddressCopy.province_id,
                    province_name = AddressCopy.province_name,
                    province_name_en = AddressCopy.province_name_en,
                    district_id = AddressCopy.district_id,
                    district_name = AddressCopy.district_name,
                    district_name_en = AddressCopy.district_name_en,
                    address = AddressCopy.address,
                    address_en = AddressCopy.address_en,
                    lineaddress = AddressCopy.lineaddress,
                    lineaddress_en = AddressCopy.lineaddress_en
                };
        }

    }
}