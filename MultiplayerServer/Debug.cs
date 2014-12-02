using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework
{
    internal class Debug
    {
        private const bool CONSOLE_OUT = true;
        private const DebugLevel CONSOLE_OUT_LEVEL = DebugLevel.VERBOSE;
        
        private static volatile Debug _instance;
        private static object _syncRoot = new Object();
        private ConcurrentQueue<LogItem> _log { get; set; }

        public static Debug Instance
        {
            get
            {
                lock(_syncRoot)
                {
                    return _instance ?? (_instance = new Debug());
                }
            }
        }

        private Debug() 
        {
            _log = new ConcurrentQueue<LogItem>();
        }
        
        internal void Log(DebugLevel level, String message)
        {
            _log.Enqueue(new LogItem(level, message));

            if (CONSOLE_OUT && level <= CONSOLE_OUT_LEVEL)
                Console.WriteLine(message);
        }

        internal String GetLog(DebugLevel level = DebugLevel.VERBOSE)
        {
            return GetLog(new DateTime(), DateTime.Now, level);
        }

        internal String GetLog(DateTime StartDate, DebugLevel level = DebugLevel.VERBOSE)
        {
            return GetLog(StartDate, DateTime.Now, level);
        }

        internal String GetLog(DateTime StartDate, DateTime EndDate, DebugLevel level = DebugLevel.VERBOSE)
        {
            String outputString = String.Empty;
            foreach (LogItem item in _log)
                if (item._level <= level)
                    if(item._datestamp > StartDate && item._datestamp < EndDate.AddSeconds(1))
                        outputString += String.Format("{0,-23}{1,-10}{2}\n", item._datestamp.ToString(), item._level.ToString(), item._message);

            return outputString;
        }

        internal Int32 ErrorCount() {
            return _log.Count;
        } 

    }

    internal enum DebugLevel
    {
        ERROR,
        WARNING,
        DEBUG,
        NETWORK,
        VERBOSE
    }

    internal class LogItem
    {
        public LogItem(DebugLevel level, String message)
        {
            _level = level;
            _message = message;
            _datestamp = DateTime.Now;
        }
        public DebugLevel _level { get; private set; }
        public String _message { get; private set; }
        public DateTime _datestamp { get; private set; }
    }

    public class DebugTest{

        public void RunTest()
        {
            Debug.Instance.Log(DebugLevel.ERROR, "some Error message");
            Debug.Instance.Log(DebugLevel.WARNING, "some Warning message");
            Debug.Instance.Log(DebugLevel.DEBUG, "some Debug message");
            Debug.Instance.Log(DebugLevel.NETWORK, "some Network message");
            Debug.Instance.Log(DebugLevel.VERBOSE, "some Verbose message");

            Console.WriteLine(Debug.Instance.GetLog());

        }
    }
}
