using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPFBankApplication.Annotations;

namespace WPFBankApplication
{
    public class LogInViewModel
    {
        private LogIn myLogIn;

        public LogIn LogInProperty
        {
            get { return myLogIn; }
            set { myLogIn = value; }
        }
    }
}
