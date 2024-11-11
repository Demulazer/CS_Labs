using Model;

namespace Tests;

public class Start
{
    public static async Task Main(string[] args)
    {
        //var songView = new SongView();
        //await songView.InitializeMenu();
        var connectionString = "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=postgres;";
        var databaseStorage = new DatabaseStorage(connectionString);

// Пример добавления песни

// Пример загрузки всех песен
        var songs = await databaseStorage.LoadSongsAsync();
        Console.WriteLine(songs.Count);
        await databaseStorage.AddSongAsync(new Song(0, new SongName("Название"), new SongAuthor("Автор")));
// Пример удаления песни по Id

    }
}