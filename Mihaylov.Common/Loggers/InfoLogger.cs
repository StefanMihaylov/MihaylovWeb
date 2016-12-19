namespace Mihaylov.Common.Loggers
{
    using System;
    using System.Text;
    using Mihaylov.Common.Loggers.Interfaces;

    public class InfoLogger : BaseFileWriter, IInfoLogger
    {
        private const string FileName = "InfoLog";

        public InfoLogger(string rootDir, string subDirectory)
            : base(rootDir, subDirectory, FileName)
        {
        }

        public void Write(string message)
        {
            this.WriteString(message);
        }

        public void Write(object obj, Exception exeption)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Exception thrown in: " + obj.ToString());
            stringBuilder.AppendLine(exeption.ToString());

            this.WriteString(stringBuilder.ToString());
        }
    }
}
