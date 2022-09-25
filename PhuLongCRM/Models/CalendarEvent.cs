using PhuLongCRM.Resources;
using System;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class CalendarEvent : Appointment
    {
        private const string timeFormat = "HH:mm dd/MM/yy";

        public CalendarEvent() { }

        public CalendarEvent(ListActivitiesAcc activity, bool isEventAllDay = false)
        {
            this.Activity = activity;
            switch (activity.activitytypecode)
            {
                case "task":
                    if(activity.statecode == "0")
                    {
                        this.LeadBorderColor = Color.FromHex("#ea520b");  //Orange Color
                        this.ItemBackgroundColor = Color.White;
                    }
                    else
                    {
                        this.LeadBorderColor = Color.FromHex("#f79364"); //Light Orange
                        this.ItemBackgroundColor = Color.FromHex("#f79364"); 
                    }
                    activitytype_label = Language.cong_viec;//"Công việc";
                    break;
                case "phonecall":
                    if (activity.statecode == "0")
                    {
                        this.LeadBorderColor = Color.FromHex("#146189"); //Blue
                        this.ItemBackgroundColor = Color.White;
                    }
                    else
                    {
                        this.LeadBorderColor = Color.FromHex("#3aa8e3"); //Light Blue
                        this.ItemBackgroundColor = Color.FromHex("#3aa8e3");
                    }
                    activitytype_label = Language.cuoc_goi;//"Cuộc gọi";
                    break;
                case "appointment":
                    if (activity.statecode == "0")
                    {
                        this.LeadBorderColor = Color.FromHex("#8eff82"); //Green
                        this.ItemBackgroundColor = Color.White;
                    }
                    else
                    {
                        this.LeadBorderColor = Color.FromHex("#d3ffce"); //Light Green
                        this.ItemBackgroundColor = Color.FromHex("#d3ffce");
                    }
                    activitytype_label = Language.cuoc_hop;// "Cuộc họp";
                    break;
            }
            switch (activity.statecode)
            {
                case "0":
                    status_label = Language.activity_open_sts; // "Open";
                    break;
                case "1":
                    status_label = Language.activity_completed_sts; //"Completed";
                    break;
                case "2":
                    status_label = Language.activity_cancelled_sts; //"Canceled";
                    break;
                case "3":
                    status_label = Language.activity_scheduled_sts; //"Scheduled";
                    break;
            }
            this.Color = LeadBorderColor;
            this.EndDate = activity.scheduledend.ToLocalTime();
            this.IsEventAllDay = isEventAllDay;
            this.StartDate = activity.scheduledstart.ToLocalTime();
            this.Title = activity.subject;

            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.UWP)
            {
                this.AllDayString = Language.ca_ngay; //"ALL DAY";
            }
            else
            {
                this.AllDayString = Language.ca_ngay; //"All Day";
            }

            this.Detail = activitytype_label + " (" + status_label + ") \n" + StartTimeString + " - " + EndTimeString;
        }

        public string AllDayString { get; }

        public string EndTimeString
        {
            get
            {
                return this.EndDate.ToLocalTime().ToString(timeFormat);
            }
        }

        public bool IsEventAllDay { get; set; }

        public Color LeadBorderColor { get; set; }

        public Color ItemBackgroundColor { get; set; }

        public string StartTimeString
        {
            get
            {
                return this.StartDate.ToLocalTime().ToString(timeFormat);
            }
        }

        public string activitytype_label { get; set; }

        public string status_label { get; set; }

        public ListActivitiesAcc Activity { get; set; }

        public DateTime? dateForGroupingWeek { get; set; }
    }
}
