namespace MarioApp2025.MarioMenu.Actions
{
    partial class FormPeppolClientActions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TabControlVariousActions = new System.Windows.Forms.TabControl();
            this.TabMonitor = new System.Windows.Forms.TabPage();
            this.ButtonCheckConnectivity = new System.Windows.Forms.Button();
            this.ButtonTimer = new System.Windows.Forms.Button();
            this.LabelMonitorReceived = new System.Windows.Forms.Label();
            this.LabelMonitorSent = new System.Windows.Forms.Label();
            this.ListBoxMonitorReceived = new System.Windows.Forms.ListBox();
            this.ListBoxMonitorSent = new System.Windows.Forms.ListBox();
            this.TabActions = new System.Windows.Forms.TabPage();
            this.ButtonZipToCloud = new System.Windows.Forms.Button();
            this.ButtonShowSharedGlobals = new System.Windows.Forms.Button();
            this.ButtonGetPeppolRegistrations = new System.Windows.Forms.Button();
            this.TextBoxCountryCode = new System.Windows.Forms.TextBox();
            this.TextBoxRegScheme = new System.Windows.Forms.TextBox();
            this.TextBoxRegIdentifier = new System.Windows.Forms.TextBox();
            this.TextBoxSupportedDocument = new System.Windows.Forms.TextBox();
            this.TextBoxLegalEntityId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TabNotifications = new System.Windows.Forms.TabPage();
            this.TextBoxSender = new System.Windows.Forms.TextBox();
            this.TextBoxReceiver = new System.Windows.Forms.TextBox();
            this.RadioButtonGetSent = new System.Windows.Forms.RadioButton();
            this.RadioButtonGetReceived = new System.Windows.Forms.RadioButton();
            this.ButtonNotifications = new System.Windows.Forms.Button();
            this.TabResponse = new System.Windows.Forms.TabPage();
            this.RichTextBoxResponses = new System.Windows.Forms.RichTextBox();
            this.TabSendDocument = new System.Windows.Forms.TabPage();
            this.ListBoxDocumentsToSend = new System.Windows.Forms.ListBox();
            this.LabelFile = new System.Windows.Forms.Label();
            this.ButtonSendUblDocument = new System.Windows.Forms.Button();
            this.ButtonCheckFile = new System.Windows.Forms.Button();
            this.TabReceiveDocument = new System.Windows.Forms.TabPage();
            this.ButtonGetUBLDocument = new System.Windows.Forms.Button();
            this.TextBoxTransmissionId = new System.Windows.Forms.TextBox();
            this.LabelTransmissionId = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabControlVariousActions.SuspendLayout();
            this.TabMonitor.SuspendLayout();
            this.TabActions.SuspendLayout();
            this.TabNotifications.SuspendLayout();
            this.TabResponse.SuspendLayout();
            this.TabSendDocument.SuspendLayout();
            this.TabReceiveDocument.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControlVariousActions
            // 
            this.TabControlVariousActions.Controls.Add(this.TabMonitor);
            this.TabControlVariousActions.Controls.Add(this.TabActions);
            this.TabControlVariousActions.Controls.Add(this.TabNotifications);
            this.TabControlVariousActions.Controls.Add(this.TabResponse);
            this.TabControlVariousActions.Controls.Add(this.TabSendDocument);
            this.TabControlVariousActions.Controls.Add(this.TabReceiveDocument);
            this.TabControlVariousActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlVariousActions.Location = new System.Drawing.Point(0, 0);
            this.TabControlVariousActions.Name = "TabControlVariousActions";
            this.TabControlVariousActions.SelectedIndex = 0;
            this.TabControlVariousActions.Size = new System.Drawing.Size(584, 385);
            this.TabControlVariousActions.TabIndex = 0;
            // 
            // TabMonitor
            // 
            this.TabMonitor.Controls.Add(this.ButtonCheckConnectivity);
            this.TabMonitor.Controls.Add(this.ButtonTimer);
            this.TabMonitor.Controls.Add(this.LabelMonitorReceived);
            this.TabMonitor.Controls.Add(this.LabelMonitorSent);
            this.TabMonitor.Controls.Add(this.ListBoxMonitorReceived);
            this.TabMonitor.Controls.Add(this.ListBoxMonitorSent);
            this.TabMonitor.Location = new System.Drawing.Point(4, 22);
            this.TabMonitor.Name = "TabMonitor";
            this.TabMonitor.Size = new System.Drawing.Size(576, 359);
            this.TabMonitor.TabIndex = 5;
            this.TabMonitor.Text = "Monitor";
            this.TabMonitor.UseVisualStyleBackColor = true;
            // 
            // ButtonCheckConnectivity
            // 
            this.ButtonCheckConnectivity.Location = new System.Drawing.Point(466, 13);
            this.ButtonCheckConnectivity.Name = "ButtonCheckConnectivity";
            this.ButtonCheckConnectivity.Size = new System.Drawing.Size(102, 23);
            this.ButtonCheckConnectivity.TabIndex = 17;
            this.ButtonCheckConnectivity.Text = "Verbinding Testen";
            this.ButtonCheckConnectivity.UseVisualStyleBackColor = true;
            this.ButtonCheckConnectivity.Click += new System.EventHandler(this.ButtonCheckConnectivity_Click);
            // 
            // ButtonTimer
            // 
            this.ButtonTimer.Location = new System.Drawing.Point(476, 212);
            this.ButtonTimer.Name = "ButtonTimer";
            this.ButtonTimer.Size = new System.Drawing.Size(92, 23);
            this.ButtonTimer.TabIndex = 11;
            this.ButtonTimer.Text = "Klok Start/Stop";
            this.ButtonTimer.UseVisualStyleBackColor = true;
            this.ButtonTimer.Click += new System.EventHandler(this.ButtonTimer_Click);
            // 
            // LabelMonitorReceived
            // 
            this.LabelMonitorReceived.AutoSize = true;
            this.LabelMonitorReceived.Location = new System.Drawing.Point(8, 217);
            this.LabelMonitorReceived.Name = "LabelMonitorReceived";
            this.LabelMonitorReceived.Size = new System.Drawing.Size(358, 13);
            this.LabelMonitorReceived.TabIndex = 10;
            this.LabelMonitorReceived.Text = "Te Ontvangen of Ontvangen (controleer tab blad Documenten Ontvangen";
            // 
            // LabelMonitorSent
            // 
            this.LabelMonitorSent.AutoSize = true;
            this.LabelMonitorSent.Location = new System.Drawing.Point(5, 18);
            this.LabelMonitorSent.Name = "LabelMonitorSent";
            this.LabelMonitorSent.Size = new System.Drawing.Size(355, 13);
            this.LabelMonitorSent.TabIndex = 9;
            this.LabelMonitorSent.Text = "Te Verzenden of Verzonden (controleer tab blad Documenten Verzenden)";
            // 
            // ListBoxMonitorReceived
            // 
            this.ListBoxMonitorReceived.FormattingEnabled = true;
            this.ListBoxMonitorReceived.Location = new System.Drawing.Point(9, 243);
            this.ListBoxMonitorReceived.Name = "ListBoxMonitorReceived";
            this.ListBoxMonitorReceived.Size = new System.Drawing.Size(560, 95);
            this.ListBoxMonitorReceived.TabIndex = 8;
            // 
            // ListBoxMonitorSent
            // 
            this.ListBoxMonitorSent.FormattingEnabled = true;
            this.ListBoxMonitorSent.Location = new System.Drawing.Point(9, 42);
            this.ListBoxMonitorSent.Name = "ListBoxMonitorSent";
            this.ListBoxMonitorSent.Size = new System.Drawing.Size(560, 95);
            this.ListBoxMonitorSent.TabIndex = 7;
            // 
            // TabActions
            // 
            this.TabActions.Controls.Add(this.ButtonZipToCloud);
            this.TabActions.Controls.Add(this.ButtonShowSharedGlobals);
            this.TabActions.Controls.Add(this.ButtonGetPeppolRegistrations);
            this.TabActions.Controls.Add(this.TextBoxCountryCode);
            this.TabActions.Controls.Add(this.TextBoxRegScheme);
            this.TabActions.Controls.Add(this.TextBoxRegIdentifier);
            this.TabActions.Controls.Add(this.TextBoxSupportedDocument);
            this.TabActions.Controls.Add(this.TextBoxLegalEntityId);
            this.TabActions.Controls.Add(this.label5);
            this.TabActions.Controls.Add(this.label4);
            this.TabActions.Controls.Add(this.label3);
            this.TabActions.Controls.Add(this.label2);
            this.TabActions.Controls.Add(this.label1);
            this.TabActions.Location = new System.Drawing.Point(4, 22);
            this.TabActions.Name = "TabActions";
            this.TabActions.Padding = new System.Windows.Forms.Padding(3);
            this.TabActions.Size = new System.Drawing.Size(576, 359);
            this.TabActions.TabIndex = 0;
            this.TabActions.Text = "Opzoekingen";
            this.TabActions.UseVisualStyleBackColor = true;
            // 
            // ButtonZipToCloud
            // 
            this.ButtonZipToCloud.Location = new System.Drawing.Point(320, 19);
            this.ButtonZipToCloud.Name = "ButtonZipToCloud";
            this.ButtonZipToCloud.Size = new System.Drawing.Size(111, 41);
            this.ButtonZipToCloud.TabIndex = 29;
            this.ButtonZipToCloud.Text = "Veiligheidskopij  van Actief Bedrijf maken";
            this.ButtonZipToCloud.UseVisualStyleBackColor = true;
            this.ButtonZipToCloud.Click += new System.EventHandler(this.ButtonZipToCloud_Click);
            // 
            // ButtonShowSharedGlobals
            // 
            this.ButtonShowSharedGlobals.Location = new System.Drawing.Point(203, 19);
            this.ButtonShowSharedGlobals.Name = "ButtonShowSharedGlobals";
            this.ButtonShowSharedGlobals.Size = new System.Drawing.Size(111, 41);
            this.ButtonShowSharedGlobals.TabIndex = 28;
            this.ButtonShowSharedGlobals.Text = "Gegevens Tonen van Actief Bedrijf";
            this.ButtonShowSharedGlobals.UseVisualStyleBackColor = true;
            this.ButtonShowSharedGlobals.Click += new System.EventHandler(this.ButtonShowSharedGlobals_Click);
            // 
            // ButtonGetPeppolRegistrations
            // 
            this.ButtonGetPeppolRegistrations.Location = new System.Drawing.Point(279, 206);
            this.ButtonGetPeppolRegistrations.Name = "ButtonGetPeppolRegistrations";
            this.ButtonGetPeppolRegistrations.Size = new System.Drawing.Size(152, 23);
            this.ButtonGetPeppolRegistrations.TabIndex = 27;
            this.ButtonGetPeppolRegistrations.Text = "Registratie(s) Opzoeken";
            this.ButtonGetPeppolRegistrations.UseVisualStyleBackColor = true;
            this.ButtonGetPeppolRegistrations.Click += new System.EventHandler(this.ButtonGetPeppolRegistrations_Click);
            // 
            // TextBoxCountryCode
            // 
            this.TextBoxCountryCode.Location = new System.Drawing.Point(100, 75);
            this.TextBoxCountryCode.Name = "TextBoxCountryCode";
            this.TextBoxCountryCode.Size = new System.Drawing.Size(77, 20);
            this.TextBoxCountryCode.TabIndex = 22;
            // 
            // TextBoxRegScheme
            // 
            this.TextBoxRegScheme.Location = new System.Drawing.Point(100, 109);
            this.TextBoxRegScheme.Name = "TextBoxRegScheme";
            this.TextBoxRegScheme.Size = new System.Drawing.Size(331, 20);
            this.TextBoxRegScheme.TabIndex = 23;
            // 
            // TextBoxRegIdentifier
            // 
            this.TextBoxRegIdentifier.Location = new System.Drawing.Point(100, 139);
            this.TextBoxRegIdentifier.Name = "TextBoxRegIdentifier";
            this.TextBoxRegIdentifier.Size = new System.Drawing.Size(331, 20);
            this.TextBoxRegIdentifier.TabIndex = 24;
            // 
            // TextBoxSupportedDocument
            // 
            this.TextBoxSupportedDocument.Location = new System.Drawing.Point(100, 172);
            this.TextBoxSupportedDocument.Name = "TextBoxSupportedDocument";
            this.TextBoxSupportedDocument.Size = new System.Drawing.Size(331, 20);
            this.TextBoxSupportedDocument.TabIndex = 25;
            // 
            // TextBoxLegalEntityId
            // 
            this.TextBoxLegalEntityId.Location = new System.Drawing.Point(100, 208);
            this.TextBoxLegalEntityId.Name = "TextBoxLegalEntityId";
            this.TextBoxLegalEntityId.Size = new System.Drawing.Size(77, 20);
            this.TextBoxLegalEntityId.TabIndex = 26;
            this.TextBoxLegalEntityId.Text = ".";
            this.TextBoxLegalEntityId.TextChanged += new System.EventHandler(this.TextBoxLegalEntityId_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "LegalEntityId";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "SupportedDoc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "RegId";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "RegScheme";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "LandCode";
            // 
            // TabNotifications
            // 
            this.TabNotifications.Controls.Add(this.TextBoxSender);
            this.TabNotifications.Controls.Add(this.TextBoxReceiver);
            this.TabNotifications.Controls.Add(this.RadioButtonGetSent);
            this.TabNotifications.Controls.Add(this.RadioButtonGetReceived);
            this.TabNotifications.Controls.Add(this.ButtonNotifications);
            this.TabNotifications.Location = new System.Drawing.Point(4, 22);
            this.TabNotifications.Name = "TabNotifications";
            this.TabNotifications.Padding = new System.Windows.Forms.Padding(3);
            this.TabNotifications.Size = new System.Drawing.Size(576, 359);
            this.TabNotifications.TabIndex = 1;
            this.TabNotifications.Text = "Meldingen";
            this.TabNotifications.UseVisualStyleBackColor = true;
            // 
            // TextBoxSender
            // 
            this.TextBoxSender.Location = new System.Drawing.Point(223, 55);
            this.TextBoxSender.Name = "TextBoxSender";
            this.TextBoxSender.ReadOnly = true;
            this.TextBoxSender.Size = new System.Drawing.Size(165, 20);
            this.TextBoxSender.TabIndex = 9;
            // 
            // TextBoxReceiver
            // 
            this.TextBoxReceiver.Location = new System.Drawing.Point(223, 24);
            this.TextBoxReceiver.Name = "TextBoxReceiver";
            this.TextBoxReceiver.ReadOnly = true;
            this.TextBoxReceiver.Size = new System.Drawing.Size(165, 20);
            this.TextBoxReceiver.TabIndex = 8;
            // 
            // RadioButtonGetSent
            // 
            this.RadioButtonGetSent.AutoSize = true;
            this.RadioButtonGetSent.Location = new System.Drawing.Point(141, 58);
            this.RadioButtonGetSent.Name = "RadioButtonGetSent";
            this.RadioButtonGetSent.Size = new System.Drawing.Size(76, 17);
            this.RadioButtonGetSent.TabIndex = 7;
            this.RadioButtonGetSent.TabStop = true;
            this.RadioButtonGetSent.Text = "Verzonden";
            this.RadioButtonGetSent.UseVisualStyleBackColor = true;
            this.RadioButtonGetSent.CheckedChanged += new System.EventHandler(this.RadioButtonGetSent_CheckedChanged);
            // 
            // RadioButtonGetReceived
            // 
            this.RadioButtonGetReceived.AutoSize = true;
            this.RadioButtonGetReceived.Location = new System.Drawing.Point(141, 24);
            this.RadioButtonGetReceived.Name = "RadioButtonGetReceived";
            this.RadioButtonGetReceived.Size = new System.Drawing.Size(78, 17);
            this.RadioButtonGetReceived.TabIndex = 6;
            this.RadioButtonGetReceived.TabStop = true;
            this.RadioButtonGetReceived.Text = "Ontvangen";
            this.RadioButtonGetReceived.UseVisualStyleBackColor = true;
            this.RadioButtonGetReceived.CheckedChanged += new System.EventHandler(this.RadioButtonGetReceived_CheckedChanged);
            // 
            // ButtonNotifications
            // 
            this.ButtonNotifications.Location = new System.Drawing.Point(15, 21);
            this.ButtonNotifications.Name = "ButtonNotifications";
            this.ButtonNotifications.Size = new System.Drawing.Size(102, 54);
            this.ButtonNotifications.TabIndex = 5;
            this.ButtonNotifications.Text = "Meldingen Ophalen";
            this.ButtonNotifications.UseVisualStyleBackColor = true;
            this.ButtonNotifications.Click += new System.EventHandler(this.ButtonNotifications_Click);
            // 
            // TabResponse
            // 
            this.TabResponse.Controls.Add(this.RichTextBoxResponses);
            this.TabResponse.Location = new System.Drawing.Point(4, 22);
            this.TabResponse.Name = "TabResponse";
            this.TabResponse.Size = new System.Drawing.Size(576, 359);
            this.TabResponse.TabIndex = 4;
            this.TabResponse.Text = "Reacties/Antwoorden";
            this.TabResponse.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxResponses
            // 
            this.RichTextBoxResponses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxResponses.Location = new System.Drawing.Point(0, 0);
            this.RichTextBoxResponses.Name = "RichTextBoxResponses";
            this.RichTextBoxResponses.Size = new System.Drawing.Size(576, 359);
            this.RichTextBoxResponses.TabIndex = 1;
            this.RichTextBoxResponses.Text = "";
            // 
            // TabSendDocument
            // 
            this.TabSendDocument.AllowDrop = true;
            this.TabSendDocument.Controls.Add(this.ListBoxDocumentsToSend);
            this.TabSendDocument.Controls.Add(this.LabelFile);
            this.TabSendDocument.Controls.Add(this.ButtonSendUblDocument);
            this.TabSendDocument.Controls.Add(this.ButtonCheckFile);
            this.TabSendDocument.Location = new System.Drawing.Point(4, 22);
            this.TabSendDocument.Name = "TabSendDocument";
            this.TabSendDocument.Size = new System.Drawing.Size(576, 359);
            this.TabSendDocument.TabIndex = 2;
            this.TabSendDocument.Text = "Document Verzenden";
            this.TabSendDocument.UseVisualStyleBackColor = true;
            // 
            // ListBoxDocumentsToSend
            // 
            this.ListBoxDocumentsToSend.FormattingEnabled = true;
            this.ListBoxDocumentsToSend.Location = new System.Drawing.Point(8, 43);
            this.ListBoxDocumentsToSend.Name = "ListBoxDocumentsToSend";
            this.ListBoxDocumentsToSend.Size = new System.Drawing.Size(560, 173);
            this.ListBoxDocumentsToSend.TabIndex = 6;
            this.ListBoxDocumentsToSend.SelectedIndexChanged += new System.EventHandler(this.ListBoxDocumentsToSend_SelectedIndexChanged);
            // 
            // LabelFile
            // 
            this.LabelFile.AutoSize = true;
            this.LabelFile.Location = new System.Drawing.Point(8, 16);
            this.LabelFile.Name = "LabelFile";
            this.LabelFile.Size = new System.Drawing.Size(0, 13);
            this.LabelFile.TabIndex = 5;
            // 
            // ButtonSendUblDocument
            // 
            this.ButtonSendUblDocument.Enabled = false;
            this.ButtonSendUblDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonSendUblDocument.Location = new System.Drawing.Point(332, 227);
            this.ButtonSendUblDocument.Name = "ButtonSendUblDocument";
            this.ButtonSendUblDocument.Size = new System.Drawing.Size(236, 46);
            this.ButtonSendUblDocument.TabIndex = 4;
            this.ButtonSendUblDocument.Text = "UBL Document Verzenden";
            this.ButtonSendUblDocument.UseVisualStyleBackColor = true;
            this.ButtonSendUblDocument.Click += new System.EventHandler(this.ButtonSendUblDocument_Click);
            // 
            // ButtonCheckFile
            // 
            this.ButtonCheckFile.Location = new System.Drawing.Point(8, 227);
            this.ButtonCheckFile.Name = "ButtonCheckFile";
            this.ButtonCheckFile.Size = new System.Drawing.Size(111, 46);
            this.ButtonCheckFile.TabIndex = 3;
            this.ButtonCheckFile.Text = "Bestand Controleren";
            this.ButtonCheckFile.UseVisualStyleBackColor = true;
            this.ButtonCheckFile.Visible = false;
            this.ButtonCheckFile.Click += new System.EventHandler(this.ButtonCheckFile_Click);
            // 
            // TabReceiveDocument
            // 
            this.TabReceiveDocument.Controls.Add(this.ButtonGetUBLDocument);
            this.TabReceiveDocument.Controls.Add(this.TextBoxTransmissionId);
            this.TabReceiveDocument.Controls.Add(this.LabelTransmissionId);
            this.TabReceiveDocument.Location = new System.Drawing.Point(4, 22);
            this.TabReceiveDocument.Name = "TabReceiveDocument";
            this.TabReceiveDocument.Size = new System.Drawing.Size(576, 359);
            this.TabReceiveDocument.TabIndex = 3;
            this.TabReceiveDocument.Text = "Document ontvangen";
            this.TabReceiveDocument.UseVisualStyleBackColor = true;
            // 
            // ButtonGetUBLDocument
            // 
            this.ButtonGetUBLDocument.Location = new System.Drawing.Point(369, 20);
            this.ButtonGetUBLDocument.Name = "ButtonGetUBLDocument";
            this.ButtonGetUBLDocument.Size = new System.Drawing.Size(119, 47);
            this.ButtonGetUBLDocument.TabIndex = 5;
            this.ButtonGetUBLDocument.Text = "Document Ophalen";
            this.ButtonGetUBLDocument.UseVisualStyleBackColor = true;
            this.ButtonGetUBLDocument.Click += new System.EventHandler(this.ButtonGetUBLDocument_Click);
            // 
            // TextBoxTransmissionId
            // 
            this.TextBoxTransmissionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxTransmissionId.Location = new System.Drawing.Point(21, 36);
            this.TextBoxTransmissionId.Name = "TextBoxTransmissionId";
            this.TextBoxTransmissionId.Size = new System.Drawing.Size(333, 26);
            this.TextBoxTransmissionId.TabIndex = 4;
            // 
            // LabelTransmissionId
            // 
            this.LabelTransmissionId.AutoSize = true;
            this.LabelTransmissionId.Location = new System.Drawing.Point(18, 20);
            this.LabelTransmissionId.Name = "LabelTransmissionId";
            this.LabelTransmissionId.Size = new System.Drawing.Size(74, 13);
            this.LabelTransmissionId.TabIndex = 3;
            this.LabelTransmissionId.Text = "Transmissie Id";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(12, 272);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 1;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 363);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(584, 22);
            this.StatusStrip.TabIndex = 10;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            this.ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            this.ToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.ToolStripStatusLabel.Text = "Ready";
            // 
            // FormPeppolClientActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(584, 385);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.TabControlVariousActions);
            this.Controls.Add(this.ButtonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FormPeppolClientActions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormPeppolClientActions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPeppolClientActions_FormClosing);
            this.TabControlVariousActions.ResumeLayout(false);
            this.TabMonitor.ResumeLayout(false);
            this.TabMonitor.PerformLayout();
            this.TabActions.ResumeLayout(false);
            this.TabActions.PerformLayout();
            this.TabNotifications.ResumeLayout(false);
            this.TabNotifications.PerformLayout();
            this.TabResponse.ResumeLayout(false);
            this.TabSendDocument.ResumeLayout(false);
            this.TabSendDocument.PerformLayout();
            this.TabReceiveDocument.ResumeLayout(false);
            this.TabReceiveDocument.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabControlVariousActions;
        private System.Windows.Forms.TabPage TabActions;
        private System.Windows.Forms.TabPage TabNotifications;
        private System.Windows.Forms.TabPage TabSendDocument;
        private System.Windows.Forms.TabPage TabReceiveDocument;
        private System.Windows.Forms.TabPage TabResponse;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.RichTextBox RichTextBoxResponses;
        private System.Windows.Forms.Button ButtonGetPeppolRegistrations;
        private System.Windows.Forms.TextBox TextBoxCountryCode;
        private System.Windows.Forms.TextBox TextBoxRegScheme;
        private System.Windows.Forms.TextBox TextBoxRegIdentifier;
        private System.Windows.Forms.TextBox TextBoxSupportedDocument;
        private System.Windows.Forms.TextBox TextBoxLegalEntityId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxSender;
        private System.Windows.Forms.TextBox TextBoxReceiver;
        private System.Windows.Forms.RadioButton RadioButtonGetSent;
        private System.Windows.Forms.RadioButton RadioButtonGetReceived;
        private System.Windows.Forms.Button ButtonNotifications;
        private System.Windows.Forms.Label LabelFile;
        private System.Windows.Forms.Button ButtonSendUblDocument;
        private System.Windows.Forms.Button ButtonCheckFile;
        private System.Windows.Forms.Button ButtonGetUBLDocument;
        private System.Windows.Forms.TextBox TextBoxTransmissionId;
        private System.Windows.Forms.Label LabelTransmissionId;
        private System.Windows.Forms.Button ButtonShowSharedGlobals;
        private System.Windows.Forms.ListBox ListBoxDocumentsToSend;
        private System.Windows.Forms.Button ButtonZipToCloud;
        private System.Windows.Forms.TabPage TabMonitor;
        private System.Windows.Forms.ListBox ListBoxMonitorReceived;
        private System.Windows.Forms.ListBox ListBoxMonitorSent;
        private System.Windows.Forms.Label LabelMonitorReceived;
        private System.Windows.Forms.Label LabelMonitorSent;
        private System.Windows.Forms.Button ButtonTimer;
        private System.Windows.Forms.Button ButtonCheckConnectivity;
    }
}