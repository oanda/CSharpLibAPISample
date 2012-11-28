using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RestApi.DataTypes
{
    public class Position
    {
        [DisplayName("Direction")]
        public string direction { get; set; }
        [DisplayName("Instrument")]
        public string instrument { get; set; }
        [DisplayName("Units")]
        public int units { get; set; }
        [DisplayName("Average Price")]
        public double avgPrice { get; set; }
    }
}
