using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginAndSignup
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();

            buttonSignup.Click += ButtonSignup_Click;
        }

        private void ButtonSignup_Click(object sender, EventArgs e)
        {
            FormSignup formSignup = new FormSignup();

            formSignup.Show();
        }
    }
}
