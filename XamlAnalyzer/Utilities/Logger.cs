using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XamlAnalyzer.Utilities
{
    public enum LogLevel
    {
        Warning = 0,
        Error = 1,
        Normal = 2
    }
   
    public  class Log
    {
        private string message;

        public int Number { get; set; }
        public string Message { get => message; set { message = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message")); } }
        public LogLevel Level { get; set; }
        public DateTime Time { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Message;
        }
    }
    public  class Logger
    {
        static int logsCount = 1;
        static private Log lastLog;
        static private string test;

        public static string Test
        {
            get => test;

            set
            {
                test = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs("Test"));
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };
        public event PropertyChangedEventHandler PropertyChanged;

        static public ObservableCollection<Log> Logs { get; set; } = new ObservableCollection<Log>();
        static public Log LastLog { get => lastLog; set { 
                lastLog = value;
                
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs("LastLog")); /*OnPropertyChanged();*/ } }


        static public void OnPropertyChanged([CallerMemberName] string pNamme = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(pNamme));
        }
        static Logger()
        {
            Logs.CollectionChanged += (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    LastLog = (Log)e.NewItems[0];
                    Test = LastLog.Time.ToString();
                     
                }
                if(e.Action== System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                {
                    LastLog =new Log() { Level = LogLevel.Normal, Message = "OK!" , Number = logsCount++,
                        Time = DateTime.Now,
                    };
                }
            };
        }
        static public Task LogAsync(LogLevel level, string message)
        {

            Logs.Add(new Utilities.Log()
            {
                Level = level,
                Message = message,
                Number = logsCount++,
                Time = DateTime.Now,
            });

            return Task.CompletedTask;
        }
        public static void Log(LogLevel level, string message)
        {

            Logs.Add(new Utilities.Log()
            {
                Level = level,
                Message = message,
                Number = logsCount++,
                Time = DateTime.Now,
            });

        }

        static object _lock = new object();
        //private static Logger instance;

        //private Logger() { }
        //public static Logger Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new Logger();
        //        }
        //        return instance;
        //    }
        //    set => instance = value;
        //}

    }
}
