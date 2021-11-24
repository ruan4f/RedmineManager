using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;

namespace RedmineManager.Util
{
    public class LogWriter
    {
        private StreamWriter sw = null;
        private static LogWriter logWriter = null;

        public String LogPath = "";
        public String LogFileName = "";

        public static int diasManterLog = ConfigurationManager.AppSettings["diasManterLog"].ParaInteiroComDefault(-30);

        private static Object theLock = new Object();

        private static DateTime currentDate;

        public static LogWriter get(string tipo = "")
        {
            lock (theLock)
            {
                if (logWriter == null)
                {
                    //if (LogPath.Length > 0 && LogFileName.Length >0)
                    logWriter = new LogWriter(tipo);
                }

                return logWriter;
            }
        }

        public static LogWriter Criar(string tipo = "")
        {
            lock (theLock)
            {
                return new LogWriter(tipo);
            }
        }

        private LogWriter(string tipo)
        {
            try
            {
                //lock (theLock)
                {
                    ConfigLog(tipo);

                    if (!Directory.Exists(LogPath))
                        Directory.CreateDirectory(LogPath);

                    String LogFileLongName = Path.Combine(LogPath, LogFileName);
                    if (!File.Exists(LogFileLongName))
                        sw = File.CreateText(LogFileLongName);
                    else
                        sw = File.AppendText(LogFileLongName);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                if (ex.Message != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine("[ERRO]: " + ex.Message);
                    }
                }

                if (ex.StackTrace != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        private void ConfigLog(string tipo)
        {
            string logPath = ConfigurationManager.AppSettings["LogPath"];
            string fileName = ConfigurationManager.AppSettings["LogFileName"] + tipo;

            //Console.WriteLine("logPath: " + logPath);
            //Console.WriteLine("fileName: " + fileName);

            //DateTime date = DateTime.Now;
            currentDate = DateTime.Now;
            string fileDate = currentDate.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "_").Replace(" ", "_").Replace(":", "");

            /*logPath += "\\" + date.Year.ToString();
            logPath += "\\" + date.Month.ToString("d2") + "\\";
            fileName += "_" + fileDate + ".txt";*/

            logPath += Path.DirectorySeparatorChar + currentDate.Year.ToString();
            logPath += Path.DirectorySeparatorChar + currentDate.Month.ToString("d2") + Path.DirectorySeparatorChar;
            fileName += "_" + fileDate + ".txt";

            LogFileName = fileName;
            LogPath = logPath;

            CleanOldLogs();
        }

        public void WriteMessage(LogWriter log, string msg, string tipo = "")
        {
            try
            {
                int nCurThread = -1;

                if (Thread.CurrentThread != null &&
                    Thread.CurrentThread.ManagedThreadId != null)
                    nCurThread = Thread.CurrentThread.ManagedThreadId;

                string newMsg = "";
                if (nCurThread != -1)
                {
                    newMsg += " [THREAD ID " + nCurThread + "]";
                }

                newMsg += DateTime.Now.ToString() + " - " + msg;

                lock (theLock)
                {
                    if (currentDate != DateTime.MinValue)
                    {
                        if (DateTime.Now.Day != currentDate.Day)
                        {
                            FinishLog();
                            log = new LogWriter(tipo);
                        }
                    }

                    Console.WriteLine(newMsg);

                    if (sw != null)
                    {
                        sw.WriteLine(newMsg);
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                if (ex.Message != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine("[ERRO]: " + ex.Message);
                    }
                }

                if (ex.StackTrace != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        public void WriteMessage(string msg, string tipo = "")
        {
            try
            {
                int nCurThread = -1;

                if (Thread.CurrentThread != null &&
                    Thread.CurrentThread.ManagedThreadId != null)
                    nCurThread = Thread.CurrentThread.ManagedThreadId;

                string newMsg = "";
                if (nCurThread != -1)
                {
                    newMsg += " [THREAD ID " + nCurThread + "]";
                }

                newMsg += DateTime.Now.ToString() + " - " + msg;

                lock (theLock)
                {
                    if (currentDate != DateTime.MinValue)
                    {
                        if (DateTime.Now.Day != currentDate.Day)
                        {
                            FinishLog();
                            logWriter = new LogWriter(tipo);
                        }
                    }

                    Console.WriteLine(newMsg);

                    if (sw != null)
                    {
                        sw.WriteLine(newMsg);
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                if (ex.Message != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine("[ERRO]: " + ex.Message);
                    }
                }

                if (ex.StackTrace != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        public void WriteBlankLine()
        {
            try
            {
                string newMsg = "\r";

                lock (theLock)
                {
                    Console.WriteLine(newMsg);

                    if (sw != null)
                    {
                        sw.WriteLine(newMsg);
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                if (ex.Message != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine("[ERRO]: " + ex.Message);
                    }
                }

                if (ex.StackTrace != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        public void CleanOldLogs()
        {
            string logPath = ConfigurationManager.AppSettings["LogPath"];

            if (Directory.Exists(LogPath))
            {
                DirectoryInfo dir = new DirectoryInfo(logPath);

                foreach (var pasta in dir.GetDirectories())
                {
                    VarrerDeletarArquivos(pasta);
                }
            }
        }

        private static void VarrerDeletarArquivos(DirectoryInfo pasta)
        {
            foreach (var arquivo in pasta.GetFiles())
            {
                if (arquivo.CreationTime <= DateTime.Now.AddDays(diasManterLog))
                {
                    arquivo.Delete();
                }
            }

            foreach (var subPasta in pasta.GetDirectories())
            {
                VarrerDeletarArquivos(subPasta);
            }
        }

        public void FinishLog()
        {
            try
            {
                lock (theLock)
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                //throw ex;
                if (ex.Message != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine("[ERRO]: " + ex.Message);
                    }
                }

                if (ex.StackTrace != null)
                {
                    lock (theLock)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

    }
}
