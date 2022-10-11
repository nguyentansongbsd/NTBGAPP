using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Config
{
    public class OrgConfig
    {
        public const string VerApp = "PL.CRM - VerDemo - Ver 4.1.6";
        //public const string VerApp = "PL.CRM - Version 4.1";

        public const string ApiUrl = "https://phulongdev.api.crm5.dynamics.com/api/data/v9.1";
        public const string Resource = "https://phulongdev.crm5.dynamics.com/";
        public const string ClientId = "1b53e0dd-04fb-495c-a8d0-3f26ebb84468"; //
        public const string ClientSecret = "SII7Q~z5TqyjVoBBIKNuSxDJabQhuFE_~i5HI"; //

        // For login by user crm
        public const string TeantId = "87bbdb08-48ba-4dbf-9c53-92ceae16c353";
        public const string ClientId_ForUserCRM = "a7544a58-b7bb-4553-9548-d56d1cfbec55";
        public const string ClientSecret_ForUserCRM = "1kO7Q~FQ_o6uhrthjqlaUWiSY-bkpViYBDBPu";
        public const string Redirect_Uri = "https://crm.phulong.com/";
        public const string Scope = "offline_access https://phulongdev.crm5.dynamics.com/.default";

        //public const string TeantId = "1958ace9-e5ba-4d51-b458-cca319ff9b4f";
        //public const string ClientId_ForUserCRM = "1d2267b7-2d9d-4b75-a45d-7531fe7b9494";
        //public const string ClientSecret_ForUserCRM = "ZpH8Q~XRbEoDfJjPgGnBV3OwEVkiMBGNJJwhraRs";
        //public const string Redirect_Uri = "https://facebook.com";
        //public const string Scope = "https://org957ed874.crm5.dynamics.com/.default";

        //sharepoint
        public const string GraphApi = "https://graph.microsoft.com/v1.0/sites/";
        public const string GraphReSource = "https://graph.microsoft.com";
        public const string SP_SiteId = "245fb505-41a2-4630-923b-b233fdd09865";
        public const string SP_UnitID = "3c197b5b-a1b2-46a1-8d6c-7e45c2b53a13";
        public const string SP_UnitTypeID = "f351dad2-e44a-474a-84f5-12835186f2b5";
        public const string SP_ProjectID = "128241a3-19be-410e-afab-7cc233fba735";
        public const string SP_ContactID = "166b0309-79b1-414b-9613-ce3529e89642";

        public const string Graph_UnitTypeID = "b!BbVfJKJBMEaSO7Iz_dCYZaTJYdWNCktOh_yG1we5P9LS2lHzSuRKR4T1EoNRhvK1";
        public const string Graph_UnitID = "b!BbVfJKJBMEaSO7Iz_dCYZaTJYdWNCktOh_yG1we5P9Jbexk8sqGhRo1sfkXCtToT";
        public const string Graph_ProjectID = "b!BbVfJKJBMEaSO7Iz_dCYZaTJYdWNCktOh_yG1we5P9KjQYISvhkOQa-rfMIz-6c1";
        public const string Graph_ContactID = "b!BbVfJKJBMEaSO7Iz_dCYZaTJYdWNCktOh_yG1we5P9IJA2sWsXlLQZYTzjUp6JZC";
    }
}
