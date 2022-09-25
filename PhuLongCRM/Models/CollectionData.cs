using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class CollectionData
    {
        public string Id { get; set; }
        public string MediaSourceId { get; set; }
        public string ImageSource { get; set; }
        public string ImageSourceBase64 { get; set; }
        public SharePointType SharePointType { get; set; }
        public int Index { get; set; }
        public string UrlPdfFile { get; set; }
        public string PdfName { get; set; }

        public CollectionData()
        { }
    }
    public enum SharePointType
    {
        Video,
        Image,
        Pdf
    }
}
