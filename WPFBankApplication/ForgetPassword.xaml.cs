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


        public bool DoValidation()
        {
            if (TextBoxEmail.Text == "")
            {
                DialogBox.Show("Error", "Email field is empty", "OK");
                return false;
            }

            var isEmailValid = TextBoxEmail.Text.Contains("@");
            var isEmailValid2 = TextBoxEmail.Text.Contains(".com");

            if (isEmailValid && isEmailValid2) return true;
            DialogBox.Show("Error", "Please enter valid email to proceed", "OK");
            return false;
        }


        //here we are getting Password and registered phone number from database by using email 

        public string GetDetails()
        {
            var phone = "";
            var pass = "";

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                var ps = c.prepareStatement("select Password, phone_number from info where Email = ?");
                ps.setString(1, TextBoxEmail.Text);
                var result = ps.executeQuery();
                while (result.next())
                {
                    pass = result.getString("Password");
                    phone = result.getString("phone_number");
                }

                //sending message to user 

                SendMobileNotification(pass, phone);
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            return string.Empty;
        }

        public void SendMobileNotification(string password, string senderPhoneNumber)
        {
            try
            {
                const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
                const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";
                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber("+91" + senderPhoneNumber);
                MessageResource.Create
                (
                    to,
                    from: new PhoneNumber("+16674018291"),
                    body: "Your passoword is " + password
                );
            }
            catch (SQLException e)
            {
                MessageBox.Show("Something went wrong. " + e.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DoValidation()) return;
            GetDetails();
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