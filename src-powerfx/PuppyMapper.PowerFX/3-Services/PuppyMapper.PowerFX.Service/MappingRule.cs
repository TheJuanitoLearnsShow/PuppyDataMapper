namespace PuppyMapper.PowerFX.Service;

// Describe each rule in the file. 
public class MappingRule
{
    private readonly string _name = string.Empty;

    public string Name
    {
        get => _name;
        init => _name = value.Trim();
    }

    public string Formula { get; init; } = string.Empty;
    public string Comments { get; init; } = string.Empty;

    public MappingRule(
        string name,
        string formula,
        string comments)
    {
        Name = name;
        Formula = formula;
        Comments = comments;
    }
    public MappingRule()
    {
        
    }
    public void Deconstruct(out string name, out string formula, out string comments)
    {
        name = this.Name;
        formula = this.Formula;
        comments = this.Comments;
    }
}