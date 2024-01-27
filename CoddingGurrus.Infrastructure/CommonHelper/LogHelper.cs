
using System.Reflection;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public static class LogHelper
    {
        public static string GetBaseDiretory()
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return baseDirectory ?? "";
        }

        public static void _LogMessageTraceIssue(string methodName, string fileName)
        {
            string _logFilePath = string.Concat(GetBaseDiretory(), @"\logs\", $"{fileName}");
            using (StreamWriter w = System.IO.File.AppendText(_logFilePath))
            {
                w.WriteLine("=========================================================================");
                w.WriteLine(DateTime.Now + " : " + methodName + "\n");
                w.WriteLine("=========================================================================");

            }
        }
    }
}
