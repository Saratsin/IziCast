using System;
using MvvmCross.Logging;
using MvvmCross;
using Sharpcaster.Logging;

namespace IziCast.Core
{
    public class SharpCasterLogProvider : ILogProvider
    {
        private SharpCasterLogProvider()
        {
        }

        public static SharpCasterLogProvider Instance { get; } = new SharpCasterLogProvider();

        public Logger GetLogger(string name)
        {
            IziCastLog.Instance.Trace($"Sharpcaster log: {name}");
            return LogMessage;
        }

        private bool LogMessage(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, object[] formatParameters = null)
        {
            return IziCastLog.Instance.Log(ToMvxLogLevel(logLevel), messageFunc, exception, formatParameters);
        }

        private MvxLogLevel ToMvxLogLevel(LogLevel logLevel)
        {
            switch(logLevel)
            {
                case LogLevel.Debug:
                    return MvxLogLevel.Debug;
                case LogLevel.Error:
                    return MvxLogLevel.Error;
                case LogLevel.Fatal:
                    return MvxLogLevel.Fatal;
                case LogLevel.Info:
                    return MvxLogLevel.Info;
                case LogLevel.Warn:
                    return MvxLogLevel.Warn;
                default:
                    return MvxLogLevel.Trace;
            }
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotSupportedException();
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotSupportedException();
        }
    }

    public class IziCastLog
    {
        public static IMvxLog Instance => Mvx.Resolve<IMvxLog>();
    }
}
