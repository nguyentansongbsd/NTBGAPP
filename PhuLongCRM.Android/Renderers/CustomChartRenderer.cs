using System;
using Android.Content;
using PhuLongCRM.Droid.Renderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Telerik.XamarinForms.Chart.RadCartesianChart), typeof(CustomChartRenderer))]
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

            //AddSecondaryHorizontalAxis(series[1]);
            AddSecondaryVerticalAxis(series[1]);
        }

        private void AddSecondaryVerticalAxis(Java.Lang.Object lineSeries)
        {
            if (lineSeries is Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.BarSeries)
            {
                var series = lineSeries as Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.BarSeries;

                Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.LinearAxis verticalAxisBar = new Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.LinearAxis();
                verticalAxisBar.Minimum = 0;
                verticalAxisBar.Maximum = 20;
                verticalAxisBar.MajorStep = 5;
                verticalAxisBar.LabelFormat = "%.0f";
                verticalAxisBar.HorizontalLocation = Com.Telerik.Widget.Chart.Engine.Axes.Common.AxisHorizontalLocation.Right;
                series.VerticalAxis = verticalAxisBar;
            }
        }

        private void AddSecondaryHorizontalAxis(Java.Lang.Object lineSeries)
        {
            if (lineSeries is Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.LineSeries)
            {
                var series = lineSeries as Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical.LineSeries;

                Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.DateTimeCategoricalAxis horizontalAXis = new Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes.DateTimeCategoricalAxis();
                horizontalAXis.VerticalLocation = Com.Telerik.Widget.Chart.Engine.Axes.Common.AxisVerticalLocation.Top;
                series.HorizontalAxis = horizontalAXis;
            }
        }
    }
}
