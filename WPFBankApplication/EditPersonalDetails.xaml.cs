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
    /// Interaction logic for EditPersonalDetails.xaml
    /// </summary>
    public partial class EditPersonalDetails
    {
        private OpenFileDialog fileDialog;
        private readonly string Accc;
        private string _imageFilePath = "";

        public EditPersonalDetails(string accountNumber)
        {
            InitializeComponent();
            Accc = accountNumber;
            GetDetails();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new AccountSettings(Accc).Show();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                fileDialog = new OpenFileDialog
                {
                    Filter = "Image files | *.jpg"
                };

                fileDialog.ShowDialog();
                _imageFilePath = fileDialog.FileName;
                var img = new ImageSourceConverter();
                AccountHolderImage.SetValue(Image.SourceProperty, img.ConvertFromString(fileDialog.FileName));
            }
            catch (Exception exception)
            {
                //MaterialDesignDialogBox.Show("Error", "Something went wrong. " + exception.Message);
            }
        }

        private void TextBox_phonenumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (DoDataValidation())
            {
                SaveDataToDatabase();
            }
            
        }

        private void SaveDataToDatabase()
        {
            string fullName = TextBoxName.Text;
            
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                PreparedStatement ps = c.prepareStatement("update info set Name = ? ,  Address = ? , phone_number = ? , Email = ? , ImagePath = ? , BirthDate = ? where account_number = ? ");
                ps.setString(1, fullName);
                ps.setString(2, textBox_address.Text);
                ps.setString(3, textBox_phonenumber.Text);
                ps.setString(4, textBox_email.Text);
                ps.setString(5, _imageFilePath);
                ps.setString(6, myDatePicker.Text);
                ps.setString(7, Accc);
                ps.executeUpdate();
                c.close();
                //MaterialDesignDialogBox.Show("Sucess", "Changes done sucessfully", "OK");
            }
            catch (SQLException exception)
            {
                //MaterialDesignDialogBox.Show("Error", "Something went wrong." + exception.Message, "OK");
            }
        }

        private void GetDetails()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                PreparedStatement ps = c.prepareStatement("select * from info where account_number = ?");
                ps.setString(1, Accc);
                ResultSet rs = ps.executeQuery();
                ImageSourceConverter img = new ImageSourceConverter();

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
                //MaterialDesignDialogBox.Show("Error", "Something went wrong." + exception.Message, "OK");
            }
        }

        
        private bool DoDataValidation()
        {
            const int PHONE_NUMBER_LENGTH = 10;

            //saving phone number leangh into a length variable
            int length = textBox_phonenumber.Text.Length;

            //checking email validation which returns bool value 
            bool isEmailValid = textBox_email.Text.Contains("@");
            bool isEmailValid2 = textBox_email.Text.Contains(".com");
            
            // Is there any textbox is empty or not. If there then it will fire error message
            if (TextBoxName.Text == "" || textBox_email.Text == "" || textBox_address.Text == "" || textBox_phonenumber.Text == "" || AccountHolderImage.Source == null || myDatePicker.Text == "")
            {
                //MaterialDesignDialogBox.Show("Error", "Please enter all field", "OK");
                return false;
            }

            // we are checking phone number here
            if (length < PHONE_NUMBER_LENGTH || length == 0 || length > PHONE_NUMBER_LENGTH)
            {
                //MaterialDesignDialogBox.Show("Error", "Please check your phone number", "OK");
                return false;
            }

            // we are checking email validation here
            if (isEmailValid && isEmailValid2) return true;
            //MaterialDesignDialogBox.Show("Error", "Please check your email ID", "OK");
            return false;
        }
    }
}
