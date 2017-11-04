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
    ///     Interaction logic for OTPVerification.xaml
    /// </summary>
    public partial class OtpVerification
    {
        private int _otp;
        public string UserPhoneNumber;

        public OtpVerification(string phoneNumber)
        {
            InitializeComponent();
            UserPhoneNumber = phoneNumber;
            Task.Factory.StartNew(() => { Thread.Sleep(1000); }).ContinueWith(t => { OtpOperations(); },
                TaskScheduler.FromCurrentSynchronizationContext());
        }


        public void OtpOperations()
        {
            //following code will generate OTP and will save it in OTP variable 

            var r = new Random();
            _otp = r.Next(10000);

            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";
            TwilioClient.Init(accountSid, authToken);
            var to = new PhoneNumber("+91" + UserPhoneNumber);
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: "Thank you for creating account in Alexa Bank of India. Following is OTP for your account - " +
                      _otp
            );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(_otp) == TextBoxOtp.Text)
            {
                DialogBox.Show("Sucess", "Thank you for confirming your account.", "OK");
                Hide();
                new LoggedIn().Show();
            }
            else
            {
                Task.Factory.StartNew(() => { Thread.Sleep(1000); })
                    .ContinueWith(
                        t => { MainSnackbar.MessageQueue.Enqueue("Sorry entered One Time Password is incorrect"); },
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void TextBoxOTP_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}