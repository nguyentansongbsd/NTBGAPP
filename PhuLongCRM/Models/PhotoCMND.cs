using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class PhotoCMND : BaseViewModel
    {
        public string _label;
        public string Label { get => _label; set { _label = value; OnPropertyChanged(nameof(Label)); } }

        private ImageSource _imageSoure;
        public ImageSource ImageSoure { get => _imageSoure; set { _imageSoure = value; OnPropertyChanged(nameof(ImageSoure)); } }
    }
}
