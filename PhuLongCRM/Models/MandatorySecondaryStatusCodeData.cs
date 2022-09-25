using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class MandatorySecondaryStatusCodeData
    {
        public static List<StatusCodeModel> MandatorySecondaryStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0","","#FFFFFF"),
                new StatusCodeModel("1",Language.ms_active_sts,"#06CF79"),//Active Nháp ms_active_sts
                new StatusCodeModel("100000000",Language.ms_applying_sts,"#03ACF5"), //Applying Đang áp dụng ms_applying_sts
                new StatusCodeModel("100000001",Language.ms_cancel_sts,"#FA7901"), //Cancel Hủy ms_cancel_sts
                new StatusCodeModel("2",Language.ms_inactive_sts,"#FDC206")// Inactive Vô hiệu lực ms_inactive_sts
            };
        }

        public static StatusCodeModel GetMandatorySecondaryStatusById(string id)
        {
            return MandatorySecondaryStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
