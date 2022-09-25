using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class CaseSubjectModel
    {
        public string subjectid { get; set; }
        public string title_format { get { return subjectid != null ? CaseSubjectData.GetCaseSubjectById(subjectid).Label : null; } }
    }

    public class CaseSubjectData
    {
        public static List<OptionSet> CaseSubject()
        {
            return new List<OptionSet>()
            {
                new OptionSet("a34e0a7b-2711-ec11-b6e5-000d3ac7fb82",Language.case_products_sub),
                new OptionSet("994e0a7b-2711-ec11-b6e5-000d3ac7fb82",Language.case_query_sub),
                new OptionSet("9b4e0a7b-2711-ec11-b6e5-000d3ac7fb82",Language.case_service_sub),
                new OptionSet("9d4e0a7b-2711-ec11-b6e5-000d3ac7fb82",Language.case_delivery_sub),
                new OptionSet("9f4e0a7b-2711-ec11-b6e5-000d3ac7fb82",Language.case_maintenance_sub),
                new OptionSet("a14e0a7b-2711-ec11-b6e5-000d3ac7fb82",Language.case_information_sub),
                new OptionSet("e180559b-92a5-e911-a845-000d3a07f3dd",Language.case_default_subject_sub),
                new OptionSet("7a2b21d3-727e-e911-a83b-000d3a07fbb4",Language.case_default_subject_sub),
            };
        }
        public static OptionSet GetCaseSubjectById(string Id)
        {
            return CaseSubject().SingleOrDefault(x => x.Val == Id);
        }
    }
}
