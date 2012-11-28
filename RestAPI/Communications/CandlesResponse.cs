using RestApi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.Communications
{
    class CandlesResponse
    {
        public string instrument  { get; set; }
        public string granularity  { get; set; }
        public List<Candle> candles { get; set; }
    }
}
