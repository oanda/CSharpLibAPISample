using RestApi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.Communications
{
    class PricesResponse
    {
        public long time { get; set; }
        public List<Price> prices { get; set; }
    }
}
