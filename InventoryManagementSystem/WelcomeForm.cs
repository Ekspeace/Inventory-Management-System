using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
            timerWelcome.Start();
        }
        int startPoint = 0;
        private void timerWelcome_Tick(object sender, EventArgs e)
        {
            startPoint += 2;
            pbWelcome.Value = startPoint;
            if(pbWelcome.Value == 100)
            {
                pbWelcome.Value = 0;
                timerWelcome.Stop();
                LoginForm loginForm = new LoginForm();
                this.Hide();
                loginForm.ShowDialog();
            }
        }
    }
}
