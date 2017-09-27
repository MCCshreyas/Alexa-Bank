using System.Windows;
using ExtraTools;
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
                DialogBox.Show("Error", "Please enter all fields","OK");
                return false;
            }
            if (!IsEmailValid || !IsEmailValid2)
            {
                DialogBox.Show("Error", "Please enter valid email", "OK");
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
                DialogBox.Show("Sucess", "Your account number is " + GetAccountNumber(),"OK");
                Hide();
                new LoggedIn().Show();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new LoggedIn().Show();
            this.Hide();
        }
    }
}
