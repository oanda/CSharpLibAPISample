using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.DataTypes
{
    public class Candle
    {
        public DateTime time { get; set; }
        public double openMid { get; set; }
        public double highMid { get; set; }
        public double lowMid { get; set; }
        public double closeMid { get; set; }
        public bool complete { get; set; }
    }
}
