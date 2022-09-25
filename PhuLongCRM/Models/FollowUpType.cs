using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class FollowUpType
    {
        public static List<StatusCodeModel> FollowUpTypeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000002",Language.giao_dich_tt_du_dot_1,"#FDC206"),
                new StatusCodeModel("100000003",Language.giao_dich_da_ky_hdmb,"#06CF79"),
                new StatusCodeModel("100000004",Language.giao_dich_installments,"#03ACF5"),
                new StatusCodeModel("100000006",Language.giao_dich_da_thanh_ly,"#04A388"),
                new StatusCodeModel("100000001",Language.dat_coc_tt_du_tien_coc,"#9A40AB"),
                new StatusCodeModel("100000000",Language.dat_coc_sign_off_rf,"#FA7901"),
                new StatusCodeModel("100000005",Language.dat_coc_da_thanh_ly,"#808080"),
                new StatusCodeModel("100000007",Language.units,"#D42A16")
            };
        }

        public static StatusCodeModel GetFollowUpTypeById(string id)
        {
            return FollowUpTypeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
