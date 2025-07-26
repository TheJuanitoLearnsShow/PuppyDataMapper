using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PuppyMapper.PowerFX.Service.JsonParser
{
    public class MappingDocumentJson
    {
        public async Task<MapperInterpreter> ParseJsonAsync(string rootMappingDocumentFilePath, IReadOnlyList<(string mapName, string filePath)> childMappingDocuments)
        {
            var doc = await ParseDocument(rootMappingDocumentFilePath);

            var childMappers = new Dictionary<string, IMappingDocument>();
            foreach (var (mapName, filePath) in childMappingDocuments)
            {
                using var childFileContents = new StreamReader(filePath);
                var childDoc = await JsonSerializer.DeserializeAsync<MappingDocumentDto>(childFileContents.BaseStream);
                childMappers.Add(mapName, childDoc!);
            }
            var mapper = new MapperInterpreter(doc!, childMappers.ToImmutableDictionary());
            return mapper;
        }

        public static async Task<MappingDocumentDto> ParseDocument(string rootMappingDocumentFilePath)
        {
            using var fileContents = new StreamReader(rootMappingDocumentFilePath);
            var doc = await JsonSerializer.DeserializeAsync<MappingDocumentDto>(fileContents.BaseStream);
            return doc!;
        }
    }
}
