using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PWmanager
{
    public partial class frmAccounts : Form
    {
        public string CurrentUser { get; set; }


        public frmAccounts()
        {
            InitializeComponent();
        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            lblUser.Text = CurrentUser;
        }
    }
}
