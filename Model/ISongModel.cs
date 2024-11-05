namespace Model;

public interface ISongModel
{
    public Task<Song> GetSongById(int id);
    public Task<List<Song>> FindSongByFull(SongName searchSongName, SongAuthor searchSongAuthor);
    public Task<List<Song>> FindSongsByName(SongName searchSongName);
    public Task RemoveSong(Song song);
    public Task<Song> CheckSong(SongName songName, SongAuthor songAuthor);
    public Task<List<Song>> ShowSongs();
    public Task AddSong(Song song);
    public Task InitializeSongListFromFile();
    public int GetLastId();
}