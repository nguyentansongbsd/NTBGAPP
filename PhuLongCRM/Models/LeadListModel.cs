using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class LeadListModel
    {
        public Guid leadid { get; set; }
        public string lastname { get; set; }
        public string fullname { get; set; }
        public string mobilephone { get; set; }
        public string mobilephone_format
        {
            get
            {
                if (mobilephone != null && mobilephone.Contains("-"))
                {
                    return mobilephone.Split('-')[1].StartsWith("84") ? mobilephone.Replace("84", "+84-") : mobilephone;
                }
                else if (mobilephone != null && mobilephone.Contains("+84"))
                {
                    return mobilephone.Replace("+84", "+84-");
                }
                else if (mobilephone != null && mobilephone.StartsWith("84"))
                {
                    return mobilephone.Replace("84", "+84-");
                }
                else
                {
                    return mobilephone;
                }
            }
        }
        public string subject { get; set; }
        public string statuscode { get; set; }
        public string telepphone1 { get; set; }
        public string emailaddress1 { get; set; }
        public string bsd_contactaddress { get; set; }
        public int leadqualitycode { get; set; }
        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get
            {
                return this.createdon.ToString("dd/MM/yyyy");
            }
        }
        public string statuscode_format { get { return statuscode != null ? LeadStatusCodeData.GetLeadStatusCodeById(statuscode)?.Name : null; } }
        public string statuscode_color { get { return statuscode != null ? LeadStatusCodeData.GetLeadStatusCodeById(statuscode)?.Background : "#808080"; } }
        public string bsd_customercode { get; set; } // mã khách hàng
        public string leadqualitycode_color
        {
            get
            {
                if (leadqualitycode == 1)
                    return "#C60707";
                else if (leadqualitycode == 2)
                    return "#D96800";
                else if (leadqualitycode == 3)
                    return "#0016D9";
                else
                    return null;
            }
        }
        public string leadqualitycode_format
        {
            get
            {
                if (leadqualitycode == 1)
                    return "Hot";
                else if (leadqualitycode == 2)
                    return "Warm";
                else if (leadqualitycode == 3)
                    return "Cold";
                else
                    return null;
            }
        }
    }
}
