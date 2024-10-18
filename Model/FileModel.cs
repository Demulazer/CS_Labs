namespace Model;
//filepath 
//tablename
public class FileModel
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public FileModel(string fileName, string filePath)
    {
        FileName = fileName;
        FilePath = filePath;
    }
    
}

// await 
// async Task = void
// async Task<int> = int
// await (a.MethodAsync())