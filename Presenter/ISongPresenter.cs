namespace Presenter;
using Model;
public interface ISongPresenter
{
    Task CheckFullDataInput(string name, string author);
    Task RemoveSongPresenter(string name, string author);
    Task<List<Song>> SongSearchPresenter(string findBy);
    Task CheckIdenticalSong();
}