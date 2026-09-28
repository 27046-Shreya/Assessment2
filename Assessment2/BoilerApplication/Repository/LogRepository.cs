using BoilerApplication.Context;
using BoilerApplication.Model;

namespace BoilerApplication.Repository;

/// <summary>
/// Defines the read and write operations of logs.
/// </summary>
public class LogRepository
{
    private readonly IFileContext<string> _fileContext;

    public LogRepository(IFileContext<string> fileContext)
    {
        this._fileContext = fileContext;
    }

    public List<string> ReadLogs()
    {
        return this._fileContext.ReadFromFile();
    }

    public void WriteLog(BoilerEventLog log)
    {
        this._fileContext.WriteToFile($"{log.Type}, {log.State}, {log.ToggleState}, {log.EventName}, {log.TimeStamp}\n");
    }

}
