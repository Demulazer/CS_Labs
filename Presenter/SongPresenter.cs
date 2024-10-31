using Model;

namespace Presenter;

public class SongPresenter : ISongPresenter
{
    private readonly SongModel _songModelLink;
    

    public SongPresenter()
    {
        _songModelLink = new SongModel();
    }

    // Метод для проверки, что хотя бы одно из полей (name или author) не null или пустое
    public async Task RemoveSongPresenter(string name, string author)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
        {
            throw new ArgumentException("Both name and author must be provided and not null.");
        }
        Song song = await _songModelLink.CheckSong(new SongName(name), new SongAuthor(author));
        if (song == null)
        {
            throw new ArgumentException("Specified song does not exist.");
        }
        await _songModelLink.RemoveSong(song);
    }

    // Метод для проверки доступности песни (например, поиск по имени или автору)
    public async Task<List<Song>> SongSearchPresenter(string findBy)
    {
        Console.WriteLine("Song is being processed");
        if (string.IsNullOrEmpty(findBy))
        {
            throw new ArgumentException("The search term cannot be null or empty.");
        }
        List<Song> songs;
        if (findBy.Contains(" - "))
        {
            Console.WriteLine("Found both name and author");
            
            string[] parts = findBy.Split(" - ");
            SongName songName = new SongName(parts[0]);
            SongAuthor songAuthor = new SongAuthor(parts[1]);
            
            songs =  await _songModelLink.FindSongByFull(songName, songAuthor);
            Console.WriteLine("Added Songs to found list");
        }
        else
        {
            Console.WriteLine("Found name only");
            
            songs = await _songModelLink.FindSongsByName(new SongName(findBy));
            
            Console.WriteLine("Added Songs to found list");
        }
        return songs;
    }

    public async Task AddSongPresenter(string songName, string songAuthor)
    {
        if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(songAuthor))
        {
            throw new ArgumentException("Both name and author must be provided.");
        }

        if (await _songModelLink.CheckSong(new SongName(songName), new SongAuthor(songAuthor)) != null)
        {
            Console.WriteLine("Song already exists");
            return;
        }
        var song = new Song(new Guid(), new SongName(songName), new SongAuthor(songAuthor));
        await _songModelLink.AddSong(song);
    }
    public async Task<List<Song>> ShowSongPresenter()
    {
        return await _songModelLink.ShowSongs();
    }
    
}