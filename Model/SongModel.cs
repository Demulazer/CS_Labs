
namespace Model;

public class SongModel : ISongModel
{
    private IDatabaseStorage _databaseStorage = new DatabaseStorage("Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=postgres;");
    public List<Song> _songList;
    

    public async Task InitializeSongListFromDatabase()
    {
        _songList = await _databaseStorage.LoadSongsFromDatabaseAsync();
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
        foreach (var song in _songList)
        {
            if (song.Id == id)
                return song;
        }
        return null!;
    }
    public int GetLastId()
    {
        return _songList.Last().Id;
    }
    public async Task<List<Song>> FindSongByFull(SongName searchSongName, SongAuthor searchSongAuthor)
    {
        //возвращаем песню по точному совпадению имени / автора
        var toReturn = _songList
            .Where(song => song.SongName.Name.Contains(searchSongName.Name) &&
                           song.SongAuthor.Author == searchSongAuthor.Author).ToList();
        if(toReturn.Count == 0)
               throw new InvalidOperationException("Name or Author is invalid, or found no songs. Please try again.");
        return toReturn;
    }

    // Метод, который возвращает список песен, если заполнено только одно из полей
    public async Task<List<Song>> FindSongsByName(SongName searchSongName)
    {
        var toReturn = _songList
            .Where(song =>
                song.SongName.Name == searchSongName.Name || song.SongName.Name.Contains(searchSongName.Name))
            .ToList();
        if(toReturn.Count == 0)
            throw new InvalidOperationException("Name is invalid, or found no songs. Please try again."); 
        Console.WriteLine("Debug - " + toReturn.Count);
        return toReturn;
    }

    public async Task RemoveSong(Song song)
    {
        _songList.Remove(song);
        await _databaseStorage.SaveSongsToDatabaseAsync(_songList);
    }

    public async Task<Song> CheckSong(SongName songName, SongAuthor songAuthor)
    {
        foreach (var song in _songList)
            if (songName.Name == song.SongName.Name && song.SongAuthor.Author == songAuthor.Author)
                return song;
        return null!;
    }

    public async Task<List<Song>> ShowSongs()
    {
        if (_songList.Count == 0)
        {
            return null;
        }
        foreach (var song in _songList)
        {
            Console.WriteLine("Debug - " + song.SongName.Name);
        }
        return _songList;
    }

    public async Task AddSong(Song song)
    {
        _songList.Add(song);
        await _databaseStorage.SaveSongsToDatabaseAsync(_songList);
    }
    
    
}