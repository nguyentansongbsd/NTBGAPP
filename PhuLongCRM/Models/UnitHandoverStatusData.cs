using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class UnitHandoverStatusData
    {
        public static List<StatusCodeModel> GetUnitHandoverStatus()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1","Active","#A0DB8E"),
                new StatusCodeModel("100000003","Cancel","#808080"),
                new StatusCodeModel("100000000","Not Handover","#14A184"),
                new StatusCodeModel("100000001","Handover","#2FCC71"),
                new StatusCodeModel("100000002","PinkBook Handover","#04A8F4"),
                new StatusCodeModel("2","Inactive","#6897BB"),
            };
        }

        public static StatusCodeModel GetUnitHandoverById(string id)
        {
            return GetUnitHandoverStatus().SingleOrDefault(x => x.Id == id);
        }
    }
}
