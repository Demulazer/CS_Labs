namespace Model;

public interface IFileStorage
{
    Task<List<Song>> InitializeFromFile();
    Task UpdateFile(List<Song> songList);
}