namespace SampleWithRoslyn;

public class Score
{
    public TestData Test1 { get; set; } = new();
    public TestData Test2 { get; set; } = new();
    public decimal TestScore { get; set; }
    public bool IsStudent { get; set; }
    public decimal Weight { get; set; }
}