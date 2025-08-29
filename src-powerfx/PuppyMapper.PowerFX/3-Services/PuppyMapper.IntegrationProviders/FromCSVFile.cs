using CsvHelper;
using System.Globalization;
using System.Text.Json;
using System.IO;
using PuppyMapper.PowerFX.Service.Integration;

public class FromCSVFile : IProvideInputData, IDisposable
{
    private readonly StreamReader _reader;
    private readonly CsvReader _csv;
    private bool _isInitialized = false;

    public FromCSVFile(FromCSVFileOptions settings)
    {
        _reader = new StreamReader(settings.FilePath);
        _csv = new CsvReader(_reader, CultureInfo.InvariantCulture);
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            await _csv.ReadAsync();
            _csv.ReadHeader();
            _isInitialized = true;
        }
    }

    public async Task<string?> GetRecordAsJson()
    {
        await InitializeAsync();
        if (await _csv.ReadAsync())
        {
            var record = _csv.GetRecord<dynamic>();
            return JsonSerializer.Serialize(record);
        }
        return null;
    }

    public void Dispose()
    {
        _reader.Dispose();
        _csv.Dispose();
    }
}