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

[assembly: Xamarin.Forms.ExportRenderer(typeof(Telerik.XamarinForms.Chart.RadCartesianChart), typeof(CustomChartRenderer))]
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
            TelerikUI.TKChartNumericAxis tKChartNumericAxis = new TelerikUI.TKChartNumericAxis(new NSNumber(0), new NSNumber(20));
            tKChartNumericAxis.MajorTickInterval = 5;
            tKChartNumericAxis.Position = TelerikUI.TKChartAxisPosition.Right;
            tKChartNumericAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.Left;
            tKChartNumericAxis.Style.MajorTickStyle.TicksHidden = false;
            tKChartNumericAxis.Style.LineHidden = false;
            this.Control.AddAxis(tKChartNumericAxis);

            var barSeries = this.Control.Series[0] as TelerikUI.TKChartColumnSeries;
            //barSeries.AllowClustering = true;
            var barSeries1 = this.Control.Series[1] as TelerikUI.TKChartColumnSeries;
            barSeries1.YAxis = tKChartNumericAxis;
            
            //barSeries1.AllowClustering = true;

            //var a = this.Control.Series[1];
            //a.YAxis = tKChartNumericAxis;

            Control.BeginUpdates();
            Control.AddSeries(barSeries);
            Control.AddSeries(barSeries1);
            Control.EndUpdates();


            //var pointsWithCategoriesAndValues = new List<TKChartDataPoint>();
            //var pointsWithCategoriesAndValues2 = new List<TKChartDataPoint>();
            //var categories = new[] { "Greetings", "Perfecto", "NearBy", "Family Store", "Fresh & Green" };
            //var values = new[] { 70, 75, 58, 59, 88 };

            //for (int i = 0; i < categories.Length; ++i)
            //{
            //    pointsWithCategoriesAndValues.Add(new TKChartDataPoint(new NSNumber(values[i]), NSObject.FromObject(categories[i])));
            //}

            //var values2 = new[] { 40, 80, 32, 69, 95 };
            //for (int i = 0; i < categories.Length; ++i)
            //{
            //    pointsWithCategoriesAndValues2.Add(new TKChartDataPoint(new NSNumber(values2[i]), NSObject.FromObject(categories[i])));
            //}

            //List<NSObject> objectCategories = new List<NSObject>();
            //for (int i = 0; i < categories.Length; i++)
            //{
            //    objectCategories.Add(new NSString(categories[i]));
            //}
            //var categoryAxis = new TKChartCategoryAxis(objectCategories.ToArray());
            //Control.YAxis = categoryAxis;

            //var series1 = new TKChartBarSeries(pointsWithCategoriesAndValues.ToArray());
            //series1.YAxis = categoryAxis;

            //var series2 = new TKChartBarSeries(pointsWithCategoriesAndValues2.ToArray());
            //series2.YAxis = categoryAxis;

            //Control.BeginUpdates();
            //Control.AddSeries(series1);
            //Control.AddSeries(series2);
            //Control.EndUpdates();



        }

        protected override void UpdateNativeWidget()
        {
            base.UpdateNativeWidget();

            //TelerikUI.TKChartNumericAxis tKChartNumericAxis = new TelerikUI.TKChartNumericAxis(new NSNumber(0), new NSNumber(20));
            //tKChartNumericAxis.MajorTickInterval = 5;
            //tKChartNumericAxis.Position = TelerikUI.TKChartAxisPosition.Right;
            //tKChartNumericAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.Left;
            //tKChartNumericAxis.Style.MajorTickStyle.TicksHidden = false;
            //tKChartNumericAxis.Style.LineHidden = false;
            //this.Control.AddAxis(tKChartNumericAxis);

            //var lineSeries = this.Control.Series[1] as TelerikUI.TKChartColumnSeries;
            //lineSeries.YAxis = tKChartNumericAxis;
            



            //TelerikUI.TKChartDateTimeAxis dateTimeAxis = new TelerikUI.TKChartDateTimeAxis();
            //dateTimeAxis.Position = TelerikUI.TKChartAxisPosition.Top;
            //dateTimeAxis.MajorTickInterval = 10;
            //dateTimeAxis.MajorTickIntervalUnit = TelerikUI.TKChartDateTimeAxisIntervalUnit.Days;
            //this.Control.AddAxis(dateTimeAxis);
            //lineSeries.XAxis = dateTimeAxis;
        }


        //protected override void OnElementChanged(ElementChangedEventArgs<RadCartesianChart> e)
        //{
        //    base.OnElementChanged(e);
        //    TKExtendedChart control = this.Control;
        //    if (control != null)
        //    {
        //        //TKChartNumericAxis tKChartNumericAxis = new TKChartNumericAxis();
        //        //tKChartNumericAxis.Position = TKChartAxisPosition.Left;
        //        //tKChartNumericAxis.Style.LabelStyle.TextColor = UIColor.Red;
        //        //tKChartNumericAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.Left;
        //        //this.Control.AddAxis(tKChartNumericAxis);

        //        this.Control.YAxis.Style.LineHidden = false;
        //        this.Control.YAxis.Style.LabelStyle.TextAlignment = TKChartAxisLabelAlignment.VerticalCenter;
        //        this.Control.YAxis.Style.LabelStyle.FirstLabelTextAlignment = TKChartAxisLabelAlignment.VerticalCenter;
        //    }

        //    //if (this.Control.Series.Length == 2)
        //    //{
        //    //    TKChartNumericAxis nativeAxis = this.Control.Series[1].YAxis as TKChartNumericAxis;
        //    //    nativeAxis.Position = TKChartAxisPosition.Left;
        //    //    nativeAxis.Style.LabelStyle.TextColor = UIColor.Green;
        //    //    nativeAxis.Style.LineHidden = false;
        //    //    nativeAxis.Style.LineStroke = new TKStroke(UIColor.White);
        //    //    this.Control.Series[1].YAxis = nativeAxis;
        //    //}
        //}
    }
}
