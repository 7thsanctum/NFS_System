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
    class Mortgage : Account
    {
        private string address;
        public string getAddress { get { return address; } }
        public Mortgage(int accNum) : base(accNum, "Mortgage")
        {
            XDocument account = XDocument.Load("Accounts/" + accNum + ".xml");

            string addressTemp = account.Element("Mortgage").Element("address").Value;
            this.address = addressTemp;
        }

        public Mortgage(int accNumNew, double balanceNew, string custIDNew, int daysSinceInterestPaidNew, int limitNew, string address)
                 : base(accNumNew, balanceNew, custIDNew, daysSinceInterestPaidNew, limitNew)
        {           
            this.address = address;
        }

        public override void printStatement()
        {
            string displaystring = "Account Number : " + accNum +
                                   "\nCustomer ID + " + custID +
                                   "\nBalance = " + balance +
                                   "\nInterest Days = " + daysSinceInterestPaid +
                                   "\nAccount Limit = " + limit +
                                   "\nProperty = " + address;
            MessageBox.Show(displaystring);
        }

        public override void Withdraw(int withdrawel)
        {

        }

        public override bool saveAccount()
        {
            XDocument newUser = new XDocument(new XElement("Mortgage",
                                    new XElement("accNum"),
                                    new XElement("balance"),
                                    new XElement("interestDays"),
                                    new XElement("address"),
                                    new XElement("value"),
                                    new XElement("custID")));
            newUser.Element("Mortgage").Element("accNum").SetValue(this.accNum);
            newUser.Element("Mortgage").Element("balance").SetValue(this.balance);
            newUser.Element("Mortgage").Element("interestDays").SetValue(this.daysSinceInterestPaid);
            newUser.Element("Mortgage").Element("address").SetValue(this.address);
            newUser.Element("Mortgage").Element("value").SetValue(this.limit);
            newUser.Element("Mortgage").Element("custID").SetValue(this.custID);

            newUser.Save("Accounts/" + this.accNum + ".xml"); // saves the changes made to the file
            return true;
        }
    }
}
