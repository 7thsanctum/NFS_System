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
    public abstract class Account
    {
        protected int accNum;
        public int getAccNum { get { return accNum; } }
        protected double balance;
        public double getBalance { get { return balance; } }
        protected string custID;
        public string getCustID { get { return custID; } }
        protected int daysSinceInterestPaid;
        public int getDayInterest { get { return daysSinceInterestPaid; } }
        protected int limit;
        public int getLimit { get { return limit; } }

        public Account() { }
        public Account(int accNum, string accountType)
        {
            string fieldname = "limit";
            if(accountType == "Mortgage") fieldname = "value";
            XDocument account = XDocument.Load("Accounts/" + accNum + ".xml");
            double tempBalance = double.Parse(account.Element(accountType).Element("balance").Value);
            int tempDays = Int32.Parse(account.Element(accountType).Element("interestDays").Value);
            int tempLimit = Int32.Parse(account.Element(accountType).Element(fieldname).Value);
            string tempID = account.Element(accountType).Element("custID").Value;

            this.accNum = accNum;
            this.balance = tempBalance;
            this.daysSinceInterestPaid = tempDays;
            this.custID = tempID;
            this.limit = tempLimit;
        }

        public Account( int accNumNew,
                        double balanceNew,
                        string custIDNew,
                        int daysSinceInterestPaidNew,
                        int limitNew)
        {
            accNum = accNumNew;
            balance = balanceNew;
            custID = custIDNew;
            daysSinceInterestPaid = daysSinceInterestPaidNew;
            limit = limitNew;
        }

        public virtual bool saveAccount()
        {
            XDocument newUser = new XDocument(new XElement("Savings",
                                    new XElement("accNum"),
                                    new XElement("balance"),
                                    new XElement("interestDays"),
                                    new XElement("limit"),
                                    new XElement("custID")));
            newUser.Element("Savings").Element("accNum").SetValue(this.accNum);
            newUser.Element("Savings").Element("balance").SetValue(this.balance);
            newUser.Element("Savings").Element("interestDays").SetValue(this.daysSinceInterestPaid);
            newUser.Element("Savings").Element("limit").SetValue(this.limit);
            newUser.Element("Savings").Element("custID").SetValue(this.custID);

            newUser.Save("Accounts/" + this.accNum + ".xml"); // saves the changes made to the file
            return true;
        }

        public void Deposit(int deposit)
        {
            XDocument account = XDocument.Load("Accounts/" + accNum + ".xml");
            balance = balance + deposit;
            account.Root.Element("balance").SetValue(balance);
            account.Save("Accounts/" + accNum + ".xml");
            
        }

        public virtual void Withdraw(int withdrawel)
        {
            XDocument account = XDocument.Load("Accounts/" + accNum + ".xml");
            balance = balance - withdrawel;
            account.Root.Element("balance").SetValue(balance);
            account.Save("Accounts/" + accNum + ".xml");
        }

        public virtual void printStatement()
        {
            string displaystring = "Account Number : " + accNum +
                                   "\nCustomer ID + " + custID +
                                   "\nBalance = " + balance +
                                   "\nInterest Days = " + daysSinceInterestPaid +
                                   "\nAccount Limit = " + limit;
            MessageBox.Show(displaystring);
        }        

        public int CalculateNewAccountNumber()
        {
            string[] filenames = (Directory.GetFiles("Accounts/", "*.xml").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToArray());
            //TODO add check for files not beginning with a digit
            int highest = Int32.Parse(filenames[0]);
            for (int i = 0; i < filenames.Length; i++)
            {
                if (highest < Int32.Parse(filenames[i])) highest = Int32.Parse(filenames[i]);
            }
            return(highest + 1);
        }

    }
}
