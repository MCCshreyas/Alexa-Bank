using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ExtraTools;
using java.lang;
using java.sql;
using Microsoft.Win32;
using Connection = com.mysql.jdbc.Connection;
using Exception = System.Exception;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for EditPersonalDetails.xaml
    /// </summary>
    public partial class EditPersonalDetails
    {
        private OpenFileDialog _fileDialog;
        public string Accc;
        public string ImageFilePath = "";

        public EditPersonalDetails(string accountNumber)
        {
            InitializeComponent();
            Accc = accountNumber;
            GetDetails();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new AccountSettings(Accc).Show();
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
                ImageFilePath = _fileDialog.FileName;
                var img = new ImageSourceConverter();
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
                SaveDataToDatabase();
        }

        private void SaveDataToDatabase()
        {
            var fullName = TextBoxName.Text;

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement(
                    "update info set Name = ? ,  Address = ? , phone_number = ? , Email = ? , ImagePath = ? , BirthDate = ? where account_number = ? ");
                ps.setString(1, fullName);
                ps.setString(2, textBox_address.Text);
                ps.setString(3, textBox_phonenumber.Text);
                ps.setString(4, textBox_email.Text);
                ps.setString(5, ImageFilePath);
                ps.setString(6, myDatePicker.Text);
                ps.setString(7, Accc);
                ps.executeUpdate();
                c.close();
                DialogBox.Show("Sucess", "Changes done sucessfully", "OK");
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong." + exception.Message, "OK");
            }
        }

        public void GetDetails()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select * from info where account_number = ?");
                ps.setString(1, Accc);
                var rs = ps.executeQuery();
                var img = new ImageSourceConverter();

                while (rs.next())
                {
                    TextBoxName.Text = rs.getString("Name");
                    textBox_address.Text = rs.getString("Address");
                    textBox_email.Text = rs.getString("Email");
                    textBox_phonenumber.Text = rs.getString("phone_number");
                    myDatePicker.Text = rs.getString("BirthDate");
                    AccountHolderImage.SetValue(Image.SourceProperty, img.ConvertFromString(rs.getString("ImagePath")));
                }
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong." + exception.Message, "OK");
            }
        }


        private bool DoDataValidation()
        {
            //saving phone number leangh into a length variable
            var length = textBox_phonenumber.Text.Length;

            //checking email validation which returns bool value 
            var isEmailValid = textBox_email.Text.Contains("@");
            var isEmailValid2 = textBox_email.Text.Contains(".com");

            // Is there any textbox is empty or not. If there then it will fire error message
            if (TextBoxName.Text == "" || textBox_email.Text == "" || textBox_address.Text == "" ||
                textBox_phonenumber.Text == "" || AccountHolderImage.Source == null || myDatePicker.Text == "")
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
            if (isEmailValid && isEmailValid2) return true;
            DialogBox.Show("Error", "Please check your email ID", "OK");
            return false;
        }
    }
}