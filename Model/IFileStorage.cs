namespace Model;

public interface IFileStorage
{
    void InitializeFromFile(out List<Song> songList);
    void UpdateFile(List<Song> songList);
}