using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GetStartedApp.Models;

namespace GetStartedApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
            
        }
        public async Task<List<Song>> GetAllSongsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/show");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                // Десериализация с учётом обёртки
                var apiResponse = JsonSerializer.Deserialize<SongsResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Преобразование в вашу структуру
                var apiSongs = apiResponse?.Songs ?? new List<ApiSong>();
                return apiSongs.Select(MapApiSongToSong).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                return new List<Song>();
            }
        }



        public async Task AddSongAsync(string songName, string songAuthor)
        {
            var request = new
            {
                songName,  // Используем имя поля, ожидаемое сервером
                songAuthor // Используем имя поля, ожидаемое сервером
            };

            var url = $"add?songName={Uri.EscapeDataString(songName)}&songAuthor={Uri.EscapeDataString(songAuthor)}";


            var response = await _httpClient.PutAsync(url, null); // Предполагается, что это POST-запрос
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Ответ сервера: {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error adding song: {response.StatusCode} - {errorMessage}");
            }
        }

        
        public async Task RemoveSongAsync(string songId)
        {
            Console.WriteLine("Removing song with id " + songId);
            var response = await _httpClient.DeleteAsync($"delete-by-id?id={songId}");
            response.EnsureSuccessStatusCode();
        }
        private static Song MapApiSongToSong(ApiSong apiSong)
        {
            return new Song
            {
                Id = apiSong.Id,
                Title = apiSong.SongName.Name,
                Artist = apiSong.SongAuthor.Author
            };
        }
    }
}
