namespace Mihaylov.Common.Loggers
{
    using System;
    using System.Text;

    public abstract class BaseErrorLogger : BaseFileWriter
    {
        public BaseErrorLogger(string rootDir, string subDirectory, string fileName)
            : base(rootDir, subDirectory, fileName)
        {
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