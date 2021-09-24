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
    public partial class ProductForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            command = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(productid, productname, productprice, productdescription, productcategory) LIKE '%"+tbProductSearch.Text+"%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void cbAddProduct_Click(object sender, EventArgs e)
        {
            ProductModuleForm productModule = new ProductModuleForm();
            productModule.btnSave.Enabled = true;
            productModule.btnUpdate.Enabled = false;
            productModule.ShowDialog();
            LoadProduct();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();
                productModule.lProductIdValue.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.tbProductName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.tbProductQuantity.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.tbProductPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.tbProductDescription.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.cbProductCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                productModule.btnSave.Enabled = false;
                productModule.btnUpdate.Enabled = true;
            
                productModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you want to delete this product?", "Delete Product Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbProduct WHERE productid LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }
            LoadProduct();
        }

        private void tbProductSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
