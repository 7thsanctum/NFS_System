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
    /// Interaction logic for UpdateUserScreen.xaml
    /// </summary>
    public partial class UpdateUserScreen : Window
    {
        Customer alterCustomer;
        public UpdateUserScreen(string username)
        {
            InitializeComponent();
            alterCustomer = new Customer(username);
            username_textbox.Text = alterCustomer.getUsername;
            password_textbox.Text = alterCustomer.getPassword;
            name_textbox.Text = alterCustomer.getName;
            address_textbox.Text = alterCustomer.getAddress;
            username_textbox.IsEnabled = false;
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (formCheck())
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you wish to alter this users information? This process cannot be reverted.", "Alter User Information", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    string username = username_textbox.Text;

                    alterCustomer.getPassword = password_textbox.Text;
                    alterCustomer.getName = name_textbox.Text;
                    alterCustomer.getAddress = address_textbox.Text;

                    if (File.Exists("Customer/" + username + ".xml"))
                    {
                        alterCustomer.saveCustomer();
                        MessageBox.Show(username + " details have been updated successfully");
                        this.Close();
                    }
                    else MessageBox.Show("This user does not exist, what have you done?!");
                }
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void username_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textContainsUnallowedCharacter(username_textbox.Text);
        }
    }
}
