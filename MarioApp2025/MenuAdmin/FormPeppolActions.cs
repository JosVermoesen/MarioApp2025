using MarioApp2025.Classes.Ademico;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static MarioApp2025.AdemicoModels;

namespace MarioApp2025.MarioMenu.Admin
{
    public partial class FormPeppolActions : Form
    {
        private readonly HttpClient httpCheck;
        public Form FormDataGridJsonPopUp { get; set; }
        public FormPeppolActions()
        {
            InitializeComponent();
            Text = "Admin - Peppol Acties" + " [" + SharedGlobals.ApiModus + "]";
            httpCheck = new HttpClient();
            FormDataGridJsonPopUp = new FormDataGridJsonPopUp { };
            RadioButtonGetReceived.Checked = true;
            TextBoxLegalEntityId.Text = ""; // Default to empty to enable country/scheme/identifier fields

            // Ensure events are hooked up if you didn't use the Designer
            this.DragEnter += TabSendDocument_DragEnter;
            this.DragDrop += TabSendDocument_DragDrop;
            this.RadioButtonGetReceived.Checked = true;
        }

        private void TabSendDocument_DragEnter(object sender, DragEventArgs e)
        {
            // Only allow file drops
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void TabSendDocument_DragDrop(object sender, DragEventArgs e)
        {
            // Use pattern matching to simplify the type check and cast
            if (e.Data is DataObject dataObject && dataObject.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            {
                // Example 1: Open each file with its default program
                // foreach (var file in files)
                //     Process.Start(new ProcessStartInfo(file) { UseShellExecute = true });

                // Example 2: Load the first file’s text into a TextBox
                if (File.Exists(files[0]))
                {
                    // Assuming you have a TextBox named LabelFile and LabelInvoiceId, LabelIssueDate
                    LabelFile.Text = files[0];
                }
            }
        }

        private void LabelFile_TextChanged(object sender, EventArgs e)
        {
            ButtonCheckFile.Enabled = !string.IsNullOrWhiteSpace(LabelFile.Text) && File.Exists(LabelFile.Text);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        async private void ButtonCheckConnectivity_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            var response = await AdemicoClient.CheckConnection(new HttpClient());
            if (response != null)
            {
                ToolStripStatusLabel.Text = response;
            }
            else
            {
                MessageBox.Show(response, "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBoxLegalEntityId_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxLegalEntityId.Text.Length > 0)
            {
                // If a Legal Entity ID is choosen, set specific values for Belgium
                TextBoxCountryCode.Text = "";
                TextBoxRegScheme.Text = "";
                TextBoxRegIdentifier.Text = "";
                TextBoxSupportedDocument.Text = "";
                TextBoxLegalEntityId.Enabled = true;
                TextBoxRegIdentifier.Enabled = false;
                TextBoxCountryCode.Enabled = false;
                TextBoxRegScheme.Enabled = false;
                TextBoxSupportedDocument.Enabled = false;
            }
            else
            {
                // If no Legal Entity ID is provided, set default values for Belgium
                TextBoxCountryCode.Text = "BE"; // Default to Belgium if no Legal Entity ID is provided                                
                TextBoxRegScheme.Text = "0208"; // Default to 0208 scheme for Belgium  
                TextBoxRegIdentifier.Text = "0440058217"; // Default to a common identifier for Belgium
                TextBoxSupportedDocument.Text = "PEPPOL_BIS_BILLING_UBL_INVOICE_V3"; // Default to UBL Invoice for Belgium            
                TextBoxLegalEntityId.Text = ""; // Default Legal Entity ID for Belgium

                TextBoxRegIdentifier.Enabled = true; // Enable the registration identifier field
                TextBoxCountryCode.Enabled = true; // Enable the country code field 
                TextBoxRegScheme.Enabled = true; // Enable the registration scheme field
                TextBoxSupportedDocument.Enabled = true; // Enable the supported document field
            }
        }

        async private void ButtonGetPeppolRegistrations_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string country = TextBoxCountryCode.Text; // Example country code
            string peppolRegistrationScheme = TextBoxRegScheme.Text; // Example scheme code, e.g., "0208"
            string peppolRegistrationIdentifier = TextBoxRegIdentifier.Text; // Example identifier, e.g., "0529835180"
            string peppolSupportedDocument = TextBoxSupportedDocument.Text; // Example supported document, e.g., "PEPPOL_BIS_BILLING_UBL_INVOICE_V3"
            string legalEntityId = TextBoxLegalEntityId.Text; // Example legal entity ID, if needed

            try
            {
                var respons = await AdemicoClient.GetPeppolRegistrationAsync(
                    country,
                    peppolRegistrationScheme,
                    peppolRegistrationIdentifier,
                    peppolSupportedDocument,
                    legalEntityId);

                if (respons.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ToolStripStatusLabel.Text = "Registration(s) found (200).";
                }
                else if (respons.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ToolStripStatusLabel.Text = "Unauthorized (401) — check credentials.";
                }
                else if (respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ToolStripStatusLabel.Text = "Not found (404) — no matching registration.";
                }
                else
                {
                    ToolStripStatusLabel.Text = $"Status: {(int)respons.StatusCode} {respons.StatusCode}";
                }

                if (!string.IsNullOrEmpty(respons.ResponseBody))
                {
                    var deserializedString = JsonConvert.DeserializeObject(respons.ResponseBody);
                    RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    DoPopUpEntitiesData(RichTextBoxResult.Text); // Show the result in a popup with JSON table view
                }
                else
                {
                    ToolStripStatusLabel.Text = "No response body.";
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Request failed: {ex.Message}";
            }
        }
        private void DoPopUpEntitiesData(string messageAsJson)
        {
            FormDataGridJsonPopUp formJsonTable = new FormDataGridJsonPopUp
            {
                jsonData = messageAsJson, // Pass the JSON data to the popup form
                Dock = DockStyle.Fill // Fill the popup form with the JSON table view                
            };
            formJsonTable.LoadRegistrationJsonData();
            formJsonTable.ShowDialog(this); // Show the popup form as a dialog, centered on the main form
        }

        private void RadioButtonGetReceived_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonGetReceived.Checked)
            {
                // Enable the fields for received documents                
                TextBoxReceiver.Enabled = true;
                TextBoxReceiver.Text = "0208:0440058217"; // Default receiver for received documents
                TextBoxSender.Enabled = false;
                TextBoxSender.Text = "";
            }
            else
            {
                // Disable the fields for received documents
                TextBoxSender.Enabled = true;
                TextBoxSender.Text = "9925:BE0440058217"; // Default sender for sent documents
                TextBoxReceiver.Enabled = false;
                TextBoxReceiver.Text = "";
            }
        }

        async private void ButtonNotifications_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string eventType = RadioButtonGetReceived.Checked ? "DOCUMENT_RECEIVED" : "DOCUMENT_SENT";
            var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                transmissionId: "", // "f8a591c77b2211f0b1ed0af13d778bd4"
                documentId: "",
                eventType: eventType, // "DOCUMENT_RECEIVED" or "DOCUMENT_SENT"
                peppolDocumentType: "", // "INVOICE"
                sender: TextBoxSender.Text, // "9925:BE0440058217",
                receiver: TextBoxReceiver.Text, // "0208:0440058217",
                startDateTime: "", // "2023-07-25T11:03:26.688Z"
                endDateTime: "", // "2023-07-29T11:03:26.688Z"
                page: "",
                pageSize: ""
            );

            if (jsonResponse != null)
            {
                ToolStripStatusLabel.Text = "Notifications retrieved successfully.";
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                DoPopUpDataGridJsonData(RichTextBoxResult.Text); // Show the result in a popup with JSON table view
            }
            else
            {
                ToolStripStatusLabel.Text = "Failed to retrieve notifications.";
                RichTextBoxResult.Text = "";
            }
        }

