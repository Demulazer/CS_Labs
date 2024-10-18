namespace Presenter;
using Model;
public interface ISongPresenter
{
    Task CheckFullDataInput(string name, string author);
    Task CheckDataInput(string name, string author);
    Task<List<Song>> SplitSongNameForSearch(string findBy);
    Task CheckIdenticalSong();
}