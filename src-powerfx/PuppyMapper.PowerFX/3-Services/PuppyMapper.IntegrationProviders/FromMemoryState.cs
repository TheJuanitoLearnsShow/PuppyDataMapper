using System.Text.Json;
using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class FromMemoryState : IProvideInputData
{
    private readonly string _propertyPath;
    private readonly string[] _pathParts;
    private bool _isInitialized = false;
    private Dictionary<string, object>[] _targetItems;
    private int _currRowIndex;

    public FromMemoryState(FromMemoryStateOptions settings)
    {
        _propertyPath = settings.PropertyPath;
        _pathParts = ParseOutputPath();
        InputId = settings.InputId;
    }
    
    private void Initialize()
    {
        if (_isInitialized) return;
        var data = MemorySateManager.GetState();
        var currentLevel = data;
        for (var pathPartIdx = 0; pathPartIdx < _pathParts.Length; pathPartIdx++)
        {
            var pathPart = _pathParts[pathPartIdx];
            var isArray = pathPart.StartsWith('[');
            if (isArray)
            {
                pathPart = pathPart.Trim('[', ']');
            }


            var isLastPartOfPath = pathPartIdx == _pathParts.Length - 1;
            if (isLastPartOfPath)
            {
                currentLevel.TryGetValue(pathPart, out var targetData);
                
                if (targetData == null)
                {
                    _targetItems = [];
                }
                else if (targetData.GetType().IsArray)
                {
                    _targetItems = targetData as Dictionary<string, object>[] ?? [];
                }
                else
                {
                    _targetItems =
                    [
                        targetData as Dictionary<string, object> ?? new Dictionary<string, object>()
                    ];
                        
                }
                break;
            } 
            if (!currentLevel.ContainsKey(pathPart) ||
                currentLevel[pathPart] is not Dictionary<string, object> nextLevel)
            {
                nextLevel = new Dictionary<string, object>();
                currentLevel[pathPart] = nextLevel;
            }

            currentLevel = nextLevel;
        }
        _currRowIndex = 0;
        _isInitialized = true;
    }
    public Task<string?> GetRecordAsJson()
    {
        Initialize();
        if (_currRowIndex >= _targetItems.Length)
        {
            return Task.FromResult<string?>(null);
        }
        var data = _targetItems[_currRowIndex];
        _currRowIndex++;
        return Task.FromResult(JsonSerializer.Serialize(data))!;
    }

    public string InputId { get; init; }
    
    private string[] ParseOutputPath()
    {
        return _propertyPath.Split('.');
    }
}