using System;
using Xamarin.Forms;
using Telerik.XamarinForms.ChartRenderer.iOS;
using PhuLongCRM.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;
using Telerik.XamarinForms.Chart;
using TelerikUI;
using UIKit;

[assembly: ExportRenderer(typeof(Telerik.XamarinForms.Chart.RadCartesianChart), typeof(CustomChartRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class CustomChartRenderer : CartesianChartRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<RadCartesianChart> e)
        {
            base.OnElementChanged(e);
            TKExtendedChart control = this.Control;
            if (control != null)
            {
                //TKChartNumericAxis tKChartNumericAxis = new TKChartNumericAxis();
                //tKChartNumericAxis.Position = TKChartAxisPosition.Left;
                //tKChartNumericAxis.Style.LabelStyle.TextColor = UIColor.Red;
                //tKChartNumericAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.Left;
                //this.Control.AddAxis(tKChartNumericAxis);

                this.Control.YAxis.Style.LineHidden = false;
                this.Control.YAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.VerticalCenter;
                this.Control.YAxis.Style.LabelStyle.FirstLabelTextAlignment = TKChartAxisLabelAlignment.VerticalCenter;
            }

            //if (this.Control.Series.Length == 2)
            //{
            //    TKChartNumericAxis nativeAxis = this.Control.Series[1].YAxis as TKChartNumericAxis;
            //    nativeAxis.Position = TKChartAxisPosition.Left;
            //    nativeAxis.Style.LabelStyle.TextColor = UIColor.Green;
            //    nativeAxis.Style.LineHidden = false;
            //    nativeAxis.Style.LineStroke = new TKStroke(UIColor.White);
            //    this.Control.Series[1].YAxis = nativeAxis;
            //}
        }
    }
}
