namespace BoilerApplication.Context;

/// <summary>
/// Defines the file operations in CSV file format.
/// </summary>
public class CSVContext<T> : IFileContext<T>
{
    private readonly string _filePath;

    public CSVContext(string filePath)
    {
        this._filePath = filePath;
    }

    public List<Log>
}
