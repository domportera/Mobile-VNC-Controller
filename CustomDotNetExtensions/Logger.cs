using System;

namespace CustomDotNetExtensions
{
    public static class Logger
    {
        public static event EventHandler<LogEventArgs> LogEvent;
        public static event EventHandler<LogEventArgs> ErrorEvent;
        public static event EventHandler<ExceptionEventArgs> ExceptionEvent;

        const string HeaderSeparator = " | ";

        public static void Log(object sender, string log, bool useDotNetConsole)
        {
            Log(sender, sender.GetType().Name, log, useDotNetConsole);
        }

        public static void Error(object sender, string log, bool useDotNetConsole)
        {
            Error(sender, sender.GetType().Name, log, useDotNetConsole);
        }

        public static void Exception(object sender, string log, Exception e, bool useDotNetConsole)
        {
            Exception(sender, sender.GetType().Name, log, e, useDotNetConsole);
        }

        public static void Log(IIdentified sender, string log, bool useDotNetConsole)
        {
            if (!sender.ShouldLog()) return;

            string header = sender.GenerateName();
            Log(sender, header, log, useDotNetConsole);
        }

        public static void Error(IIdentified sender, string log, bool useDotNetConsole)
        {
            if (!sender.ShouldLogErrors()) return;

            string header = sender.GenerateName();
            Error(sender, header, log, useDotNetConsole);
        }

        public static void Exception(IIdentified sender, string log, Exception e, bool useDotNetConsole)
        {
            string header = sender.GenerateName();
            Exception(sender, header, log, e, useDotNetConsole);
        }

        public static void Log(object sender, string header, string devLog, bool useDotNetConsole)
        {
            string log = $"{header}{HeaderSeparator}{devLog}";
            
            if(useDotNetConsole)
                Console.WriteLine(log);
            
            LogEvent?.Invoke(sender, new LogEventArgs(log));
        }

        public static void Error(object sender, string header, string devLog, bool useDotNetConsole)
        {
            string log = $"{header}{HeaderSeparator}{devLog}";
            
            if(useDotNetConsole)
                Console.Error.WriteLine(log);
            
            ErrorEvent?.Invoke(sender, new LogEventArgs(log));
        }

        public static void Exception(object sender, string header, string devLog, Exception e, bool useDotNetConsole)
        {
            string log = $"{header}{HeaderSeparator}Exception! {devLog} || {e}\n{e.StackTrace}";
            
            if(useDotNetConsole)
                Console.Error.WriteLine(log);
            
            ExceptionEvent?.Invoke(sender, new ExceptionEventArgs(log, e));
        }
    }

    public class LogEventArgs : EventArgs
    {
        public string Log { get; }

        public LogEventArgs(string log) { Log = log; }
    }

    public class ExceptionEventArgs : LogEventArgs
    {
        public Type ExceptionType { get; }
        public ExceptionEventArgs(string log, Exception e) : base(log)
        {
            ExceptionType = e.GetType();
        }
    }
}
