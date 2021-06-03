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

        private AccountGroup GetAccountGroup()
        {
            if (cboGroup.SelectedItem == null)
                return new AccountGroup
                {
                    ID = 0,
                    Name = cboGroup.Text,
                    Description = "",
                    CreatedAt = DateTime.Now
                };
            return (AccountGroup)cboGroup.SelectedItem;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var group = GetAccountGroup();

            var account = new Account
            {
                ID = 0,
                GroupID = group.ID,
                AddedBy = CurrentUser.Get().ID,
                Name = txtName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Notes = txtNotes.Text,
                CreatedAt = DateTime.Now
            };

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
