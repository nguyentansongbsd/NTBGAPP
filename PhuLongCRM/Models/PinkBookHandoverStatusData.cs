using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class PinkBookHandoverStatusData
    {
        public static List<StatusCodeModel> GetPinkBookHandoverStatus()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.hieu_luc_ban_giao_gcn_sts,"#A0DB8E"),
                new StatusCodeModel("100000000",Language.da_ban_giao_gcn_sts,"#2FCC71"),
                new StatusCodeModel("100000001",Language.huy,"#808080"),
                new StatusCodeModel("2",Language.vo_hieu_luc_ban_giao_gcn_sts,"#6897BB"),
            };
        }

        public static StatusCodeModel GetPinkBookHandoverStatusById(string id)
        {
            return GetPinkBookHandoverStatus().SingleOrDefault(x => x.Id == id);
        }
    }
}
