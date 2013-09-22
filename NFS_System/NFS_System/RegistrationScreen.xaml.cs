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
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for RegistrationScreen.xaml
    /// </summary>
    public partial class RegistrationScreen : Window
    {
        public RegistrationScreen()
        {
            InitializeComponent();
        }

        private void createNew_Click(object sender, RoutedEventArgs e)
        {
            if (formCheck())
            {
                string username = username_textbox.Text;
                string password = password_textbox.Text;
                string name = name_textbox.Text;
                string address = address_textbox.Text;
                Customer newCustomer = new Customer(username, password, name, address);
                if (!File.Exists("Customer/" + username + ".xml"))
                {
                    newCustomer.saveCustomer();
                    MessageBox.Show("Your details have been successfully registered. Please visit your local branch to have a new bank account associated with your details. Thank you and have a nice day.");
                    this.Close();                    
                }
                else MessageBox.Show("This Username is already in use, please enter a different username");
            }
        }

        private bool formCheck()
        {
            if (username_textbox.Text.Length == 0)
            {
                MessageBox.Show("Please enter a Username");
                username_textbox.Focus();
                return false;
            }            
            if (password_textbox.Text.Length == 0)
            {
                MessageBox.Show("Please enter a Password");
                password_textbox.Focus();
                return false;
            }
            if (name_textbox.Text.Length == 0)
            {
                MessageBox.Show("Please enter your Full Name");
                name_textbox.Focus();
                return false;
            }
            if (address_textbox.Text.Length == 0)
            {
                MessageBox.Show("Please enter your Address");
                address_textbox.Focus();
                return false;
            }
            return true;
        }

        private bool textContainsUnallowedCharacter(string t)
        {
            char[] UnallowedCharacters = { '.', ',',
                                           '<', '>',
                                           '/', ';',
                                           '@', '"',
                                           '!', '(',
                                           ')', ':'};
            for (int i = 0; i < UnallowedCharacters.Length; i++)
                if (username_textbox.Text.Contains(UnallowedCharacters[i]))
                {
                    int CursorIndex = username_textbox.SelectionStart - 1;
                    username_textbox.Text = username_textbox.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    username_textbox.SelectionStart = CursorIndex;
                    username_textbox.SelectionLength = 0;
                }
            return true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void username_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textContainsUnallowedCharacter(username_textbox.Text);
        }
    }
}
