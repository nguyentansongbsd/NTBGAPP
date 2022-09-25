using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class ListTaiLieuKinhDoanhViewModel : ListViewBaseViewModel2<ListTaiLieuKinhDoanhModel>
    {
        public string Keyword { get; set; }
        public ListTaiLieuKinhDoanhViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "salesliteratures";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
                            <entity name='salesliterature'>
                                <attribute name='name' />
                                <attribute name='salesliteratureid' />
                                <attribute name='createdon' />
                            <order attribute='name' descending='true' />
                            <filter type='and'>
                                <condition attribute='name' operator='like' value='%{Keyword}%' />
                            </filter>
                            <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer'>
                                <attribute name='title' alias='subjecttitle'/>
                            </link-entity>                           
                            </entity>
                          </fetch>";
            });
        }
    }
}

