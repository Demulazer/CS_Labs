namespace Model;

public interface ISongStorage
{
    Task AddSong(Song song);
    Task DeleteSong(string name, string author);
    Task<Song> FindSong(string findBy);
    Task<List<Song>> GetSongs();
}