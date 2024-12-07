namespace Model;

public interface IDatabaseStorage
{
    public Task<List<Song>> LoadSongsFromDatabaseAsync();
    public Task SaveSongsToDatabaseAsync(List<Song> songs);
    public Task DeleteSongFromDatabaseAsync(Song song);
    public Task AddSongToDatabaseAsync(Song song);
    public Task<Song?> GetSongByIdAsync(int idToFind);
    public Task<Song?> GetLastSongAsync();

    public Task<List<Song>> FindSongsByNameAndAuthorAsync(string name, string author);
    public Task<List<Song>> FindSongsByNameAsync(string name);
}