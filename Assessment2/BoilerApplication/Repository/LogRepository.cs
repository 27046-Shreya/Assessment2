using BoilerApplication.Context;
using BoilerApplication.Model;

namespace BoilerApplication.Repository;

/// <summary>
/// Defines the read and write operations of logs.
/// </summary>
public class LogRepository
{
    private readonly IFileContext<BoilerEventLog> _fileContext;

    public LogRepository(IFileContext<BoilerEventLog> fileContext)
    {
        this._fileContext = fileContext;
    }

    public List<BoilerEventLog> ReadLogs()
    {
        return this._fileContext.ReadFromFile();
    }

    public void WriteLog(BoilerEventLog log)
    {
        this._fileContext.WriteToFile(new List<BoilerEventLog> { log });
    }

}
