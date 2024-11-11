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
            const string query = "INSERT INTO Songs (SongName, SongAuthor) VALUES (@Name, @Author)";
                
            foreach (var song in songs)
            {
                await connection.ExecuteAsync(query, new 
                { 
                    Name = song.SongName.Name, 
                    Author = song.SongAuthor.Author 
                });
            }

            Console.WriteLine("Successfully saved songs to the database.");
        }
    }
}
