
namespace WPFBankApplication
{
    using System.Windows;

    using java.lang;
    using java.sql;

    using Connection = com.mysql.jdbc.Connection;

    /// <summary>
    /// Interaction logic for EditAccountHolderName.xaml
    /// </summary>
    public partial class EditAccountHolderName
    {
        private readonly string accNum;

        public EditAccountHolderName(string accountNumber)
        {
            InitializeComponent();
            accNum = accountNumber;
        }

        private bool DoValidation()
        {
            if (FirstNameTextBox.Text == string.Empty || LastNameTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please fill all the fields and then proceed","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
                return false;
            }
            return true;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string fullName = FirstNameTextBox.Text + " " + LastNameTextBox.Text;
            if (DoValidation())
            {
                SaveNewName(fullName);
            }
        }

        private void SaveNewName(string name)
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                java.sql.PreparedStatement ps = c.prepareStatement("update info set Name = ? where account_number = ?");
                ps.setString(1, name);
                ps.setString(2, accNum);
                ps.executeUpdate();
                c.close();
                MessageBox.Show("Changes saved sucessfully");
                new LoggedIn().Show();
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Stop);
            }

        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(new AccountSettings(accNum));
            parentWindow.Show();
        }
    }
}
