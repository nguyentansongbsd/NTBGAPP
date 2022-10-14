using Newtonsoft.Json;
using PhuLongCRM.Config;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace PhuLongCRM
{
    public partial class BlankPage : ContentPage
    {
        //public ObservableCollection<CategoricalData> Data { get; set; } = new ObservableCollection<CategoricalData>();
        public ObservableCollection<CategoricalData> CategoricalData { get; set; } = new ObservableCollection<CategoricalData>();
        public ObservableCollection<CategoricalData> CategoricalData2 { get; set; } = new ObservableCollection<CategoricalData>();

        public BlankPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            Init();

        }

        public async void Init()
        {
            //await LoadUnitSpecificationDetails();
            //this.CategoricalData = GetCategoricalData();
            //this.CategoricalData2 = GetCategoricalData2();
            //this.Data.Add(new CategoricalData { Category = "A", Value = 23 });
            //this.Data.Add(new CategoricalData { Category = "B", Value = 10 });
            //this.Data.Add(new CategoricalData { Category = "C", Value = 60 });
            //this.Data.Add(new CategoricalData { Category = "D", Value = 55 });
            //this.Data.Add(new CategoricalData { Category = "E", Value = 55 });
            //this.Data.Add(new CategoricalData { Category = "F", Value = 23 });
            //this.Data.Add(new CategoricalData { Category = "G", Value = 10 });
            //this.Data.Add(new CategoricalData { Category = "H", Value = 60 });
            //this.Data.Add(new CategoricalData { Category = "I", Value = 55 });
            //this.Data.Add(new CategoricalData { Category = "K", Value = 55 });
            //this.Data.Add(new CategoricalData { Category = "L", Value = 23 });
            //this.Data.Add(new CategoricalData { Category = "M", Value = 10 });
            //this.Data.Add(new CategoricalData { Category = "N", Value = 60 });
            //this.Data.Add(new CategoricalData { Category = "O", Value = 55 });
            //this.Data.Add(new CategoricalData { Category = "P", Value = 55 });

            this.CategoricalData.Add(new CategoricalData { Category = "A", Value = 100});
            this.CategoricalData.Add(new CategoricalData { Category = "B", Value = 250});
            this.CategoricalData.Add(new CategoricalData { Category = "C", Value = 175 });
            this.CategoricalData.Add(new CategoricalData { Category = "D", Value = 300});
            this.CategoricalData.Add(new CategoricalData { Category = "E", Value = 400 });
            this.CategoricalData.Add(new CategoricalData { Category = "F", Value = 350 });

            this.CategoricalData2.Add(new CategoricalData { Category = "A", Value = 2 });
            this.CategoricalData2.Add(new CategoricalData { Category = "B", Value = 4 });
            this.CategoricalData2.Add(new CategoricalData { Category = "C", Value = 6 });
            this.CategoricalData2.Add(new CategoricalData { Category = "D", Value = 7 });
            this.CategoricalData2.Add(new CategoricalData { Category = "E", Value = 10});
            this.CategoricalData2.Add(new CategoricalData { Category = "F", Value = 40 });

        }

        //private static ObservableCollection<CategoricalData> GetCategoricalData()
        //{
        //    var data = new ObservableCollection<CategoricalData>  {
        //    new CategoricalData { Category = "A", Value = 23 },
        //    new CategoricalData { Category = "B", Value = 10 },
        //    new CategoricalData { Category = "C", Value = 60 },
        //    new CategoricalData { Category = "D", Value = 55 },
        //    new CategoricalData { Category = "D", Value = 55 },
        //};

        //    return data;
        //}

        

        //public static ObservableCollection<CategoricalData> GetCategoricalData()
        //{
        //    var date = DateTime.Today;
        //    var data = new ObservableCollection<CategoricalData>
        //                {
        //                    new CategoricalData { Category = "A", Value = 0.63, Time = date.AddDays(1) },
        //                    new CategoricalData { Category = "B", Value = 0.85, Time = date.AddDays(10)},
        //                    new CategoricalData { Category = "C", Value = 0.75, Time = date.AddDays(20)},
        //                    new CategoricalData { Category = "D", Value = 0.96, Time = date.AddDays(30)},
        //                    new CategoricalData { Category = "E", Value = 0.78, Time = date.AddDays(40)},
        //                };

        //    return data;
        //}

        //public static ObservableCollection<CategoricalData> GetCategoricalData2()
        //{
        //    var date = DateTime.Now;
        //    var data = new ObservableCollection<CategoricalData>
        //            {
        //                new CategoricalData { Category = "A", Time = date.AddDays(1) , Value = 0.53},
        //                new CategoricalData { Category = "B", Time = date.AddDays(10), Value = 0.65},
        //                new CategoricalData { Category = "C", Time = date.AddDays(20), Value = 0.55},
        //                new CategoricalData { Category = "D", Time = date.AddDays(30), Value = 0.76},
        //                new CategoricalData { Category = "E", Time = date.AddDays(40), Value = 0.58},
        //            };

        //    return data;
        //}

    }
    public class CategoricalData
    {
        public object Category { get; set; }
        public double Value { get; set; }
        //public DateTime Time { get; set; }
    }
}
