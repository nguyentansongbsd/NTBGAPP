using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class LichLamViec : ContentPage
    {
        public List<OptionSet> data { get; set; }
        public LichLamViec()
        {
            InitializeComponent();
            this.BindingContext = this;
            Init();
        }

        public async void Init()
        {
            data = new List<OptionSet>() { new OptionSet("1",Language.lich_lam_viec_theo_thang), new OptionSet("2", Language.lich_lam_viec_theo_tuan), new OptionSet("3", Language.lich_lam_viec_theo_ngay),};
            listView.ItemsSource = data;
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            OptionSet item = e.Item as OptionSet;         
            if (item.Val == "1")
            {
                LoadingHelper.Show();
                LichLamViecTheoThang lichLamViecTheoThang = new LichLamViecTheoThang();
                lichLamViecTheoThang.OnComplete = async (OnComplete) =>
                {
                    if (OnComplete == true)
                    {
                        await Navigation.PushAsync(lichLamViecTheoThang);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_lich_lam_viec);
                    }
                };
            } else if (item.Val == "2")
            {
                LoadingHelper.Show();
                LichLamViecTheoTuan lichLamViecTheoTuan = new LichLamViecTheoTuan();
                lichLamViecTheoTuan.OnComplete = async (OnComplete) =>
                {
                    if (OnComplete == true)
                    {
                        await Navigation.PushAsync(lichLamViecTheoTuan);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_lich_lam_viec);
                    }
                };               
            }else if (item.Val == "3")
            {
                LoadingHelper.Show();
                LichLamViecTheoNgay lichLamViecTheoNgay = new LichLamViecTheoNgay();
                lichLamViecTheoNgay.OnComplete = async (OnComplete) =>
                {
                    if (OnComplete == true)
                    {
                        await Navigation.PushAsync(lichLamViecTheoNgay);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_lich_lam_viec);
                    }
                };              
            }         
        }
    }
}
