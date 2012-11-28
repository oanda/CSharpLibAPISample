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
        public int id { get; set; }
        [DisplayName("Type")]
        public string type { get; set; }
        [DisplayName("Direction")]
        public string direction { get; set; }
        [DisplayName("Instrument")]
        public string instrument { get; set; }
        [DisplayName("Units")]
        public int units { get; set; }
        [DisplayName("Time")]
        public long time { get; set; }
        [DisplayName("Price")]
        public double price { get; set; }
        [DisplayName("Take Profit")]
        public double takeProfit { get; set; }
        [DisplayName("Expiry")]
        public long expiry { get; set; }
        [DisplayName("High Limit")]
        public double highLimit { get; set; }
        [DisplayName("Low Limit")]
        public double lowLimit { get; set; }
        [DisplayName("Trailing Stop")]
        public int trailingStop { get; set; }
        [DisplayName("OCA Group")]
        public int ocaGroupId { get; set; }
    }
}
