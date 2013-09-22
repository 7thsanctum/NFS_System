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
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for NewAccountScreen.xaml
    /// </summary>
    public partial class NewAccountScreen : Window
    {
        Account newAccount;
        public NewAccountScreen(string username)
        {
            InitializeComponent();
            username_textbox.Text = username;
            acctype_combobox.Items.Add("Savings");
            acctype_combobox.Items.Add("Mortgage");
            address_label.Visibility = System.Windows.Visibility.Hidden;
            address_textbox.Visibility = System.Windows.Visibility.Hidden;
            accnum_textbox.Text = CalculateNewAccountNumber().ToString();
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (formCheck())
            {
                string username = username_textbox.Text;
                int accountNumber = Int32.Parse(accnum_textbox.Text);
                double balance = double.Parse(balance_textbox.Text);
                int limit = Int32.Parse(limit_textbox.Text);
                string address = address_textbox.Text;

                if (!File.Exists("Accounts/" + accountNumber + ".xml"))
                {
                    if (acctype_combobox.Text == "Savings")
                    {
                        newAccount = new Savings(accountNumber, balance, username, 0, limit);
                    }
                    else if (acctype_combobox.Text == "Mortgage")
                    {
                        newAccount = new Mortgage(accountNumber, balance, username, 0, limit, address);
                    }
                    if (newAccount.saveAccount())
                        MessageBox.Show("Your details have been successfully registered. Please visit your local branch to have a new bank account associated with your details. Thank you and have a nice day.");

                    Customer editCustomer = new Customer(username);
                    List<string> list = editCustomer.getAccountNumbers;
                    list.Add(accountNumber.ToString());
                    editCustomer.getAccountNumbers = list;
                    editCustomer.saveCustomer();

                    this.Close();
                }
                else MessageBox.Show("This account already exists...");
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
            if (accnum_textbox.Text.Length == 0)
            {
                MessageBox.Show("Invalid Account Number Generated, Seek assistance");
                accnum_textbox.Focus();
                return false;
            }
            if (balance_textbox.Text.Length == 0)
            {
                MessageBox.Show("Please enter a Starting Balance for this account.");
                balance_textbox.Focus();
                return false;
            }
            double balance;
            if (!double.TryParse(balance_textbox.Text, out balance))
            {
                MessageBox.Show("Please enter a Starting Balance for this account.");
                balance_textbox.Focus();
                return false;
            }
            if (address_textbox.Visibility == System.Windows.Visibility.Hidden && balance < 1)
            {
                MessageBox.Show("You cannot start an account with less than £1!");
                balance_textbox.Focus();
                return false;
            }
            if (limit_textbox.Text.Length == 0)
            {
                MessageBox.Show("Please enter a limit/value for this account.");
                limit_textbox.Focus();
                return false;
            }
            double limit;
            if (!double.TryParse(limit_textbox.Text, out limit))
            {
                MessageBox.Show("Please enter a valid limit/value for this account.");
                limit_textbox.Focus();
                return false;
            }
            else if (address_textbox.Visibility == System.Windows.Visibility.Visible && balance < -limit)
            {
                MessageBox.Show("You cannot take out more money than the property is worth!");
                limit_textbox.Focus();
                return false;
            }
            if (address_textbox.Text.Length == 0 && address_textbox.Visibility == System.Windows.Visibility.Visible)
            {
                MessageBox.Show("Please enter an address for this account.");
                address_textbox.Focus();
                return false;
            }
            if (!double.TryParse(deposit_textbox.Text, out limit) && address_textbox.Visibility == System.Windows.Visibility.Visible)
            {
                MessageBox.Show("Please enter a valid depoist.");
                address_textbox.Focus();
                return false;
            }
            return true;
        }

        private void acctype_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (acctype_combobox.SelectedItem == "Mortgage")
            {
                address_label.Visibility = System.Windows.Visibility.Visible;
                address_textbox.Visibility = System.Windows.Visibility.Visible;
                deposit_label.Visibility = System.Windows.Visibility.Visible;
                deposit_textbox.Visibility = System.Windows.Visibility.Visible;
                balance_textbox.IsReadOnly = true;
                limit_label.Content = "Value : ";
            }
            else
            {
                address_label.Visibility = System.Windows.Visibility.Hidden;
                address_textbox.Visibility = System.Windows.Visibility.Hidden;
                deposit_label.Visibility = System.Windows.Visibility.Hidden;
                deposit_textbox.Visibility = System.Windows.Visibility.Hidden;
                balance_textbox.IsReadOnly = false;
                limit_label.Content = "Overdraft : ";
            }
        }

        private int CalculateNewAccountNumber()
        {
            List<string> filenames = (Directory.GetFiles("Accounts/", "*.xml").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToList());
            //TODO add check for files not beginning with a digit
            int highest = 0;
            foreach (string x in filenames)
            {
                if (highest < Int32.Parse(x)) highest = Int32.Parse(x);
            }
            return (highest + 1);
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void limit_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (acctype_combobox.SelectedItem == "Mortgage")
            {
                double limit;
                double balance;
                if (double.TryParse(limit_textbox.Text, out  limit))
                {
                    if (double.TryParse(balance_textbox.Text, out balance))
                    {
                        balance_textbox.Text = (-limit).ToString();
                    }
                    else MessageBox.Show("Please enter a valid number");
                }
                else MessageBox.Show("Please enter a valid number");
            }
        }

        private void deposit_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double limit;
            double balance;
            double deposit;
            if (double.TryParse(limit_textbox.Text, out  limit))
            {
                if (double.TryParse(balance_textbox.Text, out balance))
                {
                    if (double.TryParse(deposit_textbox.Text, out deposit))
                    {
                        balance_textbox.Text = (-limit + deposit).ToString();
                    }
                    else MessageBox.Show("Please enter a valid number");
                }
                else MessageBox.Show("Please enter a valid number");
            }
            else MessageBox.Show("Please enter a valid number");
        }
    }
}
