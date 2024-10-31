namespace TestLib;
using System;
using Presenter;
using Model;
public class SongView : ISongView
{
    private SongPresenter _songPresenterLink;

    public SongView()
    {
        _songPresenterLink = new SongPresenter();
    }

    public async Task InitializeMenu()
    {
        Dictionary<int, Func<Task>> methodDictionary; 
        Dictionary<int, string > nameDictionary;
        // Словарь с ссылками на методы для меню
        methodDictionary = new Dictionary<int, Func<Task>>
        {
            { 1, ShowAllSongs },
            { 2, AddSong },
            { 3, RemoveSong },
            { 4, SearchSong }
        };

        // Словарь с именами методов
        nameDictionary = new Dictionary<int, string>
        {
            { 1, "Show All Songs" },
            { 2, "Add Song" },
            { 3, "Remove Song" },
            { 4, "Search Song" }
        };
        var menuView = new Menu(methodDictionary, nameDictionary);
        await menuView.DisplayHelp();
    }


    public async Task ShowAllSongs()
    {
        Console.WriteLine("Идём за всеми песнями");
        var songList = await _songPresenterLink.ShowSongPresenter();
        foreach (var song in songList)
        {
            Console.WriteLine(song.SongName.Name + " - " + song.SongAuthor.Author);
        }
    }

    public async Task AddSong()
    { 
        Console.WriteLine("Введите автора песни");
        var songAuthor = Console.ReadLine();
        Console.WriteLine("Введите название песни");
        var songName = Console.ReadLine();
        await _songPresenterLink.AddSongPresenter(songName, songAuthor);
        Console.WriteLine();
    }

    public async Task RemoveSong()
    {
        Console.WriteLine("Введите автора песни");
        var songAuthor = Console.ReadLine();
        // TODO - сделать вывод всех песен этого автора для удаления, норм не норм?
        Console.WriteLine("Введите название песни");
        var songName = Console.ReadLine();
        await _songPresenterLink.RemoveSongPresenter(songName, songAuthor);
    }

    public async Task<List<Song>> SearchSong()
    {
        
        List<Song> songs;
        Console.WriteLine("Введите название песни для поиска");
        var searchQuery = Console.ReadLine();
        songs = await _songPresenterLink.SongSearchPresenter(searchQuery);
        Console.WriteLine("Successfully returned with a list of songs");
        
        
        foreach (var song in songs)
        {
            Console.WriteLine(song.SongName.Name, song.SongAuthor.Author);
        }
        
        return songs;
    }
}