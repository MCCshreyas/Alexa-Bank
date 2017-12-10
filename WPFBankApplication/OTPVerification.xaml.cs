using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ExtraTools;
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
        }

        private void OtpVerification_OnLoaded(object sender, RoutedEventArgs e)
        {
            OtpOperations();
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
                from: new PhoneNumber(Resource.TWILIO_PHONENUMBER),
                body: "Thank you for creating account in Alexa Bank of India. Following is OTP for your account - " +
                      _otp
            );
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(_otp).Equals(TextBoxOTP.Text))
            {
                DialogBox.Show("Sucess", "Thank you for confirming your account.", "OK");
                Hide();
                new LoggedIn().Show();
            }
            else
            {
                MainSnackbar.MessageQueue.Enqueue("Sorry entered One Time Password is incorrect");
            }
        }

        private void TextBoxOTP_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
