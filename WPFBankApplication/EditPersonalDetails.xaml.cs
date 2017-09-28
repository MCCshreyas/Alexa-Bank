using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExtraTools;
using java.lang;
using java.sql;
using Microsoft.Win32;
using Connection = com.mysql.jdbc.Connection;
using Exception = System.Exception;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for EditPersonalDetails.xaml
    /// </summary>
    public partial class EditPersonalDetails : Window
    {
        private OpenFileDialog _fileDialog;
        public string accc;
        public string imageFilePath = "";

        public EditPersonalDetails(string accountNumber)
        {
            InitializeComponent();
            accc = accountNumber;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new AccountSettings(accc).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _fileDialog = new OpenFileDialog
                {
                    Filter = "Image files | *.jpg"
                };

                _fileDialog.ShowDialog();
                imageFilePath = _fileDialog.FileName;
                ImageSourceConverter img = new ImageSourceConverter();
                AccountHolderImage.SetValue(Image.SourceProperty, img.ConvertFromString(_fileDialog.FileName));
            }
            catch (Exception exception)
            {
                DialogBox.Show("Error", "Something went wrong. " + exception.Message);
            }
        }

        private void TextBox_phonenumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DoDataValidation())
            {
                SaveDataToDatabase();
               
            }
            
        }

        private void SaveDataToDatabase()
        {
            string fullName = textBox_firstname.Text + " " + textBox_lastname.Text;
            /*
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("insert into info(Name,Address,phone_number,Email,Password,account_number,Balance,ImagePath,Gender,MobileVerification,BirthDate)values(?,?,?,?,?,?,'100',?,?,?,?)");
                ps.setString(1, fullName);
                ps.setString(2, textBox_address.Text);
                ps.setString(3, textBox_phonenumber.Text);
                ps.setString(4, textBox_email.Text);
                ps.setString(5, textBox_pass.Password);
                ps.setString(6, accc.ToString());
                ps.setString(7, imageFilePath);
                ps.setString(8, GetGenderInfo());
                ps.setString(9, EnableMobileNotifications());
                ps.setString(10, myDatePicker.Text);
                ps.executeUpdate();
                c.close();
                DialogBox.Show("Sucess", "Account created sucessfully", "OK");

                DialogBox.Show("Sucess", "Your account number is " + accc, "OK");
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong." + exception.Message, "OK");
            }
            */
        }

        private bool DoDataValidation()
        {
            //saving phone number leangh into a length variable
            int length = textBox_phonenumber.Text.Length;

            //checking email validation which returns bool value 
            bool isEmailValid = textBox_email.Text.Contains("@");
            bool isEmailValid2 = textBox_email.Text.Contains(".com");


            // Is there any textbox is empty or not. If there then it will fire error message
            if (textBox_firstname.Text == "" || textBox_lastname.Text == "" || textBox_email.Text == "" || textBox_address.Text == "" || textBox_phonenumber.Text == "" || AccountHolderImage.Source == null || myDatePicker.Text == "")
            {
                DialogBox.Show("Error", "Please enter all field", "OK");
                return false;
            }

            // we are checking phone number here
            if (length < 10 || length == 0 || length > 10)
            {
                DialogBox.Show("Error", "Please check your phone number", "OK");
                return false;
            }

            // we are checking email validation here
            if (!isEmailValid || !isEmailValid2)
            {
                DialogBox.Show("Error", "Please check your email ID", "OK");
                return false;
            }

            return true;
        }
    }
}
