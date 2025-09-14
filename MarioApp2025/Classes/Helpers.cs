using JRO; // Add this namespace to resolve the 'JRO' reference
using MarioApp2025.Classes.Ademico;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MarioApp2025
{
    public class MarHelpers
    {
        public static async Task<string> GetPublicPeppolRegistrationAsync(
            string query, bool asJson,
            CancellationToken cancellationToken = default)
        {
            var baseUrlJson = "https://directory.peppol.eu/search/1.0/json";
            var requestUri = baseUrlJson + "?q=" + Uri.EscapeDataString("iso6523-actorid-upis:" + query);

            var baseUrlXml = "https://directory.peppol.eu/search/1.0/xml";
            var requestUriXml = baseUrlXml + "?q=" + Uri.EscapeDataString("iso6523-actorid-upis:" + query);

            if (!asJson)
            {
                // Using XML version
                using (var httpClient = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUriXml))
                {
                    // Set Accept header
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
            }

            // Using JSON version
            using (var httpClient = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                // Set Accept header
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }
        public static void SetApiMode(string apiMode)
        {
            if (apiMode == "TESTMODE")
            {
                SharedGlobals.AdemicoApiUrl = MyApiSecrets.testBaseUrl;
                SharedGlobals.AdemicoAccessToken = MyApiSecrets.testAccessToken;
                SharedGlobals.AdemicoUsername = MyApiSecrets.testUsername;
                SharedGlobals.AdemicoPassword = MyApiSecrets.testPassword;
                SharedGlobals.ApiModus = "TESTMODE";
            }            
            else
            {
                SharedGlobals.AdemicoApiUrl = MyApiSecrets.prodBaseUrl;
                SharedGlobals.AdemicoAccessToken = MyApiSecrets.prodAccessToken;
                SharedGlobals.AdemicoUsername = MyApiSecrets.prodUsername;
                SharedGlobals.AdemicoPassword = MyApiSecrets.prodPassword;
                SharedGlobals.ApiModus = "PRODUCTION";
            }

        }

        public static void ResetCompanyGlobals()
        {
            SharedGlobals.ActiveCompany = "";
            SharedGlobals.SetMarntMdvLocation("");
            SharedGlobals.CompanyName = ""; // default values
            SharedGlobals.CompanyAddress = ""; // default values
            SharedGlobals.CompanyPostalCodeAndCity = ""; // default values
            SharedGlobals.CompanyPhoneNumber = ""; // default values
            SharedGlobals.CompanyKBONumber = ""; // default values
            SharedGlobals.CompanyVATNumber = ""; // default values
            SharedGlobals.CompanyIBANNumber = ""; // default values
            SharedGlobals.CompanyBICNumber = ""; // default values
            SharedGlobals.CompanyEmailAddress = ""; // default values
            SharedGlobals.CompanyContactPerson = ""; // default values
            SharedGlobals.CompanyContactEmailAddress = ""; // default values
        }

        public static void SetCompanyGlobals(string selectedCompany)
        {
            SharedGlobals.ActiveCompany = selectedCompany;
            // Use the provided method to set the value instead of direct assignment
            SharedGlobals.SetMarntMdvLocation("\\" + selectedCompany + "\\marnt.mdv");
            string jrFile = GetHighestCounterTable(SharedGlobals.MarntMdvLocation, "jr");

            string connStr = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;
            OleDbConnection conn = new OleDbConnection
            {
                ConnectionString = connStr
            };
            conn.Open();

            string sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's046'";
            OleDbCommand cmd = new OleDbCommand
            {
                CommandText = sqlQuery,
                Connection = conn
            };
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyName = reader["v217"].ToString();
                SharedGlobals.CompanyName = companyName;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's047'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyAddress = reader["v217"].ToString();
                SharedGlobals.CompanyAddress = companyAddress;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's048'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyPostalCodeAndCity = reader["v217"].ToString();
                SharedGlobals.CompanyPostalCodeAndCity = companyPostalCodeAndCity;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's049'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyPhoneNumber = reader["v217"].ToString();
                SharedGlobals.CompanyPhoneNumber = companyPhoneNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's292'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyKBONumber = reader["v217"].ToString();
                SharedGlobals.CompanyKBONumber = companyKBONumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's051'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyVATNumber = reader["v217"].ToString();
                SharedGlobals.CompanyVATNumber = companyVATNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's293'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyIBANNumber = reader["v217"].ToString();
                SharedGlobals.CompanyIBANNumber = companyIBANNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's294'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyBICNumber = reader["v217"].ToString();
                SharedGlobals.CompanyBICNumber = companyBICNumber;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's295'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyEmailAddress = reader["v217"].ToString();
                SharedGlobals.CompanyEmailAddress = companyEmailAddress;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's052'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyContactPerson = reader["v217"].ToString();
                SharedGlobals.CompanyContactPerson = companyContactPerson;
            }
            reader.Close();

            sqlQuery = "SELECT v217 FROM " + jrFile + " WHERE  v071 = 's050'";
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string companyContactEmailAddress = reader["v217"].ToString();
                SharedGlobals.CompanyContactEmailAddress = companyContactEmailAddress;
            }
            reader.Close();
            conn.Close();
        }

        private static string GetHighestCounterTable(string location, string filter)
        {
            // Change provider depending on Access version:
            // For .mdb (Jet)
            string foundTable = string.Empty;
            string connStr = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + location;

            OleDbConnection conn = new OleDbConnection(connStr);
            conn.Open();

            // Retrieve schema for tables only
            DataTable tables = conn.GetSchema("Tables");

            foreach (DataRow row in tables.Rows)
            {
                // Use null-coalescing operator to handle potential null values
                string tableName = row["TABLE_NAME"]?.ToString() ?? string.Empty;
                string tableType = row["TABLE_TYPE"]?.ToString() ?? string.Empty;

                // Filter out system/internal tables if needed
                if (tableType == "TABLE")
                {
                    if (tableName.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
                    {
                        foundTable = tableName;
                    }
                }
            }
            conn.Close();
            return foundTable;  // Return empty string if no matching table is found
        }
    }

    public class BackupHelper
    {
        public static string ZipFolderToCloudDrive(string sourceFolderPath, string cloudFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath))
                throw new DirectoryNotFoundException($"Source folder not found: {sourceFolderPath}");

            if (!Directory.Exists(cloudFolderPath))
                throw new DirectoryNotFoundException($"Cloud folder not found: {cloudFolderPath}");

            try
            {
                string zipFileName = $"{Path.GetFileName(sourceFolderPath)}_backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                string tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);
                string destZipPath = Path.Combine(cloudFolderPath, zipFileName);

                // Create zip in temp directory
                if (File.Exists(tempZipPath))
                    File.Delete(tempZipPath);

                ZipFile.CreateFromDirectory(sourceFolderPath, tempZipPath, CompressionLevel.Optimal, false);

                // Copy to OneDrive sync folder
                File.Copy(tempZipPath, destZipPath, true);

                return destZipPath;
            }
            catch (Exception)
            {
                return "Bedrijfsdata is nog in gebruik voor dit bedrijf. Eerst marIntegraal afsluiten a.u.b.";
            }
        }
    }

    public class DatabaseHelper
    {
        public static string CompactAccessDatabase(string databasePath)
        {
            if (!File.Exists(databasePath))
                return $"Database file not found: {databasePath}";
            string tempDatabasePath = Path.Combine(Path.GetDirectoryName(databasePath), "temp_compact.mdb");
            // Use JRO to compact the database
            var jro = new JetEngine(); // Ensure the JRO namespace is correctly referenced
            jro.CompactDatabase($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};", $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={tempDatabasePath};Jet OLEDB:Engine Type=5");
            // Replace original database with compacted one
            File.Delete(databasePath);
            File.Move(tempDatabasePath, databasePath);
            return "Database compacted successfully.";
        }
    }
}





