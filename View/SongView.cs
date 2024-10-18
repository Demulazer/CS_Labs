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
        // Словарь с методами
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
            { 1, "ShowAllSongs" },
            { 2, "AddSong" },
            { 3, "RemoveSong" },
            { 4, "SearchSong" }
        };
        var menuView = new Menu(methodDictionary, nameDictionary);
        await menuView.DisplayHelp();
    }


    public async Task ShowAllSongs()
    {
        await Task.Run(() => Console.WriteLine(" "));
    }

    public async Task AddSong()
    {
        await Task.Run(() => Console.WriteLine(" "));
    }

    public async Task RemoveSong()
    {
        await Task.Run(() => Console.WriteLine(" "));
    }

    public async Task<List<Song>> SearchSong()
    {
        
        List<Song> songs = null;
        Console.WriteLine("Введите название песни для поиска");
        var searchQuery = Console.ReadLine();
        songs = await _songPresenterLink.SplitSongNameForSearch(searchQuery);
        Console.WriteLine("Successfully returned with a list of songs");
        
        
        foreach (var song in songs)
        {
            Console.WriteLine(song.SongName.Name, song.SongAuthor.Author);
        }
        
        return songs;
    }
}