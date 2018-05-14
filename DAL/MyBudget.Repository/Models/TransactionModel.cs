using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public class TransactionModel : TransactionModelSimple
    {
        public int Id { get; }
        public string Category { get; }
        public string Taker { get; }
        public string Tag { get; }

        public TransactionModel(int id = 0, DateTime date = default(DateTime), double amount = 0.0, 
            string category = null, string taker = null, string description = null, string tag = null)
            :base (date,amount, description)
        {
            Id = id;
            Category = category;
            Taker = taker;
            Tag = tag;
        }
    }
}
