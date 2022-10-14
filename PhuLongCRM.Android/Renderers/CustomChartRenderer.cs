using System;
using Android.Content;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ChartControl), typeof(CustomChartRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class CustomChartRenderer : Telerik.XamarinForms.ChartRenderer.Android.CartesianChartRenderer
    {
        private ChartControl chartControl { get; set; }
        public CustomChartRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Telerik.XamarinForms.Chart.RadCartesianChart> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                chartControl = (ChartControl)e.NewElement;
                var series = this.Control.Series.ToArray();
                AddSecondaryVerticalAxis(series[1]);
            }
        }

        private void AddSecondaryVerticalAxis(Java.Lang.Object barSeries)
        {
            if (barSeries is Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.BarSeries)
            {
                var series = barSeries as Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.BarSeries;
                
                Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.LinearAxis verticalAxisBar = new Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.LinearAxis();
                verticalAxisBar.Minimum = chartControl.MinRightBarSeries;
                verticalAxisBar.Maximum = chartControl.MaxRightBarSeries;
                verticalAxisBar.MajorStep = chartControl.MajorStepRightBarSeries;
                verticalAxisBar.LabelFormat = chartControl.LabelFormatRightBarSeries;
                verticalAxisBar.HorizontalLocation = Com.Telerik.Widget.Chart.Engine.Axes.Common.AxisHorizontalLocation.Right;
                series.VerticalAxis = verticalAxisBar;
            }
        }
    }
}
