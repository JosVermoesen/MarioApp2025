using ADODB;
using MarioApp2025.MarioMenu.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static MarioApp2025.AdemicoModels;
using Timer = System.Windows.Forms.Timer;


namespace MarioApp2025.MarioMenu.Actions
{
    public partial class FormMarPeppol : Form
    {
        private readonly Timer _timer;

        // Change the declaration of the `httpCheck` field to remove the `readonly` modifier
        private readonly HttpClient httpCheck;

        public string activeSellerDocument = "";

        public Form FormDataGridJsonPopUp { get; set; }

        // Testing ADODB connection to marnt.mdv file       
        public Recordset DocumentRS { get; private set; }

        public int totalInMapOut = 0;
        public int totalOutToRemoveFromNotifications = 0;

        public int totalReceivedInMap = 0;
        public int totalReceivedToRemoveFromNotifications = 0;

        // Update the constructor to initialize the `httpCheck` field
        public FormMarPeppol()
        {
            InitializeComponent();

            _timer = new Timer
            {
                Interval = 5 * 60 * 1000 // 5 minutes in milliseconds
            };
            _timer.Tick += Timer_Tick;
            _timer.Stop();

            if (SharedGlobals.ApiModus == "PRODUCTION")
            {
                Text = "marPeppol [" + SharedGlobals.ActiveCompany + "] " + SharedGlobals.CompanyName;
            }
            else
            {
                Text = "marPeppol [" + SharedGlobals.ActiveCompany + "] " + SharedGlobals.CompanyName + " " + SharedGlobals.ApiModus;
            }
            
            httpCheck = new HttpClient(); // Initialize here
            FormDataGridJsonPopUp = new FormDataGridJsonPopUp { };

            InitializeControls();
            RefreshMonitor();
        }

