namespace Model;


using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using Dapper;

public class DatabaseStorage
{   
    private readonly string _connectionString;

    public DatabaseStorage(string connectionString)
    {
        _connectionString = connectionString;
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<List<Song>> LoadSongsAsync()
    {
        using var connection = CreateConnection();
        const string query = "SELECT Id, SongName, SongAuthor FROM Songs";
        
        var result = await connection.QueryAsync<(int Id, string Name, string Author)>(query);

        // Преобразуем результат запроса в список объектов Song
        var songs = result.Select(row =>
            new Song(
                row.Id,
                new SongName(row.Name),
                new SongAuthor(row.Author)
            )).ToList();

        return songs;
    }


    public async Task AddSongAsync(Song song)
    {
        using var connection = CreateConnection();
        const string query = "INSERT INTO Songs (SongName, SongAuthor) VALUES (@Name, @Author)";
        await connection.ExecuteAsync(query, new { Name = song.SongName.Name, Author = song.SongAuthor.Author });
    }

    public async Task DeleteSongAsync(int id)
    {
        using var connection = CreateConnection();
        const string query = "DELETE FROM Songs WHERE Id = @Id";
        await connection.ExecuteAsync(query, new { Id = id });
    }
}
