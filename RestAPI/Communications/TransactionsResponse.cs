using RestApi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.Communications
{
    public class TransactionsResponse
    {
        public List<Transaction> transactions;
        public string nextPage;
    }
}
