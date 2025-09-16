using ADODB;
using MarioApp2025.Classes.Ademico;
using MarioApp2025.MarioMenu.Admin;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Windows.Forms.Timer;


namespace MarioApp2025.MarioMenu.Actions
{
    public partial class FormPeppolClientActions : Form
    {
        private readonly Timer _timer;

        // Change the declaration of the `httpCheck` field to remove the `readonly` modifier
        private HttpClient httpCheck;

        public string activeSellerDocument = "";

        public Form FormDataGridJsonPopUp { get; set; }

        // Testing ADODB connection to marnt.mdv file       
        public Recordset DocumentRS { get; private set; }

        public int totalInMapOut = 0;
        public int totalOutToRemoveFromNotifications = 0;

        public int totalReceivedInMap = 0;
        public int totalReceivedToRemoveFromNotifications = 0;

        // Update the constructor to initialize the `httpCheck` field
        public FormPeppolClientActions()
        {
            InitializeComponent();
            _timer = new Timer
            {
                Interval = 5 * 60 * 1000 // 5 minutes in milliseconds
            };
            _timer.Tick += Timer_Tick;
            _timer.Stop();

            Text = "Peppol Verrichtingen [ " + SharedGlobals.CompanyName + "]";
            httpCheck = new HttpClient(); // Initialize here
            FormDataGridJsonPopUp = new FormDataGridJsonPopUp { };
            RadioButtonGetReceived.Checked = true;
            TextBoxLegalEntityId.Text = ""; // Default to empty to enable country/scheme/identifier fields

            RefreshMonitor();
        }

        private void FormPeppolClientActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop(); // Stop the timer when the form is closing
            _timer.Dispose(); // Dispose of the timer to free resources
        }

        // Monitor Tab - Fill the listbox with Peppol files to be sent
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

