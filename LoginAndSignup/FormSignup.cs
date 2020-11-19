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
        private ErrorProvider errorProvider;

        public FormSignup()
        {
            InitializeComponent();

            errorProvider = new ErrorProvider();

            buttonCancel.Click += ButtonCancel_Click;
            buttonOK.Click += ButtonOK_Click;
            textBoxUsername.Leave += TextBoxUsername_Leave;
            textBoxPassword.Leave += TextBoxPassword_Leave;
            textBoxName.Leave += TextBoxName_Leave;
            textBoxEmail.Leave += TextBoxEmail_Leave;
            textBoxDateOfBirth.Leave += TextBoxDateOfBirth_Leave;
        }

        private void TextBoxDateOfBirth_Leave(object sender, EventArgs e)
        {
            DateTime dateTime;

            if (!DateTime.TryParseExact(textBoxDateOfBirth.Text, "dd/mm/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dateTime))
            {
                errorProvider.SetError(textBoxDateOfBirth, "Ngày không hợp lệ!");
                buttonOK.Enabled = false;
            }
            else
            {
                errorProvider.SetError(textBoxDateOfBirth, "");
                buttonOK.Enabled = true;
            }
        }

        private void TextBoxEmail_Leave(object sender, EventArgs e)
        {
            ValidationInput validation = new ValidationInput();
            string pattern = @"^(([a-z]+(([a-z]|[0-9]|[.])+)?)[@][a-z]+[.][a-z]+)$";

            if (!validation.IsValid(textBoxEmail.Text, pattern))
            {
                errorProvider.SetError(textBoxEmail, "Email không hợp lệ. Vui lòng nhập lại.");
                buttonOK.Enabled = false;
            }
            else
            {
                errorProvider.SetError(textBoxEmail, "");
                buttonOK.Enabled = true;
            }
        }

        private void TextBoxName_Leave(object sender, EventArgs e)
        {
            ValidationInput validation = new ValidationInput();
            string pattern = @"^[a-zA-Z\s]+[0-9]*?$";

            if (!validation.IsValid(textBoxName.Text, pattern))
            {
                errorProvider.SetError(textBoxName, "Name không được bắt đầu bằng ký số");
                buttonOK.Enabled = false;
            }
            else
            {
                errorProvider.SetError(textBoxName, "");
                buttonOK.Enabled = true;
            }
        }

        private void TextBoxPassword_Leave(object sender, EventArgs e)
        {
            ValidationInput validation = new ValidationInput();
            string pattern = @"^[\S]{3,}$";

            if (!validation.IsValid(textBoxPassword.Text, pattern))
            {
                errorProvider.SetError(textBoxPassword, "Password phải có ít nhất 3 ký tự và không có khoảng trắng");
                buttonOK.Enabled = false;
            }
            else
            {
                errorProvider.SetError(textBoxPassword, "");
                buttonOK.Enabled = true;
            }
        }

        private void TextBoxUsername_Leave(object sender, EventArgs e)
        {
            ValidationInput validation = new ValidationInput();
            string pattern = @"^([a-z]+(([a-z]|[0-9])+)?)$";

            if (!validation.IsValid(textBoxUsername.Text, pattern))
            {
                errorProvider.SetError(textBoxUsername, "Username phải duy nhất, ko có ký tự HOA, bắt đầu bởi ký tự a-z," +
                    " không khoảng trắng và các ký tự đặc biệt(chỉ gồm a - z, 0 - 9)");
                buttonOK.Enabled = false;
            }
            else
            {
                errorProvider.SetError(textBoxUsername, "");
                buttonOK.Enabled = true;
            }
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
