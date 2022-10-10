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
        public CustomChartRenderer()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<RadCartesianChart> e)
        {
            base.OnElementChanged(e);
            TelerikUI.TKChartNumericAxis tKChartNumericAxis = new TelerikUI.TKChartNumericAxis(new NSNumber(0), new NSNumber(100));
            tKChartNumericAxis.MajorTickInterval = 10;
            tKChartNumericAxis.Position = TelerikUI.TKChartAxisPosition.Right;
            tKChartNumericAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.Left;
            tKChartNumericAxis.Style.MajorTickStyle.TicksHidden = false;
            tKChartNumericAxis.Style.LineHidden = false;
            this.Control.AddAxis(tKChartNumericAxis);

            var barSeries1 = this.Control.Series[1] as TelerikUI.TKChartLineSeries;
            barSeries1.YAxis = tKChartNumericAxis;
            
        }
    }
}
