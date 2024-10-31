namespace Presenter;
using Model;
public interface ISongPresenter
{

    Task RemoveSongPresenter(string name, string author);
    Task<List<Song>> SongSearchPresenter(string findBy);
    Task<List<Song>> ShowSongPresenter();

}