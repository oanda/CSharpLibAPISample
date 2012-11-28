using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.DataTypes
{
    public class Account
    {
        public int id { get; set; }
        public string name { get; set; }
        public string homecurr { get; set; }
        public double marginRate { get; set; }
        public List<string> accountPropertyName;
        public string AccountProperties
        {
            get
            {
                string result = "";
                foreach ( var value in accountPropertyName )
                {
                    result += value;
                }
                return result;
            }
        }
    }
}
