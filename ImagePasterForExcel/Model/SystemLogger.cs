using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePasterForExcel.Model
{
    public class SystemLogger
    {
        private enum LogLevel
        {
            Error,
            Exception,
            Warn,
            Info,
            Debug
        }


        private static readonly object LockObj = new Object();
        private static readonly FileInfo LogFile = new FileInfo(@".\Logs\SystemLog.log");



        public static void Error(string msg)
        {
            Out(LogLevel.Error, msg);
        }


        public static void Error(Exception ex)
        {
            Out(LogLevel.Error, ex.Message + Environment.NewLine + ex.StackTrace);
        }

        public static void Exception(string msg)
        {
            Out(LogLevel.Exception, msg);
        }

        public static void Exception(Exception ex)
        {
            Out(LogLevel.Exception, ex.Message + Environment.NewLine + ex.StackTrace);
        }

        public static void Warn(string msg)
        {
            Out(LogLevel.Warn, msg);
        }

        public static void Info(string msg)
        {
            Out(LogLevel.Info, msg);
        }

        public static void Debug(string msg)
        {
            Out(LogLevel.Debug, msg);
        }

        private static void Out(LogLevel level, string msg)
        {
            if (!Directory.Exists(LogFile.DirectoryName)) Directory.CreateDirectory(LogFile.DirectoryName);

            var log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level} : {msg}";

            using (var stream = new StreamWriter(LogFile.FullName, true, Encoding.UTF8))
            {
                lock (LockObj)
                {
                    stream.WriteLine(log);

                }
            }

        }

    }
}
