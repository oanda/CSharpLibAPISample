using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.DataTypes
{
    public class Order
    {
        [DisplayName("ID")]
        public long id { get; set; }
        [DisplayName("Type")]
        public string type { get; set; }
        [DisplayName("Side")]
        public string side { get; set; }
        [DisplayName("Instrument")]
        public string instrument { get; set; }
        [DisplayName("Units")]
        public int units { get; set; }
        [DisplayName("Time")]
        public DateTime time { get; set; }
        [DisplayName("Price")]
        public double price { get; set; }
        [DisplayName("Take Profit")]
        public double takeProfit { get; set; }
        [DisplayName("Expiry")]
        public DateTime expiry { get; set; }
        [DisplayName("Upper Bound")]
        public double upperBound { get; set; }
        [DisplayName("Lower Bound")]
        public double lowerBound { get; set; }
        [DisplayName("Trailing Stop")]
        public int trailingStop { get; set; }
    }
}
