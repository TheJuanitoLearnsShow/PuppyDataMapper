namespace SampleWithRoslyn;

public class StudentMapper : BaseMapper
{
    private readonly Student _output;
    private readonly Score _score;
    public StudentMapper(Student output, Score score)
    {
        _output = output;
        _score = score;
    }
    /// <summary>This converts score into gpa</summary>
    void MapTestScoreToGPA() 
    {
        _output.GPA = _score.TestScore * 0.23M 
                                       * _score.Weight;
    }

    /// <summary>This converts score into name</summary>
    void MapNameScoreToStudent() 
    {
        AddWarningIf(_score.TestScore == null, "No Test found");
        AddWarningIf(_score.TestScore < 0, "Test score less than zero");
        AddValidationErrorIf(_score.Test2 == null, "Missing last Test");
        AddValidationErrorIf(_score.Test1 == _score.Test2, "Tests are the same");
        if (AnyValidationErrorsInCurrentMethod()) {
            return;
        }
        if (_score.IsStudent) 
        {
            _output.StudentName = _score.Test1.Name + _score.Test2.Name;
        }
        return;
    }

    public void DoMapping()
    {
        MapNameScoreToStudent();
        MapTestScoreToGPA();
    }
}