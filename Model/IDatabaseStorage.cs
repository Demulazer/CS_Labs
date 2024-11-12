namespace Model;

public interface IDatabaseStorage
{
    public Task<List<Song>> LoadSongsFromDatabaseAsync();
    public Task SaveSongsToDatabaseAsync(List<Song> songs);
}