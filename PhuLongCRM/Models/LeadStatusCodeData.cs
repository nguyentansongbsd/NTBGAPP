using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class LeadStatusCodeData
    {
        public static List<StatusCodeModel> LeadStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0","","#FFFFFF"),
                new StatusCodeModel("1",Language.new_sts,"#2C95F6"),//New
                new StatusCodeModel("2",Language.da_lien_he_sts,"#F43927"), //Contacted
                new StatusCodeModel("3",Language.da_xac_nhan_sts,"#F6E22D"), //Qualified
                new StatusCodeModel("4",Language.mat_khach_hang,"#932CF6"), //Lost
                new StatusCodeModel("5",Language.khong_lien_lac_duoc,"#932CF6"),//Cannot Contact
                new StatusCodeModel("6",Language.khong_quan_tam,"#932CF6"), // No Longer Interested
                new StatusCodeModel("7",Language.da_huy,"#932CF6"),//Canceled
            };
        }

        public static StatusCodeModel GetLeadStatusCodeById(string id)
        {
            return LeadStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
