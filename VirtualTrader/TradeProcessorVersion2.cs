using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTrader
{
    class TradeProcessorVersion2 : TradeProcessor
    {

        public override void ProcessTrades(Stream stream)
        //public void ProcessTrades(string url)
        {
            LogMessage("INFO: Using Updated Trade Processor Version 2");
            base.ProcessTrades(stream);
        }
    }
}
