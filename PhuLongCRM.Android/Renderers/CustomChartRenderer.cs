using System;
using Android.Content;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ChartControl), typeof(CustomChartRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class CustomChartRenderer : Telerik.XamarinForms.ChartRenderer.Android.CartesianChartRenderer
    {
        public CustomChartRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Telerik.XamarinForms.Chart.RadCartesianChart> e)
        {
            base.OnElementChanged(e);

            var series = this.Control.Series.ToArray();
            AddSecondaryVerticalAxis(series[1]);
        }

        private void AddSecondaryVerticalAxis(Java.Lang.Object lineSeries)
        {
            if (lineSeries is Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.LineSeries)
            {
                var series = lineSeries as Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.LineSeries;

                Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.LinearAxis verticalAxisBar = new Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.LinearAxis();
                verticalAxisBar.Minimum = 0;
                verticalAxisBar.Maximum = 100;
                verticalAxisBar.MajorStep = 10;
                verticalAxisBar.LabelFormat = "%.0f";
                verticalAxisBar.HorizontalLocation = Com.Telerik.Widget.Chart.Engine.Axes.Common.AxisHorizontalLocation.Right;
                series.VerticalAxis = verticalAxisBar;
            }
        }
    }
}
