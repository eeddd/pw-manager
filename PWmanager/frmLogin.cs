using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PWmanager
{
    using Model;

    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtEmail.Text = "e.orgel@dkt.com.ph";
            txtPassword.Text = "admin";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var db = new DB.DBConnect();

            if (db.Login(txtEmail.Text, txtPassword.Text, out User user))
            {
                Hide();
                frmAccounts frm = new frmAccounts();
                frm.CurrentUser = user.Email;
                frm.ShowDialog();
                Show();
            }
            else
            {
                MessageBox.Show("Invalid User", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        
    }
}
