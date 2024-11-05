
namespace Model;

public class SongModel
{
    private readonly List<Song> _songList;
    private readonly FileStorage _fileStorage = new();
    
    public SongModel()
    {
        _fileStorage.InitializeFromFile(out _songList);
    }

    public async Task<Song> GetSongById(int id)
    {
        foreach (var song in _songList)
        {
            if (song.Id == id)
                return song;
        }
        return null;
    }
    public int GetLastId()
    {
        return _songList.Last().Id;
    }
    public async Task<List<Song>> FindSongByFull(SongName searchSongName, SongAuthor searchSongAuthor)
    {
        //возвращаем песню по точному совпадению имени / автора
        return _songList
                   .Where(song => song.SongName.Name.Contains(searchSongName.Name) &&
                                  song.SongAuthor.Author == searchSongAuthor.Author).ToList() ??
               throw new InvalidOperationException();
    }

    // Метод, который возвращает список песен, если заполнено только одно из полей
    public async Task<List<Song>> FindSongsByName(SongName searchSongName)
    {
        return _songList
            .Where(song =>
                song.SongName.Name == searchSongName.Name || song.SongName.Name.Contains(searchSongName.Name))
            .ToList();
    }

    public async Task RemoveSong(Song song)
    {
        await Task.Run(() =>
        {
            _songList.Remove(song);
            _fileStorage.UpdateFile(_songList);
        });
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
        foreach (var song in _songList)
        {
            Console.WriteLine("yes - " + song.SongName.Name);
        }
        return _songList;
    }

    public async Task AddSong(Song song)
    {
        //TODO - как сюда асинхронность то добавить, через Task.Run()?
        _songList.Add(song);
        _fileStorage.UpdateFile(_songList);
    }
    
    
}