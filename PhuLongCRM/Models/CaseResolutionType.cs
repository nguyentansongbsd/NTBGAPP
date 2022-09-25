using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class CaseResolutionType
    {
        public static List<OptionSet> CaseResolutionTypeData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("5", Language.case_problem_solved_sts), //case_problem_solved_sts Problem Solved Vấn đề đã được giải quyết
                new OptionSet("1000", Language.case_information_provided_sts), //case_information_provided_sts Information Provided Cung cấp thông tin
            };
        }
    }
}
