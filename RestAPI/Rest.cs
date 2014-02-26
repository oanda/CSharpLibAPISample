using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using RestApi.DataTypes;
using RestApi.Communications;


namespace RestApi
{
    /// <summary>
    /// Provides simple functions to access the sandbox rest api and return the results as .NET objects
    /// Warning: This is early sample code.  The api may change, and this code does not handle any errors.
    /// This class also does *not* provide complete access to the API, it only providems a subset of the functionality
    /// Documentation: https://github.com/oanda/apidocs
    /// </summary>
    public class Rest
    {
        /// <summary>
        /// Server to communicate with
        /// </summary>
        /// Change to https://api-fxpractice.oanda.com/ for practice account
        static string s_apiServer = "http://api-sandbox.oanda.com/";
        
        /// <summary>
        /// Gets the list of accounts for a specific user
        /// </summary>
        /// <param name="user">the username of the user</param>
        /// <returns>list of accounts</returns>

        /// <summary>
        /// Gets the list of open trades for a given account
        /// </summary>
        /// <param name="account">the account ID of the account</param>
        /// <returns>list of open trades (empty list if there are none)</returns>
        public static List<TradeData> GetTradeList( int account )
        {
            string requestString = s_apiServer + "v1/accounts/" + account + "/trades";
            string responseString = MakeRequest(requestString);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TradesResponse tradeResponse = serializer.Deserialize<TradesResponse>( responseString );
            List<TradeData> trades = new List<TradeData>();
            
            trades.AddRange( tradeResponse.trades );
            // TODO: should loop through the "next pages"
            
            return trades;
        }

        /// <summary>
        /// Get the list of open orders for a given account
        /// </summary>
        /// <param name="account">the account ID of the account</param>
        /// <returns>list of open orders (empty list if there are none)</returns>
        public static List<Order> GetOrderList( int account )
        {
            string requestString = s_apiServer + "v1/accounts/" + account + "/orders";
            string responseString = MakeRequest(requestString);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            OrdersResponse tradeResponse = serializer.Deserialize<OrdersResponse>( responseString );
                    
            List<Order> orders = new List<Order>();
            orders.AddRange( tradeResponse.orders );
            // TODO: should loop through the "next pages"

            return orders;
        }

        /// <summary>
        /// Gets the most recent transaction ID for the account specified
        /// </summary>
        /// <param name="account">the account ID of the account to check</param>
        /// <returns>the most recent transaction ID or -1 if no transactions are found</returns>
        public static int GetMostRecentTransactionId( int account )
        {
            int result = -1;
            string requestString = s_apiServer + "v1/accounts/" + account + "/transactions?maxCount=1";
            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();

            var tradeResponse = serializer.Deserialize<TransactionsResponse>( responseString );
            if (tradeResponse.transactions.Count > 0)
            {
                result = tradeResponse.transactions[0].id;
            }
                        
            return result;
        }

        /// <summary>
        /// retrieves a list of transactions in descending order
        /// https://github.com/oanda/apidocs/blob/master/sections/transactions.md
        /// </summary>
        /// <param name="account">the id of the account to load transactions for</param>
        /// <param name="minTransId">retrieve all transactions that are more recent (larger ID) than this id</param>
        /// <returns>the list of transactions (empty list if none)</returns>
        public static List<Transaction> GetTransactionList( int account, int minTransId )
        {
            string requestString = s_apiServer + "v1/accounts/" + account + "/transactions?minId=" + minTransId;
            
            string responseString = MakeRequest(requestString);
            
            var serializer = new JavaScriptSerializer();
            
            var transactions = new List<Transaction>();
            var dataResponse = serializer.Deserialize<TransactionsResponse>( responseString );
            transactions.AddRange(dataResponse.transactions);
            // TODO: should loop through the "next pages"
            
            return transactions;
        }

