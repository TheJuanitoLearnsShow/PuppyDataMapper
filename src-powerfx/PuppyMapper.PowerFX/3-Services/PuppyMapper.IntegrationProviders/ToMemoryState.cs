using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class ToMemoryState : IProvideOutputData
{
    private readonly ToMemoryStateOptions _settings;
    private Dictionary<string, object> _data = new();

    public ToMemoryState(ToMemoryStateOptions settings)
    {
        _settings = settings;
    }

    public Task<OutputStatus> OutputData(List<Dictionary<string, object>> rows, bool simulateOnly = false)
    {
        var pathParts = ParseOutputPath();
        _data = new Dictionary<string, object>();
        for (var rowIdx = 0; rowIdx < rows.Count; rowIdx++)
        {
            var row = rows[rowIdx].ToDictionary();
            var currentLevel = _data;
            for (int pathPartIdx = 0; pathPartIdx < pathParts.Length; pathPartIdx++)
            {
                var pathPart = pathParts[pathPartIdx];
                var isArray = pathPart.StartsWith('[');
                if (isArray)
                {
                    pathPart = pathPart.Trim('[', ']');
                }
                if (pathPart == "RowIndex") 
                {
                    pathPart = rowIdx.ToString();
                }
                else if (row.TryGetValue(pathPart, out var value) ){
                    pathPart = value.ToString() ?? pathPart;
                }
                if (pathPartIdx == pathParts.Length - 1)
                {
                    if (isArray)
                    {
                        currentLevel[pathPart] = new CollectionItemWrapper(row);
                    }
                    else
                    {
                        currentLevel[pathPart] = row;
                    }
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
        ConvertCollectionWrappers(_data);
        
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
    
    // Recursively walk the dictionary and convert CollectionItemWrapper to arrays
    private void ConvertCollectionWrappers(object node)
    {
        if (node is Dictionary<string, object> dict)
        {
            var isArray = dict.Values.Any(v => v is CollectionItemWrapper);
            if (isArray)
            {
                var newItems = new List<Dictionary<string, object>>();
                foreach (var item in dict)
                {
                    int.TryParse(item.Key, out var idx);
                    var wrapper = item.Value as CollectionItemWrapper;
                    if (wrapper != null)
                    {
                        newItems.Insert(idx, wrapper.Row);
                    }
                }

                var exitingKeys = dict.Keys;
                foreach (var key in exitingKeys)
                {
                    dict.Remove(key);
                }
                dict["Items"] = newItems.ToArray();
            }
            else
            {
                foreach (var item in dict)
                {
                    ConvertCollectionWrappers(item.Value);
                }
            }
        }
    }
}

public class CollectionItemWrapper
{
    public Dictionary<string, object> Row { get; }

    public CollectionItemWrapper(Dictionary<string, object> row)
    {
       Row = row;
    }
}