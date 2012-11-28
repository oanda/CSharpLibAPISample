using RestApi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.Communications
{
    class TradesResponse
    {
        public List<TradeData> trades { get; set; }
        public string nextPage { get; set; }
    }
}
