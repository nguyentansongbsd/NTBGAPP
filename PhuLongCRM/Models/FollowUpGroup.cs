using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class FollowUpGroup
    {
        public static List<StatusCodeModel> FollowUpGroupData()
        {
            return new List<StatusCodeModel>()
            {   
                new StatusCodeModel("100000001",Language.ful_ccr_group,"#FDC206" ),//ful_ccr_group
                new StatusCodeModel("100000002",Language.ful_fin_group,"#06CF79"),//ful_fin_group
                new StatusCodeModel("100000000",Language.ful_sam_group,"#06CF79"),//ful_sam_group
            };
        }

        public static StatusCodeModel GetFollowUpGroupById(string id)
        {
            return FollowUpGroupData().SingleOrDefault(x => x.Id == id);
        }
    }
}
