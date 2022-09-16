namespace JsonToBusinessObjects.Conversion
{
    public interface IJsonToBusinessObjectsConverter
    {
        ConversionResult Convert(string jsonString);
    }
}