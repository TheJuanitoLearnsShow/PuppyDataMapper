namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideInputData
{
    Task<string?> GetRecordAsJson();
}