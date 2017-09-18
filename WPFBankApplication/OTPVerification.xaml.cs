using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ExtraTools;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for OTPVerification.xaml
    /// </summary>
    public partial class OTPVerification
    {
        public readonly string userPhoneNumber = "";
        private int OTP;
        public OTPVerification(string phoneNumber)
        {
            InitializeComponent();
            userPhoneNumber = phoneNumber;
            OTPOperations();
        }


        public void OTPOperations()
        {

            //following code will generate OTP and will save it in OTP variable 

            Random r = new Random();
            OTP = r.Next(10000);

            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";
            TwilioClient.Init(accountSid, authToken);
            var to = new PhoneNumber("+91" + userPhoneNumber);    
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: "Your OTP for Alexa account is " + OTP
            );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(OTP) == TextBoxOTP.Text)
            {
                DialogBox.Show("Sucess", "Thank you for confirming your account.","OK");
                new LoggedIn().Show();
                this.Hide();
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                }).ContinueWith(t =>
                {
                    //note you can use the message queue from any thread, but just for the demo here we 
                    //need to get the message queue from the snackbar, so need to be on the dispatcher
                    MainSnackbar.MessageQueue.Enqueue("Sorry entered One Time Password is incorrect");
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void TextBoxOTP_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
