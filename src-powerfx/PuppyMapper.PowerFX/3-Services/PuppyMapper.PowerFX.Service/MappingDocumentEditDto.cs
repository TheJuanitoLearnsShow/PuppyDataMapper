using PuppyMapper.PowerFX.Service.CustomLangParser;

namespace PuppyMapper.PowerFX.Service;

public class MappingDocumentEditDto : IMappingDocument
{
    private string _mappigRulesCode = string.Empty;
    private string _internalVarsCode  = string.Empty;

    public string MappingRulesCode
    {
        get => _mappigRulesCode;
        set
        {
            _mappigRulesCode = value;
            MappingRules = new MappingSection(MappingSectionName, 
                MappingDocumentParser.ParseMappingRules(_mappigRulesCode).ToArray());
            
        }
    }

    public string InternalVarsCode
    {
        get => _internalVarsCode;
        set
        {
            _mappigRulesCode = value;
            MappingRules = new MappingSection(VariablesSectionName, 
                MappingDocumentParser.ParseMappingRules(_internalVarsCode).ToArray());
            
        }
    }
    private const string MappingSectionName = "Mapping";
    private const string VariablesSectionName = "VARIABLES";

    public MappingSection MappingRules { get; private set; }= new();
    public MappingSection InternalVars { get; private set;}= new();
    public MappingInput[] MappingInputs { get; set; } = [];
    public MappingOutputType MappingOutputType { get; set; } = new();
}