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
    /// This class also does *not* provide complete access to the API, it only provides a subset of the functionality
    /// Documentation: https://github.com/oanda/apidocs
    /// </summary>
    public class Rest
    {
        /// <summary>
        /// Server to communicate with
        /// </summary>
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
            string requestString = s_apiServer + "accounts/" + account + "/trades";
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
            string requestString = s_apiServer + "accounts/" + account + "/orders";
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
            string requestString = s_apiServer + "accounts/" + account + "/transactions?maxCount=1";
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
            string requestString = s_apiServer + "accounts/" + account + "/transactions?minTransId=" + minTransId;
            
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
            string requestString = s_apiServer + "accounts/" + account;

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
            string requestString = s_apiServer + "accounts/" + account + "/positions";
            
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
        public static List<Candle> GetCandles(string curPair)
        {
            string requestString = s_apiServer + "v1/" + "instruments/" + curPair + "/candles?granularity=H1";
            
            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();
            var candlesResponse = serializer.Deserialize<CandlesResponse>(responseString);
            List<Candle> candles = new List<Candle>();
            candles.AddRange(candlesResponse.candles);

            return candles;
        }

        /// <summary>
        /// Execute a trade on the given account using the given parameters
        /// </summary>
        /// <param name="account">the ID of the account to use</param>
        /// <param name="requestParams">dictionary of parameters for the request key=Name, value=Value</param>
        public static void PostTrade( int account, Dictionary<string, string> requestParams )
        {
            string requestString = s_apiServer + "accounts/" + account + "/trades?";
            foreach ( var pair in requestParams )
            {
                requestString += pair.Key + "=" + pair.Value + "&";
            }
            requestString = requestString.Trim( '&' );
            var request = WebRequest.CreateHttp( requestString );
            request.Method = "POST";

            using ( var response = request.GetResponse() )
            {
                var serializer = new JavaScriptSerializer();

                using ( var reader = new StreamReader( response.GetResponseStream() ) )
                {
                    string responseString = reader.ReadToEnd().Trim();

                    // TODO: make use of the response
                }
            }
        }

        /// <summary>
        /// Gets the list of instruments that are available
        /// </summary>
        /// <returns>a list of the available instruments</returns>
        public static List<Instrument> GetInstruments()
        {
            string requestString = s_apiServer + "v1/instruments";

            string responseString = MakeRequest(requestString);

            var serializer = new JavaScriptSerializer();
            var instrumentResponse = serializer.Deserialize<InstrumentsResponse>( responseString );
    
            List<Instrument> instruments = new List<Instrument>();
            instruments.AddRange( instrumentResponse.instruments );

            return instruments;
        }

        /// <summary>
        /// Gets the current rates for the given instruments
        /// NOTE: For repeated requests, use PollRatesSession (and StartRatesSession)
        /// </summary>
        /// <param name="instruments">The list of instruments to request</param>
        /// <returns>The list of prices</returns>
        public static List<Price> GetRates( List<Instrument> instruments )
        {
            var requestBuilder = new StringBuilder(s_apiServer + "v1/instruments/price?instruments=");

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
        /// Initializes a rates session that can then be polled
        /// </summary>
        /// <param name="instruments">the list of instruments to be polled</param>
        /// <returns>the ID of the rates session</returns>
        public static long StartRatesSession( List<Instrument> instruments )
        {
            string requestString = s_apiServer + "v1/instruments/poll";

            var pollRequest = new PricePollRequest();
            pollRequest.prices = new List<string>();
            foreach ( var instrument in instruments )
            {
                pollRequest.prices.Add( instrument.instrument );
            }

            var serializer = new JavaScriptSerializer();

            var request = WebRequest.CreateHttp( requestString );
            request.Method = "POST";
            using ( var writer = new StreamWriter( request.GetRequestStream() ) )
            {
                writer.Write( serializer.Serialize( pollRequest ) );
            }

            using ( var response = request.GetResponse() )
            {
                using ( var reader = new StreamReader( response.GetResponseStream() ) )
                {
                    string responseString = reader.ReadToEnd().Trim();

                    var sessionResponse = serializer.Deserialize<SessionResponse>( responseString );
                    return sessionResponse.sessionId;
                }
            }
        }

        /// <summary>
        /// Poll the specified rates session
        /// </summary>
        /// <param name="sessionId">The ID for the rates session (obtained using StartRatesSession)</param>
        /// <returns>list of prices that have changed since the last poll request (empty list if nothing changed)</returns>
        public static List<Price> PollRatesSession(long sessionId)
        {
            string requestString = s_apiServer + "v1/instruments/poll?sessionId=" + sessionId;
            string responseString = MakeRequest(requestString);
            
            var serializer = new JavaScriptSerializer();

            var pricesResponse = serializer.Deserialize<PricesResponse>( responseString );
            var prices = new List<Price>();
            if ( pricesResponse.prices != null )
            {
                prices.AddRange( pricesResponse.prices );
            }
            return prices;
            
        }

        /// <summary>
        /// send a request and retrieve the response
        /// </summary>
        /// <param name="requestString">the request to send</param>
        /// <returns>the response string</returns>
        private static string MakeRequest(string requestString)
        {
            var request = WebRequest.CreateHttp(requestString);

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
