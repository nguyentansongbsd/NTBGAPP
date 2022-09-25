using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class StatusCodeUnit
    {
        public static StatusCodeModel GetStatusCodeById(string statusCodeId)
        {
            return StatusCodes().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> StatusCodes()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0",Language.nhap,"#333333"),
                new StatusCodeModel("1",Language.chuan_bi,"#F1C50D"),
                new StatusCodeModel("100000000",Language.san_sang,"#2FCC71"),
                new StatusCodeModel("100000007",Language.dat_cho,"#00CED1"), //Booking
                new StatusCodeModel("100000004",Language.giu_cho,"#04A8F4"),
                new StatusCodeModel("100000006",Language.dat_coc,"#14A184"),
                new StatusCodeModel("100000005",Language.dong_y_chuyen_coc,"#8F44AD"),
                new StatusCodeModel("100000003",Language.da_du_tien_coc,"#e67e22"),
                new StatusCodeModel("100000010",Language.hoan_tat_dat_coc,"#808080"), //Option
                new StatusCodeModel("100000001",Language.thanh_toan_dot_1,"#808080"),
                new StatusCodeModel("100000009",Language.da_ky_ttdc_hddc,"#A0DB8E"), //Signed D.A
                new StatusCodeModel("100000008",Language.du_dieu_dien,"#6897BB"),  //Qualified
                new StatusCodeModel("100000002",Language.da_ban,"#BF3A2B"),
            };
        }
    }

    public class StatusCodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Background { get; set; }
        public StatusCodeModel(string id,string name,string background)
        {
            Id = id;
            Name = name;
            Background = background;
        }
    }
}
