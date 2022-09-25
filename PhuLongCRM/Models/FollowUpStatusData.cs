using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class FollowUpStatusData
    {
        public static List<StatusCodeModel> FollowUpStatusCodeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.ful_nhap,"#03ACF5"), // nháp Active
                new StatusCodeModel("100000000",Language.hoan_thanh,"#06CF79"), // Complete
                new StatusCodeModel("2",Language.vo_hieu_luc,"#FDC206"), // Inactive
                new StatusCodeModel("0","","#333333")
            };
        }

        public static StatusCodeModel GetFollowUpStatusCodeById(string id)
        {
            return FollowUpStatusCodeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
