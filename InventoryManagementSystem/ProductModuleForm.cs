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
    public partial class ProductModuleForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cbProductCategory.Items.Clear();
            command = new SqlCommand("SELECT categoryname FROM tbCategory", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                cbProductCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void pbCloseWindow_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Save Product Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("INSERT INTO tbProduct(productname, productquantity, productprice, productdescription, productcategory) VALUES(@productname, @productquantity, @productprice, @productdescription, @productcategory)", connection);
                    command.Parameters.AddWithValue("@productname", tbProductName.Text);
                    command.Parameters.AddWithValue("@productquantity", Convert.ToInt16(tbProductQuantity.Text));
                    command.Parameters.AddWithValue("@productprice", Convert.ToDecimal(tbProductPrice.Text));
                    command.Parameters.AddWithValue("@productdescription", tbProductDescription.Text);
                    command.Parameters.AddWithValue("@productcategory", cbProductCategory.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Product has been successfully saved.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Clear()
        {
            tbProductName.Clear();
            tbProductQuantity.Clear();
            tbProductPrice.Clear();
            tbProductDescription.Clear();
            cbProductCategory.Items.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", "Update product Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("UPDATE tbProduct SET productname =  @productname, productquantity =  @productquantity, productprice = @productprice, productdescription = @productdescription, productcategory = @productcategory WHERE productId LIKE '" + lProductIdValue.Text + "'", connection);
                    command.Parameters.AddWithValue("@productname", tbProductName.Text);
                    command.Parameters.AddWithValue("@productquantity", Convert.ToInt16(tbProductQuantity.Text));
                    command.Parameters.AddWithValue("@productprice", Convert.ToDecimal(tbProductPrice.Text));
                    command.Parameters.AddWithValue("@productdescription", tbProductDescription.Text);
                    command.Parameters.AddWithValue("@productcategory", cbProductCategory.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Product has been successfully updated.");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
