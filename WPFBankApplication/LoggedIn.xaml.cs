using ExtraTools;
using java.lang;
using java.sql;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Exception = System.Exception;
using Process = System.Diagnostics.Process;

namespace WPFBankApplication
{
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
            }).ContinueWith(t =>
            {

                MainSnackbar.MessageQueue.Enqueue("Welcome to Alexa Bank Of India");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public bool DoValidation()
        {
            if (textBox_acc.Text == string.Empty || PasswordBox.Password == string.Empty)
            {
                DialogBox.Show("Error", "Please fill all the information and then proceed", "OK");
                return false;
            }
            return true;
        }


        public void DoLogIn()
        {
            string pass = null;
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                PreparedStatement ps = c.prepareStatement("select Password from info where account_number = ?");
                ps.setString(1, textBox_acc.Text);
                ResultSet rs = ps.executeQuery();

                while (rs.next())
                {
                    pass = rs.getString("Password");
                }

            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong - " + exception.Message, "OK");
            }


            //Getting password from user password box

            string userPassword = PasswordBox.Password;

            //checking the input password and the password saved in database
            if (userPassword == pass)
            {
                DialogBox.Show("Sucess", "Logged in sucessfully", "OK");
                new Welcome(textBox_acc.Text).Show();
                this.Hide();
            }
            else
            {
                DialogBox.Show("Error", "Please enter valid account number and password", "OK");
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
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
            this.Hide();
        }

        private void TextBox_acc_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ForgetAccountNumberHyperLink_OnClick(object sender, RoutedEventArgs e)
        {
            new ForgetAccountNumber().Show();
            this.Hide();
        }

        private void ForgetpasswordHyperLink1_OnClick(object sender, RoutedEventArgs e)
        {
            new ForgetPassword().Show();
            this.Hide();
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
