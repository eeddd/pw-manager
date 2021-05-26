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

    public partial class frmAccounts : Form
    {
        public frmAccounts()
        {
            InitializeComponent();
        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;

            using (var db = new DBConnect())
            {
                var groups = db.GetAccountGroups().ToArray();
                cboGroup.Items.AddRange(groups);

                var list = db.GetAccounts();
                LoadAccounts(list);
            }
        }

        private void cboGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGroup.SelectedItem == null) return;
            AccountGroup group = cboGroup.SelectedItem as AccountGroup;
            
            using (var db = new DBConnect())
            {
                var list = db.GetAccountsBy(group);
                LoadAccounts(list);
            }
        }

        public void LoadAccounts(List<Account> accounts)
        {
            var binding = new BindingList<Account>(accounts);
            dataGridView1.DataSource = binding;
        }
        public void ReloadAccounts()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            using (var db = new DBConnect())
            {
                var list = db.GetAccounts();
                LoadAccounts(list);
                cboGroup.SelectedIndex = -1;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
                
        private void btnNewAccount_Click(object sender, EventArgs e)
        {
            frmNewAccount frm = new frmNewAccount();

            AccountGroup[] groups = new AccountGroup[cboGroup.Items.Count];
            cboGroup.Items.CopyTo(groups, 0);
            frm.cboGroup.Items.AddRange(groups);

            frm.ShowDialog();

            ReloadAccounts();
        }
    }
}
