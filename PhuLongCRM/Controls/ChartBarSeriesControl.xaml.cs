using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PhuLongCRM.Models;
using Telerik.XamarinForms.Chart;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class ChartBarSeriesControl : ContentView
    {
        public static readonly BindableProperty FirBarSeriesDataProperty = BindableProperty.Create(nameof(FirBarSeriesData), typeof(IEnumerable), typeof(ChartBarSeriesControl), null, BindingMode.TwoWay, propertyChanged: DataChanged);
        public IEnumerable FirBarSeriesData { get => (IEnumerable)GetValue(FirBarSeriesDataProperty); set => SetValue(FirBarSeriesDataProperty, value); }
        public static readonly BindableProperty SecBarSeriesDataProperty = BindableProperty.Create(nameof(SecBarSeriesData), typeof(IEnumerable), typeof(ChartBarSeriesControl), null, BindingMode.TwoWay);
        public IEnumerable SecBarSeriesData { get => (IEnumerable)GetValue(SecBarSeriesDataProperty); set => SetValue(SecBarSeriesDataProperty, value); }

        public static readonly BindableProperty FirBarSeriesColorProperty = BindableProperty.Create(nameof(FirBarSeriesColor), typeof(Color), typeof(ChartBarSeriesControl), Color.FromHex("#2196F3"), BindingMode.TwoWay);
        public Color FirBarSeriesColor { get => (Color)GetValue(FirBarSeriesColorProperty); set => SetValue(FirBarSeriesColorProperty, value); }
        public static readonly BindableProperty SecBarSeriesColorProperty = BindableProperty.Create(nameof(SecBarSeriesColor), typeof(Color), typeof(ChartBarSeriesControl), Color.FromHex("#D42A16"), BindingMode.TwoWay);
        public Color SecBarSeriesColor { get => (Color)GetValue(SecBarSeriesColorProperty); set => SetValue(SecBarSeriesColorProperty, value); }

        public static readonly BindableProperty MinLeftBarSeriesProperty = BindableProperty.Create(nameof(MinLeftBarSeries), typeof(int), typeof(ChartBarSeriesControl), 0, BindingMode.TwoWay);
        public int MinLeftBarSeries { get => (int)GetValue(MinLeftBarSeriesProperty); set => SetValue(MinLeftBarSeriesProperty, value); }
        public static readonly BindableProperty MaxLeftBarSeriesProperty = BindableProperty.Create(nameof(MaxLeftBarSeries), typeof(int), typeof(ChartBarSeriesControl), 500, BindingMode.TwoWay);
        public int MaxLeftBarSeries { get => (int)GetValue(MaxLeftBarSeriesProperty); set => SetValue(MaxLeftBarSeriesProperty, value); }

        public static readonly BindableProperty MinRightBarSeriesProperty = BindableProperty.Create(nameof(MinRightBarSeries), typeof(int), typeof(ChartBarSeriesControl), 0, BindingMode.TwoWay);
        public int MinRightBarSeries { get => (int)GetValue(MinRightBarSeriesProperty); set => SetValue(MinRightBarSeriesProperty, value); }
        public static readonly BindableProperty MaxRightBarSeriesProperty = BindableProperty.Create(nameof(MaxRightBarSeries), typeof(int), typeof(ChartBarSeriesControl), 100, BindingMode.TwoWay);
        public int MaxRightBarSeries { get => (int)GetValue(MaxRightBarSeriesProperty); set => SetValue(MaxRightBarSeriesProperty, value); }

        public static readonly BindableProperty MajorStepLeftBarSeriesProperty = BindableProperty.Create(nameof(MajorStepLeftBarSeries), typeof(int), typeof(ChartBarSeriesControl), 100, BindingMode.TwoWay);
        public int MajorStepLeftBarSeries { get => (int)GetValue(MajorStepLeftBarSeriesProperty); set => SetValue(MajorStepLeftBarSeriesProperty, value); }
        public static readonly BindableProperty MajorStepRightBarSeriesProperty = BindableProperty.Create(nameof(MajorStepRightBarSeries), typeof(int), typeof(ChartBarSeriesControl), 10, BindingMode.TwoWay);
        public int MajorStepRightBarSeries { get => (int)GetValue(MajorStepRightBarSeriesProperty); set => SetValue(MajorStepRightBarSeriesProperty, value); }

        public static readonly BindableProperty LabelFormatLeftBarSeriesProperty = BindableProperty.Create(nameof(LabelFormatLeftBarSeries), typeof(string), typeof(ChartBarSeriesControl), "#,0 tr", BindingMode.TwoWay);
        public string LabelFormatLeftBarSeries { get => (string)GetValue(LabelFormatLeftBarSeriesProperty); set => SetValue(LabelFormatLeftBarSeriesProperty, value); }
        public static readonly BindableProperty LabelFormatRightBarSeriesProperty = BindableProperty.Create(nameof(LabelFormatRightBarSeries), typeof(string), typeof(ChartBarSeriesControl), Device.RuntimePlatform == Device.Android ? "%.0f" : null, BindingMode.TwoWay);
        public string LabelFormatRightBarSeries { get => (string)GetValue(LabelFormatRightBarSeriesProperty); set => SetValue(LabelFormatRightBarSeriesProperty, value); }

        public static readonly BindableProperty MarginRadCartesianChat1Property = BindableProperty.Create(nameof(MarginRadCartesianChat1), typeof(Thickness), typeof(ChartBarSeriesControl), new Thickness(0,20,26,0), BindingMode.TwoWay);
        public Thickness MarginRadCartesianChat1 { get => (Thickness)GetValue(MarginRadCartesianChat1Property); set => SetValue(MarginRadCartesianChat1Property, value); }
        public static readonly BindableProperty MarginRadCartesianChat2Property = BindableProperty.Create(nameof(MarginRadCartesianChat2), typeof(Thickness), typeof(ChartBarSeriesControl), new Thickness(40,20,0,0), BindingMode.TwoWay);
        public Thickness MarginRadCartesianChat2 { get => (Thickness)GetValue(MarginRadCartesianChat2Property); set => SetValue(MarginRadCartesianChat2Property, value); }

        private ObservableCollection<ChartModel> DataNull { get; set; } = new ObservableCollection<ChartModel>();

        public ChartBarSeriesControl()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                //Set Binding ItemSource
                barSeriesIOs1.SetBinding(BarSeries.ItemsSourceProperty, new Binding("FirBarSeriesData") { Source = this });
                barSeriesIOs2.SetBinding(BarSeries.ItemsSourceProperty, new Binding("DataNull") { Source = this });
                barSeriesIOs3.SetBinding(BarSeries.ItemsSourceProperty, new Binding("DataNull") { Source = this });
                barSeriesIOs4.SetBinding(BarSeries.ItemsSourceProperty, new Binding("SecBarSeriesData") { Source = this });

                //Set Binding Min Max for Y
                numericalAxisLeft.SetBinding(NumericalAxis.MinimumProperty, new Binding("MinLeftBarSeries") { Source = this });
                numericalAxisLeft.SetBinding(NumericalAxis.MaximumProperty, new Binding("MaxLeftBarSeries") { Source = this });
                numericalAxisRight.SetBinding(NumericalAxis.MinimumProperty, new Binding("MinRightBarSeries") { Source = this });
                numericalAxisRight.SetBinding(NumericalAxis.MaximumProperty, new Binding("MaxRightBarSeries") { Source = this });

                //Set Binding MajorStep for Y
                numericalAxisLeft.SetBinding(NumericalAxis.MajorStepProperty, new Binding("MajorStepLeftBarSeries") { Source = this });
                numericalAxisRight.SetBinding(NumericalAxis.MajorStepProperty, new Binding("MajorStepRightBarSeries") { Source = this });

                //Set Binding LabelFormat for Y
                numericalAxisLeft.SetBinding(NumericalAxis.LabelFormatProperty, new Binding("LabelFormatLeftBarSeries") { Source = this });
                numericalAxisRight.SetBinding(NumericalAxis.LabelFormatProperty, new Binding("LabelFormatRightBarSeries") { Source = this });

                //Set Binding Magin
                radCartesianChatIOs1.SetBinding(RadCartesianChart.MarginProperty, new Binding("MarginRadCartesianChat1") { Source = this });
                radCartesianChatIOs2.SetBinding(RadCartesianChart.MarginProperty, new Binding("MarginRadCartesianChat2") { Source = this });
            }
            else
            {
                //Set Binding ItemSource
                barSeriesAndroid1.SetBinding(BarSeries.ItemsSourceProperty, new Binding("FirBarSeriesData") { Source = this });
                barSeriesAndroid2.SetBinding(BarSeries.ItemsSourceProperty, new Binding("SecBarSeriesData") { Source = this });

                //Set Binding Min Max for Y
                numericalAxisLeftAndroid.SetBinding(NumericalAxis.MinimumProperty, new Binding("MinLeftBarSeries") { Source = this });
                numericalAxisLeftAndroid.SetBinding(NumericalAxis.MaximumProperty, new Binding("MaxLeftBarSeries") { Source = this });
                charControl.SetBinding(ChartControl.MinRightBarSeriesProperty, new Binding("MinRightBarSeries") { Source = this });
                charControl.SetBinding(ChartControl.MaxRightBarSeriesProperty, new Binding("MaxRightBarSeries") { Source = this });

                //Set Binding MajorStep
                numericalAxisLeftAndroid.SetBinding(NumericalAxis.MajorStepProperty, new Binding("MajorStepLeftBarSeries") { Source = this });
                charControl.SetBinding(ChartControl.MajorStepRightBarSeriesProperty, new Binding("MajorStepRightBarSeries") { Source = this });

                //Set Binding LabelFormat
                numericalAxisLeftAndroid.SetBinding(NumericalAxis.LabelFormatProperty, new Binding("LabelFormatLeftBarSeries") { Source = this });
                charControl.SetBinding(ChartControl.LabelFormatRightBarSeriesProperty, new Binding("LabelFormatRightBarSeries") { Source = this });
            }

        }

        private static void DataChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ChartBarSeriesControl control = (ChartBarSeriesControl)bindable;
            if (newValue == null) return;

            if (oldValue == null && newValue != null)
            {
                // Set Color for BarSeries
                if (Device.RuntimePlatform == Device.iOS)
                {
                    control.chartPaletteFirBarSeriesIOs.Entries.Add(new PaletteEntry(control.FirBarSeriesColor, control.FirBarSeriesColor));
                    control.chartPaletteFirBarSeriesIOs.Entries.Add(new PaletteEntry(Color.Transparent, Color.Transparent));
                    control.chartPaletteSecBarSeriesIOs.Entries.Add(new PaletteEntry(Color.Transparent, Color.Transparent));
                    control.chartPaletteSecBarSeriesIOs.Entries.Add(new PaletteEntry(control.SecBarSeriesColor, control.SecBarSeriesColor));
                }
                else
                {
                    control.chartPaletteBarSeriesAndroid.Entries.Add(new PaletteEntry(control.FirBarSeriesColor, control.FirBarSeriesColor));
                    control.chartPaletteBarSeriesAndroid.Entries.Add(new PaletteEntry(control.SecBarSeriesColor, control.SecBarSeriesColor));
                }
                
            }
        }
    }
}
