using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExtraTools;
using java.lang;
using java.sql;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Connection = com.mysql.jdbc.Connection;
using Exception = System.Exception;


namespace WPFBankApplication
{
    public partial class NewAccountRegistration
    {
        private OpenFileDialog _fileDialog;
        public int accc;
        public string imageFilePath = "";
        
        public NewAccountRegistration()
        {
            InitializeComponent();
            RadioButtonMale.IsChecked = true;
            myDatePicker.Focusable = false;
            MyComboBox.SelectedIndex = 2;
        }

        /// <summary>
        /// Following method will check which radiobutton is checked Male or Female. And will return result accordingly.
        /// </summary>
        public string GetGenderInfo()
        {
            if ((bool) RadioButtonMale.IsChecked)
            {
                return "Male";
            }
            else if ((bool) RadioButtonFemale.IsChecked)
                return "Female";
            else
                return null;
        }


     
        /// <summary>
        /// Following code will execute when upload image button gets clicked
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
                imageFilePath = _fileDialog.FileName;
                ImageSourceConverter img = new ImageSourceConverter();
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
<<<<<<< HEAD
            var length = textBox_phonenumber.Text.Length;

            //checking email validation which returns bool value 
            var isEmailValid = textBox_email.Text.Contains("@");
            var isEmailValid2 = textBox_email.Text.Contains(".com");


            // Is there any textbox is empty or not. If there then it will fire error message
            if (textBox_firstname.Text == "" || textBox_lastname.Text == "" || textBox_email.Text == "" ||
                textBox_pass.Password == "" || textBox_address.Text == "" || textBox_phonenumber.Text == "" ||
                AccountHolderImage.Source == null || myDatePicker.Text == "")
=======
            int length = textBox_phonenumber.Text.Length;  

            //checking email validation which returns bool value 
            bool isEmailValid = textBox_email.Text.Contains("@");
            bool isEmailValid2 = textBox_email.Text.Contains(".com");


            // Is there any textbox is empty or not. If there then it will fire error message
            if (textBox_firstname.Text == "" || textBox_lastname.Text == "" || textBox_email.Text == "" || textBox_pass.Password == "" || textBox_address.Text == "" || textBox_phonenumber.Text == "" || AccountHolderImage.Source == null || myDatePicker.Text == "")
>>>>>>> parent of 388efd3... Refactored code
            {
                DialogBox.Show("Error", "Please enter all field", "OK");
                return false;
            }

            // we are checking phone number here
            if (length < 10 || length == 0 || length > 10)
            {
                DialogBox.Show("Error","Please check your phone number","OK");
                return false;
            }
                
            // we are checking email validation here
            if (!isEmailValid || !isEmailValid2)
            {
                DialogBox.Show("Error","Please check your email ID","OK");
                return false;
            }

            return true;
        }

        
        /// <summary>
        /// following code will check is MobileNotification checkbox is check or not by user. Please refer design for it. 
        //  if it is check it will return Yes otherwise No
        /// </summary>
        public string EnableMobileNotifications()
        {
<<<<<<< HEAD
            var isMobileNotifications = CheckBoxMobileNotification.IsChecked != null && (bool) CheckBoxMobileNotification.IsChecked;
=======
            bool isMobileNotifications = (bool)CheckBoxMobileNotification.IsChecked;
>>>>>>> parent of 388efd3... Refactored code

            return isMobileNotifications ? "Yes" : "No";
        }

       
        /// <summary>
        /// Following is a JAVA code which saves data to database 
        /// </summary>
        private void SaveDataToDatabase()
        {
<<<<<<< HEAD
            var fullName = textBox_firstname.Text + " " + textBox_lastname.Text;
=======
            string fullName = textBox_firstname.Text + " " + textBox_lastname.Text;

>>>>>>> parent of 388efd3... Refactored code

            // following code will generate random number which will be user account number 
            accc = new Random().Next(1000000000);

            
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
<<<<<<< HEAD
                ps.setString(6, Accc.ToString());
                ps.setString(7, ImageFilePath);
                ps.setString(8, GetGenderInfo());
                ps.setString(9, EnableMobileNotifications());
                ps.setString(10, myDatePicker.Text);
=======
                ps.setString(6, accc.ToString());
                ps.setString(7,imageFilePath);
                ps.setString(8, GetGenderInfo());
                ps.setString(9,EnableMobileNotifications());
                ps.setString(10,myDatePicker.Text);
>>>>>>> parent of 388efd3... Refactored code
                ps.executeUpdate();
                c.close();
                DialogBox.Show("Sucess","Account created sucessfully","OK");

                DialogBox.Show("Sucess","Your account number is " + accc,"OK");
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong." + exception.Message,"OK");
            }
        }

        /// <summary>
        /// Following code will execute when save button gets clicked
        /// </summary>
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            bool internetStatus = IsInternetAvailable();  
            if (internetStatus)
            {
<<<<<<< HEAD
                if (!DoDataValidation()) return;
                SaveDataToDatabase();
                new OTPVerification(textBox_phonenumber.Text).ShowDialog();
=======
                if (DoDataValidation())
                {
                    SaveDataToDatabase();
                    new OTPVerification(textBox_phonenumber.Text).ShowDialog();
                }
>>>>>>> parent of 388efd3... Refactored code
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.sleep(1000);
                }).ContinueWith(t =>
                {
                    MainSnackbar.MessageQueue.Enqueue("Please check internet connectivity.");
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            
        }

        /// <summary>
        ///following method will check for internet connection if its there it will return true otherwise false
        /// </summary>
        
        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        private void MyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyComboBox.SelectedIndex == 0)
            {
                MaterialDesignThemes.Wpf.HintAssist.SetIsFloating(WorkDetailsTextBox, true);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(WorkDetailsTextBox, "Company name");
            }
            else if (MyComboBox.SelectedIndex == 1)
            {
                MaterialDesignThemes.Wpf.HintAssist.SetIsFloating(WorkDetailsTextBox, true);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(WorkDetailsTextBox, "Corporation name");
            }
            else if (MyComboBox.SelectedIndex == 2)
            {
                MaterialDesignThemes.Wpf.HintAssist.SetIsFloating(WorkDetailsTextBox, true);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(WorkDetailsTextBox, "College name");
            }
        }

        private void TextBox_phonenumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        
        /// <summary>
        /// following method will execute whenever the text in phone number textbox gets changed
        /// </summary>
        private void textBox_phonenumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.sleep(1000);
            }).ContinueWith(t =>
            {
                MainSnackbar.MessageQueue.Enqueue("Make sure you give correct phone number to recive OTP to activate your acoount");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }



        /// <summary>
        /// back button code which is at top left corner
        /// </summary>
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            new LoggedIn().Show();
            this.Hide();
        }
       
        /// <summary>
        /// following method will clear the textbox values
        /// </summary>
        private void Btn_clear_details_OnClick(object sender, RoutedEventArgs e)
        {
            textBox_phonenumber.Text = textBox_address.Text = textBox_email.Text = textBox_firstname.Text =
                textBox_lastname.Text = textBox_pass.Password = myDatePicker.Text = "";

            ImageSourceConverter img = new ImageSourceConverter();
            AccountHolderImage.Source = null;
        }
    }
}
