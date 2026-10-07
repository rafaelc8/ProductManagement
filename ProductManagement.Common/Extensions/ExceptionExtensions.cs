using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ProductManagement.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFilename(this Exception exception)
        {
            var st = new StackTrace(exception, true);
            for (var i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                var fileName = frame.GetFileName();
                if (!String.IsNullOrEmpty(fileName))
                {
                    return Path.GetFileName(fileName);
                }
            }
            return String.Empty;
        }

        public static int? GetLine(this Exception exception)
        {
            var st = new StackTrace(exception, true);
            for (var i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                var fileName = frame.GetFileName();
                if (!String.IsNullOrEmpty(fileName))
                {
                    return frame.GetFileLineNumber();
                }
            }
            return null;
        }

        public static string GetMethod(this Exception exception)
        {
            var st = new StackTrace(exception, true);
            for (var i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                var fileName = frame.GetFileName();
                if (!String.IsNullOrEmpty(fileName))
                {
                    if (frame.GetMethod().DeclaringType.GetInterfaces().Any(p => p == typeof(IAsyncStateMachine)))
                    {
                        var generatedType = frame.GetMethod().DeclaringType;
                        var originalType = generatedType.DeclaringType;
                        var foundMethod = originalType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                            .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);

                        Regex regexp = new Regex("(?<=<)(.*)(?=>)");
                        Match match = regexp.Match(foundMethod.Name);
                        if (match.Success)
                        {
                            return match.Groups[1].Value;
                        }
                        return foundMethod.Name;
                    }
                    else
                    {
                        return frame.GetMethod().Name;
                    }
                }
            }
            return String.Empty;
        }

    }
}
