namespace Mihaylov.Common.Loggers.Interfaces
{
    using System;

    public interface ICriticalErrorLogger : ILogger
    {
        void Write(object obj, Exception exeption);
    }
}
