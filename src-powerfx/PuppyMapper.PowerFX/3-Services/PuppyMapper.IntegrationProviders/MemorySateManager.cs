namespace PuppyMapper.IntegrationProviders;

public class MemorySateManager
{
    private static Dictionary<string, object> State { get; set; } = new();

    public static Dictionary<string, object>  GetState()
    {
        return State;
    }
    public static void ResetState()
    {
        State  = new();
    }
}