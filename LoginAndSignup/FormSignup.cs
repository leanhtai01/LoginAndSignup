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
            SetupForm();

            errorProvider = new ErrorProvider();

            AcceptButton = buttonOK;
            splitContainer1.IsSplitterFixed = true;

            buttonCancel.Click += ButtonCancel_Click;
            buttonOK.Click += ButtonOK_Click;
            textBoxUsername.Validating += TextBoxUsername_Validating;
            textBoxUsername.Validated += TextBoxUsername_Validated;
            textBoxPassword.Validating += TextBoxPassword_Validating;
            textBoxPassword.Validated += TextBoxPassword_Validated;
            textBoxName.Validating += TextBoxName_Validating;
            textBoxName.Validated += TextBoxName_Validated;
            textBoxEmail.Validating += TextBoxEmail_Validating;
            textBoxEmail.Validated += TextBoxEmail_Validated;
            textBoxDateOfBirth.Validating += TextBoxDateOfBirth_Validating;
            textBoxDateOfBirth.Validated += TextBoxDateOfBirth_Validated;
        }

        private void SetupForm()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void TextBoxDateOfBirth_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(textBoxDateOfBirth, "");
        }

        private void TextBoxEmail_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(textBoxEmail, "");
        }

        private void TextBoxName_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(textBoxName, "");
        }

        private void TextBoxPassword_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(textBoxPassword, "");
        }

        private void TextBoxUsername_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(textBoxUsername, "");
        }

        private void TextBoxValidate(TextBox textBox, string pattern, string errorMessage, CancelEventArgs e)
        {
            ValidationInput validation = new ValidationInput();

            if (!validation.IsValid(textBox.Text, pattern))
            {
                e.Cancel = true;
                textBox.Select(0, textBox.Text.Length);
                errorProvider.SetError(textBox, errorMessage);
            }
        }

        private void TextBoxDateOfBirth_Validating(object sender, CancelEventArgs e)
        {
            DateTime dateTime;

            if (!DateTime.TryParseExact(textBoxDateOfBirth.Text, "dd/mm/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dateTime))
            {
                e.Cancel = true;
                textBoxDateOfBirth.Select(0, textBoxDateOfBirth.Text.Length);
                errorProvider.SetError(textBoxDateOfBirth, "Ngày không hợp lệ!");
            }
        }

        private void TextBoxEmail_Validating(object sender, CancelEventArgs e)
        {
            string pattern = @"^(([a-z]+(([a-z]|[0-9]|[.])+)?)[@][a-z]+[.][a-z]+)$";
            string errorMessage = "Email không hợp lệ. Vui lòng nhập lại.";

            TextBoxValidate(textBoxEmail, pattern, errorMessage, e);
        }

        private void TextBoxName_Validating(object sender, CancelEventArgs e)
        {
            string pattern = @"^[a-zA-Z\s]+[0-9]*?$";
            string errorMessage = "Name không được bắt đầu bằng ký số";

            TextBoxValidate(textBoxName, pattern, errorMessage, e);
        }

        private void TextBoxPassword_Validating(object sender, CancelEventArgs e)
        {
            string pattern = @"^[\S]{3,}$";
            string errorMessage = "Password phải có ít nhất 3 ký tự và không có khoảng trắng";

            TextBoxValidate(textBoxPassword, pattern, errorMessage, e);
        }

        private void TextBoxUsername_Validating(object sender, CancelEventArgs e)
        {
            string pattern = @"^([a-z]+(([a-z]|[0-9])+)?)$";
            string errorMessage = "Username phải duy nhất, ko có ký tự HOA, bắt đầu bởi ký tự a-z," +
                " không khoảng trắng và các ký tự đặc biệt(chỉ gồm a - z, 0 - 9)";

            TextBoxValidate(textBoxUsername, pattern, errorMessage, e);
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
