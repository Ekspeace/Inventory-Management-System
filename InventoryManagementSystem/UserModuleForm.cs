using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class UserModuleForm : Form
    {

        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        public UserModuleForm()
        {
            InitializeComponent();
        }

        private void pbCloseWindow_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(tbPassword.Text != tbConfirmPassword.Text)
                {
                    MessageBox.Show("Password does not match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(MessageBox.Show("Are you sure you want to save this user?", "Save User Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("INSERT INTO tbUser(username, fullname, password, phone) VALUES(@username, @fullname, @password, @phone)", connection);
                    command.Parameters.AddWithValue("@username", tbUserName.Text);
                    command.Parameters.AddWithValue("@fullname", tbFullName.Text);
                    command.Parameters.AddWithValue("@password", tbPassword.Text);
                    command.Parameters.AddWithValue("@phone", tbPhone.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("User has been successfully saved.");
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
            tbUserName.Clear();
            tbFullName.Clear();
            tbPassword.Clear();
            tbConfirmPassword.Clear();
            tbPhone.Clear();
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
                if (tbPassword.Text != tbConfirmPassword.Text)
                {
                    MessageBox.Show("Password does not match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to update this user?", "Update User Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("UPDATE tbUser  SET fullname =  @fullname, password =  @password, phone = @phone WHERE username LIKE '"+ tbUserName.Text + "'", connection);
                    command.Parameters.AddWithValue("@fullname", tbFullName.Text);
                    command.Parameters.AddWithValue("@password", tbPassword.Text);
                    command.Parameters.AddWithValue("@phone", tbPhone.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("User has been successfully updated.");
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
