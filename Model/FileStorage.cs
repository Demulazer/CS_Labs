using Newtonsoft.Json;

namespace Model;

public class FileStorage: IFileStorage
{
    private readonly string _filePath = "/home/demulazer/Documents/Stuff/Proga/test.json";

    // Инициализация данных из JSON-файла
    public void InitializeFromFile(out List<Song> songList)
    {
        if (File.Exists(_filePath))
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                songList = JsonConvert.DeserializeObject<List<Song>>(json) ?? throw new InvalidOperationException();
                Console.WriteLine("Successfully read " + songList.Count + " songs from file");
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
    }

    // Обновление JSON-файла с текущими данными
    public void UpdateFile(List<Song> songList)
    {
        try
        {
            var json = JsonConvert.SerializeObject(songList, Formatting.Indented);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("File updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong while updating the file: {ex.Message}");
        }
    }
}