using System.Windows;
using System.Windows.Controls;
using java.lang;
using java.sql;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for EditAccountHolderName.xaml
    /// </summary>
    public partial class EditAccountHolderName : Page
    {
        private string _accNum = string.Empty;

        public EditAccountHolderName(string accountNumber)
        {
            InitializeComponent();
            _accNum = accountNumber;
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

        private void Button_Click(object sender, RoutedEventArgs e)
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
                Connection c = (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("update info set Name = ? where account_number = ?");
                ps.setString(1, name);
                ps.setString(2, _accNum);
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
