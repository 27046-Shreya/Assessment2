using BoilerApplication.Model;

namespace BoilerApplication.Context;

/// <summary>
/// Defines the file operations in CSV file format.
/// </summary>
public class CSVContext : IFileContext<string>
{
    private readonly string _filePath;
    
    public CSVContext(string filePath)
    {
        this._filePath = filePath;
        File.AppendAllText(_filePath,"LogInfo, Boiler State, Toggle state, Event Description, TimeStamp");
    }
    
    public List<string> ReadFromFile()
    {
        string content = File.ReadAllText(_filePath);
        List<string> logs = content.Split('\n').ToList();
        return logs;
    }

    public void WriteToFile(string log)
    {
        File.AppendAllText(_filePath, log);
    }
}
