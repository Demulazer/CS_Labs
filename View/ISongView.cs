namespace TestLib;
using Model;
public interface ISongView
{
    Task ShowAllSongs();
    Task AddSong();
    Task RemoveSong();
    Task<List<Song>> SearchSong();
}