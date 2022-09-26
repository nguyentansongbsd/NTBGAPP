using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class UnitSpecificationStatus
    {
        public static List<StatusCodeModel> UnitSpecificationStatusData()
        {
            return new List<StatusCodeModel>()
            {
                // chưa tạo ngôn ngữ, đang sử dụng từ model khác
                new StatusCodeModel("1",Language.ms_active_sts,"#06CF79"), // Active
                new StatusCodeModel("2",Language.ms_inactive_sts,"#808080"), // Inactive
            };
        }

        public static StatusCodeModel GetUnitSpecStatusById(string id)
        {
            return UnitSpecificationStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