        private void DoPopUpDataGridJsonData(string messageAsJson)
        {
            FormDataGridJsonPopUp.Controls.Clear();
            FormDataGridJsonPopUp formJsonTable = new FormDataGridJsonPopUp
            {
                jsonData = messageAsJson, // Pass the JSON data to the popup form
                Dock = DockStyle.Fill // Fill the popup form with the JSON table view                
            };
            formJsonTable.LoadNotificationJsonData();
            formJsonTable.ShowDialog(this); // Show the popup form as a dialog, centered on the main form
        }

        async private void ButtonLookUp_Click(object sender, EventArgs e)
        {
            if (TextBoxIdentifier.Text.Length != 12) // Check if the VAT number is exactly 12 characters long
            {
                ToolStripStatusLabel.Text = "Voer een geldig BTW-nummer in (12 tekens).";
                return;
            }
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string vatNumber = TextBoxIdentifier.Text;
            string countryCode = vatNumber.Substring(0, 2); // Replace range operator with Substring
            string vat = vatNumber.Substring(2); // Replace range operator with Substring

            string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + countryCode + "/vat/" + vat;

            try
            {
                HttpResponseMessage response = await httpCheck.GetAsync(url); // Use the HttpClient instance
                response.EnsureSuccessStatusCode(); // Throws if not successful
                string responseContent = await response.Content.ReadAsStringAsync();

                var deserializedString = JsonConvert.DeserializeObject(responseContent);
                RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);

                // Deserialize into VatResponse object
                VatResponse data = JsonConvert.DeserializeObject<VatResponse>(responseContent);

                // Access fields
                if (data != null)
                {
                    string address = data.Address;

                    if (data?.IsValid == true)
                    {
                        TextBoxCompanyName.Text = ToTitleCase(data.Name ?? string.Empty);
                        TextBoxGeographicalInformation.Text = address?.Replace("\n", " - ");

                        ToolStripStatusLabel.Text = "Onderneming met geldig EU BTW nummer.";
                        ButtonEntityNew.Enabled = true;
                    }
                    else
                    {
                        TextBoxCompanyName.Clear();
                        TextBoxGeographicalInformation.Clear();

                        ToolStripStatusLabel.Text = "Ongeldig EU BTW nummer";
                        ButtonEntityNew.Enabled = false;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ToolStripStatusLabel.Text = "Error: " + ex.Message;
            }
        }

        // Utility method to convert a string to title case
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // step 1: bring everything to lowercase
            var lower = input.ToLower(CultureInfo.CurrentCulture);

            // step 2: capitalize the first letter of each word
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(lower);
        }

