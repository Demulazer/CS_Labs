using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GetStartedApp.Models;
using Microsoft.Extensions.Configuration;

namespace GetStartedApp.Sevices
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5063/api/")
            };
            
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/show");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                return new List<Song>();
            }
        }

        public async Task AddSongAsync(string title, string artist)
        {
            var request = new
            {
                Title = title,
                Artist = artist
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("add", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error adding song: {response.StatusCode} - {errorMessage}");
            }
        }
        
        public async Task RemoveSongAsync(string songId)
        {
            var response = await _httpClient.DeleteAsync($"delete/{songId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
