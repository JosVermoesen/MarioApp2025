namespace MarioApp2025.MarioMenu.Admin
{
    partial class FormDataGridJsonPopUp
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
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabGridView = new System.Windows.Forms.TabPage();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.TabJsonView = new System.Windows.Forms.TabPage();
            this.RichTextBox = new System.Windows.Forms.RichTextBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.TabControl.SuspendLayout();
            this.TabGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.TabJsonView.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabGridView);
            this.TabControl.Controls.Add(this.TabJsonView);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(559, 352);
            this.TabControl.TabIndex = 0;
            // 
            // TabGridView
            // 
            this.TabGridView.Controls.Add(this.DataGridView);
            this.TabGridView.Location = new System.Drawing.Point(4, 22);
            this.TabGridView.Name = "TabGridView";
            this.TabGridView.Padding = new System.Windows.Forms.Padding(3);
            this.TabGridView.Size = new System.Drawing.Size(551, 326);
            this.TabGridView.TabIndex = 0;
            this.TabGridView.Text = "Tabel";
            this.TabGridView.UseVisualStyleBackColor = true;
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToDeleteRows = false;
            this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(3, 3);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.ReadOnly = true;
            this.DataGridView.Size = new System.Drawing.Size(545, 320);
            this.DataGridView.TabIndex = 0;
            this.DataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
            // 
            // TabJsonView
            // 
            this.TabJsonView.Controls.Add(this.RichTextBox);
            this.TabJsonView.Location = new System.Drawing.Point(4, 22);
            this.TabJsonView.Name = "TabJsonView";
            this.TabJsonView.Padding = new System.Windows.Forms.Padding(3);
            this.TabJsonView.Size = new System.Drawing.Size(551, 326);
            this.TabJsonView.TabIndex = 1;
            this.TabJsonView.Text = "Json";
            this.TabJsonView.UseVisualStyleBackColor = true;
            // 
            // RichTextBox
            // 
            this.RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox.Location = new System.Drawing.Point(3, 3);
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.ReadOnly = true;
            this.RichTextBox.Size = new System.Drawing.Size(545, 320);
            this.RichTextBox.TabIndex = 0;
            this.RichTextBox.Text = "";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(12, 319);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 1;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormDataGridJsonPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(559, 352);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.ButtonClose);
            this.Name = "FormDataGridJsonPopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormDataGridJsonPopUp";
            this.TabControl.ResumeLayout(false);
            this.TabGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.TabJsonView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TabGridView;
        private System.Windows.Forms.TabPage TabJsonView;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private System.Windows.Forms.DataGridView DataGridView;
    }
}