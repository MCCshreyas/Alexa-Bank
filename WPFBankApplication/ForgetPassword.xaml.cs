using System.Threading.Tasks;
using System.Windows;
using ExtraTools;
using java.lang;
using java.sql;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for ForgetPassword.xaml
    /// </summary>
    public partial class ForgetPassword 
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }

        private bool DoValidation()
        {
            if (TextBoxEmail.Text.Equals(string.Empty))
            {
                DialogBox.Show("Error", "Email field is empty", "OK");
                return false;
            }

            var isEmailValid = TextBoxEmail.Text.Contains("@");
            var isEmailValid2 = TextBoxEmail.Text.Contains(".com");

            if (isEmailValid && isEmailValid2)
                return true;
            DialogBox.Show("Error", "Please enter valid email to proceed", "OK");
            return false;
        }


        //here we are getting Password and registered phone number from database by using email 

        private async Task GetDetailsAsync()
        {
            var phone = "";
            var pass = "";

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                var ps = c.prepareStatement("select Password, phone_number from info where Email = ?");
                ps.setString(1, TextBoxEmail.Text);
                var result = ps.executeQuery();
                while (result.next())
                {
                    pass = result.getString("Password");
                    phone = result.getString("phone_number");
                }

                SendMobileNotification(pass, phone);
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private static void SendMobileNotification(string password, string senderPhoneNumber)
        {
            try
            {
                App.InitializeTwilioAccount();
                var to = new PhoneNumber("+91" + senderPhoneNumber);
                MessageResource.Create
                (
                    to,
                    from: new PhoneNumber(Resource.TWILIO_PHONENUMBER),
                    body: "Your passoword is " + password
                );
            }
            catch (SQLException e)
            {
                MessageBox.Show("Something went wrong. " + e.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
                return;
            GetDetailsAsync().Wait();
            Hide();
            new LoggedIn().Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoggedIn().Show();
        }
    }
}
