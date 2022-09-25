using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class DirectionData
    {
        public static OptionSet GetDiretionById(string diretionId)
        {
            if(string.IsNullOrWhiteSpace(diretionId)) return null;
            var diretion = Directions().Single(x=>x.Val == diretionId);
            return diretion;
        }

        public static List<OptionSetFilter> Directions()
        {
            var directions = new List<OptionSetFilter>();
            directions.Add(new OptionSetFilter { Val = "100000000", Label= Language.huong_dong });
            directions.Add(new OptionSetFilter { Val = "100000001", Label = Language.huong_tay });
            directions.Add(new OptionSetFilter { Val = "100000002", Label = Language.huong_nam });
            directions.Add(new OptionSetFilter { Val = "100000003", Label = Language.huong_bac });
            directions.Add(new OptionSetFilter { Val = "100000004", Label = Language.huong_tay_bac });
            directions.Add(new OptionSetFilter { Val = "100000005", Label = Language.huong_dong_bac });
            directions.Add(new OptionSetFilter { Val = "100000006", Label = Language.huong_tay_nam });
            directions.Add(new OptionSetFilter { Val = "100000007", Label = Language.huong_dong_nam });
            return directions;
        }
    }
}
