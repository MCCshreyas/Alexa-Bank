using System;
using System.Xml;
using Twilio;
using Twilio.Exceptions;
namespace BankApplicationLibrary
{
    public class OtpService
    {
        private string AccountSid { get; set; }
        private string AuthToken { get; set; }
        public string TwilioPhoneNumber { get; private set; }

        public OtpService()
        {
            ReadAccountInfoFromXml();
            InitializeTwilioService();
        }

        private bool InitializeTwilioService()
        {
            try
            {
                TwilioClient.Init(AccountSid, AuthToken);
                return true;
            }
            catch (TwilioException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        
        private void ReadAccountInfoFromXml()
        {
            var sid = string.Empty;
            var token = string.Empty;
            var phonenumber = string.Empty;
            try
            {
                using (var reader = XmlReader.Create(@"C:\Users\Shreyas.SHREYAS\Documents\Visual Studio 2010\Projects\WPFBankApplication\BankApplicationLibrary\TwilioAccount.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "AccountSid")
                            {
                                this.AccountSid = reader.ReadString();
                            }
                            else if (reader.Name == "AuthToken")
                            {
                                this.AuthToken = reader.ReadString();
                            }
                            else if (reader.Name == "PhoneNumber")
                            {
                                TwilioPhoneNumber = reader.ReadString();
                            }
                            else
                            {
                                throw new XmlException();
                            }
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
