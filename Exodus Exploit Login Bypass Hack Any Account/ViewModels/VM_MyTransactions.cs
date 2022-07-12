using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_MyTransactions
    {
        public long UserID;
        public List<VM_Transaction> All;
        public List<VM_Transaction> TransactionsSent;
        public List<VM_Transaction> TransactionsReceived;

        public decimal TransactionsSentSum(En_Currency curr)
        {
            if (TransactionsSent == null || !TransactionsSent.Any()) { return 0; }
            return TransactionsSent.Where(r => r.TransactionCurrency == curr).Sum(x => x.TransactionAmount);
        }

        public decimal TransactionsReceivedSum(En_Currency curr)
        {
            if (TransactionsSent == null || !TransactionsSent.Any()) { return 0; }
            return TransactionsReceived.Where(r => r.TransactionCurrency == curr).Sum(x => x.TransactionAmount);
        }

        public List<VM_User> PeopleReceivedHelpFrom()
        {
            if (TransactionsReceived == null || !TransactionsReceived.Any()) { return new List<VM_User>(); }
            return TransactionsReceived.Select(p => p.TransactionSender).ToList();
        }

        public List<VM_User> PeopleSentHelpTo()
        {
            if (TransactionsSent == null || !TransactionsSent.Any()) { return new List<VM_User>(); }
            return TransactionsSent.Select(p => p.TransactionReceiver).ToList();
        }

        public List<VM_Transaction> LastTransactions(int count = 10)
        {
            if (All == null || !All.Any()) { return new List<VM_Transaction>(); }
            return All.Take(count).ToList();
        }
    }
}