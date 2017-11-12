
namespace WPFBankApplication
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
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
            Task.Factory.StartNew(() =>
            {
                    Thread.sleep(1000);
                }).ContinueWith(
                t =>
                    {
                        MainSnackbar.MessageQueue.Enqueue("Welcome to Alexa Bank Of India");
                    },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool DoValidation()
        {
            if (textBox_acc.Text.Equals(string.Empty) && PasswordBox.Password.Equals(string.Empty))
            {
                DialogBox.Show("Error", "Please fill all the information and then proceed", "OK");
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
                var connection = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                var ps = connection.prepareStatement("select Password from info where account_number = ?");
                ps.setString(1, textBox_acc.Text);
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
                DialogBox.Show("Sucess", "Logged in sucessfully", "OK");
                new Welcome(textBox_acc.Text).Show();
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
    }
}
