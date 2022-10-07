using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookUpMultiSelect : Grid
    {
        public event EventHandler<LookUpChangeEvent> SelectedItemChange;
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpMultiSelect), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SelectedIdsProperty = BindableProperty.Create(nameof(SelectedIds), typeof(List<OptionSetFilter>), typeof(LookUpMultiSelect), null, BindingMode.TwoWay, null, propertyChanged: ItemSourceChange);
        public List<OptionSetFilter> SelectedIds { get => (List<OptionSetFilter>)GetValue(SelectedIdsProperty); set { SetValue(SelectedIdsProperty, value); } }

        private string _text;
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
        public bool ShowLead { get; set; } = false;
        public bool LoadNewLead { get; set; } = false;
        public bool ShowContact { get; set; } = false;
        public bool ShowAccount { get; set; } = false;

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
        private List<OptionSetFilter> Lead_itemselecteds { get; set; } = new List<OptionSetFilter>();
        private List<OptionSetFilter> Contact_itemselecteds { get; set; } = new List<OptionSetFilter>();
        private List<OptionSetFilter> Account_itemselecteds { get; set; } = new List<OptionSetFilter>();
        public Guid ne_customer { get; set; }
        public LookUpMultiSelect()
        {
            InitializeComponent();
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, new Binding("Placeholder") { Source = this });
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

                if (numberTab == 0)
                {
                    SetUpLead();
                    gridMain.Children.Add(ListLead);
                    Grid.SetColumn(ListLead, 0);
                    Grid.SetRow(ListLead, 1);

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

                if (numberTab == 0)
                {
                    SetUpContact();
                    gridMain.Children.Add(ListContact);
                    Grid.SetColumn(ListContact, 0);
                    Grid.SetRow(ListContact, 1);

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

                if (numberTab == 0)
                {
                    SetUpAccount();
                    gridMain.Children.Add(ListAccount);
                    Grid.SetColumn(ListAccount, 0);
                    Grid.SetRow(ListAccount, 1);

                    ListAccount.IsVisible = true;
                    var lb = TabsAccount.Content as Label;
                    VisualStateManager.GoToState(TabsAccount, "Selected");
                    VisualStateManager.GoToState(lb, "Selected");
                }
                numberTab++;
            }

            gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Grid grid = new Grid();
            grid.HeightRequest = 40;
            grid.Margin = 5;
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            //Button btnSave = new Button();
            //btnSave.Padding = 5;
            //btnSave.CornerRadius = 10;
            //btnSave.FontSize = 16;
            //btnSave.TextColor = Color.White;
            //btnSave.TextTransform = TextTransform.None;
            //btnSave.BackgroundColor = (Color)Application.Current.Resources["NavigationPrimary"];
            //btnSave.Text = "Đóng";
            //btnSave.Clicked += Clear_Clicked;
            //grid.Children.Add(btnSave);
            //Grid.SetColumn(btnSave, 0);
            //Grid.SetRow(btnSave, 0);

            Button btnClose = new Button();
            btnClose.Padding = 5;
            btnClose.CornerRadius = 10;
            btnClose.FontSize = 16;
            btnClose.TextColor = Color.White;
            btnClose.TextTransform = TextTransform.None;
            btnClose.BackgroundColor = (Color)Application.Current.Resources["NavigationPrimary"];
            btnClose.Text = Language.luu;//"Lưu";
            btnClose.Clicked += SaveButton_Clicked;
            btnClose.FontAttributes = FontAttributes.Bold;
            grid.Children.Add(btnClose);
            Grid.SetColumn(btnClose, 0);
            Grid.SetRow(btnClose, 0);

            gridMain.Children.Add(grid);
            Grid.SetColumn(grid, 0);
            Grid.SetRow(grid, 2);

            BoxView boxView = new BoxView();
            boxView.HeightRequest = 1;
            boxView.BackgroundColor = Color.FromHex("F1F1F1");
            boxView.VerticalOptions = LayoutOptions.EndAndExpand;
            gridTabs.Children.Add(boxView);
            Grid.SetColumn(boxView, 0);
            Grid.SetRow(boxView, 0);
            Grid.SetColumnSpan(boxView, numberTab);
            if (numberTab > 1)
            {
                gridMain.Children.Add(gridTabs);
                Grid.SetColumn(gridTabs, 0);
                Grid.SetRow(gridTabs, 0);
            }
        }
        private void Account_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(TabsAccount);
            if (ListAccount == null)
            {
                SetUpAccount();
                gridMain.Children.Add(ListAccount);
                Grid.SetColumn(ListAccount, 0);
                Grid.SetRow(ListAccount, 1);
            }

            ListAccount.IsVisible = true;
            if (ListLead != null)
                ListLead.IsVisible = false;
            if (ListContact != null)
                ListContact.IsVisible = false;
        }
        private void Contact_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(TabsContact);
            if (ListContact == null)
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
            if (ListLead == null)
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
                for (int i = 0; i < gridTabs.Children.Count - 1; i++)
                {
                    if (gridTabs.Children[i] == radBorder)
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
            string ne_cus = null;
            if (ne_customer != Guid.Empty)
            {
                ne_cus = "<condition attribute='contactid' operator='ne' value='" + ne_customer + @"' />";
            }
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Val' />
                    <attribute name='fullname' alias='Label' />
                    <attribute name='mobilephone' alias='SDT' />
                    <attribute name='bsd_identitycardnumber' alias='CMND' />
                    <attribute name='bsd_passport' alias='HC' />
                    <order attribute='fullname' descending='false' />                   
                    <filter type='and'>
                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                        { ne_cus}
                        <filter type='or'>
                            <condition attribute='bsd_fullname' operator='like' value='%25key%25' />
                            <condition attribute='bsd_identitycardnumber' operator='like' value='%25key%25' />
                            <condition attribute='mobilephone' operator='like' value='%25key%25' />
                            <condition attribute='bsd_passport' operator='like' value='%25key%25' />
                            <condition attribute='bsd_identitycard' operator='like' value='%25key%25' />
                            <condition attribute='emailaddress1' operator='like' value='%25key%25' />
                            <condition attribute='bsd_customercode' operator='like' value='%25key%25' />
                        </filter>
                    </filter>
                  </entity>
                </fetch>";
            string entity = "contacts";

            ListContact = new ListViewMultiTabs(fetch, entity, true, Contact_itemselecteds);
            ListContact.ItemTapped = async (item) =>
            {
                if (item != null)
                {
                    item.Title = CodeContac;

                    if (SelectedIds == null)
                        SelectedIds = new List<OptionSetFilter>();

                    if (item.Selected == true)
                    {
                        SelectedIds.Remove(SelectedIds.FirstOrDefault(x => x.Val == item.Val));
                    }
                    else
                    {
                        SelectedIds.Add(item);
                    }
                    item.Selected = !item.Selected;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                }
            };
        }
        private void SetUpLead()
        {
            string loadNewLead = null;
            if (LoadNewLead)
            {
                loadNewLead = @" <condition attribute='statecode' operator='in'>
                                    <value>0</value>
                                  </condition>";
            }
            string ne_cus = null;
            if (ne_customer != Guid.Empty)
            {
                ne_cus = "<condition attribute='leadid' operator='ne' value='" + ne_customer + @"' />";
            }

            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' alias='Label' />
                                <attribute name='leadid' alias='Val' />
                                <attribute name='mobilephone' alias='SDT' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    {loadNewLead}
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    {ne_cus}
                                </filter>
                              </entity>
                            </fetch>";

            string entity = "leads";

            ListLead = new ListViewMultiTabs(fetch, entity, true, Lead_itemselecteds);
            ListLead.ItemTapped = async (item) =>
            {
                if (item != null)
                {
                    item.Title = CodeLead;

                    if (SelectedIds == null)
                        SelectedIds = new List<OptionSetFilter>();

                    if (item.Selected == true)
                    {
                        SelectedIds.Remove(SelectedIds.FirstOrDefault(x => x.Val == item.Val));
                    }
                    else
                    {
                        SelectedIds.Add(item);
                    }
                    item.Selected = !item.Selected;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                }
            };
        }
        private void SetUpAccount()
        {
            string ne_cus = null;
            if (ne_customer != Guid.Empty)
            {
                ne_cus = "<condition attribute='accountid' operator='ne' value='" + ne_customer + @"' />";
            }
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='accountid' alias='Val'/>
                                <attribute name='telephone1' alias='SDT'/>
                                <attribute name='bsd_registrationcode' alias='SoGPKD'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    {ne_cus}
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

            ListAccount = new ListViewMultiTabs(fetch, entity, true, Account_itemselecteds);
            ListAccount.ItemTapped = async (item) =>
            {
                if (item != null)
                {
                    item.Title = CodeAccount;

                    if (SelectedIds == null)
                        SelectedIds = new List<OptionSetFilter>();

                    if (item.Selected == true)
                    {
                        SelectedIds.Remove(SelectedIds.FirstOrDefault(x=>x.Val == item.Val));
                    }
                    else
                    {
                        SelectedIds.Add(item);
                    }
                    item.Selected = !item.Selected;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
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
        public void SetFlexLayout(List<OptionSetFilter> selectedInSource)
        {
            if (selectedInSource != null && selectedInSource.Count > 0)
            {
                Lead_itemselecteds = selectedInSource.Where(x => x.Title == CodeLead).ToList();
                Contact_itemselecteds = selectedInSource.Where(x => x.Title == CodeContac).ToList();
                Account_itemselecteds = selectedInSource.Where(x => x.Title == CodeAccount).ToList();
            }
            if (selectedInSource.Count > 0)
            {
                this.Entry.IsVisible = false;
                this.flexLayout.IsVisible = true;
            }
            else
            {
                ClearFlexLayout();
            }
            selectedInSource.Add(new OptionSetFilter()
            {
                Val = "0"
            });

            BindableLayout.SetItemsSource(flexLayout, selectedInSource);
            var last = flexLayout.Children.Last() as StackLayout;
            //var radBorder = flexLayout.Children.Last() as RadBorder;
            var radBorder = last.Children[0] as RadBorder;
            radBorder.BackgroundColor = Color.Gray;
            (radBorder.Content as Label).Text = "\uf00d";
            (radBorder.Content as Label).FontSize = Device.RuntimePlatform == Device.iOS ? 16 : 17;
            (radBorder.Content as Label).HorizontalTextAlignment = TextAlignment.Center;
            (radBorder.Content as Label).VerticalTextAlignment = TextAlignment.Center;
            (radBorder.Content as Label).FontFamily = "FontAwesomeSolid";
            (radBorder.Content as Label).TextColor = Color.White;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Clear_Clicked;

            radBorder.GestureRecognizers.Add(tap);
        }
        public void ClearFlexLayout()
        {
            flexLayout.IsVisible = false;
            Entry.IsVisible = true;
        }
        private void Clear_Clicked(object sender, EventArgs e) => ClearData();
        public void ClearData()
        {
            if (SelectedIds == null || SelectedIds.Any() == false) return;
            this.Text = null;
            if (ListLead != null)
                ListLead.viewModel.Data.ForEach(x => x.Selected = false);
            if (ListContact != null)
                ListContact.viewModel.Data.ForEach(x => x.Selected = false);
            if (ListAccount != null)
                ListAccount.viewModel.Data.ForEach(x => x.Selected = false);
            SelectedIds = null;
            ClearFlexLayout();
        }
        public async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (SelectedIds != null)
                SetData();
            await CenterModal.Hide();
        }
        private static void ItemSourceChange(BindableObject bindable, object oldValue, object value)
        {
            LookUpMultiSelect control = (LookUpMultiSelect)bindable;
            if (control.SelectedIds != null)
                control.SetData();
        }
        private void SetData()
        {
            if (SelectedIds != null)
            {
                List<OptionSetFilter> item = new List<OptionSetFilter>(SelectedIds);
                SetFlexLayout(item);
            }
        }
    }
}