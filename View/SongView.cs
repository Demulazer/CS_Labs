using Model;
using Presenter;

namespace TestLib;

public class SongView : ISongView
{
    private readonly SongPresenter _songPresenterLink;

    public SongView()
    {
        _songPresenterLink = new SongPresenter();
    }


    public async Task ShowAllSongs()
    {
        Console.WriteLine("Retrieving songs...");
        var songList = await _songPresenterLink.ShowSongPresenter();
        Console.WriteLine("Retrieved songs...");
        foreach (var song in songList)
        {
            Console.WriteLine(song.Id + ". " + song.SongName.Name + " - " + song.SongAuthor.Author);
        }
    }

    public async Task AddSong()
    {
        Console.WriteLine("Введите автора песни");
        var songAuthor = Console.ReadLine();
        Console.WriteLine("Введите название песни");
        var songName = Console.ReadLine();
        var result = await _songPresenterLink.AddSongPresenter(songName!, songAuthor!);
        Console.WriteLine(!result ? "Identical song already exists" : "Song added successfully");
    }

    public async Task RemoveSong()
    {
        Console.WriteLine("Введите автора песни, или её ID");
        var input = Console.ReadLine();
        //если ввели Id
        Console.WriteLine("Read user input");
        if (input == null)
        {
            Console.WriteLine("Bad input");
            return;
        }
        if (input.All (x => !char.IsLetter(x)))
        {
            await _songPresenterLink.RemoveSongPresenter(int.Parse(input));
        }
        else
        {
            Console.WriteLine("Введите название песни");
            var songName = Console.ReadLine();
            await _songPresenterLink.RemoveSongPresenter(songName!, input);
        }
        Console.WriteLine("Song removed successfully");
    }

    public async Task<List<Song>> SearchSong()
    {
        List<Song> songs;
        Console.WriteLine("Введите название песни для поиска");
        var searchQuery = Console.ReadLine();
        songs = await _songPresenterLink.SongSearchPresenter(searchQuery);

        foreach (var song in songs) Console.WriteLine(song.Id + ". " +  song.SongName.Name + " - "  + song.SongAuthor.Author);

        return songs;
    }

    private Task ExitProgram()
    {
        Console.WriteLine("Shutting down due to user input");
        Environment.Exit(42);
        return Task.CompletedTask;
    }

    public async Task InitializeMenu()
    {
        Dictionary<int, Func<Task>> methodDictionary;
        Dictionary<int, string> nameDictionary;
        // Словарь с ссылками на методы для меню
        methodDictionary = new Dictionary<int, Func<Task>>
        {
            { 1, ShowAllSongs },
            { 2, AddSong },
            { 3, RemoveSong },
            { 4, SearchSong },
            {5, ExitProgram}
        };

        // Словарь с именами методов
        nameDictionary = new Dictionary<int, string>
        {
            { 1, "Show All Songs" },
            { 2, "Add Song" },
            { 3, "Remove Song" },
            { 4, "Search Song" },
            {5, "Exit"}
        };
        var menuView = new Menu(methodDictionary, nameDictionary);
        await menuView.OptionsLoop();
    }
}