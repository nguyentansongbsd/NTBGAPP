using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class PinkBookHandoverStatusData
    {
        public static List<StatusCodeModel> GetPinkBookHandoverStatus()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1","Active","#A0DB8E"),
                new StatusCodeModel("100000000","Handed over certificate","#2FCC71"),
                new StatusCodeModel("100000001","Cancelled","#808080"),
                new StatusCodeModel("2","Inactive","#6897BB"),
            };
        }

        public static StatusCodeModel GetPinkBookHandoverStatusById(string id)
        {
            return GetPinkBookHandoverStatus().SingleOrDefault(x => x.Id == id);
        }
    }
}
