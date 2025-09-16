using MarioApp2025.Classes.Ademico;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MarioApp2025.MarioMenu.Actions
{
    public partial class FormChooseCompany : Form
    {        
        public FormChooseCompany()
        {
            InitializeComponent();
            LabelMimDataLocation.Text = SharedGlobals.MimDataLocation;
            Text = "Mario2025 - Bedrijf Activeren";
            if (LabelMimDataLocation.Text.Length > 0)
            {
                FillCompanyList();
            }
        }

        private void CheckBoxIsAdmin_CheckedChanged(object sender, System.EventArgs e)
        {
            TextBoxIsAdminPassword.Visible = CheckBoxIsAdmin.Checked;
            ButtonValidate.Visible = CheckBoxIsAdmin.Checked;

        }
        private void ButtonValidate_Click(object sender, System.EventArgs e)
        {
            if (TextBoxIsAdminPassword.Text == "Thequickbrownfoxjumpsoverthelazydog123!")
            {
                SharedGlobals.IsAdmin = true;
                Properties.Settings.Default.IsAdmin = true;
                Properties.Settings.Default.Save();
                MessageBox.Show("Admin rechten verkregen.\nHerstart het programma om de admin rechten beschikbaar te maken.");
                TextBoxIsAdminPassword.Visible = false;
                ButtonValidate.Visible = false;
                CheckBoxIsAdmin.Checked = false;
            }
            else
            {
                Properties.Settings.Default.IsAdmin = false;
                Properties.Settings.Default.Save();
                SharedGlobals.IsAdmin = false;
                MessageBox.Show("Foutief paswoord");
            }
            TextBoxIsAdminPassword.Text = "";

        }
        private void ButtonClose_Click(object sender, System.EventArgs e)
        {            
            Close();
        }
        private void ListBoxCompanies_Click(object sender, System.EventArgs e)
        {            
            if (ListBoxCompanies.SelectedItem != null)
            {
                string selectedItem = ListBoxCompanies.SelectedItem.ToString();
                int separatorPos = selectedItem.IndexOf(" - ");
                if (separatorPos > 0)
                {                 
                    string selectedCompany = selectedItem.Substring(0, separatorPos);
                    // check if company is 098 to 099 (demo companies)
                    if (selectedCompany == "098" && SharedGlobals.ApiModus == "PRODUCTION" || selectedCompany == "099" && SharedGlobals.ApiModus == "PRODUCTION")
                    {
                        MessageBox.Show("Waarschuwing: de demo bedrijven 098 en 099 zijn niet geschikt voor productie gebruik.\n\n" +
                            "Gelieve een ander bedrijf te kiezen.", SharedGlobals.ApiModus);

                        return;
                    }

                    // check if company is 001 to 097 (real companies)  
                    if (int.TryParse(selectedCompany, out int companyNumber))
                    {
                        if (companyNumber >= 1 && companyNumber <= 97 && SharedGlobals.ApiModus == "TESTMODE")
                        {
                            MessageBox.Show("Waarschuwing: de echte bedrijven 001 tot en met 097 zijn niet geschikt voor test gebruik.\n\n" +
                                "Gelieve een ander bedrijf te kiezen.", SharedGlobals.ApiModus);
                            return;
                        }
                    }
                    MarHelpers.SetCompanyGlobals(selectedCompany);

                    try
                    {
                        string peppolOutDirectoryPath = SharedGlobals.MimDataLocation + "\\" + selectedCompany + "\\peppol\\out";
                        SharedGlobals.PeppolOutFiles = Directory.GetFiles(peppolOutDirectoryPath, "*.xml").Length;
                        Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Het lijkt alsof dit bedrijf belangrijke inhoudsopgaves ontbreekt\nOpen dit bedrijf eerst met marIntegraal 11.3.026 (of hoger)");
                        MarHelpers.ResetCompanyGlobals();
                    }
                }
            }
        }

        private void FillCompanyList()
        {
            ListBoxCompanies.Items.Clear();

            string MyPath = LabelMimDataLocation.Text;
            ResultExploringFileSystem(MyPath);
        }
        private bool ResultExploringFileSystem(string path)
        {
            if (path == "" || path == null)
            {
                return false;
            }

            if (Directory.Exists(path))
            {
                ProcessDirectory(path, "companyFolder");
                return true;
            }
            else
            {
                MessageBox.Show(path + " is not a valid directory");
                return false;
            }
        }
        private void ProcessDirectory(string targetDirectory, string listType)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                if (listType == "companyFolder")
                {
                    int subDirLength = subdirectory.Length;
                    string subDirMap = subdirectory.Substring(subDirLength - 3);
                    ProcessMarntTxtFile(subdirectory, subDirMap);
                }
            }
        }
        private void ProcessMarntTxtFile(string companyPath, string mapName)
        {
            string marntTxtPath = companyPath + @"\marnt.txt";
            if (File.Exists(marntTxtPath))
            {
                AddToMimList(marntTxtPath, mapName);
            }
            else
            {
                MessageBox.Show(marntTxtPath + " is not a valid file");
            }
        }
        private void AddToMimList(string marntTxtPath, string mapName)
        {
            // TODO add content to list view
            try
            {
                var stream = new StreamReader(marntTxtPath);
                if (stream.Peek() > -1)    // not EOF
                {
                    string line = stream.ReadLine();
                    ListBoxCompanies.Items.Add(mapName + " - " + line);
                }
                stream.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show(marntTxtPath + " could not be read");
                MessageBox.Show(e.Message);
            }
        }

        private void ButtonValidateGuid_Click(object sender, EventArgs e)
        {
            string guidInput = TextBoxGuidToValidate.Text;
            if (guidInput.Length != 36)
            {
                MessageBox.Show("Waarschuwing: de ingegeven GUID is niet correct.\nEen geldige GUID heeft 36 tekens.");
                TextBoxGuidToValidate.Text = "";
                return;
            }

            if (guidInput != MyApiSecrets.guidToAccess)
            {
                MessageBox.Show("Waarschuwing: de ingegeven GUID komt niet overeen met de verwachte GUID.\nControleer of de GUID correct is.");
                TextBoxGuidToValidate.Text = "";
                return;
            }
            SharedGlobals.UserGuid = TextBoxGuidToValidate.Text;
            MessageBox.Show("De ingegeven GUID is correct.\nU kan nu aan de slag met Peppol voor marIntegraal.");
            Properties.Settings.Default.GuidToControl = SharedGlobals.UserGuid;
            Properties.Settings.Default.Save();

            LabelUserGuid.Visible = false;
            TextBoxGuidToValidate.Visible = false;
            ButtonValidateGuid.Visible = false;
        }

        private void FormChooseCompany_Load(object sender, EventArgs e)
        {
            if (SharedGlobals.UserGuid != MyApiSecrets.guidToAccess)
            {
                LabelUserGuid.Visible = true;
                TextBoxGuidToValidate.Visible = true;
                ButtonValidateGuid.Visible = true;
            }

            if (SharedGlobals.ApiModus == "TESTMODE")
            {
                RadioButtonTestMode.Checked = true;
            }
            else
            {
                RadioButtonProductionMode.Checked = true;
            }
        }

        private void RadioButtonTestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonTestMode.Checked)
            {
                MarHelpers.SetApiMode("TESTMODE");
                Properties.Settings.Default.peppolTestMode = true;
                Properties.Settings.Default.Save();
                // Application.Restart();
            }
        }

        private void RadioButtonProductionMode_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonProductionMode.Checked)
            {
                MarHelpers.SetApiMode("PRODUCTION");
                Properties.Settings.Default.peppolTestMode = false;
                Properties.Settings.Default.Save();
                // Application.Restart();
            }
        }

        private void RadioButtonTestMode_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void RadioButtonProductionMode_Click(object sender, EventArgs e)
        {            
            Application.Restart();
        }        
    }
}
