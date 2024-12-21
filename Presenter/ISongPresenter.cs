using Model;

namespace Presenter;

public interface ISongPresenter
{
    Task RemoveSongPresenter(string name, string author);
    Task RemoveSongPresenter(int id);
    Task<List<Song>> SongSearchPresenter(string findBy);
    Task<List<Song>> ShowSongPresenter();
    Task AddSongPresenter(string songName, string songAuthor);
}