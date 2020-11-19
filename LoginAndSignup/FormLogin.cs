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
            SetupForm();

            AcceptButton = buttonLogin;
            splitContainer1.IsSplitterFixed = true;

            buttonSignup.Click += ButtonSignup_Click;
            buttonLogin.Click += ButtonLogin_Click;
        }

        private void SetupForm()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            using (var db = new QLBHDataContext())
            {
                var user = db.Users.SingleOrDefault(u => u.f_Username.Equals(textBoxUsername.Text));

                if (user != null)
                {
                    EncryptPassword encryptPw = new EncryptPassword();

                    if (encryptPw.IsPasswordValid(textBoxPassword.Text, user.f_Password))
                    {
                        MessageBox.Show("Logged in successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Login failed!");
                    }
                }
                else
                {
                    MessageBox.Show("Login failed!");
                }
            }
        }

        private void ButtonSignup_Click(object sender, EventArgs e)
        {
            FormSignup formSignup = new FormSignup();

            formSignup.ShowDialog();
        }
    }
}
