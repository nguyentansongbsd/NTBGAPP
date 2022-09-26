using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AcceptanceStatus
    {
        public static List<StatusCodeModel> AcceptanceStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.acceptance_active_sts,"#06CF79"),
                new StatusCodeModel("100000002",Language.acceptance_cancelled_sts,"#333333"),
                new StatusCodeModel("100000003",Language.acceptance_closed_sts,"#E96F60"),
                new StatusCodeModel("100000001",Language.acceptance_confirmed_acceptance_sts,"#03ACF5"),
                new StatusCodeModel("100000000",Language.acceptance_confirmed_information_sts,"#F1C50D"),
                new StatusCodeModel("2",Language.acceptance_inactive_sts,"#808080"),
            };
        }

        public static StatusCodeModel GetAcceptanceStatusById(string id)
        {
            return AcceptanceStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
