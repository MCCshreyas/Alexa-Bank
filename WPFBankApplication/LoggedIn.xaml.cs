using MaterialDesignThemes.Wpf;

namespace WPFBankApplication
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;
    using ExtraTools;

    using java.lang;
    using java.sql;
    using Exception = System.Exception;
    using Process = System.Diagnostics.Process;

    /// <summary>
    /// Interaction logic for LoggedIn.xaml
    /// </summary>
    public partial class LoggedIn
    {
        public LoggedIn()
        {
            InitializeComponent();
            ShowWelcomeSnakbar();
        }

        private LoadingWindow lw = new LoadingWindow();

        private void ShowWelcomeSnakbar() => MainSnackbar.MessageQueue.Enqueue("Welcome to Alexa Bank Of India");


        private bool DoValidation()
        {
            if (TextBoxAcc.Text.Equals(string.Empty) && PasswordBox.Password.Equals(string.Empty))
            {
                DialogHostMessage.Text = "Please fill all the fields";
                DialogHostCaption.Text = "Error";
                DialogHostRightButton.Content = "OK";
                MyDialogHost.IsOpen = true;
                return false;
            }
            return true;
        }


        private void DoLogIn()
        {
            var pass = string.Empty;
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection = DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                var ps = connection.prepareStatement("select Password from info where account_number = ?");
                ps.setString(1, TextBoxAcc.Text);
                var rs = ps.executeQuery();
                
                while (rs.next())
                {
                    pass = rs.getString("Password");
                }

            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong - " + exception.Message, "OK");
            }

            // Getting password from user password box
            var userPassword = PasswordBox.Password;

            // checking the input password and the password saved in database
            if (userPassword == pass)
            {
                
                DialogHostMessage.Text = "Sign in sucessfully";
                DialogHostCaption.Text = "Sucess";
                DialogHostRightButton.Content = "OK";
                MyDialogHost.IsOpen = true;
                new Welcome(TextBoxAcc.Text).Show();
                Hide();
            }
            else
            {
                DialogBox.Show("Error", "Please enter valid account number and password", "OK");
            }
        }


        private void Button1Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DoValidation())
                {
                    DoLogIn();
                }
            }
            catch (Exception error)
            {
                DialogBox.Show("Exception", "Something went wrong " + error.Message, "OK");
            }
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            new NewAccountRegistration().Show();
            Hide();
        }

        private void TextBox_acc_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ForgetAccountNumberHyperLink_OnClick(object sender, RoutedEventArgs e)
        {
            new ForgetAccountNumber().Show();
            Hide();
        }

        private void ForgetpasswordHyperLink1_OnClick(object sender, RoutedEventArgs e)
        {
            new ForgetPassword().Show();
            Hide();
        }

        private void ButtonGitHub_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MCCshreyas");
        }

        private void ButtonEmail_OnClick(object sender, RoutedEventArgs e)
        {
            DialogBox.Show("Contact", "Email - shreyasjejurkar123@live.com", "OK");
        }

        private void ButtonTwitter_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://twitter.com/MCCshreyas");
        }

        private void LoggedIn_OnLoaded(object sender, RoutedEventArgs e)
        {
          /*  while(!Resource.IsInternetAvailable())
            {
                DialogBox.Show("Warning", "Please check internet connectivity", "OK");
            }
            */
        }

        private void DialogHostRightButton_OnClick(object sender, RoutedEventArgs e)
        {
            MyDialogHost.IsOpen = false;
        }
    }
}
