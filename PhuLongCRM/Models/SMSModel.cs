using System;
using PhuLongCRM.Helper;

namespace PhuLongCRM.Models
{
    public class SMSModel
    {
        public string id { get; set; }
        public string address { get; set; }
        public string phone { get { return PhoneNumberHelper.formatPhoneNumber(address); } }

        public string name { get; set; }
        public string msg { get; set; }
        public string readState { get; set; }
        public string timeTick { get; set; }
        public DateTime time
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(this.timeTick)).ToLocalTime();
            }
        }

        public Xamarin.Forms.Color backgroundColor { get { return type == "1" ? Xamarin.Forms.Color.LightGray : Xamarin.Forms.Color.SkyBlue; } }
        public Xamarin.Forms.Color textColor { get { return type == "1" ? Xamarin.Forms.Color.Black : Xamarin.Forms.Color.White; } }
        public Xamarin.Forms.Thickness paddingContent { get { return type == "1" ? new Xamarin.Forms.Thickness(10,5,50,5) : new Xamarin.Forms.Thickness(50,5,10,5); } }

        public string folderName { get; set; }
        public string type { get; set; }
    }
}
