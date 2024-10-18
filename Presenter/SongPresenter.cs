using Model;

namespace Presenter;

public class SongPresenter : ISongPresenter
{
    private readonly SongModel _songModel;

    public SongPresenter(SongModel songModel)
    {
        _songModel = songModel;
    }

    public SongPresenter()
    {
        _songModel = new SongModel();
    }
    // Метод для проверки, что оба поля (name и author) не null или пусты
    public async Task CheckFullDataInput(string name, string author)
    {
        await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
            {
                throw new ArgumentException("Both name and author must be provided and not null.");
            }
        });
    }

    // Метод для проверки, что хотя бы одно из полей (name или author) не null или пустое
    public async Task CheckDataInput(string name, string author)
    {
        await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(author))
            {
                throw new ArgumentException("Either name or author must be provided and not null.");
            }
        });
    }

    // Метод для проверки доступности песни (например, поиск по имени или автору)
    public async Task<List<Song>> SplitSongNameForSearch(string findBy)
    {
        Console.WriteLine("Song is being processed");
        if (string.IsNullOrEmpty(findBy))
        {
            throw new ArgumentException("The search term cannot be null or empty.");
        }
        List<Song> songs = new List<Song>();
        if (findBy.Contains(" - "))
        {
            Console.WriteLine("Found both name and author");
            string[] parts = findBy.Split(" - ");
            SongName songName = new SongName(parts[0]);
            SongAuthor songAuthor = new SongAuthor(parts[1]);
            songs = _songModel.FindExactSong(songName, songAuthor);
            Console.WriteLine("Added Songs to found list");
        }
        else
        {
            Console.WriteLine("Found name only");
            songs = _songModel.FindSongsByOneField(new SongName(findBy));
            Console.WriteLine("Added Songs to found list");
        }
        return songs;
    }

    // Метод для проверки наличия одинаковых песен (заглушка)
    public async Task CheckIdenticalSong()
    {
        await Task.Run(() =>
        {
            throw new NotImplementedException();
        });
    }
}