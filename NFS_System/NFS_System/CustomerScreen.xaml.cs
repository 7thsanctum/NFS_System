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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class CustomerScreen : Window
    {
        Account account;
        string accounttype;
        public CustomerScreen(string user)
        {
            InitializeComponent();
            textBlock1.Text = user;

            Customer customer = new Customer(user);
            List<string> accNumbers = customer.getAccountNumbers;
            foreach(string x in accNumbers)
                accountList.Items.Add(x);

            withdraw.IsEnabled = false;
            deposit.IsEnabled = false;
            accountList.SelectedIndex = 0;            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void withdraw_Click(object sender, RoutedEventArgs e)
        {
            InputBox input = new InputBox("Please enter the amount you would like to withdraw.");
            input.ShowDialog();
            int withdrawel = Int32.Parse(input.InputBox_Input.Text);
            if (!(account.getBalance - withdrawel < account.getLimit)) account.Withdraw(withdrawel);
            else MessageBox.Show("You do not have the required funds to make that withdrawel.");
            displayInfo();
        }

        private void deposit_Click(object sender, RoutedEventArgs e)
        {
            InputBox input = new InputBox("Please enter the amount you would like to deposit.");
            input.ShowDialog();
            int deposit = Int32.Parse(input.InputBox_Input.Text);
            account.Deposit(deposit);
            displayInfo();
        }

        private void displayInfo()
        {
            accountDetail.Items.Clear();
            accountDetail.Items.Add("Owner : " + account.getCustID);
            accountDetail.Items.Add("Account Number : " + account.getAccNum);
            accountDetail.Items.Add(string.Format("Balance : {0:C}", account.getBalance));
            accountDetail.Items.Add("Day Interest : " + account.getDayInterest);
            if (accounttype == "Mortgage") accountDetail.Items.Add(string.Format("Value : {0:C}", account.getLimit));
            else accountDetail.Items.Add(string.Format("Limit : {0:C}", account.getLimit));
            if (accounttype == "Mortgage")
            {
                Mortgage mort = (Mortgage)account;
                accountDetail.Items.Add("Address : " + mort.getAddress); 
            }
        }


        private void statement_Click(object sender, RoutedEventArgs e)
        {
            account.printStatement();
        }

        private void accountList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            string accountListaccountNumber = accountList.SelectedValue.ToString();
            string directory = ("Accounts/" + accountListaccountNumber + ".xml");
            if (File.Exists(directory))
            {
                deposit.IsEnabled = true;
                statement.IsEnabled = true;
                XDocument accountFile = XDocument.Load(directory);
                accounttype = accountFile.Root.Name.ToString();
                if (accounttype == "Savings")
                {
                    account = new Savings(Int32.Parse(accountListaccountNumber));
                    withdraw.IsEnabled = true;
                    displayInfo();
                }
                else if (accounttype == "Mortgage")
                {
                    account = new Mortgage(Int32.Parse(accountListaccountNumber));
                    withdraw.IsEnabled = false;
                    displayInfo();
                }
                else
                {
                    disableForm();
                    MessageBox.Show("ERROR 0002 : Invalid Node Identifier, Node not recognised. Please Contact Database Admin For assistance");
                }
            }
            else
            {
                disableForm();
                MessageBox.Show("WARNING THIS ACCOUNT HAS BEEN CORRUPTED : PLEASE CONTACT DATABASE ADMIN IMMEDIATELY");
            }
        }

        private void disableForm()
        {
            deposit.IsEnabled = false;
            withdraw.IsEnabled = false;
            statement.IsEnabled = false;
        }
    }
}
