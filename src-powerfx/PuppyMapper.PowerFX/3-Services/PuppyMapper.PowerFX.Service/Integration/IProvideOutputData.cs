namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideOutputData
{
    Task<OutputStatus> OutputData(List<Dictionary<string, object>> rows, bool simulateOnly = false);
    public string GetDisplayData();
}