namespace Model;


using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using Dapper;

public class DatabaseStorage : IDatabaseStorage
{   
    private readonly string _connectionString;

    public DatabaseStorage(string connectionString)
    {
        _connectionString = connectionString;
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<List<Song>> LoadSongsFromDatabaseAsync()
    {
        using (var connection = CreateConnection())
        {
            const string query = "SELECT Id, SongName, SongAuthor FROM Songs";
                
            var result = await connection.QueryAsync<(int Id, string Name, string Author)>(query);

            var songs = result.Select(row =>
                new Song(
                    row.Id,
                    new SongName( row.Name ),
                    new SongAuthor(row.Author ) 
                )).ToList();

            Console.WriteLine($"Successfully loaded {songs.Count} songs from the database.");
            return songs;
        }
    }


    public async Task SaveSongsToDatabaseAsync(List<Song> songs)
    {
        using (var connection = CreateConnection())
        {
            const string query1 = "DELETE FROM Songs";
            await connection.ExecuteAsync(query1, songs);
            const string query = "INSERT INTO Songs (Id, SongName, SongAuthor) VALUES (@Id, @Name, @Author)";
                
            foreach (var song in songs)
            {
                await connection.ExecuteAsync(query, new 
                {
                    Id = song.Id,
                    Name = song.SongName.Name, 
                    Author = song.SongAuthor.Author 
                });
            }

            Console.WriteLine("Successfully saved songs to the database.");
        }
    }
    public async Task AddSongToDatabaseAsync(Song song)
    {
        Console.WriteLine("Adding song to the database.");
        Console.WriteLine("Debug - trying to add song " +  song.Id + " " + song.SongAuthor.Author + " " + song.SongName.Name);
        using (var connection = CreateConnection())
        {
            const string query = "INSERT INTO Songs (Id, SongName, SongAuthor) VALUES (@Id, @Name, @Author)";
                
            await connection.ExecuteAsync(query, new 
            { 
                Id = song.Id,
                Name = song.SongName.Name, 
                Author = song.SongAuthor.Author 
            });

            Console.WriteLine("Song added to the database.");
        }
    }
    public async Task DeleteSongFromDatabaseAsync(Song song)
    {
        using (var connection = CreateConnection())
        {
            const string query = "DELETE FROM Songs WHERE Id = @Id";
                
            await connection.ExecuteAsync(query, new { Id = song.Id });

            Console.WriteLine($"Song with Id {song.Id} deleted from the database.");
        }
    }
    public async Task<Song?> GetLastSongAsync()
    {
        using (var connection = CreateConnection())
        {
            const string query = "SELECT Id, SongName, SongAuthor FROM Songs ORDER BY Id DESC LIMIT 1";
                
            var result = await connection.QueryFirstOrDefaultAsync<(int Id, string Name, string Author)>(query);
            Console.WriteLine("Debug - in getlastsongAsync " + result.Id + " " + result.Name);
            return new Song(
                    result.Id, 
                    new SongName(result.Name ), 
                    new SongAuthor(result.Author))
                ;
        }
    }
    public async Task<Song?> GetSongByIdAsync(int Id)
    {
        using (var connection = CreateConnection())
        {
            Console.WriteLine("Debug - we are in DatabaseStorage, removing, getting song from the database.");
            const string query = "SELECT Id, SongName, SongAuthor FROM Songs Where Id = @Id";
                
            var oldResult = await connection.QueryAsync<(int Id, string Name, string Author)>(query);
            var result = oldResult.LastOrDefault();
            Console.WriteLine("Debug, in DatabaseStorage - ", result.Id, result.Name, result.Author);
            return result != default 
                ? new Song(
                    result.Id, 
                    new SongName(result.Name ), 
                    new SongAuthor(result.Author))
                : null;
        }
    }
    public async Task<List<Song>> FindSongsByNameAsync(string name)
    {
        using (var connection = CreateConnection())
        {

            const string query = "SELECT Id, SongName, SongAuthor FROM Songs WHERE SongName::text LIKE @name";
            name = "%" + name + "%";
            var result = await connection.QueryAsync<(int Id, string Name, string Author)>(query, new { Name = name });

            var songs = result.Select(row =>
                new Song(
                    row.Id,
                    new SongName(row.Name ),
                    new SongAuthor(row.Author)
                )).ToList();
            if (songs.Count == 0) throw new ArgumentException("No songs found");
            return songs;
        }
    }

    // Поиск всех песен по названию и автору
    public async Task<List<Song>> FindSongsByNameAndAuthorAsync(string name, string author)
    {
        using (var connection = CreateConnection())
        {
            const string query = "SELECT Id, SongName, SongAuthor FROM Songs WHERE SongName = @Name AND SongAuthor = @Author";

            var result = await connection.QueryAsync<(int Id, string Name, string Author)>(query, new { Name = name, Author = author });

            var songs = result.Select(row =>
                new Song(
                    row.Id,
                    new SongName(row.Name),
                    new SongAuthor(row.Author)
                )).ToList();

            return songs;
        }
    }
}
