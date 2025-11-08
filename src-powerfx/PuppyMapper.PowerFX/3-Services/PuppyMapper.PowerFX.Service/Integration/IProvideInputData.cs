namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideInputData
{
    Task<string?> GetRecordAsJson();
    string InputId { get; init; }
}