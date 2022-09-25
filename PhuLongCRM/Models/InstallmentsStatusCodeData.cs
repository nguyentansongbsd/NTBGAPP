using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class InstallmentsStatusCodeData
    {
        public static List<StatusCodeModel> InstallmentsStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.ful_nhap,"#06CF79"), // nháp
                new StatusCodeModel("100000000",Language.chua_thanh_toan,"#03ACF5"),  // chưa thnah toán
                new StatusCodeModel("100000001",Language.da_thanh_toan,"#FDC206"),  // đã thah toán
                new StatusCodeModel("2",Language.vo_hieu_luc,"#FA7901"), //Inactive
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetInstallmentsStatusCodeById(string id)
        {
            return InstallmentsStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
