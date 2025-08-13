using PuppyMapper.PowerFX.Service.CustomLangParser;

namespace PuppyMapper.PowerFX.Service;

public class MappingDocumentEditDto : IMappingDocument
{
    private string _mappigRulesCode = string.Empty;

    public string MappingRulesCode
    {
        get => _mappigRulesCode;
        set
        {
            _mappigRulesCode = value;
            MappingRules = MappingDocumentParser.ParseMappingRules(_mappigRulesCode);
            
        }
    }

    private const string MappingSectionName = "Mapping";

    public List<MappingRule> MappingRules { get; private set; }= new();
    public MappingSection InternalVars { get; private set;}= new();
    public MappingInput[] MappingInputs { get; set; } = [];
    public MappingOutputType MappingOutputType { get; set; } = new();
}