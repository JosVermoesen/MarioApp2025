using IDEALSoftware.VpeCommunity;
using MarioApp2025.Classes.Ademico;
using MarioApp2025.MarioMenu.Actions;
using MarioApp2025.MarioMenu.Admin;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static QRCoder.QRCodeGenerator;

namespace MarioApp2025
{
    public partial class FormMario : Form
    {
        private readonly VpeControl AutoPageBreak;
        
        public Form FormDataGridJsonPopUp { get; set; }

        public FormMario()
        {
            InitializeComponent();
            Text = "Mario2025";
            AutoPageBreak = new VpeControl();
        }

        private void MenuItemCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuItemMdvToSql_Click(object sender, EventArgs e)
        {
            var form = new FormMarioTools();
            form.ShowDialog(this);
        }

        private void FormMario_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.IsUpgraded)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsUpgraded = true;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.MainTop <= 0)
            {
                this.Width = 816;
                this.Height = 489;
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.Top = Properties.Settings.Default.MainTop;
                this.Left = Properties.Settings.Default.MainLeft;
                this.Width = Properties.Settings.Default.MainWidth;
                this.Height = Properties.Settings.Default.MainHeight;
            }
            SharedGlobals.UserGuid = Properties.Settings.Default.GuidToControl;
            MarHelpers.SetApiMode(SharedGlobals.ApiModus);
        }
        private void FormMario_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainTop = this.Top;
            Properties.Settings.Default.MainLeft = this.Left;
            Properties.Settings.Default.MainWidth = this.Width;
            Properties.Settings.Default.MainHeight = this.Height;
            Properties.Settings.Default.Save();
        }

        private void MenuItemUserSettings_Click(object sender, EventArgs e)
        {
            var form = new FormChooseCompany();
            form.ShowDialog(this);
            if (SharedGlobals.ActiveCompany != "")
            {
                Text = "Mario2025 - [" + SharedGlobals.ActiveCompany + "] " + SharedGlobals.CompanyName + " [" + SharedGlobals.ApiModus + "]";
                MenuItemZipCompany.Text = "Zip bedrijf " + SharedGlobals.ActiveCompany + " naar Marnt Cloud";
            }
            else
            {
                Text = "Mario2025" + " [" + SharedGlobals.ApiModus + "]";
                MenuItemZipCompany.Text = "Zip bedrijf naar Marnt Cloud";
            }
        }

        private void FormMario_Shown(object sender, EventArgs e)
        {
            bool isAdmin = Properties.Settings.Default.IsAdmin;
            AdminMenuItem.Visible = isAdmin; // Show or hide the Admin menu based on the IsAdmin setting             

            // Load MimDataLocation from marIntegraal settings
            // Value must contains "\marnt\data"
            string value = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "marIntegraal",     // Section
                "Bedrijfsinhoudsopgave2025",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            bool containsPath = value.ToLower().Contains(@"\marnt\data".ToLower());
            if (!containsPath)
            {
                MessageBox.Show("De locatie van de bedrijfsinhoudsopgave is niet correct ingesteld.\n\nDuidt in marIntegraal een correcte locatie aan a.u.b.", "Fout in locatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            SharedGlobals.SetMimDataLocation(value);

            // Load MarNT CloudLocation from settings
            value = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "dnnInstellingen",     // Section
                "Cloud",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            // A marNT Clouddrive Location must ends with  "\marnt"                        
            containsPath = value.ToLower().EndsWith(@"\marnt".ToLower());
            if (!containsPath)
            {
                MessageBox.Show("De locatie van de bedrijfsinhoudsopgave is niet correct ingesteld.\n\nDuidt in marIntegraal een correcte locatie aan a.u.b.", "Fout in locatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            SharedGlobals.MarntCloudLocation = value;

            // Load MarNT Archive CloudLocation from settings
            value = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "dnnInstellingen",     // Section
                "Archief",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            // A Clouddrive marNT archive Location must end with "\marnt\archief"                        
            containsPath = value.ToLower().EndsWith(@"\marnt\archief".ToLower());
            if (!containsPath)
            {
                MessageBox.Show("De locatie van de bedrijfsinhoudsopgave is niet correct ingesteld.\n\nDuidt in marIntegraal een correcte locatie aan a.u.b.", "Fout in locatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            SharedGlobals.MarntCLoudArchiveLocation = value;

            // A Clouddrive MarNT Mario Location must end with "\marnt\mario"                        
            value = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "dnnInstellingen",     // Section
                "Mario",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            // A Clouddrive marNT Location must end with "\marnt"                        
            containsPath = value.ToLower().EndsWith(@"\marnt\mario".ToLower());
            if (!containsPath)
            {
                MessageBox.Show("De locatie van de bedrijfsinhoudsopgave is niet correct ingesteld.\n\nDuidt in marIntegraal een correcte locatie aan a.u.b.", "Fout in locatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            SharedGlobals.MarntCloudMarioLocation = value;

            // Trigger the user settings to set the company if needed
            MenuItemUserSettings_Click(string.Empty, EventArgs.Empty);
        }

        private void MenuItemPeppolActions_Click(object sender, EventArgs e)
        {
            if (SharedGlobals.CompanyKBONumber == "")
            {
                MessageBox.Show("Eerst een bedrijf activeren a.u.b.", "Kies eerst een bedrijf", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (SharedGlobals.UserGuid  != MyApiSecrets.guidToAccess)
            {
                MessageBox.Show("Eerst uw sleutel activeren", "Toegangsleutel ontbreekt of onjuist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var form = new FormPeppolClientActions();
            form.ShowDialog(this);
        }

        private void MenuItemPeppolTesting_Click(object sender, EventArgs e)
        {
            var form = new FormPeppolActions();
            form.ShowDialog(this);
        }

        private void MenuItemPeppolSettings_Click(object sender, EventArgs e)
        {
            var form = new FormAdminSettings();
            form.ShowDialog(this);

        }

        private void MenuItemAutoPageBreak_Click(object sender, EventArgs e)
        {
            // =====================================================================
            //                             Auto Page Break
            // =====================================================================
            OpenFileDialog SPPathFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"c:\",
                Filter = "c# txt files (*.cs)|*.cs|All files (*.txt)|*.txt",
                Multiselect = true,
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (SPPathFileDialog.ShowDialog() == DialogResult.OK)
            {
                // string fileContent = string.Empty;
                string firstFullPath; // = string.Empty;

                //Get the path of specified file
                firstFullPath = SPPathFileDialog.FileName;
                // var arrayFileNames = SPPathFileDialog.FileNames;

                string sLine, block = "";
                StreamReader stream;

                FileInfo fi = new FileInfo(firstFullPath);
                if (!fi.Exists)
                {
                    MessageBox.Show("File \"" + firstFullPath + "\" not found.");
                    return;
                }

                stream = new StreamReader(firstFullPath);
                MenuItemAutoPageBreak.Enabled = false;
                while (stream.Peek() > -1)    // not EOF
                {
                    sLine = stream.ReadLine();

                    // Replace all TAB characters with blanks, since the plain text object created with
                    // VPE.Print[Box]() or VPE.Write[Box]() does not handle TAB's
                    // NOTE: The RTF (Rich Text Format) object of the VPE Professional Edition DOES handle
                    //       tabs and also hanging indents, but this demo is intended for the
                    //       VPE Standard Edition.
                    sLine = sLine.Replace("\t", "   ");
                    block = block + "\n" + sLine;
                }
                stream.Close();

                MessageBox.Show("We loaded the file\n \"" + firstFullPath + "\"\n\n" +
                    "into memory using the slow \"ReadLine\" method.\n" +
                    "We also replaced all tab-characters with blanks.\n" +
                    "NOW we call VPE and create a document from the data.\n" +
                    "VPE will create the page breaks itself. This will work very fast!");

                AutoPageBreak.OpenDoc();
                AutoPageBreak.SelectFont("Courier New", 8);

                // Set the bottom margin, so the report will fit
                // onto A4 as well as onto US-Letter paper:
                // =============================================
                AutoPageBreak.SetOutRect(2, 2, 19, 26.5);

                // Header will be placed outside default output rectangle:
                AutoPageBreak.NoPen();
                AutoPageBreak.TextUnderline = true;
                AutoPageBreak.DefineHeader(1, 1, -7, -0.5, "Auto Text Break Demo - Page @PAGE");

                // On every intial page:
                // VLEFT   = VLEFTMARGIN
                // VTOP    = VTOPMARGIN
                // VRIGHT  = VRIGHTMARGIN
                // VBOTTOM = VTOPMARGIN !!!!!!!!!!
                AutoPageBreak.TextUnderline = false;
                AutoPageBreak.SetPen(0.03, PenStyle.Solid, Color.Black);
                AutoPageBreak.WriteBox(AutoPageBreak.nLeft, AutoPageBreak.nBottom, AutoPageBreak.nRight, AutoPageBreak.nFree, "[N TO BC LtGray CE S 12 B]Start of Listing");
                AutoPageBreak.WriteBox(AutoPageBreak.nLeft, AutoPageBreak.nBottom, AutoPageBreak.nRight, AutoPageBreak.nFree, block);
                AutoPageBreak.WriteBox(AutoPageBreak.nLeft, AutoPageBreak.nBottom, AutoPageBreak.nRight, AutoPageBreak.nFree, "[N TO BC LtGray CE S 12 B]End of Listing");

                AutoPageBreak.Preview();

                ExportDocument("Auto Break.pdf", AutoPageBreak);
                MenuItemAutoPageBreak.Enabled = true;

            }
        }

        public static void ExportDocument(string FileName, VpeControl VpeDoc)
        {
            if (MessageBox.Show("Create PDF file?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FileName = Path.GetTempPath() + FileName;
                if (VpeDoc.WriteDoc(FileName))
                {
                    MessageBox.Show("PDF file \"" + FileName + "\" created successfully.");
                    Process process = new Process();
                    process.StartInfo.FileName = FileName;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }
                else
                {
                    MessageBox.Show("Error creating PDF file \"" + FileName + "\"\n" +
                        "Possible reasons:\n" +
                        " - the file is open in Acrobat Reader\n" +
                        " - hard disk full\n" +
                        " - no access rights to the folder\n" +
                        " - out of memory\n" +
                        " - export module missing");
                }
            }
        }

        private void MenuItemZipCompany_Click(object sender, EventArgs e)
        {
            if (SharedGlobals.ActiveCompany == "")
            {
                MessageBox.Show("Eerst een bedrijf activeren a.u.b.", "Kies eerst een bedrijf", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string sourceFolderPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany;
            string cloudFolderPath = SharedGlobals.MarntCLoudArchiveLocation;

            try
            {
                string zipResult = BackupHelper.ZipFolderToCloudDrive(sourceFolderPath, cloudFolderPath);
                Application.DoEvents();
                Cursor.Current = Cursors.Default;
                ToolStripStatusLabel.Text = "Ready";
                Application.DoEvents();

                var answer = MessageBox.Show(
                    zipResult + " met succes.\n\nFolder openen?", "Zip naar Marnt Cloud",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (answer == DialogResult.No) return;
                OpenFolder(cloudFolderPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Fout bij het zippen van de map naar Marnt Cloud. Controleer of de Marnt Cloud locatie correct is ingesteld.", "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
            }
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
    }
}
