using System.Windows;
using ExtraTools;
using java.lang;
using java.sql;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    public class Operations
    {
        //please check method name of each. Method name is itself explanetory about what it does. 
        // It's JAVA code
        // all method are static so you dont need to create an object of this class to call them just Operations.<method_name> to call it.


        public static bool DoesSendMobileNotifications(string accountNumber)
        {
            var status = "";

            try
            {
                Class.forName("com.mysql.jdbc.Driver");

                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select MobileVerification from info where account_number = ?");
                ps.setString(1, accountNumber);
                var rs = ps.executeQuery();
                while (rs.next())
                    status = rs.getString("MobileVerification");

                return status == "Yes";
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
                return false;
            }
        }

        public static string GetAccountHolderMobileNumber(string accountNumber)
        {
            var phoneNum = "";

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select phone_number from info where account_number = ?");
                ps.setString(1, accountNumber);
                var rs = ps.executeQuery();
                while (rs.next())
                    phoneNum = rs.getString("phone_number");

                return phoneNum;
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
                return null;
            }
        }


        public static string GetCurrentBalance(string accountNumber)
        {
            var balance = "";
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select Balance from info where account_number = ?");
                ps.setString(1, accountNumber);
                var result = ps.executeQuery();
                while (result.next())
                    balance = result.getString("Balance");

                return balance;
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
                return "";
            }
        }


        public static string GetPassword(string accountNumber)
        {
            var pass = string.Empty;
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select Password from info where account_number = ?");
                ps.setString(1, accountNumber);
                var result = ps.executeQuery();
                while (result.next())
                    pass = result.getString("Password");

                return pass;
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return null;
            }
        }

        public static string GetAccountHolderName(string accountNumber)
        {
            var name = "";
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select Name from info where account_number = ?");
                ps.setString(1, accountNumber);
                var result = ps.executeQuery();
                while (result.next())
                    name = result.getString("Name");

                return name;
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return null;
            }
        }
    }
}