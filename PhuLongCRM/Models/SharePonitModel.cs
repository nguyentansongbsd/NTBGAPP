using System;
using System.Collections.Generic;

namespace PhuLongCRM.Models
{
    public class SharePonitModel
    {
        public int documentid { get; set; }
        public string sharepointdocumentid { get; set; }
        public string absoluteurl { get; set; }
        public string fullname { get; set; }
        public string filetype { get; set; }
        public string relativelocation { get; set; }
        public string author { get; set; }
    }
    public class SharePointGraphModel
    {
        public string eTag { get; set; }
        public string name { get; set; }
        public string id
        {
            get
            {
                var b = eTag.Split(',');
                return b[0].Replace('"', ' ').Replace("{", "").Replace("}", "").Trim();
            }
        }
        public string type
        {
            get
            {
                if (name.Contains(".jpg") || name.Contains(".jpeg") || name.Contains(".png"))
                    return "image";
                else if (name.Contains(".mp4") || name.Contains(".flv") || name.Contains(".m3u8") || name.Contains(".3gp") || name.Contains(".mov") || name.Contains(".avi") || name.Contains(".wmv"))
                    return "video";
                else if (name.Contains(".pdf"))
                    return "pdf";
                else return null;
            }
        }
    }

}
