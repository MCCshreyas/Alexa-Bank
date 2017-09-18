using System.Windows;
using java.lang;
using java.sql;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for ForgetAccountNumber.xaml
    /// </summary>
    public partial class ForgetAccountNumber : Window
    {
        public ForgetAccountNumber()
        {
            InitializeComponent();
        }


        public bool DoValidation()
        {
            bool IsEmailValid = TextBoxEmailAddresss.Text.Contains("@");
            bool IsEmailValid2 = TextBoxEmailAddresss.Text.Contains(".com");


            if (TextBoxEmailAddresss.Text == "" || TextBoxPassword.Password == "")
            {
                ErrorDialog.IsOpen = true;
                return false;
            }
            else if (!IsEmailValid || !IsEmailValid2)
            {
                EmailErrorDialog.IsOpen = true;
                return false;
            }

            return true;

        }


        public string GetAccountNumber()
        {
            string accountNumber = "";
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("select account_number from info where Email = ? and Password = ?");
                ps.setString(1, TextBoxEmailAddresss.Text);
                ps.setString(2, TextBoxPassword.Password);
                ResultSet result = ps.executeQuery();
                while (result.next())
                {
                    accountNumber = result.getString("account_number");
                }

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
            if (DoValidation())
            {
                MessageBox.Show("Your account number is " + GetAccountNumber(),"Sucess",MessageBoxButton.OK,MessageBoxImage.Information);
                Hide();
                new LoggedIn().Show();
            }
        }
    }
}
