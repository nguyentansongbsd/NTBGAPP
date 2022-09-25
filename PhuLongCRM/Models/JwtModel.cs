﻿using System;
using System.Collections.Generic;

namespace PhuLongCRM.Models
{
	public class JwtModel
	{
        public string aud { get; set; }
        public string iss { get; set; }
        public int iat { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public string acr { get; set; }
        public string aio { get; set; }
        public List<string> amr { get; set; }
        public string appid { get; set; }
        public string appidacr { get; set; }
        public string ipaddr { get; set; }
        public string name { get; set; }
        public string oid { get; set; }
        public string onprem_sid { get; set; }
        public string puid { get; set; }
        public string rh { get; set; }
        public string scp { get; set; }
        public string sub { get; set; }
        public string tid { get; set; }
        public string unique_name { get; set; }
        public string upn { get; set; }
        public string uti { get; set; }
        public string ver { get; set; }
        public List<string> wids { get; set; }
    }
}

