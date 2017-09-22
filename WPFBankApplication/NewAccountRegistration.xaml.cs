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
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Connection = com.mysql.jdbc.Connection;
using Exception = System.Exception;


namespace WPFBankApplication
{
    public partial class NewAccountRegistration
    {
        private OpenFileDialog _fileDialog;
        private int accc;
        private string imageFilePath = string.Empty;
        
        public NewAccountRegistration()
        {
            InitializeComponent();
            RadioButtonMale.IsChecked = true;
            myDatePicker.Focusable = false;
            MyComboBox.SelectedIndex = 2;
        }


        //following code will check is user is Male or Female as per the radiobuttons in design
        //if Male it will return "Male" otherwise "Female" if user wont select either of them it will return empty string see line no 65 of the file

        public string GetGenderInfo()
        {
            if ((bool) RadioButtonMale.IsChecked)
            {
                return "Male";
            }
            else if ((bool) RadioButtonFemale.IsChecked)
                return "Female";
            else
                return string.Empty;
        }


        //following code will execute when upload image button gets clicked
        //Please refer design


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
                MessageBox.Show("Something went wrong. " + exception.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        
        private bool DoDataValidation()
        {
            int length = textBox_phonenumber.Text.Length;   //saving phone number leangh into a length variable

            //checking email validation which returns bool value 
            bool isEmailValid = textBox_email.Text.Contains("@");
            bool isEmailValid2 = textBox_email.Text.Contains(".com");


            if (textBox_firstname.Text == "" || textBox_lastname.Text == "" || textBox_email.Text == "" || textBox_pass.Password == "" || textBox_address.Text == "" || textBox_phonenumber.Text == "" || AccountHolderImage.Source == null || myDatePicker.Text == "")
            {
               // MessageBox.Show("Please fill all the information before proceding", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                FieldErrorDialog.IsOpen = true;
                return false;
            }

            // we are checking phone number here
            if (length < 10 || length == 0 || length > 10)
            {
                MessageBox.Show("Please check your phone number","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
                return false;
            }
                
            // we are checking email validation here
            if (!isEmailValid || !isEmailValid2)
            {
                MessageBox.Show("Please check your email ID","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
                return false;
            }

            return true;
        }


        //following code will check is MobileNotification checkbox is check or not by user. Please refer design
        // if it is check it will return Yes otherwise No

        public string EnableMobileNotifications()
        {
            bool isMobileNotifications = (bool)CheckBoxMobileNotification.IsChecked;

            return isMobileNotifications ? "Yes" : "No";
        }

        //following method will save the data to database;

        private void SaveDataToDatabase()
        {
            //creating a user full name and saving it to variable fullName;

            string fullName = textBox_firstname.Text + " " + textBox_lastname.Text;

            //following code will generate a random value and same value will be account number of the user. value < 1000000000

            Random r = new Random();
            accc = r.Next(1000000000);

            //following code will save data to database.
            //NOTE - Follwing code is in JAVA :) 

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
                ps.setString(7,imageFilePath);
                ps.setString(8, GetGenderInfo());
                ps.setString(9,EnableMobileNotifications());
                ps.setString(10,myDatePicker.Text);
                ps.executeUpdate();
                c.close();
                MessageBox.Show("Account created sucessfully","Sucess",MessageBoxButton.OK,MessageBoxImage.Information);

                MessageBox.Show("Your account number is " + accc, "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SQLException exception)
            {
                MessageBox.Show("Something went wrong." + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {


            bool internetStatus = IsInternetAvailable();  //checking internet connection for generation of OTP
            if (internetStatus)
            {
                if (DoDataValidation())
                {
                    SaveDataToDatabase();
                    this.Hide();
                    DialogBox.Show("Loading", "Please wait", "OK");
                    new OTPVerification(textBox_phonenumber.Text).ShowDialog();
                }
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


        //following method will check for internet connection if its there it will return true otherwise false

        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

      
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            new LoggedIn().Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new LoggedIn().Show();
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


        // following method will always execute whenever user changes the mobile number in 
        //phone number text box

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
    }
}
