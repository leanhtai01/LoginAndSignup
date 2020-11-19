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
    public partial class FormSignup : Form
    {
        public FormSignup()
        {
            InitializeComponent();

            buttonCancel.Click += ButtonCancel_Click;
            buttonOK.Click += ButtonOK_Click;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            using (var db = new QLBHDataContext())
            {
                var newUser = GetUser();
                
                if (db.Users.FirstOrDefault(u => u.f_Username.Equals(newUser.f_Username)) == null)
                {
                    db.Users.InsertOnSubmit(newUser);
                    db.SubmitChanges();
                    MessageBox.Show("Signed up successfully!");
                }
                else
                {
                    MessageBox.Show("Username already exists! Please try other username!");
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Get user from UI
        /// </summary>
        /// <returns></returns>
        private User GetUser()
        {
            EncryptPassword encryptPw = new EncryptPassword();

            return new User
            {
                f_Username = textBoxUsername.Text,
                f_Password = encryptPw.GetSaltedPassword(textBoxPassword.Text),
                f_Name = textBoxName.Text,
                f_Email = textBoxEmail.Text,
                f_DOB = DateTime.ParseExact(textBoxDateOfBirth.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                f_Permission = 0
            };
        }
    }
}
