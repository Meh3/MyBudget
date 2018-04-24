using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public class TransactionModel : TransactionModelSimple
    {
        public int Id { get; private set; }
        public string Category { get; private set; }
        public string Taker { get; private set; }
        public string Tag { get; private set; }

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
