namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideInputData
{
    Task<string?> GetRecordAsJson();
}

public interface IHaveInputOptions
{
    IProvideInputData BuildProvider();
}