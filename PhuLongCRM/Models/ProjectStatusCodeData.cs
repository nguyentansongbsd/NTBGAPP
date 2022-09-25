using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class ProjectStatusCodeData
    {
        public static List<StatusCodeModel> ProjectStatusCodeDatas()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.project_active_sts,"#14A184"),//Active project_active_sts
                new StatusCodeModel("861450002",Language.project_publish_sts,"#2FCC71"),//Publish project_publish_sts
                new StatusCodeModel("861450001",Language.project_unpublish_sts,"#808080"),//Unpublish project_unpublish_sts
                new StatusCodeModel("2",Language.project_inactive_sts,"#D90825")//Inactive project_inactive_sts
            };
        }
        public static StatusCodeModel GetProjectStatusCodeById(string id)
        {
            return ProjectStatusCodeDatas().Single(x => x.Id == id);
        }
    }
}
