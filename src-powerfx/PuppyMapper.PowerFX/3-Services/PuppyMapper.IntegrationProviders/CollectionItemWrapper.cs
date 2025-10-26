namespace PuppyMapper.IntegrationProviders;

public class CollectionItemWrapper
{
    public Dictionary<string, object> Row { get; }

    public CollectionItemWrapper(Dictionary<string, object> row)
    {
        Row = row;
    }
}