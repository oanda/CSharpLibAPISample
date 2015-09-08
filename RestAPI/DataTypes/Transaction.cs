using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.DataTypes
{
    public class Transaction
    {
        [DisplayName("ID")]
        public long id { get; set; }
        [DisplayName("Account")]
        public int accountId { get; set; }
        [DisplayName("Type")]
        public string type { get; set; }
        [DisplayName("Instrument")]
        public string instrument { get; set; }
        [DisplayName("Units")]
        public long units { get; set; }
        [DisplayName("Time")]
        public long DateTime { get; set; }
        [DisplayName("Price")]
        public double price { get; set; }
        [DisplayName("Balance")]
        public double balance { get; set; }
        [DisplayName("Interest")]
        public double interest { get; set; }
        [DisplayName("P/L")]
        public double profitLoss { get; set; }
        [DisplayName("High Order Limit")]
        public double highOrderLimit { get; set; }
        [DisplayName("Low Order Limit")]
        public double lowOrderLimit { get; set; }
        [DisplayName("Amount")]
        public double amount { get; set; }
        [DisplayName("Stop Loss")]
        public double stopLoss { get; set; }
        [DisplayName("Take Profit")]
        public double takeProfit { get; set; }
        [DisplayName("Duration")]
        public long duration { get; set; }
        [DisplayName("Completion Code")]
        public int completionCode { get; set; }
        [DisplayName("Transaction Link")]
        public long transactionLink { get; set; }
        [DisplayName("Order Link")]
        public long orderLink { get; set; }
        [DisplayName("Diaspora")]
        public long diaspora { get; set; }
        [DisplayName("Trailing Stop")]
        public int trailingStop { get; set; }
        [DisplayName("Margin Used")]
        public double marginUsed { get; set; }


        /// <summary>
        /// Gets a basic title for the type of transaction
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            switch ( type )
            {
                case "CloseOrder":
                    return "Order Closed";
                case "SellLimit":
                    return "Sell Limit Order Created";
                case "BuyLimit":
                    return "Buy Limit Order Created";
            }
            return type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetReadableString()
        {
            // TODO: make this pretty
            string readable = units + " " + instrument + " at " + price;
            if ( profitLoss != 0 )
            {
                readable += "\nP/L: " + profitLoss;
            }
            return readable;
        }
    }
}
