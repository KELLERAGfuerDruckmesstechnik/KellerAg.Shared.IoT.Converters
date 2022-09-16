namespace JsonToBusinessObjects.CommandRules
{
    using Newtonsoft.Json.Linq;

    internal interface ICommandRule
    {
        char HandledCommandCharacter { get; }
        ICommandModification CreateModificationObject(JToken variables);
    }
}