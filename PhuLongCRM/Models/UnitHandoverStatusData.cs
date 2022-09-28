using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class UnitHandoverStatusData
    {
        public static List<StatusCodeModel> GetUnitHandoverStatus()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.hieu_luc_ban_giao_gcn_sts,"#A0DB8E"),
                new StatusCodeModel("100000003",Language.huy,"#808080"),
                new StatusCodeModel("100000000",Language.khong_ban_giao,"#14A184"),
                new StatusCodeModel("100000001",Language.ban_giao_sts,"#2FCC71"),
                new StatusCodeModel("100000002",Language.ban_giao_so_hong,"#04A8F4"),
                new StatusCodeModel("2",Language.vo_hieu_luc,"#6897BB"),
            };
        }

        public static StatusCodeModel GetUnitHandoverById(string id)
        {
            return GetUnitHandoverStatus().SingleOrDefault(x => x.Id == id);
        }
    }
}
