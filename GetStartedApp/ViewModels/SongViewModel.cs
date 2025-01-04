using CommunityToolkit.Mvvm.ComponentModel;
using GetStartedApp.Models;

namespace GetStartedApp.ViewModels;

public partial class SongViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;
    
    [ObservableProperty]
    private string? _title;   
    
    [ObservableProperty]
    private string? _artist;

    public SongViewModel()
    {
        
    }

    public SongViewModel(Song song)
    {
        _id = song.Id;
        _title = song.Title;
        _artist = song.Artist;
    }

    public Song GetSong()
    {
        return new Song
        {
            Id = Id,
            Title = Title,
            Artist = Artist
        };
    }
}