namespace Model;

public class SongModel
{
    private readonly List<Song> _songList;
    public SongModel(List<Song> songList)
    {
        _songList = songList;
    }
    public SongModel()
    {
        _songList = new List<Song>();
        _songList.Add( new Song(new SongName("Name1"), new SongAuthor("Author1")));
        _songList.Add( new Song(new SongName("Name2"), new SongAuthor("Author2")));
        _songList.Add( new Song(new SongName("Name3"), new SongAuthor("Author3")));
        _songList.Add( new Song(new SongName("Name4"), new SongAuthor("Author4")));
        _songList.Add( new Song(new SongName("Name4"), new SongAuthor("Author4")));
        _songList.Add( new Song(new SongName("Name41"), new SongAuthor("Author4")));
        _songList.Add( new Song(new SongName("Name42"), new SongAuthor("Author4")));
        _songList.Add( new Song(new SongName("Name43"), new SongAuthor("Author4")));
    }
    public List<Song> FindExactSong(SongName searchSongName, SongAuthor searchSongAuthor)
    {
        //возвращаем песню по точному совпадению имени / автора
            return _songList
                .Where(song => song.SongName.Name.Contains(searchSongName.Name) && song.SongAuthor.Author == searchSongAuthor.Author).ToList() ?? throw new InvalidOperationException();
    }

    // Метод, который возвращает список песен, если заполнено только одно из полей
    public List<Song> FindSongsByOneField(SongName searchSongName)
    {
        Console.WriteLine("Looking for songs by name");
        // Если заполнено только поле Name
        if (!string.IsNullOrEmpty(searchSongName.Name))
        {
            Console.WriteLine("we must be stuck?");
            return _songList
                .Where(song => song.SongName.Name == searchSongName.Name || song.SongName.Name.Contains(searchSongName.Name))
                .ToList();
        }
        Console.WriteLine("No songs found");
        // Если ни одно поле не заполнено, возвращаем пустой список
        return new List<Song>();
    }
}
