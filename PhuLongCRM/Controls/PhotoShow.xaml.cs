using PhuLongCRM.Models;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoShow : ContentPage
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<PhotoCMND>), typeof(LookUp), null, BindingMode.TwoWay, null);
        public ObservableCollection<PhotoCMND> ItemsSource { get => (ObservableCollection<PhotoCMND>)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }
        private int index { get; set; } = 0;

        public PhotoShow(ObservableCollection<PhotoCMND> list_image)
        {
            InitializeComponent();
            this.BindingContext = this;
            if(list_image != null && list_image.Count>0)
            {
                ItemsSource = list_image;
            }
        }    

        public async void Show(Page view, int i)
        {
            if (ItemsSource != null)
            {
                await view.Navigation.PushModalAsync(this);
                if (ItemsSource != null && ItemsSource.Count > 0)
                {
                    if (i >= 0 && i < ItemsSource.Count)
                    //carousel.ScrollTo(i);
                    {
                        image.Source = ItemsSource[i].ImageSoure;
                        index = i;
                    }
                }
            }
        }

        private async void Hide_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void Image_Swiped(object sender, MR.Gestures.SwipeEventArgs e)
        {
            if (ItemsSource != null && image.Scale == 1)
            {
                if(e.Direction == MR.Gestures.Direction.Left)
                {
                    ScrollTo(index - 1);
                }
                else if (e.Direction == MR.Gestures.Direction.Left)
                {
                    ScrollTo(index + 1);
                }
               var  a = ((int)e.Direction);
            }
        }

        private void ScrollTo(int i)
        {
            if(ItemsSource!= null)
            {
                if(0 >= i && i <= ItemsSource.Count)
                {
                    image.Source = ItemsSource[i].ImageSoure;
                    index = i;
                }
            }
        }
    } 
}