        private void InitializeControls()
        {
            // Focus on LabelResponseCode.Text = "AP"; // Acceptatie
            // Focus on LabelResponseCode.Text = "RE"; // Weigering
            // Code Description
            // AP   Accepted:
            //      Status is used only when the Buyer has given a final approval of the invoice
            //      and the next step is payment.
            // RE   Rejected:
            //      MLR reject or Invoice reject.Status is used only when the Buyer will not
            //      process the referenced Invoice any further.Buyer is rejecting this invoice
            //      but not necessarily the commercial transaction.Although it can be used also
            //      for rejection for commercial reasons (invoice not corresponding to delivery).
            // PD   Paid:
            //      Fully paid or partially paid.When partially paid:
            //      Status is used together with Clarification Reason code PPD,
            //      only when the Buyer has initiated the payment of the invoice
            //      without having paid the accepted amount in full.
            // AB   Buyer acknowledges:
            //      Status is used when Buyer has received a readable invoice message
            //      that can be understood and submitted for processing by the Buyer.            
            // IP   In process:
            //      Status is used when the processing of the Invoice has started in Buyers system.
            // UQ   Under query:
            //      Status is used when Buyer will not proceed to accept the Invoice
            //      without receiving additional information from the Seller.
            // CA   Conditionally accepted:
            //      Status is used when Buyer is accepting the Invoice under conditions stated
            //      in 'Status Reason' and proceed to pay accordingly unless disputed by Seller.            

            ComboBoxResponseCode.Items.Clear();
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("AP", "Accepted"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("RE", "Rejected"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("PD", "Paid"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("AB", "Buyer acknowledges"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("IP", "In process"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("UQ", "Under query"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("CA", "Conditionally accepted"));            
            ComboBoxResponseCode.DisplayMember = "Value";
            ComboBoxResponseCode.ValueMember = "Key";
            ComboBoxResponseCode.SelectedIndex = 0;

            // Code Description
            // REC  Receiver unknown
            // UNR  Not recognized
            // NO   No issue
            // REF  References incorrect
            // LEG  Legal information incorrect
            // QUA  Item quality insufficient
            // DEL  Delivery proposed or provided is not acceptable
            // PRI  Prices incorrect
            // QTY  Quantity incorrect
            // ITM  Items incorrect
            // PAY  Payment terms incorrect            
            // FIN  Finance incorrect
            // PPD  Partially Paid
            // OTH  Other
            ComboBoxClarificationCode.Items.Clear();
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("REC", "Receiver unknown"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("UNR", "Not recognized"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("NO", "No issue"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("REF", "References incorrect"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("LEG", "Legal information incorrect"));            
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("QUA", "Item quality insufficient"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("DEL", "Delivery not acceptable"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("PRI", "Prices incorrect"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("QTY", "Quantity incorrect"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("ITM", "Items incorrect"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("PAY", "Payment terms incorrect"));            
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("FIN", "Finance incorrect"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("PPD", "Partially Paid"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("OTH", "Other"));
            ComboBoxClarificationCode.DisplayMember = "Value";
            ComboBoxClarificationCode.ValueMember = "Key";
            ComboBoxClarificationCode.SelectedIndex = 0;

            ButtonToggleTabs_Click(null, null); // Start with only Monitor tab visible
        }

        // Common functions for multiple tabs
        private void RefreshMonitor()
        {
            totalInMapOut = 0;
            totalOutToRemoveFromNotifications = 0;
            totalReceivedInMap = 0;
            totalReceivedToRemoveFromNotifications = 0;

            FillListPeppolToSend(ListBoxDocumentsPeppolOut);
            LabelTotalnMapOut.Text = totalInMapOut.ToString();
            LabelTotalOutToRemoveFromNotifications.Text = totalOutToRemoveFromNotifications.ToString();

            FillListPeppolToReceive(ListBoxMonitorForPeppolIn);

            ToolStripStatusLabel.Text = "Ready";
            Application.DoEvents();
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
                ButtonTimer.Text = "Kringloop procedure starten (timer) ";
                ToolStripStatusLabel.Text = "Timer is gestopt.";
                ButtonRefreshAll.Enabled = true;
                return;
            }
            else
            {
                ButtonTimer.Text = "Kringloop procedure stoppen (timer) ";
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
        
        // Responses Tab

        // Send and receive UBL Document Tab        
        private void FillListPeppolToReceive(ListBox listboxIn)
        {
            string folderInPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in";
            listboxIn.Items.Clear();
            MessageBox.Show(folderInPath);

            // Check the API notifications for received documents for this company and fill the listbox


        }

        private void FillListPeppolToSend(ListBox listboxOut)
        {
            string folderOutPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\out";
            listboxOut.Items.Clear();

            if (Directory.Exists(folderOutPath))
            {
                string[] xmlFiles = Directory.GetFiles(folderOutPath, "*.xml");
                listboxOut.Items.Clear();
                listboxOut.Items.AddRange(xmlFiles);
                LabelFileOut.Text = "";
            }
            else
            {
                ToolStripStatusLabel.Text = "Geen te verzenden documenten voor " + SharedGlobals.ActiveCompany;
                listboxOut.Visible = false;
            }

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

        private void ListBoxDocumentsToSend_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonSendUblDocument.Enabled = false; // Disable the button when selecting a new file
            LabelFileOut.Text = "";

            if (ListBoxDocumentsPeppolOut.SelectedItem != null)
            {
                LabelFileOut.Text = ListBoxDocumentsPeppolOut.SelectedItem.ToString().ToUpper();
                string checkResult = ReadUBLInvoice(LabelFileOut.Text, false, false);
                string existingResult = GetSellersDocumentResultRS(checkResult.ToUpper());

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

                    // notificationResult = await InvoiceNotificationState(checkResult); // Wait for the async task to complete

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

            string filePath = LabelFileOut.Text.Trim().ToLower();
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
            string confirmMessage = $"Weet u zeker dat u het UBL document {Path.GetFileName(LabelFileOut.Text)} wilt verzenden?";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig Verzending", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                ToolStripStatusLabel.Text = "Verzending geannuleerd door gebruiker.";
                return; // User chose not to proceed
            }

            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string filePath = LabelFileOut.Text.Trim();
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
                    RefreshMonitor();
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
            if (TextBoxDocToReceiveTransId.Text.Length == 0)
            {
                MessageBox.Show("Gelieve een Transmission ID in te vullen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Ready";
                return;
            }

            string confirmMessage = $"Weet u zeker dat u het UBL document met Transmission ID {TextBoxDocToReceiveTransId.Text} wilt ophalen?";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig Ophalen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                ToolStripStatusLabel.Text = "Ophalen geannuleerd door gebruiker.";
                return; // User chose not to proceed
            }

            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string ademicoUrl = SharedGlobals.AdemicoApiUrl;
            string accessToken = SharedGlobals.AdemicoAccessToken;
            string username = SharedGlobals.AdemicoUsername;
            string password = SharedGlobals.AdemicoPassword;

            if (TextBoxDocToReceiveTransId.Text.Length == 0)
            {
                MessageBox.Show("Gelieve een Transmission ID in te vullen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Ready";
                return;
            }
            string transmissionId = TextBoxDocToReceiveTransId.Text.Trim();
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
        
        async private void ButtonPublicSearch_Click(object sender, EventArgs e)
        {
            string toSearch;

            if (CheckBoxReceiver.Checked)
            {
                toSearch = TbNotificationReceiver.Text.Trim();
            }
            else
            {
                toSearch = TbNotificationSender.Text.Trim();
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
                DataGridViewNotifications.Rows.Clear();
                DataGridViewNotifications.Columns.Clear();
                DataGridViewNotifications.Columns.Add("A110", "Id");
                DataGridViewNotifications.Columns.Add("v404", "KBO");
                DataGridViewNotifications.Columns.Add("A100", "Name");
                DataGridViewNotifications.Columns.Add("v407", "Ondersteund");

                DocumentRS.MoveFirst();
                while (!DocumentRS.EOF)
                {
                    string id = DocumentRS.Fields["A110"].Value.ToString().Trim();
                    string kbo = DocumentRS.Fields["v404"].Value.ToString().Trim();
                    string name = DocumentRS.Fields["A100"].Value.ToString().Trim();
                    string supported = DocumentRS.Fields["v407"].Value.ToString().Trim();
                    DataGridViewNotifications.Rows.Add(id, kbo, name, supported);
                    DocumentRS.MoveNext();
                }
                DataGridViewNotifications.AutoResizeColumns();
                DataGridViewNotifications.Visible = true;
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
                                    ToolStripStatusLabel.Text = "Bezig... " + numberUpdated + " of " + recordCount + " - " + customerName;
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
            string confirmMessage =
                "Weet u zeker dat u de fiches voor alle Belgische " +
                "B2B klanten van het active bedrijf wilt bijwerken " +
                "met hun Supported Documents?\n\n" + "Dit kan enige tijd in beslag nemen.";
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
                DataGridViewNotifications.Visible = false;
            }
            else
            {
                ToolStripStatusLabel.Text = "Failed to update Customers Supported Documents.";
                MessageBox.Show("Failed to update Customers Supported Documents.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async private void ButtonInvoiceReceivedResponse_Click(object sender, EventArgs e)
        {            
            string selectedResponseCode =
                ((KeyValuePair<string, string>)ComboBoxResponseCode.SelectedItem).Key;

            string selectedClarificationCode =
                ((KeyValuePair<string, string>)ComboBoxClarificationCode.SelectedItem).Key;

            var clarifications = new List<InvoiceResponseClarification>();            
            if (selectedResponseCode == "AP" || selectedResponseCode == "PD")
            {
                clarifications = null;
            }
            else
            {
                // Example clarification for rejection
                var clarification = new InvoiceResponseClarification
                {
                    ClarificationType = "OPStatusReason",
                    ClarificationCode = selectedClarificationCode,
                    Clarification = TextBoxClarification.Text
                };
                clarifications.Add(clarification);
            }
            
            try
            {
                var result = await AdemicoClient.SendInvoiceResponseAsync(
                    invoiceTransmissionId: TextBoxToAcceptOrRejectTransId.Text,  // "2278c90b96ea11f0a46406668988840f",
                    responseCode: selectedResponseCode, // "AP" = Accepted, see your code table
                    note: TextBoxNote.Text,
                    effectiveDate: DateTime.UtcNow.ToString("yyyy-MM-dd"), // Optional, can be empty
                    clarifications: clarifications  // or a list of InvoiceResponseClarification if needed
                );

                if (result != null)
                {
                    ToolStripStatusLabel.Text = "Invoice Response sent successfully.";
                    var deserializedString = JsonConvert.DeserializeObject(result.ResponseBody);
                    RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    MessageBox.Show(
                        RichTextBoxResponses.Text,
                        "Send Invoice Response Result",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    ToolStripStatusLabel.Text = "Failed to send Invoice Response.";
                    RichTextBoxResponses.Text = "";
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Error: {ex.Message}";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RadioButtonAccept_CheckedChanged(object sender, EventArgs e)
        {
            ButtonInvoiceReceivedResponse.Text = "Acceptatie Verzenden";
            LabelResponseCode.Text = "AP";
            // Factuur geaccepteerd voor betalingsverwerking.
        }

        private void RadiobuttonReject_CheckedChanged(object sender, EventArgs e)
        {
            ButtonInvoiceReceivedResponse.Text = "Weigering Verzenden";
            LabelResponseCode.Text = "RE";
        }

        // Notifications Tab
        private void CheckBoxSender_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationSender.Enabled = CheckBoxSender.Checked;
            if (!CheckBoxSender.Checked)
            {
                TbNotificationSender.Text = "";                
            }
            else
            {
                TbNotificationSender.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default sender for sent documents                
                TbNotificationReceiver.Text = "";
                TbNotificationReceiver.Enabled = false;
                CheckBoxReceiver.Checked = false;
            }
        }

        private void CheckBoxReceiver_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationReceiver.Enabled = CheckBoxReceiver.Checked;
            if (!CheckBoxReceiver.Checked)
            {
                TbNotificationReceiver.Text = "";
            }
            else
            {
                TbNotificationReceiver.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default receiver for received documents                
                TbNotificationSender.Text = "";
                TbNotificationSender.Enabled = false;
                CheckBoxSender.Checked = false;
            }
        }

        async private void ButtonNotifications_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                transmissionId: TbNotificationTransId.Text, // "f8a591c77b2211f0b1ed0af13d778bd4"
                documentId: TbNotificationDocumentId.Text,
                eventType: TbNotificationEventType.Text, // "DOCUMENT_RECEIVED" or "DOCUMENT_SENT"
                peppolDocumentType: TbNotificationPeppolDocumentType.Text, // "INVOICE"
                sender: TbNotificationSender.Text, // "9925:BE0440058217",
                receiver: TbNotificationReceiver.Text, // "0208:0440058217",
                startDateTime: "", // "2023-07-25T11:03:26.688Z"
                endDateTime: "", // "2023-07-29T11:03:26.688Z"
                page: "",
                pageSize: "50"
            );

            if (jsonResponse != null)
            {
                ToolStripStatusLabel.Text = "Notifications retrieved successfully.";
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                // DoPopUpDataGridJsonData(RichTextBoxResponses.Text); // Show the result in a popup with JSON table view

                // jsonString is your JSON from the API
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Root data = System.Text.Json.JsonSerializer.Deserialize<Root>(jsonResponse, options);
                // Assuming you have a DataGridView named dgvNotifications
                DataGridViewNotifications.AutoGenerateColumns = false;
                DataGridViewNotifications.Columns.Clear();

                // Add columns
                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Transmissie ID",
                    DataPropertyName = "TransmissionId",
                    // Width = 280
                });
                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Document ID",
                    DataPropertyName = "DocumentId",
                    // Width = 120
                });
                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Ontvangstdatum",
                    DataPropertyName = "ReceivedDate",
                    // Width = 150
                });

                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "MeldDatum",
                    DataPropertyName = "notificationDate",
                    Width = 150
                });

                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Status Document",
                    DataPropertyName = "documentStatus",
                    Width = 150
                });
                // Bind
                DataGridViewNotifications.DataSource = data.Notifications;
                DataGridViewNotifications.Refresh();
                DataGridViewNotifications.Visible = true;
            }
            else
            {
                ToolStripStatusLabel.Text = "Failed to retrieve notifications.";
                RichTextBoxResponses.Text = "";
            }

        }

        private void CheckBoxEventType_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationEventType.Enabled = CheckBoxEventType.Checked;
            if (!CheckBoxEventType.Checked)
            {
                TbNotificationEventType.Text = "";
            }
            else
            {
                // string eventType = RadioButtonGetReceived.Checked ? "DOCUMENT_RECEIVED" : "DOCUMENT_SENT";
                TbNotificationEventType.Text = "DOCUMENT_RECEIVED"; // Default event type for received documents                
                TbNotificationEventType.Focus();
            }
        }

        private void CheckBoxPeppolDocumentType_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationPeppolDocumentType.Enabled = CheckBoxPeppolDocumentType.Checked;
            if (!CheckBoxPeppolDocumentType.Checked)
            {
                TbNotificationPeppolDocumentType.Text = "";
            }
            else
            {
                TbNotificationPeppolDocumentType.Text = "INVOICE"; // Default peppol document type for invoices
                TbNotificationPeppolDocumentType.Focus();
            }
        }

        private void CheckBoxDocumentId_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationDocumentId.Enabled = CheckBoxDocumentId.Checked;
            if (!CheckBoxDocumentId.Checked)
            {
                TbNotificationDocumentId.Text = "";
            }
            else
            {
                TbNotificationDocumentId.Text = ""; // No default, user must enter document ID
                TbNotificationDocumentId.Focus();
            }
        }

        private void CheckBoxTransmissionId_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationTransId.Enabled = CheckBoxTransmissionId.Checked;
            if (!CheckBoxTransmissionId.Checked)
            {
                TbNotificationTransId.Text = "";
            }
            else
            {
                TbNotificationTransId.Text = ""; // No default, user must enter transmission ID
                TbNotificationTransId.Focus();

            }
        }

        private void TextBoxDocToReceiveTransId_TextChanged(object sender, EventArgs e)
        {
            ButtonGetUBLDocument.Enabled = TextBoxDocToReceiveTransId.Text.Length > 0;
        }

        private void TextBoxToAcceptOrRejectTransId_TextChanged(object sender, EventArgs e)
        {
            // e91c2e8396f411f08fb302bb4e4747f9
            ButtonInvoiceReceivedResponse.Enabled = TextBoxToAcceptOrRejectTransId.Text.Length > 0;
        }

        private void ComboBoxResponseCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCode = ((KeyValuePair<string, string>)ComboBoxResponseCode.SelectedItem).Key;
            if (selectedCode == "AP" || selectedCode == "PD")
            {
                // Accepted or Pending - disable clarification fields
                TextBoxNote.Text = "Document geaccepteerd voor betalingsverwerking.";
                TextBoxClarification.Text = "";
                TextBoxClarification.Visible = false;
                ComboBoxClarificationCode.Visible = false;
                LabelClarification.Visible = false;                
                ComboBoxClarificationCode.SelectedIndex = -1;                
            }
            else if (selectedCode == "RE")
            {
                // Rejected or other - enable clarification fields
                ComboBoxClarificationCode.SelectedIndex = 0; // Default to first clarification code
                TextBoxNote.Text = "Document geweigerd.";
                TextBoxClarification.Text = "";
                TextBoxClarification.Visible = true;
                ComboBoxClarificationCode.Visible = true;
                LabelClarification.Visible = true;                
            }
            else // Other codes
            {
                TextBoxNote.Text = "";
                TextBoxClarification.Text = "";
                TextBoxClarification.Visible = true;
                ComboBoxClarificationCode.Visible = true;
                LabelClarification.Visible = true;
            }
        }

        private void ButtonToggleTabs_Click(object sender, EventArgs e)
        {
            // TabNotifications
            // TabResponse
            // TabSendDocument
            // TabReceiveDocument
            if (!TabControlVariousActions.TabPages.Contains(TabNotifications))
            {
                // Show the tabs
                TabControlVariousActions.TabPages.Add(TabNotifications);
                TabControlVariousActions.TabPages.Add(TabResponse);
                TabControlVariousActions.TabPages.Add(TabSendDocument);
                TabControlVariousActions.TabPages.Add(TabReceiveDocument);
            } else
            {
                // Hide the tabs
                TabControlVariousActions.TabPages.Remove(TabNotifications);
                TabControlVariousActions.TabPages.Remove(TabResponse);
                TabControlVariousActions.TabPages.Remove(TabSendDocument);
                TabControlVariousActions.TabPages.Remove(TabReceiveDocument);
            }
        }
    }
}