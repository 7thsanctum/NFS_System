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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        int attempts;
        string errortext = "";

        private string logintype;
        public LogIn(string type)
        {
            logintype = type;
            attempts = 0;
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool usercheck = false;
            bool passcheck = false;
            bool formfilled = true;
            
            if (attempts < 3)
            {
                formfilled = isFormFilled();
   
                if (formfilled)
                {
                    string username = textBoxUser.Text;
                    string password = passwordBox1.Password;
                    if (File.Exists(logintype + "/" + username + ".xml"))
                    {
                        XDocument user = XDocument.Load(logintype + "/" + username + ".xml");
                        
                        usercheck = true;
                        if (password == user.Element(logintype).Element("password").Value) passcheck = true;
                        else
                        {
                            if (logintype == "Customer") attempts = attempts + 1;
                            if (attempts > 2) pinReset(username);
                        }

                        if (usercheck && passcheck) newWindow(username); 
                        else errormessage.Text = "Sorry! Please enter existing username/password.";
                    }
                    else errormessage.Text = "File not found?!?!";
                }
                else errormessage.Text = errortext;
            }
            else
            {
                MessageBox.Show("Your PIN number has been reset for security purposes, please contact your local clerk to have this reset. Apologies for any inconvenience.");
                errormessage.Text = "This form has been locked for security reasons.";
                textBoxUser.Text = "";
                passwordBox1.Password = "";
                Login.IsEnabled = false;
                textBoxUser.IsEnabled = false;
                passwordBox1.IsEnabled = false;
            }
        }

        private void pinReset(String username)
        {
            Random randomPIN = new Random();        //Random Number
            int newPIN = randomPIN.Next(0, 9999);   //Gets a random number between 0 - 9999
            XDocument user = XDocument.Load(logintype + "/" + username + ".xml");           // loads the file to be altered into memory
            user.Element(logintype).Element("password").SetValue(newPIN.ToString("0000"));  // changes the users password to the new randomly generated one, .ToString("0000") ensures there is leading zeros if value is < 1000
            user.Save(logintype + "/" + username + ".xml"); // saves the changes made to the file
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            RegistrationScreen register = new RegistrationScreen();
            this.Close();
            register.ShowDialog();            
        }

        private bool isFormFilled()
        {
            if (textBoxUser.Text.Length == 0)
            {
                errortext = "Enter a Username.";
                textBoxUser.Focus();
                return false;
            }
            if (passwordBox1.Password.Length == 0)
            {
                errortext = errortext + "Enter a Password.";
                passwordBox1.Focus();                
                return false;
            }
            return true;
        }

        private void newWindow(String username)
        {
            this.Close();
            if (logintype == "Customer")
            {
                CustomerScreen customer = new CustomerScreen(username);
                customer.ShowDialog();
            }
            else if (logintype == "Clerk")
            {
                ClerkScreen clerk = new ClerkScreen(username);
                clerk.ShowDialog();
            }
            else if (logintype == "Chief")
            {
                ChiefScreen chief = new ChiefScreen(username);
                chief.ShowDialog();
            }            
        }

    }
}