        /// <summary>
        /// Gets account specific details for the given account
        /// </summary>
        /// <param name="account">the ID of the account to retrieve</param>
        /// <returns>the AccountDetails for the account</returns>
        public static AccountDetails GetAccountDetails( int account )
        {   // eg. https://oanda-cs-dev:1342/accounts/123456
            string requestString = s_apiServer + "v1/accounts/" + account;

            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();

            var accountDetails = serializer.Deserialize<AccountDetails>( responseString );
            return accountDetails;
        }

        /// <summary>
        /// Get the current open positions for the account specified
        /// </summary>
        /// <param name="account">the ID of the account</param>
        /// <returns>list of positions (or empty list if there are none)</returns>
        public static List<Position> GetPositions( int account )
        {
            string requestString = s_apiServer + "v1/accounts/" + account + "/positions";
            
            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();
            var positionResponse = serializer.Deserialize<PositionsResponse>( responseString );
            var positions = new List<Position>();
            positions.AddRange( positionResponse.positions );

            return positions;
        }

        /// <summary>
        /// Retrieves H1 candles for the specified pair
        /// </summary>
        /// <param name="curPair">string name of the desired pair</param>
        /// <returns>list of candles up to the present</returns>
        public static List<Candle> GetCandles(string curPair, int accountid)
        {
            string requestString = s_apiServer + "v1/" + "history?" + "accountId="+ accountid +"&instrument=" + curPair + "&granularity=H1&candleFormat=midpoint";
            
            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();
            var candlesResponse = serializer.Deserialize<CandlesResponse>(responseString);
            List<Candle> candles = new List<Candle>();
            candles.AddRange(candlesResponse.candles);

            return candles;
        }

        /// <summary>
        /// Execute a marketOrder on the given account using the given parameters
        /// </summary>
        /// <param name="account">the ID of the account to use</param>
        /// <param name="requestParams">dictionary of parameters for the request key=Name, value=Value</param>
        public static void PostMarketOrder( int account, Dictionary<string, string> requestParams )
        {
            string requestString = s_apiServer + "v1/accounts/" + account + "/orders";

            var postData = "";
            foreach ( var pair in requestParams )
            {
                postData += pair.Key + "=" + pair.Value + "&";
            }
            postData += "type=market";

            string responseString = MakeRequest(requestString, "POST", postData);
            // TODO: make use of the response
        }

        /// <summary>
        /// Gets the list of instruments that are available
        /// </summary>
        /// <returns>a list of the available instruments</returns>
        public static List<Instrument> GetInstruments( int account )
        {
            string requestString = s_apiServer + "v1/instruments?accountId=" + account;

            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();
            var instrumentResponse = serializer.Deserialize<InstrumentsResponse>( responseString );
    
            List<Instrument> instruments = new List<Instrument>();
            instruments.AddRange( instrumentResponse.instruments );

            return instruments;
        }

        /// <summary>
        /// Gets the current rates for the given instruments
        /// </summary>
        /// <param name="instruments">The list of instruments to request</param>
        /// <returns>The list of prices</returns>
        public static List<Price> GetRates( List<Instrument> instruments )
        {
            var requestBuilder = new StringBuilder(s_apiServer + "v1/quote?instruments=");

            foreach ( var instrument in instruments )
            {
                requestBuilder.Append(instrument.instrument + ",");
            }
            // Grab the string and remove the trailing comma
            string requestString = requestBuilder.ToString().Trim(',');

            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();
            var pricesResponse = serializer.Deserialize<PricesResponse>( responseString );
            List<Price> prices = new List<Price>();
            prices.AddRange( pricesResponse.prices );

            return prices;
        }

        /// <summary>
        /// send a request and retrieve the response
        /// </summary>
        /// <param name="requestString">the request to send</param>
        /// <returns>the response string</returns>
        private static string MakeRequest(string requestString, string method="GET", string postData=null)
        {
            var request = WebRequest.CreateHttp(requestString);

            /*
            // for non-sandbox requests
            var accessToken = "<your access token here>";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            */

            request.Method = method;
            if (method == "POST")
            {
                var data = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            using (var response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd().Trim();

                    return responseString;
                }
            }
        }
    }
}
