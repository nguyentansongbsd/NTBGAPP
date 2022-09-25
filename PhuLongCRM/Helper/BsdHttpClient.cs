using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PhuLongCRM.Helper
{
    sealed class BsdHttpClient
    {
        private static HttpClient _httpClient = null;
        static internal HttpClient Instance()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }

            return _httpClient;
        }
    }
}
