using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class ListInforUnitTLKD 
    {
        public Guid productid { get; set; }
        public string name { get; set; }
        public string bsd_projectname { get; set; }
        public string description { get; set; }
        public string bsd_unitscodesams { get; set; }
        public string statuscode { get; set; }
        public string statuscode_format { get { return statuscode != null ? StatusCodeUnit.GetStatusCodeById(statuscode.ToString())?.Name : null; } }
        public string statuscode_color { get { return statuscode != null ? StatusCodeUnit.GetStatusCodeById(statuscode.ToString())?.Background : null; } }
        public DateTime createdon { get; set; }
        public string createdonformat
        {
            get => StringHelper.DateFormat(createdon);
        }
        public string productnumber { get; set; }
    }
}

