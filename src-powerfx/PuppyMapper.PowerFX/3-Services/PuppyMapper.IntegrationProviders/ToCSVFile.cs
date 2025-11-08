using System.Globalization;
using System.Text.Json;
using CsvHelper;
using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class ToCSVFile : IProvideOutputData, IDisposable
{
    private readonly ToCSVFileOptions _settings;
    private CsvWriter? _currCsv;
    private Dictionary<string, CsvWriter> _writers = new Dictionary<string, CsvWriter>();
    private string? _currFilePath;

    public ToCSVFile(ToCSVFileOptions settings)
    {
        _settings = settings;
        OutputId = settings.OutputId;
    }

    public string OutputId { get; init; }

    private bool NeedsNewWriter(Dictionary<string, object> row)
    {
        var finalFilePath = row.Keys.Aggregate(_settings.FilePath, (currFilePath, key) => 
            currFilePath
            .Replace($"{{{key}}}", row[key].ToString()));
        var hasChanged = (_currFilePath != finalFilePath);
        _currFilePath = finalFilePath;
        return hasChanged;
    }

    private async Task SwitchWriter(Dictionary<string, object> row)
    {
        var writerExists = _writers.TryGetValue(_currFilePath!, out _currCsv);
        if (!writerExists)
        {
            await InitializeNewWriter(row, _currFilePath!);
        }
    }

    private async Task InitializeNewWriter(Dictionary<string, object> row, string finalFilePath)
    {
        var fileStream = new StreamWriter(finalFilePath);
        _currCsv = new CsvWriter(fileStream, CultureInfo.InvariantCulture);
        _writers.Add(finalFilePath, _currCsv);
        foreach (var prop in row.Keys)
        {
            _currCsv.WriteField(prop);
        }
        await _currCsv.NextRecordAsync();
    }


    public void Dispose()
    {
        FinishWriters();
    }

    private void FinishWriters()
    {
        var kvs = _writers.ToList();
        foreach (var kv in kvs)
        {
            var csvWriter = kv.Value;
            csvWriter.Flush();
            csvWriter.Dispose();
            _writers.Remove(kv.Key);
        }
    }

    public async Task<OutputStatus> OutputData(List<Dictionary<string, object>> rows, bool simulateOnly = false)
    {
        foreach (var row in rows)
        {
            if (NeedsNewWriter(row))
            {
                await SwitchWriter(row);
            }
            foreach (var colValue in row.Values)
            {
                _currCsv!.WriteField(colValue);
            }
            await _currCsv!.NextRecordAsync();
        }

        FinishWriters();
        return new OutputStatus()
        {
            StatusMessage = $"Data written to {_settings.FilePath}"
        };
    }

    public string GetDisplayData()
    {
        return string.Empty;
    }
}