using System;
using Telerik.XamarinForms.Chart;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class ChartControl : RadCartesianChart
    {
        public static readonly BindableProperty MinRightBarSeriesProperty = BindableProperty.Create(nameof(MinRightBarSeries), typeof(int), typeof(ChartControl), 0, BindingMode.TwoWay);
        public int MinRightBarSeries { get => (int)GetValue(MinRightBarSeriesProperty); set => SetValue(MinRightBarSeriesProperty, value); }
        public static readonly BindableProperty MaxRightBarSeriesProperty = BindableProperty.Create(nameof(MaxRightBarSeries), typeof(int), typeof(ChartControl), 10, BindingMode.TwoWay);
        public int MaxRightBarSeries { get => (int)GetValue(MaxRightBarSeriesProperty); set => SetValue(MaxRightBarSeriesProperty, value); }

        public static readonly BindableProperty MajorStepRightBarSeriesProperty = BindableProperty.Create(nameof(MajorStepRightBarSeries), typeof(int), typeof(ChartBarSeriesControl), 10, BindingMode.TwoWay);
        public int MajorStepRightBarSeries { get => (int)GetValue(MajorStepRightBarSeriesProperty); set => SetValue(MajorStepRightBarSeriesProperty, value); }

        public static readonly BindableProperty LabelFormatRightBarSeriesProperty = BindableProperty.Create(nameof(LabelFormatRightBarSeries), typeof(string), typeof(ChartBarSeriesControl), "%.0f", BindingMode.TwoWay);
        public string LabelFormatRightBarSeries { get => (string)GetValue(LabelFormatRightBarSeriesProperty); set => SetValue(LabelFormatRightBarSeriesProperty, value); }
    }
}
