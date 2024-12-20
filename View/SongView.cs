using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Presenter;

namespace TestLib;

public class SongView : ISongView
{
    private readonly SongPresenter _songPresenterLink = new();

    public async Task ShowAllSongs()
    {
        Console.WriteLine("Retrieving songs...");
        var songList = await _songPresenterLink.ShowSongPresenter();
        Console.WriteLine("Retrieved songs...");
        if (songList == null)
        {
            Console.WriteLine("No songs found");
        }
        else
        {
            foreach (var song in songList)
            {
                Console.WriteLine(song.Id + ". " + song.SongName.Name + " - " + song.SongAuthor.Author);
            }
        }
    }

    public async Task AddSong()
    {
        Console.WriteLine("Введите автора песни");
        var songAuthor = Console.ReadLine();
        Console.WriteLine("Введите название песни");
        var songName = Console.ReadLine();
        await _songPresenterLink.AddSongPresenter(songName!, songAuthor!);
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
            Console.WriteLine("Enter song name");
            var songName = Console.ReadLine();
            await _songPresenterLink.RemoveSongPresenter(songName!, input);
        }
        Console.WriteLine("Song removed successfully");
    }

    public async Task<List<Song>> SearchSong()
    {
        List<Song> songs;
        Console.WriteLine("Enter song name for search");
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
    public void InitializeApi()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddEndpointsApiExplorer();
        
        var app = builder.Build();
        // app.UseHttpsRedirection();
        // app.MapGet("/show", async () =>
        // {
        //     return await _songPresenterLink.ShowSongPresenter();
        // }).WithName("ShowSongs");
        // app.MapPut("/add", async (string songName, string songAuthor) =>
        // {
        //     if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(songAuthor))
        //         return Results.BadRequest("Invalid song data.");
        //     await _songPresenterLink.AddSongPresenter(songName,songAuthor);
        //     return Results.Ok("Song added successfully.");
        //    
        // }).WithName("AddSong");
        // app.MapDelete("/delete-by-id", async (int id) =>
        // {
        //     if (id < 0)
        //         return Results.BadRequest("Invalid id data.");
        //     await _songPresenterLink.RemoveSongPresenter(id);
        //     return Results.Ok("Song deleted by id successfully.");
        // }).WithName("DeleteById");
        //
        // app.MapDelete("/delete-by-name-and-author", async (string name, string author) =>
        // {
        //     if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
        //         return Results.BadRequest("Invalid song data.");
        //     await _songPresenterLink.RemoveSongPresenter(name, author);
        //     return Results.Ok("Song deleted successfully.");
        // }).WithName("DeleteByNameAndAuthor");
        AddEndpoint.Map(app);
        SearchEndpoint.Map(app);
        DeleteEndpoint.MapId(app);
        DeleteEndpoint.MapName(app);
        ShowEndpoint.Map(app);
        // app.MapGet("/search", async (string find) =>
        // {
        //     if (string.IsNullOrEmpty(find))
        //         return null;
        //     return await _songPresenterLink.SongSearchPresenter(find);
        // }).WithName("Search");
        app.Run();
    }
}