using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class CategoryForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        public CategoryForm()
        {
            InitializeComponent();
            LoadCategory();
        }
        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            command = new SqlCommand("SELECT * FROM tbCategory", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void cbAddCategory_Click(object sender, EventArgs e)
        {
            CategoryModuleForm categoryModule = new CategoryModuleForm();
            categoryModule.btnSave.Enabled = true;
            categoryModule.btnUpdate.Enabled = false;
            categoryModule.ShowDialog();
            LoadCategory();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CategoryModuleForm categoryModule = new CategoryModuleForm();
                categoryModule.lCategoryIdValue.Text = dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                categoryModule.tbCategoryName.Text = dgvCategory.Rows[e.RowIndex].Cells[2].Value.ToString();

                categoryModule.btnSave.Enabled = false;
                categoryModule.btnUpdate.Enabled = true;
                categoryModule.lCategoryId.Visible = true;
                categoryModule.lCategoryIdValue.Visible = true;
                categoryModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you want to delete this category?", "Delete Category Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbCategory WHERE categoryid LIKE '" + dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }
            LoadCategory();
        }
    }
}
