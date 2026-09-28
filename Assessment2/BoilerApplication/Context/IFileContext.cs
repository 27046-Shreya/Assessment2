namespace BoilerApplication.Context;

/// <summary>
/// Defines the signature of the file context of different file formats.
/// </summary>
public interface IFileContext<T>
{
    /// <summary>
    /// Reads the contents of the file.
    /// </summary>
    /// <returns>List of instances present in the file.</returns>
    public List<T> ReadFromFile();

    /// <summary>
    /// Writes the content into the file.
    /// </summary>
    /// <param name="writeInstance">instance to be written into the file</param>
    public void WriteToFile(T writeInstance);
}
