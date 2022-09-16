namespace JsonToBusinessObjects.Conversion
{
    using System.Linq;
    using DataContainers;
    using Messages;

    public class ConversionResult
    {
        public ConversionResult(BusinessObjectRoot businessObjectRoot, IConversionMessages conversionMessages)
        {
            BusinessObjectRoot = businessObjectRoot;
            ConversionMessages = conversionMessages;
        }

        public BusinessObjectRoot BusinessObjectRoot { get; }
        public IConversionMessages ConversionMessages { get; }

        public bool HasErrors => ConversionMessages.Errors.Any() || ConversionMessages.FatalErrors.Any();
        public bool HasWarnings => ConversionMessages.Warnings.Any();
    }
}