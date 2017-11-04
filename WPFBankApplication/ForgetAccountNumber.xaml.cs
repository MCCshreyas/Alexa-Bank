using System.Windows;
using ExtraTools;
using java.lang;
using java.sql;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for ForgetAccountNumber.xaml
    /// </summary>
    public partial class ForgetAccountNumber
    {
        public ForgetAccountNumber()
        {
            InitializeComponent();
        }


        public bool DoValidation()
        {
            var isEmailValid = TextBoxEmailAddresss.Text.Contains("@");
            var isEmailValid2 = TextBoxEmailAddresss.Text.Contains(".com");


            if (TextBoxEmailAddresss.Text == "" || TextBoxPassword.Password == "")
            {
                DialogBox.Show("Error", "Please enter all fields", "OK");
                return false;
            }
            if (isEmailValid && isEmailValid2) return true;
            DialogBox.Show("Error", "Please enter valid email", "OK");
            return false;
        }


        public string GetAccountNumber()
        {
            var accountNumber = "";
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                var ps = c.prepareStatement("select account_number from info where Email = ? and Password = ?");
                ps.setString(1, TextBoxEmailAddresss.Text);
                ps.setString(2, TextBoxPassword.Password);
                var result = ps.executeQuery();
                while (result.next())
                    accountNumber = result.getString("account_number");

                return accountNumber;
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            return "";
        }

        private void ButtonSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DoValidation()) return;
            DialogBox.Show("Sucess", "Your account number is " + GetAccountNumber(), "OK");
            Hide();
            new LoggedIn().Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new LoggedIn().Show();
            Hide();
        }
    }
}
