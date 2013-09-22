using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Customer_Button_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new LogIn("Customer");            
            newWindow.ShowDialog();
        }

        private void Clerk_Button_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new LogIn("Clerk");
            newWindow.ShowDialog();
        }

        private void Chief_Button_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new LogIn("Chief");
            newWindow.ShowDialog();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new RegistrationScreen();
            newWindow.ShowDialog();
        }
    }
}