        private void ButtonTimer_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                ButtonTimer.Text = "Start Automatisch Vernieuwen";
                ToolStripStatusLabel.Text = "Timer is gestopt.";
                ButtonRefreshAll.Enabled = true;
                return;
            }
            else
            {
                ButtonTimer.Text = "Stop Automatisch Vernieuwen";
                Timer timer = new Timer
                {
                    Interval = 5 * 60 * 1000 // 5 minutes in milliseconds
                };
                timer.Tick += Timer_Tick;
                ToolStripStatusLabel.Text = "Timer is gestart. Automatisch bijwerken om de 5 minuten.";
                ButtonRefreshAll.Enabled = false;
                _timer.Start();
            }

        }

        // Actions Tab        
        private void ButtonShowSharedGlobals_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Naam Bedrijf (KBO): {SharedGlobals.CompanyName}\n" +
                $"Adres: {SharedGlobals.CompanyAddress}\n" +
                $"Postcode en Plaats: {SharedGlobals.CompanyPostalCodeAndCity}\n" +
                $"Telefoon: {SharedGlobals.CompanyPhoneNumber}\n" +
                $"KBO Nummer: {SharedGlobals.CompanyKBONumber}\n" +
                $"BTW Nummer (zonder 'BE'): {SharedGlobals.CompanyVATNumber}\n" +
                $"IBAN Rekening Nummer: {SharedGlobals.CompanyIBANNumber}\n" +
                $"BIC Nummer: {SharedGlobals.CompanyBICNumber}\n" +
                $"Email Adres Bedrijf: {SharedGlobals.CompanyEmailAddress}\n" +
                $"Contactpersoon: {SharedGlobals.CompanyContactPerson}\n" +
                $"Email Adres Contactpersoon: {SharedGlobals.CompanyContactEmailAddress}\n\n" +
                $"Peppol Documenten in map OUT: {SharedGlobals.PeppolOutFiles}\n\n" +
                $"Mapnummer Actief Bedrijf: {SharedGlobals.ActiveCompany}\n" +
                $"Inhoudsopgave Mar Mdv Bestand: {SharedGlobals.MarntMdvLocation}\n" +
                $"Inhoudsopgave Mar Data: {SharedGlobals.MimDataLocation}\n",
                "Variabele gegevens van actief bedrijf",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ButtonMarVariables_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Inhoudsopgave Mar Data: {SharedGlobals.MimDataLocation}\n" +
                $"Inhoudsopgave Marnt Cloud: {SharedGlobals.MarntCloudLocation}\n" +
                $"Inhoudsopgave Archief Cloud: {SharedGlobals.MarntCLoudArchiveLocation}\n" +
                $"Inhoudsopgave Manueel Cloud: {SharedGlobals.MarntCloudMarioLocation}\n",
                "Variabele gegevens van MarIntegraal op dit toestel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
                    RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    DoPopUpEntitiesData(RichTextBoxResponses.Text); // Show the result in a popup with JSON table view
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
                TextBoxRegIdentifier.Text = SharedGlobals.CompanyKBONumber; // Default to a common identifier for Belgium
                TextBoxSupportedDocument.Text = "UBL_BE_INVOICE_3_0"; // Default to UBL Invoice for Belgium            
                TextBoxLegalEntityId.Text = ""; // Default Legal Entity ID for Belgium

                TextBoxRegIdentifier.Enabled = true; // Enable the registration identifier field
                TextBoxCountryCode.Enabled = true; // Enable the country code field 
                TextBoxRegScheme.Enabled = true; // Enable the registration scheme field
                TextBoxSupportedDocument.Enabled = true; // Enable the supported document field
            }
        }

        // Notifications Tab
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
                RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                DoPopUpDataGridJsonData(RichTextBoxResponses.Text); // Show the result in a popup with JSON table view
            }
            else
            {
                ToolStripStatusLabel.Text = "Failed to retrieve notifications.";
                RichTextBoxResponses.Text = "";
            }

        }

        private void RadioButtonGetReceived_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonGetReceived.Checked)
            {
                // Enable the fields for received documents                
                TextBoxReceiver.Enabled = true;
                TextBoxReceiver.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default receiver for received documents
                TextBoxSender.Enabled = false;
                TextBoxSender.Text = "";
            }
        }

        private void RadioButtonGetSent_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonGetSent.Checked)
            {
                // Enable the fields for sent documents                
                TextBoxSender.Enabled = true;
                TextBoxSender.Text = "9925:BE" + SharedGlobals.CompanyKBONumber; // Default sender for sent documents
                TextBoxReceiver.Enabled = false;
                TextBoxReceiver.Text = "";
            }
        }

        // Responses Tab

        // Send and receive UBL Document Tab        
        private void FillListPeppolToReceive(ListBox listboxIn)
        {
            string folderInPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in";
            listboxIn.Items.Clear();
            // MessageBox.Show(folderInPath);

            // Check the API notifications for received documents for this company and fill the listbox


        }

        private void FillListPeppolToSend(ListBox listboxOut, bool isMonitorListBox)
        {
            string folderOutPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\out";
            listboxOut.Items.Clear();

            if (Directory.Exists(folderOutPath))
            {
                string[] xmlFiles = Directory.GetFiles(folderOutPath, "*.xml");
                listboxOut.Items.Clear();
                listboxOut.Items.AddRange(xmlFiles);
                LabelFile.Text = "";
            }
            else
            {
                ToolStripStatusLabel.Text = "Geen te verzenden documenten voor " + SharedGlobals.ActiveCompany;
                listboxOut.Visible = false;
            }

            if (isMonitorListBox)
            {
                totalInMapOut = listboxOut.Items.Count;

                // Refresh the real number of total files to be sent
                int localTotalOutToRemove = 0;
                foreach (var item in listboxOut.Items)
                {
                    string documentId = ReadUBLInvoice(item.ToString(), false, false).ToUpper();
                    string existingResult = GetSellersDocumentResultRS(documentId);
                    if (existingResult != "")
                    {
                        localTotalOutToRemove++;
                    }
                }
                totalOutToRemoveFromNotifications = localTotalOutToRemove;
            }
        }

        async private void ListBoxDocumentsToSend_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonSendUblDocument.Enabled = false; // Disable the button when selecting a new file
            LabelFile.Text = "";

            if (ListBoxDocumentsPeppolOut.SelectedItem != null)
            {
                LabelFile.Text = ListBoxDocumentsPeppolOut.SelectedItem.ToString().ToUpper();
                string checkResult = ReadUBLInvoice(LabelFile.Text, false, false);
                string existingResult = GetSellersDocumentResultRS(checkResult.ToUpper());
                string notificationResult = "";

                if (existingResult == "")
                {
                    ToolStripStatusLabel.Text = checkResult + " verkoopdocument nog te verzenden.";
                    ButtonSendUblDocument.Enabled = true; // Enable the button if the file is valid and not yet sent                    
                    Application.DoEvents();
                }
                else
                {
                    // Check V405 field in marnt.mdv table for this document
                    // Check if the document result state in marnt.mdv table conforms that it was really sent
                    // If already sent but stated as error, refresh the database if needed
                    // With error, show the error message from the response body in the database
                    
                    notificationResult = await InvoiceNotificationState(checkResult); // Wait for the async task to complete

                    ButtonSendUblDocument.Enabled = false; // Disable the button if the file was already sent                    
                    RichTextBoxResponses.Text = existingResult;
                    Application.DoEvents();
                    // MessageBox.Show("Verzendbewijs aanwezig in boekhouding:\n\n" + existingResult, checkResult + " reeds verzonden");
                    ToolStripStatusLabel.Text = checkResult + " verkoopdocument is reeds verzonden.";
                }
            }
        }
                
        async private Task<string> InvoiceNotificationState(string documentId)
        {            
            // Check later the notification state of the invoice with the given document ID
            // This is a placeholder implementation; replace with actual logic as needed
            // Possible states: PENDING, SENT, FAILED, etc.

            var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                transmissionId: "", // "f8a591c77b2211f0b1ed0af13d778bd4"
                documentId: documentId,
                eventType: "", // "DOCUMENT_RECEIVED" or "DOCUMENT_SENT"
                peppolDocumentType: "", // "INVOICE"
                sender: "0208:" + SharedGlobals.CompanyKBONumber, // "9925:BE0440058217",
                receiver: "", // "0208:0440058217",
                startDateTime: "", // "2023-07-25T11:03:26.688Z"
                endDateTime: "", // "2023-07-29T11:03:26.688Z"
                page: "",
                pageSize: ""
            );

            if (jsonResponse != null)
            {
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                return JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                return "Failed to retrieve notification";
            }
        }

        private void ButtonCheckFile_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string filePath = LabelFile.Text.Trim().ToLower();
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            if (extension != ".xml") // && extension != ".ubl")
            {
                // Show a message box if the file is not a valid UBL XML file
                ToolStripStatusLabel.Text = "Dit is geen geldig UBL XML bestand";
                ButtonSendUblDocument.Enabled = false; // Disable the button if the file is not valid
                return;
            }
            else
            {
                // HandleUblDocument(filePath);
                activeSellerDocument = ReadUBLInvoice(filePath, false, true).ToLower();
                if (activeSellerDocument.Contains("error loading xml") || activeSellerDocument.Contains("not found"))
                {
                    ToolStripStatusLabel.Text = "Dit is geen geldig UBL XML bestand";
                    ButtonSendUblDocument.Enabled = false; // Disable the button if the file is not valid                 
                }
                else
                {
                    ToolStripStatusLabel.Text = "UBL XML bestand is geldig";
                    ButtonSendUblDocument.Enabled = true; // Enable the button if the file is valid                 
                    MessageBox.Show("Document ID: " + activeSellerDocument + " is geldig en klaar om verzonden te worden!", "UBL Document Gegevens", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        async private void ButtonSendUblDocument_Click(object sender, EventArgs e)
        {
            string confirmMessage = $"Weet u zeker dat u het UBL document {Path.GetFileName(LabelFile.Text)} wilt verzenden?";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig Verzending", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                ToolStripStatusLabel.Text = "Verzending geannuleerd door gebruiker.";
                return; // User chose not to proceed
            }

            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string filePath = LabelFile.Text.Trim();
            activeSellerDocument = ReadUBLInvoice(filePath, false, false).ToUpper();

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
                    RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    bool updatedRS = SetSellersDocumentResultRS(activeSellerDocument, RichTextBoxResponses.Text);

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

        // Receive UBL Document Tab
        async private void ButtonGetUBLDocument_Click(object sender, EventArgs e)
        {
            if (TextBoxTransmissionId.Text.Length == 0)
            {
                MessageBox.Show("Gelieve een Transmission ID in te vullen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Ready";
                return;
            }

            string confirmMessage = $"Weet u zeker dat u het UBL document met Transmission ID {TextBoxTransmissionId.Text} wilt ophalen?";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig Ophalen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                ToolStripStatusLabel.Text = "Ophalen geannuleerd door gebruiker.";
                return; // User chose not to proceed
            }

            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string ademicoUrl = MyApiSecrets.testBaseUrl;
            string accessToken = MyApiSecrets.testAccessToken;
            string username = MyApiSecrets.testUsername;
            string password = MyApiSecrets.testPassword;

            if (TextBoxTransmissionId.Text.Length == 0)
            {
                MessageBox.Show("Gelieve een Transmission ID in te vullen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Ready";
                return;
            }
            string transmissionId = TextBoxTransmissionId.Text.Trim();
            string requestUrl = $"{ademicoUrl}/api/peppol/v1/invoices/{transmissionId}/ubl?accessToken={accessToken}";

            try
            {
                HttpClient client = new HttpClient();
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
                // Combine folder path with the filename you want
                string outputFile = Path.Combine(myDocumentsFolderPath, "invoice.xml");

                // Save to file in My Documents
                await Task.Run(() => File.WriteAllText(outputFile, invoiceXml));
                Application.DoEvents();

                string documentId = ReadUBLInvoice(outputFile, false, true).ToUpper();
                string destinationPath = MoveXmlDocumentToMarPeppolIn(documentId);
                ToolStripStatusLabel.Text = "UBL Invoice " + documentId + ".XML retrieved and saved successfully.";
                Application.DoEvents();


                MessageBox.Show(
                    "UBL Invoice XML retrieved and saved successfully.\n\n" +
                    $"Invoice saved to: {documentId}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ToolStripStatusLabel.Text = "Ready";
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        // Common functions for multiple tabs
        private void RefreshMonitor()
        {
            totalInMapOut = 0;
            totalOutToRemoveFromNotifications = 0;
            totalReceivedInMap = 0;
            totalReceivedToRemoveFromNotifications = 0;

            FillListPeppolToSend(ListBoxDocumentsPeppolOut, false);
            FillListPeppolToSend(ListBoxMonitorPeppolOut, true);
            LabelTotalnMapOut.Text = totalInMapOut.ToString();
            LabelTotalOutToRemoveFromNotifications.Text = totalOutToRemoveFromNotifications.ToString();

            FillListPeppolToReceive(ListBoxMonitorForPeppolIn);

            ToolStripStatusLabel.Text = "Ready";
            Application.DoEvents();
        }

        private string MoveXmlDocumentToMarPeppolIn(string documentId)
        {
            // 1️⃣ Locate the original file in My Documents
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string originalFile = Path.Combine(documentPath, "invoice.xml");

            // 2️⃣ Create a temporary renamed file path in the same folder
            string renamedFile = Path.Combine(documentPath, documentId + ".xml");

            // 3️⃣ Rename (move) the file
            if (File.Exists(originalFile))
            {
                File.Move(originalFile, renamedFile);
                // Console.WriteLine($"Renamed to: {renamedFile}");
            }
            else
            {
                Console.WriteLine("Original file not found.");
                return "";
            }

            // 4️⃣ Copy the renamed file to another location
            string destinationFolder = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in";
            // Directory.CreateDirectory(destinationFolder);    // ensure it exists
            string destinationFile = Path.Combine(destinationFolder, documentId + ".xml");

            File.Copy(renamedFile, destinationFile, overwrite: true);
            Console.WriteLine($"Copied to: {destinationFile}");
            // TODO: Remove the temporary renamed file if needed
            try
            {
                if (File.Exists(renamedFile))
                {
                    File.Delete(renamedFile);
                    // Console.WriteLine($"Deleted temporary file: {renamedFile}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting temporary file: {ex.Message}");
            }

            return destinationFile;
        }

        public string GetSellersDocumentResultRS(string document)
        {
            string sSQL = "SELECT  * FROM Dokumenten WHERE  v033 = '" + document.Substring(0, 11) + "'";  // 'Journalen.v066";
            // Open the connection and execute the insert command.
            // The connection is automatically closed when the
            // code exits the using block.
            string connectionString = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;
            DocumentRS = new Recordset()
            {
                CursorLocation = CursorLocationEnum.adUseClient
            };
            DocumentRS.Open(sSQL, connectionString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic);

            string fieldValue = "";
            if (DocumentRS.RecordCount == 1)
            {
                if (DocumentRS.Fields["v405"].Value == null)
                {
                    fieldValue = "";
                }
                else
                {
                    fieldValue = DocumentRS.Fields["v405"].Value.ToString();
                }
            }

            DocumentRS?.Close();
            return fieldValue;
        }

        public bool SetSellersDocumentResultRS(string document, string jsonResult)
        {
            Cursor.Current = Cursors.WaitCursor;
            string sSQL = "SELECT * FROM Dokumenten WHERE  v033 = '" + document.Substring(0, 11) + "'";  // 'Journalen.v066";
            // Open the connection and execute the insert command.
            // The connection is automatically closed when the
            // code exits the using block.
            string connectionString = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;
            DocumentRS = new Recordset()
            {
                CursorLocation = CursorLocationEnum.adUseClient
            };
            DocumentRS.Open(sSQL, connectionString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic);
            Cursor.Current = Cursors.Default;
            if (DocumentRS.RecordCount == 1)
            {
                try
                {
                    DocumentRS.Fields["v405"].Value = jsonResult; // Set the field to the JSON result
                    DocumentRS.Fields["dnnSync"].Value = "False"; // Mark as not synced to DNN yet 
                    DocumentRS.Update();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        
        private void DoPopUpEntitiesData(string messageAsJson)
        {
            FormDataGridJsonPopUp formJsonTable = new FormDataGridJsonPopUp
            {
                jsonData = messageAsJson, // Pass the JSON data to the popup form
                jsonType = "registration",
                Dock = DockStyle.Fill // Fill the popup form with the JSON table view                
            };
            formJsonTable.LoadRegistrationJsonData();
            formJsonTable.ShowDialog(this); // Show the popup form as a dialog, centered on the main form
        }

        private void DoPopUpDataGridJsonData(string messageAsJson)
        {
            FormDataGridJsonPopUp.Controls.Clear();
            FormDataGridJsonPopUp formJsonTable = new FormDataGridJsonPopUp
            {
                jsonData = messageAsJson, // Pass the JSON data to the popup form
                jsonType = "notification", // or "registration" based on context
                Dock = DockStyle.Fill // Fill the popup form with the JSON table view                
            };
            formJsonTable.LoadNotificationJsonData();
            formJsonTable.ShowDialog(this); // Show the popup form as a dialog, centered on the main form
        }

        // Helper for safe node text extraction        
        private static string NodeText(XmlNode parentNode, string xpath, XmlNamespaceManager ns)
        {
            var node = parentNode.SelectSingleNode(xpath, ns);
            return node?.InnerText.Trim() ?? "";
        }

        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                MessageBox.Show("The specified folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Your repeated instructions here
            ToolStripStatusLabel.Text = "10 minutes passed! Refreshing lists...";
            Application.DoEvents();

            RefreshMonitor();

        }

        private static string ReadUBLInvoice(string filePath, bool messageBox, bool justDocumentId)
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (Exception ex)
            {
                if (messageBox)
                    MessageBox.Show($"Error loading XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ex.Message;
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
            string documentId = invoiceIdNode?.InnerText ?? "";
            if (justDocumentId)
                return documentId;

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
                if (messageBox)
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
            if (messageBox)
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

                if (messageBox)
                    MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (messageBox)
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

                if (messageBox)
                    MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // MessageBox.Show("No AccountingCustomerParty element found.");
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

                if (messageBox)
                    MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (messageBox)
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
                if (messageBox)
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

                if (messageBox)
                    MessageBox.Show(msgTax.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                if (messageBox)
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

                if (messageBox)
                    MessageBox.Show(msgLines.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return documentId;
        }

        async private void ButtonCheckVat_Click(object sender, EventArgs e)
        {
            string vatNumber = TextBoxVatNumber.Text;
            string countryCode = vatNumber.Substring(0, 2);
            string vat = vatNumber.Substring(2);
            LabelResponse.Text = "Bezig...";
            LabelResponseContent.Text = "Bezig...";

            string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + countryCode + "/vat/" + vat;

            httpCheck = new HttpClient();

            HttpResponseMessage response = await httpCheck.GetAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();
            LabelResponse.Text = response.ToString();
            LabelResponseContent.Text = responseContent;
        }

        async private void ButtonPublicSearch_Click(object sender, EventArgs e)
        {
            string toSearch;

            if (RadioButtonGetReceived.Checked)
            {
                toSearch = TextBoxReceiver.Text.Trim();
            }
            else
            {
                toSearch = TextBoxSender.Text.Trim();
            }

            string result = await MarHelpers.GetPublicPeppolRegistrationAsync(toSearch, true);            
            if (result != null)
            {
                ToolStripStatusLabel.Text = "Notifications retrieved successfully.";
                var deserializedString = JsonConvert.DeserializeObject(result);
                RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                MessageBox.Show(
                RichTextBoxResponses.Text,
                "Public Peppol Registration Search Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
            else
            {
                ToolStripStatusLabel.Text = "Failed Public Peppol Registration Search";
                RichTextBoxResponses.Text = "";
            }
        }

        async public Task<int> RefreshSupportedDocumentsCustomersRS()
        {
            int numberUpdated = 0;
            string sSQL =
                "SELECT Klanten.A110, Klanten.A100, Klanten.v404, Klanten.v150, Klanten.v407, Klanten.dnnSync FROM Klanten WHERE trim(Klanten.v150) = 'BE' AND len(trim(Klanten.v404)) = 10;";

            string connectionString = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;
            DocumentRS = new Recordset()
            {
                CursorLocation = CursorLocationEnum.adUseClient
            };
            DocumentRS.Open(sSQL, connectionString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic);            
            if (DocumentRS.RecordCount > 0)
            {
                int recordCount = DocumentRS.RecordCount;
                // Assuming you have a DataGridView named dataGridViewCustomers on your form
                dataGridViewCustomers.Rows.Clear();
                dataGridViewCustomers.Columns.Clear();
                dataGridViewCustomers.Columns.Add("A110", "Id");
                dataGridViewCustomers.Columns.Add("v404", "KBO");
                dataGridViewCustomers.Columns.Add("A100", "Name");                
                dataGridViewCustomers.Columns.Add("v407", "Ondersteund");

                DocumentRS.MoveFirst();
                while (!DocumentRS.EOF)
                {
                    string id = DocumentRS.Fields["A110"].Value.ToString().Trim();
                    string kbo = DocumentRS.Fields["v404"].Value.ToString().Trim();
                    string name = DocumentRS.Fields["A100"].Value.ToString().Trim();
                    string supported = DocumentRS.Fields["v407"].Value.ToString().Trim();
                    dataGridViewCustomers.Rows.Add(id, kbo, name, supported);
                    DocumentRS.MoveNext();
                }
                dataGridViewCustomers.AutoResizeColumns();
                dataGridViewCustomers.Visible = true;
                Application.DoEvents();

                try
                {
                    DocumentRS.MoveFirst();
                    while (!DocumentRS.EOF)
                    {
                        string customerId = DocumentRS.Fields["A110"].Value.ToString().Trim();
                        string customerName = DocumentRS.Fields["A100"].Value.ToString().Trim();
                        string customerKbo = DocumentRS.Fields["v404"].Value.ToString().Trim();
                        string customerCountry = DocumentRS.Fields["v150"].Value.ToString().Trim();
                        string supportedDocuments = DocumentRS.Fields["v407"].Value.ToString().Trim();

                        if (customerKbo.Length == 10)                            
                        {
                            string result = await MarHelpers.GetPublicPeppolRegistrationAsync("0208:" + customerKbo, false);
                            if (result != "")
                            {   
                                bool same = XmlComparer.AreXmlStringsEqual(result, supportedDocuments);
                                if (!same)
                                {   
                                    DocumentRS.Fields["V407"].Value = result; // Set the field to the JSON result
                                    DocumentRS.Fields["dnnSync"].Value = "False"; // Mark as to be synced 
                                    numberUpdated++;
                                    DocumentRS.Update();
                                    ToolStripStatusLabel.Text = "Bezig... " + numberUpdated + " of "+ recordCount +" - " + customerName;
                                    Application.DoEvents();
                                }
                            }
                        }
                        DocumentRS.MoveNext();
                    }
                    DocumentRS?.Close();         
                    return numberUpdated; // Return the number of updated records 
                }
                catch (Exception)
                {                       
                    return 0;
                }
            }
            else
            {                
                return 0;
            }
        }

        private async void ButtonUpdateBECustomersSupported_Click(object sender, EventArgs e)
        {
            string confirmMessage = "Weet u zeker dat u de Supported Documents van alle Belgische klanten wilt bijwerken? Dit kan enige tijd duren.";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig bijwerken", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return; // User chose No, exit the method
            }

            ToolStripStatusLabel.Text = "Bezig...";
            Application.UseWaitCursor = true;
            Application.DoEvents();
            int updated = await RefreshSupportedDocumentsCustomersRS(); // Await the Task<bool> to get the result
            Application.UseWaitCursor = false;
            if (updated > 0)
            {
                ToolStripStatusLabel.Text = updated + " Customers Supported Documents updated successfully.";
                MessageBox.Show("Customers Supported Documents updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridViewCustomers.Visible = false;
            }
            else
            {
                ToolStripStatusLabel.Text = "Failed to update Customers Supported Documents.";
                MessageBox.Show("Failed to update Customers Supported Documents.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

