using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

public interface IMappingDocument
{
    MappingSection MappingRules { get; }
    MappingSection InternalVars { get; }
    MappingInput[] MappingInputs { get; }
    MappingOutputType MappingOutputType { get; }
}
