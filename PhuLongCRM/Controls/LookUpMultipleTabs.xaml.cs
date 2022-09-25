using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.ViewModels;
using PhuLongCRM.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookUpMultipleTabs : Grid
    {
        public event EventHandler<LookUpChangeEvent> SelectedItemChange;
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public object SelectedItem { get => (object)GetValue(SelectedItemProperty); set { SetValue(SelectedItemProperty, value); } }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        public bool ShowLead { get; set; } = false;
        public bool LoadNewLead { get; set; } = false;
        public bool ShowContact { get; set; } = false;
        public bool ShowAccount { get; set; } = false;
        public bool ShowAddButton { get; set; } = false;

        private RadBorder TabsLead;

        private RadBorder TabsContact;

        private RadBorder TabsAccount;

        private ListViewMultiTabs ListLead;

        private ListViewMultiTabs ListContact;

        private ListViewMultiTabs ListAccount;

        public CenterModal CenterModal { get; set; }

        private Grid gridMain;

        private Grid gridTabs;
        public bool PreOpenOneTime { get; set; } = true;
        private int numberTab { get; set; } = 0;

        public static string CodeAccount = "3";

        public static string CodeContac = "2";

        public static string CodeLead = "1";

        public static readonly BindableProperty RootProperty = BindableProperty.Create(nameof(Root), typeof(Page), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public Page Root { get => (Page)GetValue(RootProperty); set => SetValue(RootProperty, value); }

        public LookUpMultipleTabs()
        {
            InitializeComponent();
            this.Entry.BindingContext = this;
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, "Placeholder");
            this.Entry.SetBinding(EntryNoneBorder.TextProperty, "SelectedItem.Label");
            this.BtnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = this, Converter = new Converters.NullToHideConverter() });
        }

        public void Clear_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = null;
            SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
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
            if (this.CenterModal == null) return;

            if (ShowLead == false && ShowContact == false && ShowAccount == false) return;

            if (gridTabs == null && gridMain == null)
            {
                SetUpModal();
            }    

            CenterModal.Title = Placeholder;
            CenterModal.Body = gridMain;
            await CenterModal.Show();
        }    

        private void SetUpModal()
        {
            gridTabs = new Grid();
            gridMain = new Grid();
            gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            if (ShowLead == true)
            {
                TabsLead = CreateTabs(Language.kh_tiem_nang);
                gridTabs.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                gridTabs.Children.Add(TabsLead);
                Grid.SetColumn(TabsLead, numberTab);
                Grid.SetRow(TabsLead, 0);
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += Lead_Tapped;
                TabsLead.GestureRecognizers.Add(tap);

                SetUpLead();
                gridMain.Children.Add(ListLead);
                Grid.SetColumn(ListLead, 0);
                Grid.SetRow(ListLead, 1);
                ListLead.IsVisible = false;

                if (numberTab == 0)
                {
                    ListLead.IsVisible = true;
                    var lb = TabsLead.Content as Label;
                    VisualStateManager.GoToState(TabsLead, "Selected");
                    VisualStateManager.GoToState(lb, "Selected");
                }
                numberTab++;

            }
            if (ShowContact == true)
            {
                TabsContact = CreateTabs(Language.kh_ca_nhan);
                gridTabs.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                gridTabs.Children.Add(TabsContact);
                Grid.SetColumn(TabsContact, numberTab);
                Grid.SetRow(TabsContact, 0);
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += Contact_Tapped;
                TabsContact.GestureRecognizers.Add(tap);

                SetUpContact();
                gridMain.Children.Add(ListContact);
                Grid.SetColumn(ListContact, 0);
                Grid.SetRow(ListContact, 1);
                ListContact.IsVisible = false;

                if (numberTab == 0)
                {
                    ListContact.IsVisible = true;
                    var lb = TabsContact.Content as Label;
                    VisualStateManager.GoToState(TabsContact, "Selected");
                    VisualStateManager.GoToState(lb, "Selected");
                }
                numberTab++;
            }
            if (ShowAccount == true)
            {
                TabsAccount = CreateTabs(Language.kh_doanh_nghiep);
                gridTabs.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                gridTabs.Children.Add(TabsAccount);
                Grid.SetColumn(TabsAccount, numberTab);
                Grid.SetRow(TabsAccount, 0);
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += Account_Tapped;
                TabsAccount.GestureRecognizers.Add(tap);

                SetUpAccount();
                gridMain.Children.Add(ListAccount);
                Grid.SetColumn(ListAccount, 0);
                Grid.SetRow(ListAccount, 1);
                ListAccount.IsVisible = false;

                if (numberTab == 0)
                {
                    ListAccount.IsVisible = true;
                    var lb = TabsAccount.Content as Label;
                    VisualStateManager.GoToState(TabsAccount, "Selected");
                    VisualStateManager.GoToState(lb, "Selected");
                }
                numberTab++;
            }

            if (ShowAddButton == true)
            {
                //gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                //Grid grid = new Grid();
                //grid.HeightRequest = 40;
                //grid.Margin = 5;
                //grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                //grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                //grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                //Button btnNewContact = new Button();
                //btnNewContact.CornerRadius = 10;
                //btnNewContact.FontSize = 15;
                //btnNewContact.TextColor = Color.White;
                //btnNewContact.BackgroundColor = (Color)Application.Current.Resources["NavigationPrimary"];
                //btnNewContact.Text = Language.them_kh_ca_nhan;
                //btnNewContact.Clicked += NewContact_Clicked;
                //grid.Children.Add(btnNewContact);
                //Grid.SetColumn(btnNewContact, 0);
                //Grid.SetRow(btnNewContact, 0);

                //Button btnNewAccount = new Button();
                //btnNewAccount.CornerRadius = 10;
                //btnNewAccount.FontSize = 15;
                //btnNewAccount.TextColor = Color.White;
                //btnNewAccount.BackgroundColor = (Color)Application.Current.Resources["NavigationPrimary"];
                //btnNewAccount.Text = Language.them_kh_doanh_nghiep;
                //btnNewAccount.Clicked += NewAccount_Clicked;
                //grid.Children.Add(btnNewAccount);
                //Grid.SetColumn(btnNewAccount, 1);
                //Grid.SetRow(btnNewAccount, 0);

                Button btnNewCustomer = new Button();
                btnNewCustomer.CornerRadius = 22;
                btnNewCustomer.Margin = 10;
                btnNewCustomer.FontSize = 18;
                btnNewCustomer.TextColor = Color.White;
                btnNewCustomer.BackgroundColor = (Color)Application.Current.Resources["NavigationPrimary"];
                btnNewCustomer.Text = "\uf067";
                btnNewCustomer.FontFamily = "FontAwesomeSolid";
                btnNewCustomer.HeightRequest = 44;
                btnNewCustomer.WidthRequest = 44;
                btnNewCustomer.HorizontalOptions = LayoutOptions.End;
                btnNewCustomer.VerticalOptions = LayoutOptions.End;
                btnNewCustomer.Clicked += NewCustomer_Clicked;

                gridMain.Children.Add(btnNewCustomer);
                Grid.SetColumn(btnNewCustomer, 0);
                Grid.SetRow(btnNewCustomer, 1);
            }

            BoxView boxView = new BoxView();
            boxView.HeightRequest = 1;
            boxView.BackgroundColor = Color.FromHex("F1F1F1");
            boxView.VerticalOptions = LayoutOptions.EndAndExpand;
            gridTabs.Children.Add(boxView);
            Grid.SetColumn(boxView, 0);
            Grid.SetRow(boxView, 0);
            Grid.SetColumnSpan(boxView, numberTab);
            if(numberTab > 1)
            {
                gridMain.Children.Add(gridTabs);
                Grid.SetColumn(gridTabs, 0);
                Grid.SetRow(gridTabs, 0);
            }             
        }

        private async void NewCustomer_Clicked(object sender, EventArgs e)
        {
            if (Root != null)
            {
                LoadingHelper.Show();
                string[] options = new string[] { Language.khach_hang_ca_nhan_option, Language.khach_hang_doanh_nghiep_option };
                string asw = await Root.DisplayActionSheet(Language.tao_khach_hang, Language.huy, null, options);
                if (asw == Language.khach_hang_ca_nhan_option)
                {
                    await Navigation.PushAsync(new ContactForm());
                }
                else if (asw == Language.khach_hang_doanh_nghiep_option)
                {
                    await Navigation.PushAsync(new AccountForm());
                }
                LoadingHelper.Hide();
            }
        }

        private async void NewAccount_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new AccountForm());
            LoadingHelper.Hide();
        }

        private async void NewContact_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new ContactForm());
            LoadingHelper.Hide();
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(TabsAccount);
            if(ListAccount == null)
            {
                SetUpAccount();
                gridMain.Children.Add(ListAccount);
                Grid.SetColumn(ListAccount, 0);
                Grid.SetRow(ListAccount, 1);
            }    

            ListAccount.IsVisible = true;
            if (ListLead != null)
                ListLead.IsVisible = false;
            if(ListContact != null)
                ListContact.IsVisible = false;
        }

        private void Contact_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(TabsContact);
            if(ListContact == null)
            {
                SetUpContact();
                gridMain.Children.Add(ListContact);
                Grid.SetColumn(ListContact, 0);
                Grid.SetRow(ListContact, 1);
            }    

            ListContact.IsVisible = true;
            if (ListLead != null)
                ListLead.IsVisible = false;
            if (ListAccount != null)
                ListAccount.IsVisible = false;
        }

        private void Lead_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(TabsLead);
            if(ListLead == null)
            {
                SetUpLead();
                gridMain.Children.Add(ListLead);
                Grid.SetColumn(ListLead, 0);
                Grid.SetRow(ListLead, 1);
            }    

            ListLead.IsVisible = true;
            if (ListContact != null)
                ListContact.IsVisible = false;
            if (ListAccount != null)
                ListAccount.IsVisible = false;
        }
        public RadBorder CreateTabs(string NameTab)
        {
            RadBorder rd = new RadBorder();
            rd.Style = (Style)Application.Current.Resources["rabBorder_Tab"];
            Label lb = new Label();
            lb.Style = (Style)Application.Current.Resources["Lb_Tab"];
            lb.Text = NameTab;
            lb.LineBreakMode = LineBreakMode.TailTruncation;
            rd.Content = lb;
            return rd;
        }
      
        private void Tab_Tapped(RadBorder radBorder)
        {
            if (radBorder != null && gridTabs != null)
            {
                for (int i = 0; i < gridTabs.Children.Count-1; i++)
                {
                    if(gridTabs.Children[i] == radBorder)
                    {
                        var rd = gridTabs.Children[i] as RadBorder;
                        var lb = rd.Content as Label;
                        VisualStateManager.GoToState(rd, "Selected");
                        VisualStateManager.GoToState(lb, "Selected");
                    }
                    else
                    {
                        var rd = gridTabs.Children[i] as RadBorder;
                        var lb = rd.Content as Label;
                        VisualStateManager.GoToState(rd, "Normal");
                        VisualStateManager.GoToState(lb, "Normal");
                    }
                }
            }
        }
        private void SetUpContact()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Val' />
                    <attribute name='fullname' alias='Label' />
                    <attribute name='mobilephone' alias='SDT' />
                    <attribute name='bsd_identitycardnumber' alias='CMND' />
                    <attribute name='bsd_identitycard' alias='CCCD' />
                    <attribute name='bsd_customercode' alias='CustomerCode'/>
                    <attribute name='bsd_passport' alias='HC' />
                    <order attribute='fullname' descending='false' />                   
                    <filter type='and'>
                        <condition attribute='statecode' operator='eq' value='0' />
                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                        <filter type='or'>
                            <condition attribute='bsd_fullname' operator='like' value='%25key%25' />
                            <condition attribute='bsd_identitycardnumber' operator='like' value='%25key%25' />
                            <condition attribute='mobilephone' operator='like' value='%25key%25' />
                            <condition attribute='emailaddress1' operator='like' value='%25key%25' />
                            <condition attribute='bsd_customercode' operator='like' value='%25key%25' />
                        </filter>
                    </filter>
                  </entity>
                </fetch>";
            string entity = "contacts";

            ListContact = new ListViewMultiTabs(fetch,entity, moreInfo: true);
            ListContact.ItemTapped = async (item) =>
            {
                if (item != null)
                {
                    item.Title = CodeContac;
                    SelectedItem = item;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                    await CenterModal.Hide();
                }
            };
        }

        private void SetUpLead()
        {
            string loadNewLead = null;
            if(LoadNewLead)
            {
                loadNewLead = @" <condition attribute='statecode' operator='in'>
                                    <value>0</value>
                                  </condition>";
            }

            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' alias='Label' />
                                <attribute name='leadid' alias='Val' />
                                <attribute name='mobilephone' alias='SDT' />
                                <attribute name='bsd_customercode' alias='CustomerCode'/>
                                <attribute name='bsd_identitycardnumberid' alias='CMND' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    {loadNewLead}
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    <filter type='or'>
                                        <condition attribute='lastname' operator='like' value='%25key%25' />
                                        <condition attribute='mobilephone' operator='like' value='%25key%25' />
                                        <condition entityname='Topic' attribute='bsd_name' operator='like' value='%25key%25' />
                                        <condition attribute='bsd_customercode' operator='like' value='%25key%25' />
                                    </filter>
                                </filter>
                                <link-entity name='bsd_topic' from='bsd_topicid' to='bsd_topic' link-type='inner' alias='Topic'>
                                    <attribute name='bsd_name' />
                                </link-entity>
                              </entity>
                            </fetch>";

            string entity = "leads";

            ListLead = new ListViewMultiTabs(fetch, entity, moreInfo: true);
            ListLead.ItemTapped = async (item) =>
            {
                if (item != null)
                {
                    item.Title = CodeLead;
                    SelectedItem = item;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                    await CenterModal.Hide();
                }
            };
        }

        private void SetUpAccount()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='accountid' alias='Val'/>
                                <attribute name='telephone1' alias='SDT'/>
                                <attribute name='bsd_customercode' alias='CustomerCode'/>
                                <attribute name='bsd_registrationcode' alias='SoGPKD'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='statecode' operator='eq' value='0' />
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    <filter type='or'>
                                        <condition attribute='name' operator='like' value='%25key%25' />
                                        <condition attribute='telephone1' operator='like' value='%25key%25' />
                                        <condition attribute='bsd_registrationcode' operator='like' value='%25key%25' />
                                        <condition attribute='bsd_customercode' operator='like' value='%25key%25' />
                                    </filter>
                                </filter>
                              </entity>
                            </fetch>";
   
            string entity = "accounts";

            ListAccount = new ListViewMultiTabs(fetch, entity, moreInfo: true);
            ListAccount.ItemTapped = async (item) =>
            {
                if (item != null)
                {
                    item.Title = CodeAccount;
                    SelectedItem = item;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                    await CenterModal.Hide();
                }
            };
        }

        public void Refresh()
        {
            if (ShowLead == true && ListLead != null)
            {
                ListLead.Refresh();
            }
            if (ShowContact == true && ListContact != null)
            {
                ListContact.Refresh();
            }
            if (ShowAccount == true && ListAccount != null)
            {
                ListAccount.Refresh();
            }
        }
    }
}