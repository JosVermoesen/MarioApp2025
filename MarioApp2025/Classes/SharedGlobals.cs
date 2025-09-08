using MarioApp2025.Classes.Ademico;
using System.Windows.Forms;

namespace MarioApp2025
{
    public static class SharedGlobals
    {
        static SharedGlobals()
        {
            DbJetProvider = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=";            
            UserGuid = "";

            MimDataLocation = "";
            MarntMdvLocation = "";
            IsAdmin = false; // default to false

            ActiveCompany = "";
            CompanyName = ""; // value s046 in marnt.mdv file is Company Name
            CompanyAddress = ""; // value s047 in marnt.mdv file is Company Address
            CompanyPostalCodeAndCity = ""; // value s048 in marnt.mdv file is Company Postal Code and City
            CompanyPhoneNumber = ""; // value s049 in marnt.mdv file is Company Phone Number
            CompanyKBONumber = ""; // value s292 in marnt.mdv file is Company KBO Number
            CompanyVATNumber = ""; // value s051 in marnt.mdv file is Company VAT Number
            CompanyIBANNumber = ""; // value s293 in marnt.mdv file is Company IBAN Number
            CompanyBICNumber = ""; // value s294 in marnt.mdv file is Company BIC Number
            CompanyEmailAddress = ""; // value s295 in marnt.mdv file is Company Email Address
            CompanyContactPerson = ""; // value s052 in marnt.mdv file is Company Contact Person
            CompanyContactEmailAddress = ""; // value s050 in marnt.mdv file is Company Contact Email Address

            MarntCloudLocation = ""; // default values
            MarntCLoudArchiveLocation = ""; // default values
            MarntCloudMarioLocation = ""; // default values

            PeppolOutFiles = 0;

            ApiModus = "PRODUCTION";
            AdemicoApiUrl = "";
            AdemicoAccessToken = "";
            AdemicoUsername = "";
            AdemicoPassword = "";
        } // default values
        public static string ApiModus { get; set; } = "";
        public static string AdemicoApiUrl { get; set; } = "";
        public static string AdemicoAccessToken { get; set; } = "";
        public static string AdemicoUsername { get; set; } = "";
        public static string AdemicoPassword { get; set; } = "";

        public static string MarntCloudMarioLocation { get; set; } = ""; // default values
        public static string MarntCLoudArchiveLocation { get; set; } = ""; // default values
        public static string MarntCloudLocation { get; set; } = ""; // default values
        public static int PeppolOutFiles { get; set; } = 0; // default values
        public static string CompanyName { get; set; } = ""; // default values
        public static string CompanyAddress { get; set; } = ""; // default values
        public static string CompanyPostalCodeAndCity { get; set; } = ""; // default values
        public static string CompanyPhoneNumber { get; set; } = ""; // default values
        public static string CompanyKBONumber { get; set; } = ""; // default values
        public static string CompanyVATNumber { get; set; } = ""; // default values
        public static string CompanyIBANNumber { get; set; } = ""; // default values
        public static string CompanyBICNumber { get; set; } = ""; // default values
        public static string CompanyEmailAddress { get; set; } = ""; // default values
        public static string CompanyContactPerson { get; set; } = ""; // default values
        public static string CompanyContactEmailAddress { get; set; } = ""; // default values

        public static bool IsAdmin { get; set; } = true; // default to true                
        public static string UserGuid { get; set; }
        public static string DbJetProvider { get; }
        public static string MimDataLocation { get; private set; }
        public static string ActiveCompany { get; set; } = ""; // default values
        public static void SetMimDataLocation(string newString)
        {
            MimDataLocation = newString;
        }
        public static string MarntMdvLocation { get; private set; }
        public static void SetMarntMdvLocation(string newString)
        {
            MarntMdvLocation = newString;
        }
    }
}
