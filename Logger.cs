using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tagger
{
    public class Logger : ILogger
    {
        private static Logger _logger = new Logger();

        private static StringBuilder sb = new StringBuilder();

        public static Logger GetGlobalLogger()
        {
            return _logger;
        }

        public void Log(string msg)
        {
            sb.AppendLine(DateTime.Now.ToString() + " - INFO - " + msg);
        }

        public void LogWarn(string msg)
        {
            sb.AppendLine(DateTime.Now.ToString() + " - WARN - " + msg);
        }

        public void LogErr(string msg)
        {
            sb.AppendLine(DateTime.Now.ToString() + " - ERRR - " + msg);
        }

        public string ToString()
        {
            return sb.ToString();
        }

        public void Clear()
        {
            sb.Clear();
        }
    }
}
