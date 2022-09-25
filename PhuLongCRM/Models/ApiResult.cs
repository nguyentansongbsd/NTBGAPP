using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ApiResult
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResult(int errorCode, string mesage)
        {
            ErrorCode = errorCode;
            Message = mesage;
        }
        public ApiResult(object data)
        {
            Data = data;
            ErrorCode = 200;
        }
        public ApiResult()
        {
            this.ErrorCode = 200;
        }
    }
}
