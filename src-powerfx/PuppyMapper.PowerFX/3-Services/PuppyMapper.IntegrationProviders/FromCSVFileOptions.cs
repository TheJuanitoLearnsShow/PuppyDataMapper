using PuppyMapper.PowerFX.Service.Integration;

public class FromCSVFileOptions : IHaveInputOptions
{
    public string FilePath { get; set; }
    public IProvideInputData BuildProvider()
    {
        return new FromCSVFile(this);
    }
}
