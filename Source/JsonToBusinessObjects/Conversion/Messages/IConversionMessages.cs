namespace JsonToBusinessObjects.Conversion.Messages
{
    using System.Collections.Generic;

    public interface IConversionMessages
    {
        IEnumerable<ConversionMessage> FatalErrors { get; }
        IEnumerable<ConversionMessage> Errors { get; }
        IEnumerable<ConversionMessage> Warnings { get; }
        IEnumerable<ConversionMessage> Infos { get; }
    }
}