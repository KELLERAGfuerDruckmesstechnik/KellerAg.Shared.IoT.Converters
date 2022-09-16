namespace JsonToBusinessObjects.CommandRules
{
    using DataContainers;

    internal interface ICommandModification
    {
        bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot);
        void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot);
    }
}