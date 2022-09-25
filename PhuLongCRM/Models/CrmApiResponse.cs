using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class CrmApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Content { get; set; }
        public ErrorResponse ErrorResponse { get; set; }

        public string GetErrorMessage()
        {
            return ErrorResponse?.error?.message?.ToString() ?? Language.da_co_loi_xay_ra_vui_long_thu_lai_sau;
        }
    }
}
