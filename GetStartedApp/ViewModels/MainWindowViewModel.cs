using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Models;
using GetStartedApp.Services;

namespace GetStartedApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    public static ObservableCollection<Song> Songs { get; } = new();

    [ObservableProperty]
    private ObservableCollection<Song> _filteredSongs = Songs;

    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(AddSongCommand))]
    private int _newSongId;
    
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(AddSongCommand))]
    private string? _newSongTitle;
    
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(AddSongCommand))]
    private string? _newSongArtist;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SearchSongCommand))]
    private string? _searchText;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SearchSongCommand))]
    private string? _searchAuthor;

    [ObservableProperty]
    private bool _isSearchVisible;
    public MainWindowViewModel()
    {

        _apiService = new ApiService();
        LoadSongsCommand = new AsyncRelayCommand(LoadSongsAsync);
        AddSongCommand = new AsyncRelayCommand(AddSongAsync);
        RemoveSongCommand = new AsyncRelayCommand<Song>(RemoveSongAsync);
        DoNothingCommand = new MyOwnSuperUltraCommand(DoNothing);
        LoadSongsAsync().ConfigureAwait(false);
    }
    public  ICommand DoNothingCommand { get; }
    public IAsyncRelayCommand LoadSongsCommand { get; }
    public IAsyncRelayCommand AddSongCommand { get; }
    public IAsyncRelayCommand<Song> RemoveSongCommand { get; }
        
    partial void OnSearchTextChanged(string? value) => SearchSong();
    
    partial void OnSearchAuthorChanged(string? value) => SearchSong();
        
    private bool CanSearchSong() => !string.IsNullOrWhiteSpace(SearchText) || !string.IsNullOrWhiteSpace(SearchAuthor);
    public bool CanAddSong => !string.IsNullOrWhiteSpace(NewSongTitle) && !string.IsNullOrWhiteSpace(NewSongArtist);

    public bool DoNothing(object o)
    {
        return true;
    }
    partial void OnNewSongTitleChanged(string? value)
    {
        OnPropertyChanged(nameof(CanAddSong));
    }
    partial void OnNewSongArtistChanged(string? value)
    {
        OnPropertyChanged(nameof(CanAddSong));
    }


    [RelayCommand(CanExecute = nameof(CanSearchSong))]
    private void SearchSong()
    {
        var query = Songs.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(song => song.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(SearchAuthor))
        {
            query = query.Where(song => song.Artist.Contains(SearchAuthor, StringComparison.OrdinalIgnoreCase));
        }

        FilteredSongs = new ObservableCollection<Song>(query);
    }
        
    [RelayCommand]
    private void ToggleSearch()
    {
        IsSearchVisible = !IsSearchVisible;
        if (!IsSearchVisible)
        {
            // Очищаем поля поиска
            SearchText = null;
            SearchAuthor = null;

            // Применяем фильтр для отображения всех песен
            SearchSong();
        }
    }
    private async Task LoadSongsAsync()
    {
        try
        {
            var songs = await _apiService.GetAllSongsAsync();
            Songs.Clear();
            foreach (var song in songs)
            {
                Songs.Add(song);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки песен: {ex.Message}");
        }
    }

    private async Task AddSongAsync()
    {
        if (string.IsNullOrWhiteSpace(NewSongTitle) || string.IsNullOrWhiteSpace(NewSongArtist))
            return;

        try
        {
            await _apiService.AddSongAsync(NewSongTitle, NewSongArtist);
                
            NewSongTitle = null;
            NewSongArtist = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка добавления песни: {ex.Message}");
        }
        await LoadSongsAsync();
        SearchSong();
    }

    private async Task RemoveSongAsync(Song? song)
    {
        if (song == null) return;
        Console.WriteLine("id песни - " + song.Id + " имя - " + song.Title + " " + song.Id.ToString());
        try
        {
            await _apiService.RemoveSongAsync(song.Id.ToString());
            Songs.Remove(song);
            SearchSong();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка удаления песни: {ex.Message}");
        }

    }

}