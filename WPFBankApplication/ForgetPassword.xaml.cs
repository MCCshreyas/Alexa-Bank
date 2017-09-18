using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExtraTools;
using java.lang;
using java.sql;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for ForgetPassword.xaml
    /// </summary>
    public partial class ForgetPassword : Window
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }


        public bool DoValidation()
        {
            if (TextBoxEmail.Text == "")
            {
                ErrorDialog.IsOpen = true;
                return false;
            }

            bool isEmailValid = TextBoxEmail.Text.Contains("@");
            bool isEmailValid2 = TextBoxEmail.Text.Contains(".com");

            if (!isEmailValid || !isEmailValid2)
            {
                ErrorDialog.IsOpen = true;
                return false;
            }

            return true;
        }


        public string GetDetails()
        {
            string phone = "";
            string pass = "";

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("select Password, phone_number from info where Email = ?");
                ps.setString(1, TextBoxEmail.Text);
                ResultSet result = ps.executeQuery();
                while (result.next())
                {
                    pass = result.getString("Password");
                    phone = result.getString("phone_number");
                }

                SendMobileNotification(pass,phone);

            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);

            }

            return string.Empty;
        }

        private void SendMobileNotification(string password , string senderPhoneNumber)
        {
            try
            {
                const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
                const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";
                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber("+91" + senderPhoneNumber);
                var message = MessageResource.Create
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
            if (DoValidation())
            {
                GetDetails();
                this.Hide();
                new LoggedIn().Show();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new LoggedIn().Show();

        }
    }
}
