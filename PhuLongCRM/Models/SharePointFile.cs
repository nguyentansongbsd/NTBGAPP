using PhuLongCRM.Config;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class SharePointFile : BaseViewModel
    {
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
    }

    public class SharePointFieldResult
    {
        public List<SharePointFile> value { get; set; }
    }
}
