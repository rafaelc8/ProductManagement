using ProductManagement.Common.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProductManagement.Common.Helpers
{
    public static class LogHelper
    {
        private static string version { get; set; }

        public static void SetVersion(string value)
        {
            version = value;
        }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Error(object message, Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Error(message, exception);
        }

        public static void Error(string message, [CallerFilePath] string filepath = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {

            StackTrace stackTrace = new StackTrace();
            log4net.GlobalContext.Properties["filename"] = Path.GetFileName(filepath);
            log4net.GlobalContext.Properties["member"] = member;
            log4net.GlobalContext.Properties["line"] = line;
            log4net.GlobalContext.Properties["version"] = version;
            log.Error(message);
        }

        public static void Error(Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Error("", exception);
        }

        public static void Info(object message, Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Info(message, exception);
        }
        public static void Info(string message, [CallerFilePath] string filepath = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {

            StackTrace stackTrace = new StackTrace();
            log4net.GlobalContext.Properties["filename"] = Path.GetFileName(filepath);
            log4net.GlobalContext.Properties["member"] = member;
            log4net.GlobalContext.Properties["line"] = line;
            log4net.GlobalContext.Properties["version"] = version;
            log.Info(message);
        }

        public static void Info(Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Info("", exception);
        }

        public static void Debug(object message, Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Debug(message, exception);
        }

        public static void Debug(string message, [CallerFilePath] string filepath = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {

            StackTrace stackTrace = new StackTrace();
            log4net.GlobalContext.Properties["filename"] = Path.GetFileName(filepath);
            log4net.GlobalContext.Properties["member"] = member;
            log4net.GlobalContext.Properties["line"] = line;
            log4net.GlobalContext.Properties["version"] = version;
            log.Debug(message);
        }

        public static void Debug(Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Debug("", exception);
        }


        public static void Warn(object message, Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Warn(message, exception);
        }

        public static void Warn(string message, [CallerFilePath] string filepath = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {

            StackTrace stackTrace = new StackTrace();
            log4net.GlobalContext.Properties["filename"] = Path.GetFileName(filepath);
            log4net.GlobalContext.Properties["member"] = member;
            log4net.GlobalContext.Properties["line"] = line;
            log4net.GlobalContext.Properties["version"] = version;
            log.Warn(message);
        }

        public static void Warn(Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Debug("", exception);
        }

        public static void Fatal(object message, Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Fatal(message, exception);
        }

        public static void Fatal(string message, [CallerFilePath] string filepath = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {

            StackTrace stackTrace = new StackTrace();
            log4net.GlobalContext.Properties["filename"] = Path.GetFileName(filepath);
            log4net.GlobalContext.Properties["member"] = member;
            log4net.GlobalContext.Properties["line"] = line;
            log4net.GlobalContext.Properties["version"] = version;
            log.Fatal(message);
        }

        public static void Fatal(Exception exception)
        {
            log4net.GlobalContext.Properties["filename"] = exception.GetFilename();
            log4net.GlobalContext.Properties["member"] = exception.GetMethod();
            log4net.GlobalContext.Properties["line"] = exception.GetLine();
            log4net.GlobalContext.Properties["version"] = version;
            log.Fatal("", exception);
        }
    }
}
