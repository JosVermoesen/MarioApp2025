﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarioApp2025.MarioMenu.Admin
{
    public partial class FormAdminSettings : Form
    {
        private bool restartApp = false;
        public FormAdminSettings()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            if (restartApp)
            {
                Application.Restart();
            }
            else
                Close();
        }

        private void ButtonCreateGuid_Click(object sender, EventArgs e)
        {
            TextBoxGuid.Text = Guid.NewGuid().ToString();
        }

        private void FormAdminSettings_Load(object sender, EventArgs e)
        {
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
            restartApp  = true;
            ButtonClose.Text = "Sluiten en Herstarten";
        }

        private void RadioButtonProductionMode_Click(object sender, EventArgs e)
        {
            restartApp = true;
            ButtonClose.Text = "Sluiten en Herstarten";
        }
    }
}
