using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public static class EventLogger
    {
        public static void Error(Exception e)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                StringBuilder stackTrace = new StringBuilder();
                stackTrace.Append(Environment.NewLine)
                    .Append("------------Stack Trace------------")
                    .Append(Environment.NewLine)
                    .Append(e.StackTrace)
                    .Append(Environment.NewLine)
                    .Append("------------End Stack Trace------------")
                    .Append(Environment.NewLine);

                Write("ERROR",e.Message, w, stackTrace.ToString());
            }
        }

        public static void Warning(string message)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Write("WARNING",message, w);
            }
        }

        public static void Information(string message)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Write("INFORMATION",message, w);
            }
        }

        public static void Write(string title, string logMessage, TextWriter w, string stackTrack = "")
        {
            w.Write("\r\nLog Entry : {0}", title);
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            if(!string.IsNullOrEmpty(stackTrack))
                w.WriteLine("  :{0}", stackTrack);
            w.WriteLine("-------------------------------");
        }
    }
}
