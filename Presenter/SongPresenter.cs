using Model;

namespace Presenter;

public class SongPresenter : ISongPresenter
{
    private readonly ISongModel _songModelLink = new SongModel();

    public SongPresenter()
    {
        _songModelLink.InitializeSongListFromDatabase();
    }
    public SongPresenter(ISongModel songModel)
    {
        _songModelLink = songModel;
    }


    public async Task RemoveSongPresenter(string name, string author)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
            throw new ArgumentException("Both name and author must be provided and not null.");
        var song = await _songModelLink.CheckSong(new SongName(name), new SongAuthor(author));
        if (song == null)
            throw new ArgumentException("Specified song does not exist.");
        await _songModelLink.RemoveSong(song);
    }
    public async Task RemoveSongPresenter(int id)
    {
        Console.WriteLine("Debug - we are in RemoveSongPresenter, id branch, removing id " + id);
        var song = await _songModelLink.GetSongById(id);
        Console.WriteLine("Debug - we are still in RemoveSongPresenter, id branch, found song by id " + song);
        if (song == null)
            throw new ArgumentException("Specified song does not exist.");
        Console.WriteLine("Debug - found song " + song.SongName.Name + " - " + song.SongAuthor.Author + " -- " + song.Id);
        Console.WriteLine("Debug - trying to remove song");
        await _songModelLink.RemoveSong(song);
        Console.WriteLine("Debug - we are leaving RemoveSongPresenter, id branch");
    }
    
    public async Task<List<Song>> SongSearchPresenter(string findBy)
    { 
        if (string.IsNullOrEmpty(findBy)) 
            throw new ArgumentException("The search term cannot be null or empty.");
        List<Song> songs;
        if (findBy.Contains('-'))
        {
            Console.WriteLine("Debug - Found both name and author");

            var parts = findBy.Split("-");
            var songName = new SongName(parts[0]);
            var songAuthor = new SongAuthor(parts[1]);

            songs = await _songModelLink.FindSongByFull(songName, songAuthor);
            Console.WriteLine("Added Songs to found list");
        }
        else
        {
            Console.WriteLine("Debug - Found name only");

            songs = await _songModelLink.FindSongsByName(new SongName(findBy));
            
        }

        return songs;
    }

    public async Task<List<Song>> ShowSongPresenter()
    {
        Console.WriteLine("Debug: in ShowSongPresenter");
        return await _songModelLink.ShowSongs();
    }


    public async Task AddSongPresenter(string songName, string songAuthor)
    {
        Console.WriteLine("DEBUG - We are in presenter adding song" + songName + ", " + songAuthor);
        if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(songAuthor))
            throw new ArgumentException("Both name and author must be provided.");
        //TODO - что за warning Expression is always true according to nullable reference types' annotations?
        if (await _songModelLink.CheckSong(new(songName), new(songAuthor)) != null)
        {
            Console.WriteLine("Debug - found identical song");
            return;
        }

        var song = new Song(await _songModelLink.GetLastId() + 1, new SongName(songName), new SongAuthor(songAuthor));
        Console.WriteLine("Debug - adding song in presenter, id is " + song.Id );
        await _songModelLink.AddSong(song);
        
    }
}