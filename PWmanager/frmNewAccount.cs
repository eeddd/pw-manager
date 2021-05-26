using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PWmanager
{
    using Model;
    using DB;

    public partial class frmNewAccount : Form
    {
        public frmNewAccount()
        {
            InitializeComponent();
        }

        private void frmNewAccount_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AccountGroup grp = (AccountGroup)cboGroup.SelectedItem;
            if (cboGroup.SelectedItem == null)
                grp = new AccountGroup(0, "", "", DateTime.Now);

            var account = new Account(0, grp.ID,
                CurrentUser.Get().ID,
                txtName.Text,
                txtEmail.Text,
                txtPassword.Text,
                txtNotes.Text,
                DateTime.Now);
            try
            {
                using (var db = new DBConnect())
                {
                    int rows = db.InsertAccount(account);
                    if (rows > 0)
                    {
                        MessageBox.Show("New account saved");
                        Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Unable to save new account!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
