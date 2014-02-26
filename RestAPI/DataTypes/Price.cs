using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.DataTypes
{
    public class Price
    {
        public string instrument { get; set; }
        public DateTime time;
        public double bid { get; set; }
        public double ask { get; set; }
    }
}
