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
    /// Interaction logic for ClerkScreen.xaml
    /// </summary>
    public partial class ClerkScreen : Window
    {
        Customer customer;
        Account account;
        string accounttype;

        public ClerkScreen(string username)
        {
            InitializeComponent();
            textBlock1.Text = username;

            disableForm();

            createNew.IsEnabled = false;
            closeUser.IsEnabled = false;

            accountList.SelectedIndex = 0;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }        

        private void loadAccount_Click(object sender, RoutedEventArgs e)
        {
            account = null;
            UserSelect selectedUser = new UserSelect("Please select the username");
            selectedUser.ShowDialog();
            if (selectedUser.User_Selection.Text.Length > 0)
            {               
                customer = new Customer(selectedUser.User_Selection.Text);
                username.Text = "Customer ID : " + selectedUser.User_Selection.Text;
                createNew.IsEnabled = true;
                updateAccount.IsEnabled = true;
                closeUser.IsEnabled = true;
                refreshAccNum();               
            }
        }

        private void updateAccount_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserScreen updateUser = new UpdateUserScreen(customer.getUsername);
            updateUser.ShowDialog();
            refreshAccNum();
        }

        private void createNew_Click(object sender, RoutedEventArgs e)
        {
            NewAccountScreen newAccount = new NewAccountScreen(customer.getUsername);
            newAccount.ShowDialog();
            customer = new Customer(customer.getUsername);
            refreshAccNum();
        }

        private void closeAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you wish to delete this bank account? This process cannot be reverted.", "Delete Bank Account", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string accountToBeRemoved = accountList.SelectedValue.ToString();
                string directory = ("Accounts/" + accountToBeRemoved + ".xml");
                File.Delete(directory);
                List<string> list = customer.getAccountNumbers;                
                list.Remove(accountToBeRemoved);
                customer.getAccountNumbers = list;
                customer.saveCustomer();
                account = null;
            }
            refreshAccNum();
        }

        private void closeUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you wish to delete this users account? This process cannot be reverted.", "Delete User", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                List<string> accNumbers = customer.getAccountNumbers;
                string customerID = customer.getUsername;
                foreach (string x in accNumbers)
                    if (File.Exists("Accounts/" + accNumbers + ".xml")) File.Delete("Accounts/" + accNumbers + ".xml");
                if (File.Exists("Customer/" + customerID + ".xml")) File.Delete("Customer/" + customerID + ".xml");
                disableForm();
            }
        }

        private void accountList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accountList.SelectedIndex != -1)
            {
                string accountListaccountNumber = accountList.SelectedValue.ToString();
                string directory = ("Accounts/" + accountListaccountNumber + ".xml");
                closeUser.IsEnabled = true;
                if (File.Exists(directory))
                {
                    updateAccount.IsEnabled = true;
                    createNew.IsEnabled = true;
                    closeAccount.IsEnabled = true;
                    closeUser.IsEnabled = true;
                    XDocument accountFile = XDocument.Load(directory);
                    accounttype = accountFile.Root.Name.ToString();
                    if (accounttype == "Savings")
                    {
                        account = new Savings(Int32.Parse(accountListaccountNumber));
                    }
                    else if (accounttype == "Mortgage")
                    {
                        account = new Mortgage(Int32.Parse(accountListaccountNumber));
                    }
                    else
                    {
                        disableForm();
                        MessageBox.Show("ERROR 0002 : Invalid Node Identifier, Node not recognised. Please Contact Database Admin For assistance");
                    }
                    displayInfo();
                }
                else
                {
                    disableForm();
                    MessageBox.Show("WARNING THIS ACCOUNT HAS BEEN CORRUPTED : PLEASE CONTACT DATABASE ADMIN IMMEDIATELY");
                }
            }
            else if (account == null) closeAccount.IsEnabled = false;
        }

        private void disableForm()
        {
            updateAccount.IsEnabled = false;
            closeAccount.IsEnabled = false;
            createNew.IsEnabled = false;
            accountDetail.Items.Clear();
            accountList.Items.Clear();
        }

        private void refreshAccNum()
        {
            accountList.Items.Clear();
            accountDetail.Items.Clear();
            
            List<string> accNumbers = customer.getAccountNumbers;
            foreach (string x in accNumbers)
            {
                if (x == "") continue;
                else accountList.Items.Add(x);
            }
            if (account != null) displayInfo();
            accountList.SelectedIndex = 0;
            
        }

        private void displayInfo()
        {
            accountDetail.Items.Clear();
            accountDetail.Items.Add("Owner : " + account.getCustID);
            accountDetail.Items.Add("Account Number : " + account.getAccNum);
            accountDetail.Items.Add(string.Format("Balance : {0:C}", account.getBalance));
            accountDetail.Items.Add("Day Interest : " + account.getDayInterest);
            if (accounttype == "Mortgage") accountDetail.Items.Add("Value : " + account.getLimit);
            else accountDetail.Items.Add("Limit : " + account.getLimit);
            if (accounttype == "Mortgage")
            {
                Mortgage mort = (Mortgage)account;
                accountDetail.Items.Add("Address : " + mort.getAddress);
            }
        }
    }
}
