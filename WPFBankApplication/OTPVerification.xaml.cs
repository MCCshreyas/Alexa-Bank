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
        private readonly string _userPhoneNumber;
        private int _otp;

        public OtpVerification(string phoneNumber)
        {
            InitializeComponent();
            _userPhoneNumber = phoneNumber;
            Task.Factory.StartNew(() => { Thread.Sleep(1000); }).ContinueWith(t => { OtpOperations(); },
                TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void OtpOperations()
        {
            //following code will generate OTP and will save it in OTP variable 

            var r = new Random();
            _otp = r.Next(10000);

            App.InitializeTwilioAccount();

            var to = new PhoneNumber("+91" + _userPhoneNumber);
            MessageResource.Create
            (
                to,
                @from: new PhoneNumber("+16674018291"),
                body: "Thank you for creating account in Alexa Bank of India. Following is OTP for your account - " +
                      _otp
            );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(_otp).Equals(TextBoxOTP.Text))
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
