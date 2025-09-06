using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MarioApp2025.MarioMenu.Admin
{
    public partial class FormDataGridJsonPopUp : Form
    {
        public string jsonData = string.Empty; // Initialize to avoid CS8618
        public string jsonType = string.Empty;

        public FormDataGridJsonPopUp()
        {
            InitializeComponent();
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = DataGridView.SelectedRows[0];
                var cellValues = new StringBuilder();
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    cellValues.AppendLine($"{DataGridView.Columns[cell.ColumnIndex].HeaderText}: {cell.Value}");
                }

                MessageBox.Show(cellValues.ToString(), "Selected Row Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void LoadNotificationJsonData()
        {
            var root = JsonConvert.DeserializeObject<RootNotification>(jsonData);
            RichTextBox.Text = jsonData;

            if (root?.Notifications != null)
            {
                DataGridView.DataSource = root.Notifications;
            }
        }
        public void LoadRegistrationJsonData()
        {
            var root = JsonConvert.DeserializeObject<RootRegistration>(jsonData);

            if (root?.LegalEntities != null)
            {
                // Flatten for display: each peppolRegistration becomes its own row
                var flatList = new List<LegalEntityDisplay>();

                foreach (var entity in root.LegalEntities)
                {
                    foreach (var reg in entity.PeppolRegistrations ?? Enumerable.Empty<PeppolRegistration>())
                    {
                        flatList.Add(new LegalEntityDisplay
                        {
                            LegalEntityId = entity.LegalEntityId,
                            Name = entity.LegalEntityDetails?.Name,
                            CountryCode = entity.LegalEntityDetails?.CountryCode,
                            Address = entity.LegalEntityDetails?.GeographicalInformation,
                            RegistrationDate = entity.LegalEntityDetails?.RegistrationDate ?? default,
                            ContactName = entity.LegalEntityDetails?.Contacts?.FirstOrDefault()?.Name,
                            ContactEmail = entity.LegalEntityDetails?.Contacts?.FirstOrDefault()?.Email,
                            PeppolScheme = reg.PeppolRegistrationDetails?.PeppolIdentifier?.Scheme,
                            PeppolIdentifier = reg.PeppolRegistrationDetails?.PeppolIdentifier?.Identifier,
                            SupportedDocuments = string.Join(", ", reg.PeppolRegistrationDetails?.SupportedDocuments ?? Enumerable.Empty<string>())
                        });
                    }
                }

                DataGridView.DataSource = flatList;
            }

            RichTextBox.Text = jsonData;
        }

        // Root JSON mapping
        public class RootRegistration
        {
            [JsonProperty("pagination")]
            public Pagination Pagination { get; set; }

            [JsonProperty("legalEntities")]
            public List<LegalEntity> LegalEntities { get; set; }
        }
        public class Pagination
        {
            public int Count { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        public class LegalEntity
        {
            public int LegalEntityId { get; set; }
            public LegalEntityDetails LegalEntityDetails { get; set; }
            public List<PeppolRegistration> PeppolRegistrations { get; set; }
        }
        public class LegalEntityDetails
        {
            public bool PublishInPeppolDirectory { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            [JsonProperty("geographicalInformation")]
            public string GeographicalInformation { get; set; }
            public DateTime RegistrationDate { get; set; }
            public List<Contact> Contacts { get; set; }
            public List<object> PeppolAdditionalIdentifiers { get; set; }
        }
        public class Contact
        {
            public string ContactType { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }

        public class PeppolRegistration
        {
            public int PeppolRegistrationId { get; set; }
            public PeppolRegistrationDetails PeppolRegistrationDetails { get; set; }
        }
        public class PeppolRegistrationDetails
        {
            public PeppolIdentifier PeppolIdentifier { get; set; }
            public List<string> SupportedDocuments { get; set; }
            public bool PeppolRegistration { get; set; }
        }
        public class PeppolIdentifier
        {
            public string Scheme { get; set; }
            public string Identifier { get; set; }
        }

        // Flattened display model
        public class LegalEntityDisplay
        {
            public int LegalEntityId { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            public string Address { get; set; }
            public DateTime RegistrationDate { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
            public string PeppolScheme { get; set; }
            public string PeppolIdentifier { get; set; }
            public string SupportedDocuments { get; set; }
        }

        public class RootNotification
        {
            [JsonProperty("pagination")]
            public Pagination Pagination { get; set; } = new Pagination(); // Default initialization

            [JsonProperty("notifications")]
            public List<Notification> Notifications { get; set; } = new List<Notification>(); // Default initialization
        }
        public class Notification
        {
            public string EventType { get; set; }
            public int NotificationId { get; set; }
            public string TransmissionId { get; set; }
            public string SbdhTransmissionId { get; set; }
            public DateTime NotificationDate { get; set; }
            public string DocumentId { get; set; }
            public string PeppolDocumentType { get; set; }
            public string DocumentStatus { get; set; }
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public List<object> Details { get; set; } = new List<object>();
        }
    }
}

