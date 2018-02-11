using java.lang;
using java.sql;
using ExtraTools;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Exception = System.Exception;
using Process = System.Diagnostics.Process;
using BankApplicationLibrary;

namespace WPFBankApplication
{
    public partial class LoggedIn
    {
        public LoggedIn()
        {
            InitializeComponent();
            ShowWelcomeSnakbar();
        }

        private void ShowWelcomeSnakbar() => MainSnackbar.MessageQueue.Enqueue("Welcome to Alexa Bank Of India");

        private bool DoValidation()
        {
            if (TextBoxAcc.Text.Equals(string.Empty) && PasswordBox.Text.Equals(string.Empty))
            {
                DialogBox.Show("ERROR", "Please fill fields", "OK");
                return false;
            }
            return true;
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DoValidation())
                {
                    if (AuthenticateLogIn())
                    {
                        new Welcome(TextBoxAcc.Text).Show();
                        Hide();
                    }

                    var login = new Customer();
                }
            }
            catch (Exception error)
            {
                DialogBox.Show("Exception", "Something went wrong " + error.Message, "OK");
            }
        }

        private bool AuthenticateLogIn()
        {
            string databasePassword = string.Empty;
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection = DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                var ps = connection.prepareStatement("select Password from info where account_number = ?");
                ps.setString(1, TextBoxAcc.Text);
                var rs = ps.executeQuery();

                while (rs.next())
                {
                    databasePassword = rs.getString("Password");
                }

            }
            catch (SQLException exception)
            {
                throw new Exception("Something went wrong " + exception);
            }

            if (PasswordBox.Text.Equals(databasePassword))
            {
                return true;
            }

            return false;
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

        #region ForgetModulesClickEvent

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
        #endregion

        #region FloatingButtonControls

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

        #endregion

        private void LoggedIn_OnLoaded(object sender, RoutedEventArgs e)
        {
            /*  while(!Resource.IsInternetAvailable())
              {
                  DialogBox.Show("Warning", "Please check internet connectivity", "OK");
              }
              */
        }
    }
}
