using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

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

            ReloadAccountGroups();
            ReloadAccounts();
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
        public void ReloadAccountGroups()
        {
            cboGroup.Items.Clear();
            using (var db = new DBConnect())
            {
                var groups = db.GetAccountGroups().ToArray();
                cboGroup.Items.AddRange(groups);
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
            if (frm.DialogResult == DialogResult.OK)
            {
                ReloadAccountGroups();
                ReloadAccounts();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
                btnDelete.Visible = true;
            else
                btnDelete.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("Empty Rows", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            int selectedRowCount =
            dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            List<Account> accounts = new List<Account>();

            for (int i = 0; i < selectedRowCount; i++)
            {
                accounts.Add((Account)dataGridView1.SelectedRows[i].DataBoundItem);
            }
            
            using(var db = new DBConnect())
            {
                int rows = db.DeleteAccounts(accounts);
                if (rows > 0)
                {
                    ReloadAccounts();
                }
            }
        }
    }
}
