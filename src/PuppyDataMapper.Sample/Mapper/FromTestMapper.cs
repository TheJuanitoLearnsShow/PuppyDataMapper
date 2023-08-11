using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mapper;

//--
// See https://aka.ms/new-console-template for more information
public partial class FromTestToStatEntryMapper : FromTestToStatEntryMapperBase
{
    public override DateTime MapEndOfMonth(Test test)
    {
        throw new NotImplementedException();
    }
}
public class Test
{
    public decimal MyScore { get; internal set; }
    public int Score { get; internal set; }
    public DateTime DateTaken { get; internal set; }
}
public class TestSummary
{
    public ScoresData Scores { get; internal set; }
}

public class ScoresData
{
    public int StdScore { get; internal set; }
    public decimal Avg { get; internal set; }
}

public class StatEntry
{
    public string AdjustedScore { get; set; }
    public DateTime Eom { get; set; }
    public decimal NextTestProb { get; set; }
    public decimal FinalScore { get; set; }
}