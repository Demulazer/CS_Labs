using Newtonsoft.Json;

namespace Model;

public class FileStorage: IFileStorage
{
    private const string FilePath = "/home/demulazer/Documents/Stuff/Proga/test.json";

    // Инициализация данных из JSON-файла
    public async Task<List<Song>> InitializeFromFile()
    {
        List<Song> songList;
        if (File.Exists(FilePath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(FilePath);
                songList = JsonConvert.DeserializeObject<List<Song>>(json) ?? throw new InvalidOperationException();
                Console.WriteLine("Successfully read " + songList.Count + " songs from file");
                //foreach (var song in songList)
                //{
                //    Console.WriteLine(song.Id);
               // }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных из файла: {ex.Message}");
                songList = new List<Song>(); // Создаем пустой список, если произошла ошибка
            }
        }
        else
        {
            songList = new List<Song>();
        }
        return songList;
    }

    // Обновление JSON-файла с текущими данными
    public async Task UpdateFile(List<Song> songList)
    {
        try
        {
            var json = JsonConvert.SerializeObject(songList, Formatting.Indented);
            await File.WriteAllTextAsync(FilePath, json);
            Console.WriteLine("File updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong while updating the file: {ex.Message}");
        }
    }
}