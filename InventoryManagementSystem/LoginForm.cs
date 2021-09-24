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
    public partial class LoginForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tshel\OneDrive\Documents\dbMs.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void pbShowPassword_Click(object sender, EventArgs e)
        {
            if (tbPassword.UseSystemPasswordChar == false)
                tbPassword.UseSystemPasswordChar = true;
            else
                tbPassword.UseSystemPasswordChar = false;
        }

        private void lClearAllTextBox_Click(object sender, EventArgs e)
        {
            tbPassword.Clear();
            tbUserName.Clear();
            tbUserName.Focus();
        }

        private void pbCloseWindow_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                command = new SqlCommand("SELECT * FROM tbUser WHERE username=@username AND password=@password", connection);
                command.Parameters.AddWithValue("@username", tbUserName.Text);
                command.Parameters.AddWithValue("@password", tbPassword.Text);
                connection.Open();
                dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    MessageBox.Show("Welcome " + dr["fullname"].ToString(), "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm mainForm = new MainForm();
                    this.Hide();
                    mainForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid user name or password!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
