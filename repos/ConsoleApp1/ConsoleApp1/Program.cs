using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
            
            ILog log = LogManager.GetLogger(typeof(Program));
           
            //log.Info("Check the AWS Console CloudWatch with log4net12 Logs console in us-east-1");
            //log.Info("to see messages in the log streams for the");
            //log.Info("log group Log4net.ConfigExample");
            log.Debug("testing Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntestingtesting Cloud Watch\ntesting\ntesting");
        }
    }
}
