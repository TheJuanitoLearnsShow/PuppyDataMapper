namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideOutputData
{
    Task<OutputStatus> OutputData(List<Dictionary<string, object>> rows, bool simulateOnly = false);
    public string OutputId { get; init; }
    public string GetDisplayData();
}