namespace JsonToBusinessObjects.Conversion.Messages
{
    using System.Collections.Generic;

    public class ConversionMessages : IConversionMessages
    {
        private readonly IList<ConversionMessage> _fatalErrors = new List<ConversionMessage>();
        private readonly IList<ConversionMessage> _errors = new List<ConversionMessage>();
        private readonly IList<ConversionMessage> _warnings = new List<ConversionMessage>();
        private readonly IList<ConversionMessage> _infos = new List<ConversionMessage>();

        public IEnumerable<ConversionMessage> FatalErrors => _fatalErrors;
        public IEnumerable<ConversionMessage> Errors => _errors;
        public IEnumerable<ConversionMessage> Warnings => _warnings;
        public IEnumerable<ConversionMessage> Infos => _infos;

        public void AddFatalError(ConversionMessage m) => _fatalErrors.Add(m);
        public void AddError(ConversionMessage m) => _errors.Add(m);
        public void AddWarning(ConversionMessage m) => _warnings.Add(m);
        public void AddInfo(ConversionMessage m) => _infos.Add(m);
    }
}