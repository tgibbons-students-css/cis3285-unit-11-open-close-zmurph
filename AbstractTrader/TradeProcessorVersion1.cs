using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTrader
{
    public class TradeProcessorVersion1 : TradeProcessor
    {
        protected override IEnumerable<string> ReadTradeData(Stream stream)
        {
            LogMessage("INFO: ReadTradeData version 1");
            var tradeData = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tradeData.Add(line);
                }
            }
            return tradeData;
        }
        protected void LogMessage(string message, params object[] args)
        {
            Console.WriteLine(message, args);
            // added for Request 408
            using (StreamWriter logfile = File.AppendText("log.xml"))
            {
                logfile.WriteLine("<log>" + message + "</log>", args);
            }

        }
        public virtual void ProcessTrades(Stream stream)
        //public void ProcessTrades(string url)
        {
            var lines = ReadTradeData(stream);
            //var lines = ReadURLTradeData(url);
            var trades = ParseTrades(lines);
            StoreTrades(trades);
        }
        protected override IEnumerable<TradeRecord> ParseTrades(IEnumerable<string> tradeData)
        {
            LogMessage("INFO: ParseTrades version 1");
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in tradeData)
            {
                var fields = line.Split(new char[] { ',' });

                var trade = MapTradeDataToTradeRecord(fields);

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }

        protected TradeRecord MapTradeDataToTradeRecord(string[] fields)
        {
            var sourceCurrencyCode = fields[0].Substring(0, 3);
            var destinationCurrencyCode = fields[0].Substring(3, 3);
            var tradeAmount = int.Parse(fields[1]);
            var tradePrice = decimal.Parse(fields[2]);
            const float LotSize = 100000f;
            var trade = new TradeRecord
            {
                SourceCurrency = sourceCurrencyCode,
                DestinationCurrency = destinationCurrencyCode,
                Lots = tradeAmount / LotSize,
                Price = tradePrice
            };

            return trade;
        }

        protected override void StoreTrades(IEnumerable<TradeRecord> trades)
        {
            LogMessage("INFO: Simulating database connection in StoreTrades");
            // Not really connecting to database in this sample
            LogMessage("INFO: {0} trades processed", trades.Count());
        }

    }
}
