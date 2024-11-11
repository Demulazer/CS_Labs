using Model;
using TestLib;

namespace Tests;

public class Start
{
    public static async Task Main(string[] args)
    {
        var songView = new SongView();
        await songView.InitializeMenu();
        //var connectionString = "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=postgres;";
        //var databaseStorage = new DatabaseStorage(connectionString);

// Пример добавления песни

// Пример загрузки всех песен
        //var songs = await databaseStorage.LoadSongsFromDatabaseAsync();
        
// Пример удаления песни по Id

    }
}