        async private void ButtonEntityNew_Click(object sender, EventArgs e)
        {
            if (!RadioButtonBothSchemes.Checked && !RadioButtonOnlyScheme0208.Checked)
            {
                ToolStripStatusLabel.Text = "Kies een registratie schema.";
                MessageBox.Show("Selecteer de nodige registratie schemas.", "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string name = TextBoxCompanyName.Text;
            string countryCode = "BE"; // Belgium
            string geographicalInformation = TextBoxGeographicalInformation.Text;

            string websiteUrl = TextBoxWebsiteUrl.Text; // Optional
            string contactType = "public"; // or "technical"
            string contactName = TextBoxContactName.Text; // Example contact name
            string contactPhoneNumber = TextBoxPhone.Text; // Example phone number
            string contactEmail = TextBoxContactEmail.Text; // Example email
            string additionalInformation = TextBoxAdditionalInformation.Text; // Optional
            string identifier9925 = TextBoxIdentifier.Text; // Full VAT number, e.g., "BE0421132626"
            string identifier0208 = TextBoxIdentifier.Text.Substring(2); // Remove country code for identifier, e.g., "0421132626"

            if (RadioButtonBothSchemes.Checked)
            {
                // Build the request using your Postman example values
                var entity = new CreateLegalEntityModel
                {
                    LegalEntityDetails = new LegalEntityDetails
                    {
                        PublishInPeppolDirectory = CheckBoxPublishDetails.Checked,
                        Name = name,
                        CountryCode = countryCode,
                        GeographicalInformation = geographicalInformation,
                        WebsiteUrl = websiteUrl,
                        Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            ContactType = contactType,
                            Name = contactName,
                            PhoneNumber = contactPhoneNumber,
                            Email = contactEmail
                        }
                    },
                        AdditionalInformation = additionalInformation
                    },
                    PeppolRegistrations = new List<PeppolRegistration>
                {
                    //new PeppolRegistration
                    //{
                    //    PeppolIdentifier = new PeppolIdentifier
                    //    {
                    //        Scheme = "0208",
                    //            Identifier = identifier0208 // "0421132626"
                    //    },
                    //    SupportedDocuments = new List<string>
                    //    {
                    //        "UBL_BE_INVOICE_3_0",
                    //        "UBL_BE_CREDIT_NOTE_3_0",
                    //        "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                    //        "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                    //        "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                    //        "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                    //    },
                    //    PublishInSmp = true
                    //},
                    new PeppolRegistration
                    {
                        PeppolIdentifier = new PeppolIdentifier
                        {
                            Scheme = "9925",
                            Identifier = identifier9925  // "BE0421132626"
                        },
                        SupportedDocuments = new List<string>
                        {
                            "UBL_BE_INVOICE_3_0",
                            "UBL_BE_CREDIT_NOTE_3_0",
                            "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                            "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                            "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                            "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                        },
                        PublishInSmp = true
                    }
                }
                };

                try
                {
                    var result = await AdemicoClient.CreateOrRegisterLegalEntityAsync(request: entity);

                    ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";
                    MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    ToolStripStatusLabel.Text = "Error during entity creation.";
                }

                ButtonEntityNew.Enabled = false; // Disable the button after creating the entity
                TextBoxCompanyName.Clear();
                TextBoxGeographicalInformation.Clear();
            }
            else
            {
                // Build the request using your Postman example values
                var entity = new CreateLegalEntityModel
                {
                    LegalEntityDetails = new LegalEntityDetails
                    {
                        PublishInPeppolDirectory = true,
                        Name = name,
                        CountryCode = countryCode,
                        GeographicalInformation = geographicalInformation,
                        WebsiteUrl = websiteUrl,
                        Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            ContactType = contactType,
                            Name = contactName,
                            PhoneNumber = contactPhoneNumber,
                            Email = contactEmail
                        }
                    },
                        AdditionalInformation = additionalInformation
                    },
                    PeppolRegistrations = new List<PeppolRegistration>
                {
                    new PeppolRegistration
                    {
                        PeppolIdentifier = new PeppolIdentifier
                        {
                            Scheme = "0208",
                                Identifier = identifier0208 // "0421132626"
                        },
                        SupportedDocuments = new List<string>
                        {
                            "UBL_BE_INVOICE_3_0",
                            "UBL_BE_CREDIT_NOTE_3_0",
                            "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                            "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                            "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                            "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                        },
                        PublishInSmp = true
                    }
                }
                };
                try
                {
                    var result = await AdemicoClient.CreateOrRegisterLegalEntityAsync(request: entity);

                    ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";
                    MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    ToolStripStatusLabel.Text = "Error during entity creation.";
                }

