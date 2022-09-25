using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabControl : Grid
    {
        public static readonly BindableProperty ListTabProperty = BindableProperty.Create(nameof(ListTab), typeof(string), typeof(TabControl), null, propertyChanged: ListTabChanged);
        private static void ListTabChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            TabControl control = (TabControl)bindable;
            control.SetUpTab();
        }
        public string ListTab
        {
            get { return (string)GetValue(ListTabProperty); }
            set { SetValue(ListTabProperty, value); }
        }

        public event EventHandler<LookUpChangeEvent> IndexTab;

        //ngôn ngữ
        private static System.Globalization.CultureInfo resourceCulture;

        ResourceManager resourceManager = new ResourceManager("PhuLongCRM.Resources.Language", typeof(Resources.Language).Assembly);      

        public TabControl()
        {
            InitializeComponent();
        }

        private void SetUpTab()
        {
            if (!string.IsNullOrWhiteSpace(ListTab))
            {
                var tabs = ListTab.Split(',').ToList();
                if (tabs != null && tabs.Count > 0)
                {
                    for (int i = 0; i < tabs.Count; i++)
                    {
                        var tab = CreateTabs(tabs[i]);
                        this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        this.Children.Add(tab);
                        Grid.SetColumn(tab, i);
                        Grid.SetRow(tab, 0);
                        TapGestureRecognizer tapped = new TapGestureRecognizer();
                        tapped.Tapped += Tapped_Tapped;
                        tab.GestureRecognizers.Add(tapped);
                    }
                    BoxView boxView = new BoxView();
                    boxView.HeightRequest = 1;
                    boxView.BackgroundColor = Color.FromHex("939393");
                    boxView.VerticalOptions = LayoutOptions.EndAndExpand;
                    this.Children.Add(boxView);
                    Grid.SetColumn(boxView, 0);
                    Grid.SetRow(boxView, 0);
                    Grid.SetColumnSpan(boxView, tabs.Count);
                    Tab_Tapped(this.Children[0] as RadBorder);
                }
            }
        }

        private void Tapped_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(sender as RadBorder);
        }

        public RadBorder CreateTabs(string NameTab)
        {
            RadBorder rd = new RadBorder();
            rd.Style = (Style)Application.Current.Resources["rabBorder_Tab"]; // UI trong app.xaml

            Label lb = new Label();
            lb.Style = (Style)Application.Current.Resources["Lb_Tab"];// UI trong app.xaml
            lb.Text = GetStringByKey(NameTab);
            lb.LineBreakMode = LineBreakMode.TailTruncation;
            rd.Content = lb;
            return rd;
        }

        private void Tab_Tapped(RadBorder radBorder)
        {
            if (radBorder != null)
            {
                for (int i = 0; i < this.Children.Count -1; i++) //-1 do có boxview ở cuối
                {
                    if (this.Children[i] == radBorder)
                    {
                        var rd = this.Children[i] as RadBorder;
                        var lb = rd.Content as Label;
                        VisualStateManager.GoToState(rd, "Selected");// UI trong app.xaml
                        VisualStateManager.GoToState(lb, "Selected");// UI trong app.xaml
                        EventHandler<LookUpChangeEvent> eventHandler = IndexTab;
                        eventHandler?.Invoke((object)this, new LookUpChangeEvent { Item = i});
                    }
                    else
                    {
                        var rd = this.Children[i] as RadBorder;
                        var lb = rd.Content as Label;
                        VisualStateManager.GoToState(rd, "Normal");// UI trong app.xaml
                        VisualStateManager.GoToState(lb, "Normal");// UI trong app.xaml
                    }
                }
            }
        }
        private string GetStringByKey(string key)
        {
            var value = resourceManager.GetString(key, new CultureInfo(UserLogged.Language));
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            else
                return key;
        }
    }
}