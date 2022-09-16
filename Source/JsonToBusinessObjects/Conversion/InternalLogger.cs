namespace JsonToBusinessObjects.Conversion
{
    using System;
    using Infrastructure.Logging;
    using Messages;

    public class InternalLogger : ILogger
    {
        private readonly ConversionMessages _conversionMessages = new ConversionMessages();

        public IConversionMessages ConversionMessages => _conversionMessages;

        public void Debug(object message) => Debug(message, null);
        public void Info(object message) => Info(message, null);
        public void Warn(object message) => Warn(message, null);
        public void Error(object message) => Error(message, null);
        public void Fatal(object message) => Fatal(message, null);

        public void Debug(object message, Exception exception) {} // Client is not interested in debug messages
        public void Info(object message, Exception exception) => _conversionMessages.AddInfo(new ConversionMessage(message.ToString(), exception));
        public void Warn(object message, Exception exception) => _conversionMessages.AddWarning(new ConversionMessage(message.ToString(), exception));
        public void Error(object message, Exception exception) => _conversionMessages.AddError(new ConversionMessage(message.ToString(), exception));
        public void Fatal(object message, Exception exception) => _conversionMessages.AddFatalError(new ConversionMessage(message.ToString(), exception));
    }
}