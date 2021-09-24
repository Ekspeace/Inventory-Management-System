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
    public partial class OrderModuleForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();

        }

        private void pbCloseWindow_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            command = new SqlCommand("SELECT customerid, customername FROM tbCustomer WHERE CONCAT(customerid, customername) LIKE '%" + tbCustomerSearch.Text + "%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            connection.Close();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            command = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(productid, productname, productprice, productdescription, productcategory) LIKE '%" + tbProductSearch.Text + "%'", connection);
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

        private void tbProductSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void tbCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbCustomerId.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            tbCustomerName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbProductId.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            tbProductName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString(); 
            tbProductPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void nudProductQuantity_ValueChanged(object sender, EventArgs e)
        {
            GetQty();
            if(Convert.ToInt16(nudProductQuantity.Value) > qty)
            {
                MessageBox.Show("InStock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudProductQuantity.Value = nudProductQuantity.Value - 1;
                return;
            }
            if (Convert.ToInt16(nudProductQuantity.Value) > 0)
            {
                if (tbProductPrice.Text != "")
                {
                    decimal total = Convert.ToDecimal(tbProductPrice.Text) * Convert.ToInt16(nudProductQuantity.Value);
                    tbOrderTotalAmount.Text = total.ToString();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbCustomerId.Text == "")
                {
                    MessageBox.Show("Please select a customer!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (tbProductId.Text == "")
                {
                    MessageBox.Show("Please select a product!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to save this order?", "Save Order Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("INSERT INTO tbOrder(orderdate, productid, customerid, quantity, price, total) VALUES(@orderdate, @productid, @customerid, @quantity, @price, @total)", connection);
                    command.Parameters.AddWithValue("@orderdate", dtpOrderDate.Value);
                    command.Parameters.AddWithValue("@productid", Convert.ToInt16(tbProductId.Text));
                    command.Parameters.AddWithValue("@customerid",Convert.ToInt16(tbCustomerId.Text));
                    command.Parameters.AddWithValue("@quantity", nudProductQuantity.Value);
                    command.Parameters.AddWithValue("@price", Convert.ToDecimal(tbProductPrice.Text));
                    command.Parameters.AddWithValue("@total", Convert.ToDecimal(tbOrderTotalAmount.Text));
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Order has been successfully saved.");

                    command = new SqlCommand("UPDATE tbProduct  SET productquantity = (productquantity - @productquantity) WHERE productid LIKE '" + tbProductId.Text + "'", connection);
                    command.Parameters.AddWithValue("@productquantity", nudProductQuantity.Value);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    Clear();
                    LoadProduct();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            tbCustomerId.Clear();
            tbCustomerName.Clear();

            tbProductId.Clear();
            tbProductName.Clear();
            tbProductPrice.Clear();
            tbOrderTotalAmount.Clear();
            nudProductQuantity.Value = 0;
            dtpOrderDate.Value = DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void GetQty()
        {
            command = new SqlCommand("SELECT productquantity FROM tbProduct WHERE productid ='" + tbProductId.Text + "'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                qty = Convert.ToInt16(dr[0].ToString());
            }
            dr.Close();
            connection.Close();
        }
    }
}
