using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Helper
{
    public class ActivityDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Phone { get; set; }
        public DataTemplate Meet { get; set; }
        public DataTemplate Task { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var activity = item as HoatDongListModel;
            if (activity.activitytypecode == "phonecall")
                return Phone;
            else if (activity.activitytypecode == "task")
                return Task;
            else
                return Meet;
        }
    }
}
