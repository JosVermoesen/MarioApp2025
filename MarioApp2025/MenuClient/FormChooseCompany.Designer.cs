namespace MarioApp2025.MarioMenu.Actions
{
    partial class FormChooseCompany
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
            this.ListBoxCompanies = new System.Windows.Forms.ListBox();
            this.LabelMimDataLocation = new System.Windows.Forms.Label();
            this.CheckBoxIsAdmin = new System.Windows.Forms.CheckBox();
            this.TextBoxIsAdminPassword = new System.Windows.Forms.TextBox();
            this.ButtonValidate = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.LabelUserGuid = new System.Windows.Forms.Label();
            this.TextBoxGuidToValidate = new System.Windows.Forms.TextBox();
            this.ButtonValidateGuid = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ListBoxCompanies
            // 
            this.ListBoxCompanies.FormattingEnabled = true;
            this.ListBoxCompanies.Location = new System.Drawing.Point(8, 36);
            this.ListBoxCompanies.Name = "ListBoxCompanies";
            this.ListBoxCompanies.Size = new System.Drawing.Size(472, 69);
            this.ListBoxCompanies.TabIndex = 0;
            this.ListBoxCompanies.Click += new System.EventHandler(this.ListBoxCompanies_Click);
            // 
            // LabelMimDataLocation
            // 
            this.LabelMimDataLocation.AutoSize = true;
            this.LabelMimDataLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelMimDataLocation.Location = new System.Drawing.Point(9, 9);
            this.LabelMimDataLocation.Name = "LabelMimDataLocation";
            this.LabelMimDataLocation.Size = new System.Drawing.Size(0, 16);
            this.LabelMimDataLocation.TabIndex = 1;
            // 
            // CheckBoxIsAdmin
            // 
            this.CheckBoxIsAdmin.AutoSize = true;
            this.CheckBoxIsAdmin.Location = new System.Drawing.Point(8, 158);
            this.CheckBoxIsAdmin.Name = "CheckBoxIsAdmin";
            this.CheckBoxIsAdmin.Size = new System.Drawing.Size(81, 17);
            this.CheckBoxIsAdmin.TabIndex = 2;
            this.CheckBoxIsAdmin.Text = "Beheerder?";
            this.CheckBoxIsAdmin.UseVisualStyleBackColor = true;
            this.CheckBoxIsAdmin.CheckedChanged += new System.EventHandler(this.CheckBoxIsAdmin_CheckedChanged);
            // 
            // TextBoxIsAdminPassword
            // 
            this.TextBoxIsAdminPassword.Location = new System.Drawing.Point(106, 156);
            this.TextBoxIsAdminPassword.Name = "TextBoxIsAdminPassword";
            this.TextBoxIsAdminPassword.PasswordChar = '*';
            this.TextBoxIsAdminPassword.Size = new System.Drawing.Size(374, 20);
            this.TextBoxIsAdminPassword.TabIndex = 3;
            this.TextBoxIsAdminPassword.Visible = false;
            // 
            // ButtonValidate
            // 
            this.ButtonValidate.Location = new System.Drawing.Point(106, 182);
            this.ButtonValidate.Name = "ButtonValidate";
            this.ButtonValidate.Size = new System.Drawing.Size(74, 23);
            this.ButtonValidate.TabIndex = 4;
            this.ButtonValidate.Text = "Valideren";
            this.ButtonValidate.UseVisualStyleBackColor = true;
            this.ButtonValidate.Visible = false;
            this.ButtonValidate.Click += new System.EventHandler(this.ButtonValidate_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(405, 182);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 5;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // LabelUserGuid
            // 
            this.LabelUserGuid.AutoSize = true;
            this.LabelUserGuid.Location = new System.Drawing.Point(8, 114);
            this.LabelUserGuid.Name = "LabelUserGuid";
            this.LabelUserGuid.Size = new System.Drawing.Size(142, 13);
            this.LabelUserGuid.TabIndex = 6;
            this.LabelUserGuid.Text = "Activeer uw Toegangsleutel:";
            this.LabelUserGuid.Visible = false;
            // 
            // TextBoxGuidToValidate
            // 
            this.TextBoxGuidToValidate.Location = new System.Drawing.Point(8, 130);
            this.TextBoxGuidToValidate.Name = "TextBoxGuidToValidate";
            this.TextBoxGuidToValidate.PasswordChar = '*';
            this.TextBoxGuidToValidate.Size = new System.Drawing.Size(390, 20);
            this.TextBoxGuidToValidate.TabIndex = 7;
            this.TextBoxGuidToValidate.Visible = false;
            // 
            // ButtonValidateGuid
            // 
            this.ButtonValidateGuid.Location = new System.Drawing.Point(404, 127);
            this.ButtonValidateGuid.Name = "ButtonValidateGuid";
            this.ButtonValidateGuid.Size = new System.Drawing.Size(74, 23);
            this.ButtonValidateGuid.TabIndex = 8;
            this.ButtonValidateGuid.Text = "Valideren";
            this.ButtonValidateGuid.UseVisualStyleBackColor = true;
            this.ButtonValidateGuid.Visible = false;
            this.ButtonValidateGuid.Click += new System.EventHandler(this.ButtonValidateGuid_Click);
            // 
            // FormChooseCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(494, 213);
            this.Controls.Add(this.ButtonValidateGuid);
            this.Controls.Add(this.TextBoxGuidToValidate);
            this.Controls.Add(this.LabelUserGuid);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonValidate);
            this.Controls.Add(this.TextBoxIsAdminPassword);
            this.Controls.Add(this.CheckBoxIsAdmin);
            this.Controls.Add(this.LabelMimDataLocation);
            this.Controls.Add(this.ListBoxCompanies);
            this.Name = "FormChooseCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormUserSettings";
            this.Load += new System.EventHandler(this.FormChooseCompany_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxCompanies;
        private System.Windows.Forms.Label LabelMimDataLocation;
        private System.Windows.Forms.CheckBox CheckBoxIsAdmin;
        private System.Windows.Forms.TextBox TextBoxIsAdminPassword;
        private System.Windows.Forms.Button ButtonValidate;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label LabelUserGuid;
        private System.Windows.Forms.TextBox TextBoxGuidToValidate;
        private System.Windows.Forms.Button ButtonValidateGuid;
    }
}