using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static MarioApp2025.AdemicoModels;

namespace MarioApp2025
{
    public class AdemicoClient
    {
        static readonly string ademicoUrl = SharedGlobals.AdemicoApiUrl;
        static readonly string accessToken = SharedGlobals.AdemicoAccessToken;
        static readonly string username = SharedGlobals.AdemicoUsername;
        static readonly string password = SharedGlobals.AdemicoPassword;

        public static class PeppolInvoiceSender
        {
            public sealed class ApiResult
            {
                public HttpStatusCode StatusCode { get; }
                public string ResponseBody { get; }
                public ApiResult(HttpStatusCode statusCode, string responseBody)
                {
                    StatusCode = statusCode;
                    ResponseBody = responseBody;
                }
            }

            /// <summary>
            /// Sends a UBL invoice/credit note file to the Buyer via the Peppol API.
            /// Matches the Postman definition: multipart/form-data with Basic authentication.
            /// </summary>
            public static async Task<ApiResult> SendUblInvoiceAsync(
                string filePath,
                bool xC5Reporting = false,
                CancellationToken cancellationToken = default)
            {
                string requestUri = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/invoices/ubl-submissions"
                                    + "?accessToken=" + Uri.EscapeDataString(accessToken);

                using (var http = new HttpClient())
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    http.DefaultRequestHeaders.Accept.Clear();
                    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    http.DefaultRequestHeaders.Add("X-C5-REPORTING", xC5Reporting.ToString().ToLowerInvariant());

                    using (var form = new MultipartFormDataContent())
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                        form.Add(fileContent, "file", Path.GetFileName(filePath));

                        using (var response = await http.PostAsync(requestUri, form, cancellationToken).ConfigureAwait(false))
                        {
                            string body = response.Content == null
                                ? null
                                : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            return new ApiResult(response.StatusCode, body);
                        }
                    }
                }
            }
        }

        public static async Task<string> GetNotificationsAsync(
            string transmissionId, string documentId, string eventType, string peppolDocumentType, string sender,
            string receiver, string startDateTime, string endDateTime, string page, string pageSize,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(transmissionId)) queryParams.Add("transmissionId=" + Encode(transmissionId));
            if (!string.IsNullOrWhiteSpace(documentId)) queryParams.Add("documentId=" + Encode(documentId));
            if (!string.IsNullOrWhiteSpace(eventType)) queryParams.Add("eventType=" + Encode(eventType));
            if (!string.IsNullOrWhiteSpace(peppolDocumentType)) queryParams.Add("peppolDocumentType=" + Encode(peppolDocumentType));
            if (!string.IsNullOrWhiteSpace(sender)) queryParams.Add("sender=" + Encode(sender));
            if (!string.IsNullOrWhiteSpace(receiver)) queryParams.Add("receiver=" + Encode(receiver));
            if (!string.IsNullOrWhiteSpace(startDateTime)) queryParams.Add("startDateTime=" + Encode(startDateTime));
            if (!string.IsNullOrWhiteSpace(endDateTime)) queryParams.Add("endDateTime=" + Encode(endDateTime));
            if (!string.IsNullOrWhiteSpace(page)) queryParams.Add("page=" + Encode(page));
            if (!string.IsNullOrWhiteSpace(pageSize)) queryParams.Add("pageSize=" + Encode(pageSize));
            if (!string.IsNullOrWhiteSpace(accessToken)) queryParams.Add("accessToken=" + Encode(accessToken));

            var fullUrl = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/notifications";
            if (queryParams.Count > 0)
                fullUrl += "?" + string.Join("&", queryParams);

            using (var httpClient = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, fullUrl))
            {
                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }

        public sealed class LegalEntityResult
        {
            public HttpStatusCode StatusCode { get; }
            public string ResponseBody { get; }
            public LegalEntityResult(HttpStatusCode statusCode, string responseBody)
            {
                StatusCode = statusCode;
                ResponseBody = responseBody;
            }
        }

        public static async Task<LegalEntityResult> GetPeppolRegistrationAsync(
                string country,
                string peppolRegistrationScheme,
                string peppolRegistrationIdentifier,
                string peppolSupportedDocument,
                string legalEntityId,
                CancellationToken cancellationToken = default)
        {
            var baseUrl = ademicoUrl.EndsWith("/") ? ademicoUrl : ademicoUrl + "/";

            var query = new Dictionary<string, string>
            {
                ["country"] = country ?? throw new ArgumentNullException("country"),
                ["peppolRegistrationScheme"] = peppolRegistrationScheme ?? throw new ArgumentNullException("peppolRegistrationScheme"),
                ["peppolRegistrationIdentifier"] = peppolRegistrationIdentifier ?? throw new ArgumentNullException("peppolRegistrationIdentifier"),
                ["peppolSupportedDocument"] = peppolSupportedDocument ?? throw new ArgumentNullException("peppolSupportedDocument"),
                ["legalEntityId"] = legalEntityId ?? throw new ArgumentNullException("legalEntityId"),
                ["accessToken"] = accessToken
            };
            string queryString;
            using (var formContent = new FormUrlEncodedContent(query))
            {
                queryString = await formContent.ReadAsStringAsync().ConfigureAwait(false);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) })
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestUri = "api/peppol/v1/legal-entities?" + queryString;
                using (var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false))
                {
                    string body = response.Content == null
                        ? null
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return new LegalEntityResult(response.StatusCode, body);
                }
            }
        }

        public static async Task<string> CheckConnection(HttpClient client)
        {
            string requestUri = ademicoUrl.TrimEnd('/') + "/api/peppol/v1/tools/connectivity"
                                + "?accessToken=" + Uri.EscapeDataString(accessToken);

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return "Access granted ✅";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return "Unauthorized ❌ — check your username/password/accesstoken.";
                }
                else
                {
                    return "Unexpected status: " + ((int)response.StatusCode) + " " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public sealed class ApiResult
        {
            public HttpStatusCode StatusCode { get; }
            public string ResponseBody { get; }
            public ApiResult(HttpStatusCode statusCode, string responseBody)
            {
                StatusCode = statusCode;
                ResponseBody = responseBody;
            }
        }

        public static async Task<ApiResult> CreateOrRegisterLegalEntityAsync(
            CreateLegalEntityModel request,
            CancellationToken cancellationToken = default)
        {
            var baseUrl = ademicoUrl.TrimEnd('/');
            var requestUri = baseUrl + "/api/peppol/v1/legal-entities?accessToken=" + Uri.EscapeDataString(accessToken);

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };
            var jsonOptions = jsonSerializerOptions;

            using (var http = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonSerializer.Serialize(request, jsonOptions);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    using (var response = await http.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false))
                    {
                        string body = response.Content == null
                            ? null
                            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        return new ApiResult(response.StatusCode, body);
                    }
                }
            }
        }

        private static string Encode(string val)
        {
            return Uri.EscapeDataString(val ?? string.Empty);
        }
    }

    public class AdemicoModels
    {
        public class ViesApproximate
        {
            public string Name { get; set; }
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string CompanyType { get; set; }
            public int MatchName { get; set; }
            public int MatchStreet { get; set; }
            public int MatchPostalCode { get; set; }
            public int MatchCity { get; set; }
            public int MatchCompanyType { get; set; }
        }
        public class VatResponse
        {
            public bool IsValid { get; set; } = false;
            public DateTime RequestDate { get; set; }
            public string UserError { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string RequestIdentifier { get; set; }
            public string OriginalVatNumber { get; set; }
            public string VatNumber { get; set; }
            public ViesApproximate ViesApproximate { get; set; }
        }
        public class Contact
        {
            public string ContactType { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }
        public class LegalEntityDetails
        {
            public bool PublishInPeppolDirectory { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            public string GeographicalInformation { get; set; }
            public string WebsiteUrl { get; set; }
            public List<Contact> Contacts { get; set; }
            public string AdditionalInformation { get; set; }
        }
        public class PeppolIdentifier
        {
            public string Scheme { get; set; }
            public string Identifier { get; set; }
        }
        public class PeppolRegistration
        {
            public PeppolIdentifier PeppolIdentifier { get; set; }
            public List<string> SupportedDocuments { get; set; }
            public bool PublishInSmp { get; set; }
        }
        public class CreateLegalEntityModel
        {
            public LegalEntityDetails LegalEntityDetails { get; set; }
            public List<PeppolRegistration> PeppolRegistrations { get; set; }
        }
    }
}