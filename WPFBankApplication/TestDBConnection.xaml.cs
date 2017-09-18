using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using com.mysql;
using com.mysql.jdbc;
using java.lang;
using CsharpMySql;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for TestDBConnection.xaml
    /// </summary>
    public partial class TestDbConnection : Window
    {
        public TestDbConnection()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Class.forName("com.mysql.jdbc.Driver");
            Connection c;

            PreparedStatement ps = c.prepareStatement()




        }
    }
}
