using java.lang;
using java.sql;
using Exception = System.Exception;

namespace WPFBankApplication
{
    public class LogIn
    {
        private string acn;
        private string pass;

        public string AccountNumber
        {
            get { return acn; }
            set { acn = value; }
        }

        public string Password
        {
            get { return pass; }
            set
            {
                pass = value;
            }
        }


        
    }
}