                ButtonEntityNew.Enabled = false; // Disable the button after creating the entity
                TextBoxCompanyName.Clear();
                TextBoxGeographicalInformation.Clear();
            }
        }

        async private void ButtonGetUBLDocument_Click(object sender, EventArgs e)
        {
            string ademicoUrl = SharedGlobals.AdemicoApiUrl; 
            string accessToken = SharedGlobals.AdemicoAccessToken;
            string username = SharedGlobals.AdemicoUsername;
            string password = SharedGlobals.AdemicoPassword;

            string transmissionId = TextBoxTransmissionId.Text.Trim();

            string requestUrl = $"{ademicoUrl}/api/peppol/v1/invoices/{transmissionId}/ubl?accessToken={accessToken}";

            StatusStrip.Text = "Bezig...";
            try
            {
                using (var client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    // Accept XML
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    // Read the content
                    string invoiceXml = await response.Content.ReadAsStringAsync();

                    // Save to file
                    string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string outputFile = Path.Combine(myDocumentsFolderPath, "invoice.xml");

                    // Save to file in My Documents
                    using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                    {
                        await writer.WriteAsync(invoiceXml);
                    }

                    StatusStrip.Text = "UBL Invoice XML retrieved and saved successfully.";

                    MessageBox.Show(
                        "UBL Invoice XML retrieved and saved successfully.\n\n" +
                        $"Invoice saved to: {outputFile}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                StatusStrip.Text = "Error: " + ex.Message;
            }
        }

        private void ButtonCheckFile_Click(object sender, EventArgs e)
        {
            string filePath = LabelFile.Text.Trim();
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (extension != ".xml") // && extension != ".ubl")
            {
                // Show a message box if the file is not a valid UBL XML file
                ButtonSendUblDocument.Enabled = false; // Disable the button if the file is not valid
                return;
            }
            else
            {
                // HandleUblDocument(filePath);
                ReadUBLInvoice(filePath);


                // If the file is valid, enable the button
                ButtonSendUblDocument.Enabled = true;
                ToolStripStatusLabel.Text = "Bestand is een geldig UBL XML bestand.";
            }
        }

        private static void ReadUBLInvoice(string filePath)
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (Exception)
            {
                return;
            }

            var ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            var sb = new StringBuilder();

            // UBL Version
            var ublVersionNode = xmlDoc.SelectSingleNode("//cbc:UBLVersionID", ns);
            sb.AppendLine("UBL VersionID: " + (ublVersionNode?.InnerText ?? "not found"));

            // Document ID
            var invoiceIdNode = xmlDoc.SelectSingleNode("//cbc:ID", ns);
            sb.AppendLine("Document ID: " + (invoiceIdNode?.InnerText ?? "not found"));

            // IssueDate
            var issueDateNode = xmlDoc.SelectSingleNode("//cbc:IssueDate", ns);
            sb.AppendLine("IssueDate: " + (issueDateNode?.InnerText ?? "not found"));

            // DueDate
            var dueDateNode = xmlDoc.SelectSingleNode("//cbc:DueDate", ns);
            sb.AppendLine("DueDate: " + (dueDateNode?.InnerText ?? "not found"));

            // InvoiceTypeCode
            var invTypeNode = xmlDoc.SelectSingleNode("//cbc:InvoiceTypeCode", ns);
            if (invTypeNode != null)
            {
                var invoiceTypeCode = invTypeNode.InnerText;
                var listID = invTypeNode.Attributes?["listID"]?.Value ?? "";
                sb.AppendLine("invoiceTypeCode: " + invoiceTypeCode);
                sb.AppendLine("invoice listID: " + listID);
            }
            else
            {
                MessageBox.Show("InvoiceTypeCode element not found.");
            }

            // OrderReference
            var orderList = xmlDoc.SelectNodes("//cac:OrderReference", ns);
            if (orderList != null)
            {
                foreach (XmlNode ordNode in orderList)
                {
                    var orderId = ordNode.SelectSingleNode("cbc:ID", ns)?.InnerText ?? "Order ID: not available";
                    sb.AppendLine("Order ID: " + orderId);
                }
            }
            MessageBox.Show(sb.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Supplier info
            var supplierNode = xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party", ns);
            if (supplierNode != null)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Supplier info");
                msg.AppendLine("-------------");
                msg.AppendLine("endpointOndernemingsnummer " + NodeText(supplierNode, "cbc:EndpointID", ns));
                msg.AppendLine("supplierID: " + NodeText(supplierNode, "cac:PartyIdentification/cbc:ID", ns));
                msg.AppendLine("tradingName: " + NodeText(supplierNode, "cac:PartyName/cbc:Name", ns));
                msg.AppendLine("street: " + NodeText(supplierNode, "cac:PostalAddress/cbc:StreetName", ns));
                msg.AppendLine("city: " + NodeText(supplierNode, "cac:PostalAddress/cbc:CityName", ns));
                msg.AppendLine("postalZone: " + NodeText(supplierNode, "cac:PostalAddress/cbc:PostalZone", ns));
                msg.AppendLine("countryCode: " + NodeText(supplierNode, "cac:PostalAddress/cac:Country/cbc:IdentificationCode", ns));
                msg.AppendLine("vatNumber: " + NodeText(supplierNode, "cac:PartyTaxScheme/cbc:CompanyID", ns));
                MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No AccountingSupplierParty element found.");
            }

            // Customer info
            var custNode = xmlDoc.SelectSingleNode("//cac:AccountingCustomerParty", ns);
            if (custNode != null)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Customer info");
                msg.AppendLine("-------------");
                msg.AppendLine("custAssignedAccountID: " + NodeText(custNode, "cbc:CustomerAssignedAccountID", ns));
                msg.AppendLine("custEndpointID: " + NodeText(custNode, "cac:Party/cbc:EndpointID", ns));
                msg.AppendLine("custName: " + NodeText(custNode, "cac:Party/cac:PartyName/cbc:Name", ns));
                msg.AppendLine("custStreet: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:StreetName", ns));
                msg.AppendLine("custCity: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:CityName", ns));
                msg.AppendLine("custPostalZone: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:PostalZone", ns));
                msg.AppendLine("custCountryCode: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode", ns));
                msg.AppendLine("custTaxID: " + NodeText(custNode, "cac:Party/cac:PartyTaxScheme/cbc:CompanyID", ns));
                msg.AppendLine("custTaxScheme: " + NodeText(custNode, "cac:Party/cac:PartyTaxScheme/cac:TaxScheme/cbc:ID", ns));
                MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No AccountingCustomerParty element found.");
            }

            // PaymentMeans
            var pmNodes = xmlDoc.SelectNodes("//cac:PaymentMeans", ns);
            if (pmNodes != null && pmNodes.Count > 0)
            {
                var msg = new StringBuilder();
                msg.AppendLine("PaymentMeans");
                msg.AppendLine("------------");
                foreach (XmlNode pmNode in pmNodes)
                {
                    msg.AppendLine("PaymentMeansCode: " + NodeText(pmNode, "cbc:PaymentMeansCode", ns));
                    msg.AppendLine("PaymentID: " + NodeText(pmNode, "cbc:PaymentID", ns));
                    msg.AppendLine("Payee IBAN: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cbc:ID", ns));
                    msg.AppendLine("Account Name: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cbc:Name", ns));
                    msg.AppendLine("BIC/Branch ID: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cac:FinancialInstitutionBranch/cbc:ID", ns));
                    msg.AppendLine();
                    msg.AppendLine("Card account (if present)");
                    msg.AppendLine("Card Account ID: " + NodeText(pmNode, "cac:CardAccount/cbc:ID", ns));
                    msg.AppendLine("Card Account Name: " + NodeText(pmNode, "cac:CardAccount/cbc:Name", ns));
                    msg.AppendLine();
                    msg.AppendLine("Direct debit mandate (if present)");
                    msg.AppendLine("Mandate ID: " + NodeText(pmNode, "cac:PaymentMandate/cbc:ID", ns));
                    msg.AppendLine("Mandate Date: " + NodeText(pmNode, "cac:PaymentMandate/cbc:PaymentMandateDate", ns));
                }
                MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No PaymentMeans element found.");
            }

            // TaxTotal
            var msgTax = new StringBuilder();
            msgTax.AppendLine("TaxTotal");
            msgTax.AppendLine("--------");
            var taxAmountEl = xmlDoc.SelectSingleNode("//cac:TaxTotal/cbc:TaxAmount", ns);
            var currencyID = taxAmountEl?.Attributes?["currencyID"]?.Value ?? "";
            if (taxAmountEl != null && string.IsNullOrEmpty(currencyID))
            {
                MessageBox.Show("Attribute currencyID is missing on <cbc:TaxAmount>");
            }

            var taxTotals = xmlDoc.SelectNodes("//cac:TaxTotal", ns);
            if (taxTotals != null)
            {
                foreach (XmlNode taxTotalElem in taxTotals)
                {
                    var ttAmount = taxTotalElem.SelectSingleNode("cbc:TaxAmount", ns)?.InnerText ?? "";
                    msgTax.AppendLine($"TaxTotal: {ttAmount} {currencyID}");

                    var subtotals = taxTotalElem.SelectNodes("cac:TaxSubtotal", ns);
                    foreach (XmlNode subElem in subtotals)
                    {
                        msgTax.AppendLine();
                        msgTax.AppendLine("SubDetail");
                        msgTax.AppendLine("TaxableAmount: " + NodeText(subElem, "cbc:TaxableAmount", ns));
                        msgTax.AppendLine("TaxAmount: " + NodeText(subElem, "cbc:TaxAmount", ns));
                        msgTax.AppendLine("Percent: " + NodeText(subElem, "cac:TaxCategory/cbc:Percent", ns) + "%");
                        msgTax.AppendLine();
                    }
                }
            }

            // LegalMonetaryTotal
            var msgMoney = new StringBuilder();
            msgMoney.AppendLine("LegalMonetaryTotal");
            msgMoney.AppendLine("------------------");
            var moneyTotalEl = xmlDoc.SelectSingleNode("//cac:LegalMonetaryTotal", ns);
            if (moneyTotalEl != null)
            {
                msgMoney.AppendLine("LineExtensionAmount: " + NodeText(moneyTotalEl, "cbc:LineExtensionAmount", ns));
                msgMoney.AppendLine("TaxExclusiveAmount: " + NodeText(moneyTotalEl, "cbc:TaxExclusiveAmount", ns));
                msgMoney.AppendLine("TaxInclusiveAmount: " + NodeText(moneyTotalEl, "cbc:TaxInclusiveAmount", ns));
                msgMoney.AppendLine("PayableAmount: " + NodeText(moneyTotalEl, "cbc:PayableAmount", ns) + $" ({currencyID})");
                MessageBox.Show(msgMoney.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // InvoiceLines
            var msgLines = new StringBuilder();
            var invoiceLines = xmlDoc.SelectNodes("//cac:InvoiceLine", ns);
            if (invoiceLines != null)
            {
                foreach (XmlNode lineNode in invoiceLines)
                {
                    var desc = NodeText(lineNode, ".//cbc:Description", ns);
                    var qty = NodeText(lineNode, ".//cbc:InvoicedQuantity", ns);
                    var price = NodeText(lineNode, ".//cbc:PriceAmount", ns);
                    msgLines.AppendLine($"Item: {desc}, Quantity: {qty}, Price: {price}");
                }
            }
        }

        // Helper for safe node text extraction
        private static string NodeText(XmlNode parentNode, string xpath, XmlNamespaceManager ns)
        {
            var node = parentNode.SelectSingleNode(xpath, ns);
            return node?.InnerText.Trim() ?? "";
        }

        async private void ButtonSendUblDocument_Click(object sender, EventArgs e)
        {
            string filePath = LabelFile.Text.Trim();

            try
            {
                var result = await AdemicoClient.PeppolInvoiceSender.SendUblInvoiceAsync(
                    filePath: filePath,
                    xC5Reporting: false // or true if needed for Singapore reporting
                );

                ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";

                if (!string.IsNullOrEmpty(result.ResponseBody)) // Ensure ResponseBody is not null or empty
                {
                    var deserializedString = JsonConvert.DeserializeObject(value: result.ResponseBody);
                    RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    // Result is shown in the main form textbox now and can be copied from there
                    // Datagrid popup is not useful here
                    // DoPopUpDataGridJsonData(RichTextBoxResult.Text); // Show the result in a popup with JSON table view
                    MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ToolStripStatusLabel.Text = "No response body.";
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Error: {ex.Message}";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBoxPublishDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPublishDetails.Checked)
            {
                ToolStripStatusLabel.Text = "Bedrijfsgegevens worden gepubliceerd in de Peppol Directory.";
                MessageBox.Show("Indien aangevinkt, worden de bedrijfsgegevens gepubliceerd in de Peppol Directory.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ToolStripStatusLabel.Text = "Bedrijfsgegevens worden niet gepubliceerd in de Peppol Directory.";
            }
        }
    }
}


