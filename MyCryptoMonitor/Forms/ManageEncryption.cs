﻿using MyCryptoMonitor.Statics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyCryptoMonitor.Forms
{
    public partial class ManageEncryption : Form
    {
        #region Constructor
        public ManageEncryption()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void Setup()
        {
            cbEnableEncryption.Checked = UserConfigService.Encrypted;
            btnEncrypt.Text = UserConfigService.Encrypted ? "Decrypt" : "Encrypt";
            cbEnableEncryption.ForeColor = UserConfigService.Encrypted ? Color.Green : Color.Crimson;
            btnEncrypt.Enabled = true;
            txtPassword.Enabled = true;
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Events
        private void Encrypt_Load(object sender, EventArgs e)
        {
            Setup();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
                return;

            Cursor.Current = Cursors.WaitCursor;
            btnEncrypt.Enabled = false;
            txtPassword.Enabled = false;

            if (UserConfigService.Encrypted && EncryptionService.ValidatePassword(txtPassword.Text))
                EncryptionService.DecryptFiles();

            else if (!UserConfigService.Encrypted)
                EncryptionService.EncryptFiles(txtPassword.Text);

            Setup();
        }
        #endregion
    }
}
