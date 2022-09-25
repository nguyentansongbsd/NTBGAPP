using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class ContactTypeData
    {
        public static List<OptionSet> ContactTypes() {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.khach_hang), // khach hanfg Customer
                new OptionSet("100000001",Language.cong_tac_vien), // cong tac vien Collaborator
                new OptionSet("100000002",Language.nguoi_uy_quyen), // người ủy quyền Authorized
                new OptionSet("100000003",Language.nguoi_dai_dien_phap_ly), //Legal Representative
            };
        }

        public static OptionSet GetContactTypeById(string Id)
        {
            return ContactTypes().SingleOrDefault(x => x.Val == Id);
        }
    }
}
