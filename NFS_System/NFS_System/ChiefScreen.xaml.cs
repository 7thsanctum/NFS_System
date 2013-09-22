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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for ChiefScreen.xaml
    /// </summary>
    public partial class ChiefScreen : Window
    {
        string user;
        public ChiefScreen(string username)
        {
            InitializeComponent();
            user = username;
            textBlock1.Text = DateTime.Now.ToString("d/M/yyyy");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void createNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clerk_button_Click(object sender, RoutedEventArgs e)
        {
            ClerkScreen chiefClerk = new ClerkScreen(user);
            chiefClerk.ShowDialog();
        }

        private void setinterest_button_Click(object sender, RoutedEventArgs e)
        {
            SetInterestScreen setInterest = new SetInterestScreen();
            setInterest.ShowDialog();
        }

        private void status_button_Click(object sender, RoutedEventArgs e)
        {
            accountList.Items.Clear();
            List<string> filenames = (Directory.GetFiles("Accounts/", "*.xml").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToList());

            foreach (string x in filenames)
            {
                XDocument report = XDocument.Load("Accounts/" + x + ".xml");
                string accNum = (Int32.Parse(report.Root.Element("accNum").Value)).ToString("0000");
                double balance = double.Parse(report.Root.Element("balance").Value);
                string type = report.Root.Name.ToString();
                string output = String.Format("Account Number : {0, -4} , Balance : {1:C} , Type : {2, -8}", accNum, balance, type);
                accountList.Items.Add(output);
            }
        }

        private void startInterest_button_Click(object sender, RoutedEventArgs e)
        {
            InputBox input = new InputBox("Please enter the number of days you would like to\n calculate interest for.");
            DateTime lastDate = DateTime.Parse(XDocument.Load("NFS_System_Files/InterestRates.xml").Root.Element("interestLast").Value);
            TimeSpan daysBetween = DateTime.Now - lastDate;
            input.InputBox_Input.Text = (daysBetween.Days).ToString();
            input.ShowDialog();

            List<string> filenames = (Directory.GetFiles("Accounts/", "*.xml").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToList());

            foreach (string x in filenames)
            {
                XDocument report = XDocument.Load("Accounts/" + x + ".xml");
                string type = report.Root.Name.ToString();
                double balance = double.Parse(report.Root.Element("balance").Value);
                report.Root.Element("balance").SetValue(balance + calculateInterestRates(type, balance));
                report.Save("Accounts/" + x + ".xml");
            }
        }

        private double calculateInterestRates(string type, double balance)
        {
            XDocument rates = XDocument.Load("NFS_System_Files/InterestRates.xml");
            double savingRate = double.Parse(rates.Root.Element("Savings").Value);
            double mortRate = double.Parse(rates.Root.Element("Mortgage").Value);

            if (type == "Savings")
            {
                return (balance * savingRate) * 30;
            }
            else
            {
                double newbalance = balance;
                for (int d = 0; d < 30; d++)
                {
                    newbalance = newbalance + (newbalance * mortRate);
                }
                return newbalance - balance;
            }
        }
    }
}
