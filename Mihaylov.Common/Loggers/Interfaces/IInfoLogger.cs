namespace Mihaylov.Common.Loggers.Interfaces
{
    using System;

    public interface IInfoLogger : ILogger
    {
        void Write(string message);

        void Write(object obj, Exception exeption);
    }
}
