using PuppyDataMapper;

public class TableParser {
    public FieldMap GetMappings(string[] headers, string line)
    {
        var cols = line.Split('|').Select(c => c.Trim()).Skip(1).ToArray();
        var fieldMap = new FieldMap();
        
        
        for (int headerIdx = 0; headerIdx < headers.Length; headerIdx++)
        {
            var headerName = headers[headerIdx];
            if (headerName.StartsWith("Output", StringComparison.CurrentCultureIgnoreCase))
            {
                fieldMap.OutputTo = cols[headerIdx];
            }
            else if (headerName.StartsWith("Formula", StringComparison.CurrentCultureIgnoreCase))
            {
                fieldMap.Formula = cols[headerIdx];
            }
            else if (headerName.StartsWith("Comments", StringComparison.CurrentCultureIgnoreCase))
            {
                fieldMap.Comments = cols[headerIdx];
            }
            else if (headerName.StartsWith("Type", StringComparison.CurrentCultureIgnoreCase))
            {
                fieldMap.OutputType = cols[headerIdx];
            }
        }
        return fieldMap;
    }
}