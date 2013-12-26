using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Diagnostics;

namespace ShinePhoto.Helpers
{
    public class DebugLogger : ILog
    {
        #region Fields
        private readonly Type _type;
        #endregion

        #region Constructors
        public DebugLogger(Type type)
        {
            _type = type;
        }
        #endregion

        #region Helper Methods
        private string CreateLogMessage(string format, params object[] args)
        {
            return string.Format("[{0}] {1}",
                                 DateTime.Now.ToString("o"),
                                 string.Format(format, args));
        }
        #endregion

        #region ILog Members
        public void Error(Exception exception)
        {
            Debug.WriteLine(CreateLogMessage(exception.ToString()), "ERROR");
        }
        public void Info(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "INFO");
        }
        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "WARN");
        }
        #endregion
    }

    public class Log4netLogger : ILog
    {
        #region Fields
        private readonly log4net.ILog _innerLogger;
        #endregion

        #region Constructors
        public Log4netLogger(Type type)
        {
            _innerLogger = log4net.LogManager.GetLogger(type);
        }
        #endregion

        #region ILog Members
        public void Error(Exception exception)
        {
            _innerLogger.Error(exception.Message, exception);
        }
        public void Info(string format, params object[] args)
        {
            _innerLogger.InfoFormat(format, args);
        }
        public void Warn(string format, params object[] args)
        {
            _innerLogger.WarnFormat(format, args);
        }
        #endregion
    }

}
