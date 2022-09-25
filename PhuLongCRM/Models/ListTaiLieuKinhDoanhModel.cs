using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ListTaiLieuKinhDoanhModel 
    {
        public Guid salesliteratureid { get; set; }
        public string name { get; set; }
        public string subjecttitle { get; set; }
        public int literaturetypecode { get; set; }
        public string literaturetypecode_format
        {
            get
            {
                switch (literaturetypecode)
                {
                    case 0:
                        return Language.document_presentation_type;// "Presentation"; //document_presentation_type
                    case 1:
                        return Language.document_product_sheet_type;//"Product Sheet"; //document_product_sheet_type
                    case 2:
                        return Language.document_policies_and_procedures_type;//"Policies And Procedures"; //document_policies_and_procedures_type
                    case 3:
                        return Language.document_sales_literature_type;//"Sales Literature"; //document_sales_literature_type
                    case 4:
                        return Language.document_spreadsheets_type;//"Spreadsheets"; //document_spreadsheets_type
                    case 5:
                        return Language.document_news_type;//"News"; //document_news_type
                    case 6:
                        return Language.document_bulletins_type;//"Bulletins"; //document_bulletins_type
                    case 7:
                        return Language.document_price_sheets_type;//"Price Sheets"; //document_price_sheets_type
                    case 8:
                        return Language.document_manuals_type;//"Manuals"; //document_manuals_type
                    case 9:
                        return Language.document_company_background_type;//"Company Background"; //document_company_background_type
                    case 100001:
                        return Language.document_marketing_collateral_type;//"Marketing Collateral"; //document_marketing_collateral_type
                    default:
                        return " ";
                }
            }
        }
        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get
            {
                return this.createdon.ToString("dd/MM/yyyy");
            }
        }
    }
}

