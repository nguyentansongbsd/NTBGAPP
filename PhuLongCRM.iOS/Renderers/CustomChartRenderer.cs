using System;
using Xamarin.Forms;
using Telerik.XamarinForms.ChartRenderer.iOS;
using PhuLongCRM.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;
using Telerik.XamarinForms.Chart;
using TelerikUI;
using UIKit;
using Foundation;
using System.Collections.Generic;
using PhuLongCRM.Controls;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ChartControl), typeof(CustomChartRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class CustomChartRenderer : Telerik.XamarinForms.ChartRenderer.iOS.CartesianChartRenderer
    {
        TelerikUI.TKChartNumericAxis tKChartNumericAxis { get; set; } = new TelerikUI.TKChartNumericAxis(new NSNumber(0), new NSNumber(100));
        public CustomChartRenderer()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<RadCartesianChart> e)
        {
            base.OnElementChanged(e);
            tKChartNumericAxis.MajorTickInterval = 10;
            tKChartNumericAxis.Position = TelerikUI.TKChartAxisPosition.Right;
            tKChartNumericAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.Left;
            tKChartNumericAxis.Style.MajorTickStyle.TicksHidden = false;
            tKChartNumericAxis.Style.LineHidden = false;
            this.Control.AddAxis(tKChartNumericAxis);
        }

        protected override void UpdateNativeWidget()
        {
            base.UpdateNativeWidget();

            if (this.Control.Series.Length == 2)
            {
                if (this.Control.Series[1] is TKChartColumnSeries)
                {
                    var barSeries = this.Control.Series[1] as TKChartColumnSeries;
                    barSeries.AllowClustering = true;
                    barSeries.YAxis = tKChartNumericAxis;
                }
            }

            //var barSeries = this.Control.Series[0] as TelerikUI.TKChartColumnSeries;
            //var barSeries1 = this.Control.Series[1] as TelerikUI.TKChartColumnSeries;
            //barSeries1.AllowClustering = true;
            //barSeries1.YAxis = tKChartNumericAxis;


            //TKChartColumnSeries series = new TKChartColumnSeries(barSeries1.);


            //series.Selection = TKChartSeriesSelection.Series;

            //this.Control.AddSeries(barSeries1);
        }
    }
}
