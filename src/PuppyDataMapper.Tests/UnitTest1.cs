namespace PuppyDataMapper.Tests;

public class DataMapperGeneratorTests
{
    [Fact]
    public void GenerateFromXmlMapping()
    {
        var generatedCode = DataMapperGenerator.GenerateClassFile("GradeExportMapper", "SampleMapping.xml");
        File.WriteAllText("generatedClass.cs", generatedCode);
    }
}
