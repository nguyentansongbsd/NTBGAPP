using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class StatusCodeActivity
    {
        public static StatusCodeModel GetStatusCodeById(string statusCodeId)
        {
            return StatusCodes().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> StatusCodes()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.activity_completed_sts,"#03ACF5"),
                new StatusCodeModel("0",Language.activity_open_sts,"#06CF79"),
                new StatusCodeModel("2",Language.activity_cancelled_sts,"#333333"),
                new StatusCodeModel("3",Language.activity_scheduled_sts,"#04A388"),
            };
        }
    }
}
