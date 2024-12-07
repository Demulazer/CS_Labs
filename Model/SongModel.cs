
namespace Model;

public class SongModel : ISongModel
{
    private IDatabaseStorage _databaseStorage = new DatabaseStorage("Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=postgres;");
     

    public async Task InitializeSongListFromDatabase()
    {
       await _databaseStorage.LoadSongsFromDatabaseAsync();
    }
    public SongModel ()
    {

    }
    public SongModel (IDatabaseStorage databaseStorage)
    {
        _databaseStorage = databaseStorage;
    }

    public async Task<Song> GetSongById(int id)
    {
        return await _databaseStorage.GetSongByIdAsync(id);
    }
    public int GetLastId()
    {
        return _databaseStorage.GetLastSongAsync().Id;
    }
    public async Task<List<Song>> FindSongByFull(SongName searchSongName, SongAuthor searchSongAuthor)
    {
        //возвращаем песню по точному совпадению имени / автора
        var toReturn = await _databaseStorage.FindSongsByNameAndAuthorAsync(searchSongName.Name, searchSongAuthor.Author);
        if(toReturn.Count == 0)
               throw new InvalidOperationException("Name or Author is invalid, or found no songs. Please try again.");
        return toReturn;
    }

    // Метод, который возвращает список песен, если заполнено только одно из полей
    public async Task<List<Song>> FindSongsByName(SongName searchSongName)
    {
        var toReturn = await _databaseStorage.FindSongsByNameAsync(searchSongName.Name);
        if(toReturn.Count == 0)
            throw new InvalidOperationException("Name is invalid, or found no songs. Please try again."); 
        return toReturn;
    }

    public async Task RemoveSong(Song song)
    {
        await _databaseStorage.DeleteSongFromDatabaseAsync(song);
    }

    public async Task<Song> CheckSong(SongName songName, SongAuthor songAuthor)
    {
        var songs = await _databaseStorage.FindSongsByNameAndAuthorAsync(songName.Name, songAuthor.Author);
        
        return songs.FirstOrDefault();
    }

    public async Task<List<Song>> ShowSongs()
    {
        return await _databaseStorage.LoadSongsFromDatabaseAsync();
    }

    public async Task AddSong(Song song)
    {
        await _databaseStorage.AddSongToDatabaseAsync(song);
    }
    
    
}