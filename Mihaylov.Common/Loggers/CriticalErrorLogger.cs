namespace Mihaylov.Common.Loggers
{
    using Mihaylov.Common.Loggers.Interfaces;

    public class CriticalErrorLogger : BaseErrorLogger, ICriticalErrorLogger
    {
        private const string FileName = "CriticalErrorLog";

        public CriticalErrorLogger(string rootDir, string subDirectory)
            : base(rootDir, subDirectory, FileName)
        {
        }
    }
}
