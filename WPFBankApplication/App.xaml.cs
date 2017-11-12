using System.Windows;
using System.Windows.Media;
using ExtraTools;
using java.lang;
using java.sql;
using Twilio;
using Twilio.Exceptions;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Following method will initialize twilio account
        /// </summary>
        public static void InitializeTwilioAccount()
        {
            try
            {
                const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
                const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";

                TwilioClient.Init(accountSid, authToken);
            }
            catch (TwilioException tw)
            {
                DialogBox.Show("ERROR", "Unable to initialize Twilio service " + tw.Message, "OK");
            }
        }
    }
}
