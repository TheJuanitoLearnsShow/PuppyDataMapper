using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class ToMemoryState : IProvideOutputData
{
    private readonly ToMemoryStateOptions _settings;
    private readonly Dictionary<string, object> _data = new();

    public ToMemoryState(ToMemoryStateOptions settings)
    {
        _settings = settings;
    }

    public Task<OutputStatus> OutputData(List<Dictionary<string, object>> rows)
    {
        var pathParts = ParseOutputPath();
        for (var rowIdx = 0; rowIdx < rows.Count; rowIdx++)
        {
            var row = rows[rowIdx];
            Dictionary<string, object> currentLevel = _data;
            for (int pathPartIdx = 0; pathPartIdx < pathParts.Length; pathPartIdx++)
            {
                var pathPart = pathParts[pathPartIdx];
                if (pathPart == "RowIndex")
                {
                    pathPart = rowIdx.ToString();
                }
                else if (row.TryGetValue(pathPart, out var value) ){
                    pathPart = value.ToString() ?? pathPart;
                }
                if (pathPartIdx == pathParts.Length - 1)
                {
                    currentLevel[pathPart] = row;
                }
                else
                {
                    if (!currentLevel.ContainsKey(pathPart) ||
                        currentLevel[pathPart] is not Dictionary<string, object> nextLevel)
                    {
                        nextLevel = new Dictionary<string, object>();
                        currentLevel[pathPart] = nextLevel;
                    }

                    currentLevel = nextLevel;
                }
            }
        }

        return Task.FromResult(new OutputStatus()
        {
            StatusMessage = "Data written to in-memory state"
        });
    }

    private string[] ParseOutputPath()
    {
        return _settings.PropertyPath.Split('.');
    }
    
    public string GetDisplayData()
    {
        return System.Text.Json.JsonSerializer.Serialize(_data, new System.Text.Json.JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null
        });
    }
}