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
    public partial class OrderForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        public OrderForm()
        {
            InitializeComponent();
            LoadOrder();
        }
        public void LoadOrder()
        {
            decimal total = 0;
            int i = 0;
            dgvOrder.Rows.Clear();
            command = new SqlCommand("SELECT orderid, orderdate, O.productid, P.productname, O.customerid, C.customername, quantity, price, total FROM tbOrder AS O JOIN tbCustomer AS C ON O.customerid = C.customerid JOIN tbProduct AS P ON O.productid=P.productid WHERE CONCAT(orderid, orderdate, O.productid, P.productname, O.customerid, C.customername, quantity, price) LIKE '%"+tbOrderSearch.Text+"%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvOrder.Rows.Add(i, dr[0].ToString(), Convert.ToDateTime(dr[1].ToString()).ToString("yyyy/MM/dd"), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                total += Convert.ToDecimal(dr[8].ToString());
            }
            dr.Close();
            connection.Close();

            lQuantity.Text = i.ToString();
            lTotalAmount.Text = total.ToString();
        }

        private void cbAddOrder_Click(object sender, EventArgs e)
        {
            OrderModuleForm orderModule = new OrderModuleForm();
            orderModule.ShowDialog();
            LoadOrder();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrder.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you want to delete this order?", "Delete Order Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbOrder WHERE orderid LIKE '" + dgvOrder.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been successfully deleted!");

                    if (DateTime.Now > Convert.ToDateTime(dgvOrder.Rows[e.RowIndex].Cells[2].Value.ToString()))
                    {
                        command = new SqlCommand("UPDATE tbProduct  SET productquantity = (productquantity + @productquantity) WHERE productid LIKE '" + dgvOrder.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", connection);
                        command.Parameters.AddWithValue("@productquantity", dgvOrder.Rows[e.RowIndex].Cells[7].Value.ToString());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            LoadOrder();
        }

        private void tbOrderSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrder();
        }
    }
}
