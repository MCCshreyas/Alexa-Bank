using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ExtraTools;
using java.lang;
using java.sql;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Connection = com.mysql.jdbc.Connection;
using Exception = System.Exception;

namespace WPFBankApplication
{
    public partial class NewAccountRegistration
    {
        private OpenFileDialog _fileDialog;
        public int Accc;
        public string ImageFilePath = "";

        public NewAccountRegistration()
        {
            InitializeComponent();
            RadioButtonMale.IsChecked = true;
            MyDatePicker.Focusable = false;
            MyComboBox.SelectedIndex = 2;
        }

        /// <summary>
        ///     Following method will check which radiobutton is checked Male or Female. And will return result accordingly.
        /// </summary>
        public string GetGenderInfo()
        {
            if (RadioButtonMale.IsChecked != null && (bool) RadioButtonMale.IsChecked)
                return "Male";
            if (RadioButtonFemale.IsChecked != null && (bool) RadioButtonFemale.IsChecked)
                return "Female";
            return null;
        }


        /// <summary>
        ///     Following code will execute when upload image button gets clicked
        /// </summary>
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

        private bool DoDataValidation()
        {
            //saving phone number leangh into a length variable
            var length = TextBoxPhonenumber.Text.Length;

            //checking email validation which returns bool value 
            var isEmailValid = TextBoxEmail.Text.Contains("@");
            var isEmailValid2 = TextBoxEmail.Text.Contains(".com");


            // Is there any textbox is empty or not. If there then it will fire error message
            if (TextBoxFirstname.Text == "" || TextBoxLastname.Text == "" || TextBoxEmail.Text == "" ||
                TextBoxPass.Password == "" || TextBoxAddress.Text == "" || TextBoxPhonenumber.Text == "" ||
                AccountHolderImage.Source == null || MyDatePicker.Text == "")
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


        //  if it is check it will return Yes otherwise No
        /// <summary>
        ///     following code will check is MobileNotification checkbox is check or not by user. Please refer design for it.
        /// </summary>
        public string EnableMobileNotifications()
        {
            var isMobileNotifications = CheckBoxMobileNotification.IsChecked != null &&
                                        (bool) CheckBoxMobileNotification.IsChecked;

            return isMobileNotifications ? "Yes" : "No";
        }


        /// <summary>
        ///     Following is a JAVA code which saves data to database
        /// </summary>
        private void SaveDataToDatabase()
        {
            var fullName = TextBoxFirstname.Text + " " + TextBoxLastname.Text;

            // following code will generate random number which will be user account number 
            Accc = new Random().Next(1000000000);

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement(
                    "insert into info(Name,Address,phone_number,Email,Password,account_number,Balance,ImagePath,Gender,MobileVerification,BirthDate)values(?,?,?,?,?,?,'100',?,?,?,?)");
                ps.setString(1, fullName);
                ps.setString(2, TextBoxAddress.Text);
                ps.setString(3, TextBoxPhonenumber.Text);
                ps.setString(4, TextBoxEmail.Text);
                ps.setString(5, TextBoxPass.Password);
                ps.setString(6, Accc.ToString());
                ps.setString(7, ImageFilePath);
                ps.setString(8, GetGenderInfo());
                ps.setString(9, EnableMobileNotifications());
                ps.setString(10, MyDatePicker.Text);
                ps.executeUpdate();
                c.close();
                DialogBox.Show("Sucess", "Account created sucessfully", "OK");

                DialogBox.Show("Sucess", "Your account number is " + Accc, "OK");
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong." + exception.Message, "OK");
            }
        }

        /// <summary>
        ///     Following code will execute whsen save button gets clicked
        /// </summary>
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            var internetStatus = IsInternetAvailable();
            if (internetStatus)
            {
                if (!DoDataValidation()) return;
                SaveDataToDatabase();
                new OtpVerification(TextBoxPhonenumber.Text).ShowDialog();
            }
            else
            {
                Task.Factory.StartNew(() => { Thread.sleep(1000); })
                    .ContinueWith(t => { MainSnackbar.MessageQueue.Enqueue("Please check internet connectivity."); },
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        /// <summary>
        ///     following method will check for internet connection if its there it will return true otherwise false
        /// </summary>
        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        private void MyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MyComboBox.SelectedIndex)
            {
                case 0:
                    HintAssist.SetIsFloating(WorkDetailsTextBox, true);
                    HintAssist.SetHint(WorkDetailsTextBox, "Company name");
                    break;
                case 1:
                    HintAssist.SetIsFloating(WorkDetailsTextBox, true);
                    HintAssist.SetHint(WorkDetailsTextBox, "Corporation name");
                    break;
                case 2:
                    HintAssist.SetIsFloating(WorkDetailsTextBox, true);
                    HintAssist.SetHint(WorkDetailsTextBox, "College name");
                    break;
            }
        }

        private void TextBox_phonenumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        /// <summary>
        ///     following method will execute whenever the text in phone number textbox gets changed
        /// </summary>
        private void textBox_phonenumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.Factory.StartNew(() => { Thread.sleep(1000); })
                .ContinueWith(
                    t =>
                    {
                        MainSnackbar.MessageQueue.Enqueue(
                            "Make sure you give correct phone number to recive OTP to activate your acoount");
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        /// <summary>
        ///     back button code which is at top left corner
        /// </summary>
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            new LoggedIn().Show();
            Hide();
        }

        /// <summary>
        ///     following method will clear the textbox values
        /// </summary>
        private void Btn_clear_details_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxPhonenumber.Text = TextBoxAddress.Text = TextBoxEmail.Text = TextBoxFirstname.Text =
                TextBoxLastname.Text = TextBoxPass.Password = MyDatePicker.Text = "";

            var img = new ImageSourceConverter();
            AccountHolderImage.Source = null;
        }
    }
}