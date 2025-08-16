using PuppyMapper.PowerFX.Service.CustomLangParser;

namespace PuppyMapper.PowerFX.Service;

public class MappingDocumentEditDto : IMappingDocument
{
    private string _mappingRulesCode = string.Empty;
    private string _internalVarsCode  = string.Empty;

    public string MappingRulesCode
    {
        get => _mappingRulesCode;
        set
        {
            _mappingRulesCode = value;
            MappingRules = new MappingSection(MappingSectionName, 
                MappingDocumentParser.ParseMappingRules(_mappingRulesCode).ToArray());
            
        }
    }

    public string InternalVarsCode
    {
        get => _internalVarsCode;
        set
        {
            _internalVarsCode = value;
            InternalVars = new MappingSection(VariablesSectionName, 
                MappingDocumentParser.ParseMappingRules(_internalVarsCode).ToArray());
            
        }
    }
    private const string MappingSectionName = "Mapping";
    private const string VariablesSectionName = "VARIABLES";

    public MappingSection MappingRules { get; set; }= new();
    public MappingSection InternalVars { get; set;}= new();
    public MappingInput[] MappingInputs { get; set; } = [];
    public MappingOutputType MappingOutputType { get; set; } = new();
}