using Model;
using Moq;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ModelTests
{
    private Mock<IFileStorage> _storageMock;
    private SongModel _songModel;
    [SetUp]
    public void Setup()
    {
        _storageMock = new Mock<IFileStorage>();
        _songModel = new SongModel(_storageMock.Object);
        var songList = new List<Song>();
        var validSong = new Song(1, new SongName("name"), new SongAuthor("author"));
        songList.Add(validSong);
        _songModel._songList = songList;
    }

    [Test]
    public async Task GetSongByID_ShouldReturnSong_WhenGivenValidID()
    {
        int id = 1;
        Console.Write(_songModel._songList.Count);
        var res = await _songModel.GetSongById(id);
            
        Assert.That(res.SongName.Name, Is.EqualTo("name"));
        Assert.That(res.SongAuthor.Author, Is.EqualTo("author"));
        Assert.That(res.Id, Is.EqualTo(1));
    }
    [Test]
    public async Task GetSongByID_ShouldReturnNull_WhenGivenInvalidID()
    {
        int id = 999;
            
        var res = await _songModel.GetSongById(id);
        Assert.That(res, Is.Null);
    }
    [Test]
    public async Task GetLastID_ShouldReturnLastID()
    {
        var res = _songModel.GetLastId();
        Assert.That(res, Is.GreaterThan(-1));
    }
    [Test]
    public async Task FindSongByFull_ShouldReturnSongList_WhenGivenValidNameAndAuthor()
    {
        var searchName = new SongName("name"); 
        var searchAuthor =  new SongAuthor("author");
            
        var res = await _songModel.FindSongByFull(searchName, searchAuthor);
        Assert.That(res.Count, Is.EqualTo(1));
        Assert.That(res[0].SongName.Name, Is.EqualTo("name"));
        Assert.That(res[0].SongAuthor.Author, Is.EqualTo("author"));
    }
        
    [Test]
    public async Task FindSongByFull_ShouldThrowInvalidOperationException_WhenGivenInvalidNameAndAuthor()
    {
        var searchName = new SongName("bad name"); 
        var searchAuthor =  new SongAuthor("bad author");
            
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _songModel.FindSongByFull(searchName, searchAuthor));
        Assert.That(exception.Message, Is.EqualTo("Name or Author is invalid, or found no songs. Please try again."));
    }
        
    [Test]
    public async Task FindSongByName_ShouldThrowInvalidOperationException_WhenGivenInvalidName()
    {
        var searchName = new SongName("bad name"); 
            
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _songModel.FindSongsByName(searchName));
        Assert.That(exception.Message, Is.EqualTo("Name is invalid, or found no songs. Please try again."));
    }
        
    [Test]
    public async Task FindSongByName_ShouldReturnSongList_WhenGivenValidName()
    {
        var searchName = new SongName("name"); 
        var searchAuthor =  new SongAuthor("author");
            
        var res = await _songModel.FindSongsByName(searchName);
        Assert.That(res.Count, Is.EqualTo(1));
        Assert.That(res[0].SongName.Name, Is.EqualTo("name"));
        Assert.That(res[0].SongAuthor.Author, Is.EqualTo("author"));
    }

    [Test]
    public void RemoveSong_ShouldRemoveSongAndCallUpdateFile_WhenGivenValidSong()
    {
        var song = new Song(1, new SongName("name"), new SongAuthor("author"));
        var preCount = _songModel._songList.Count;
        var res = _songModel.RemoveSong(song);
        Assert.That(preCount, Is.EqualTo(_songModel._songList.Count + 1));
    }
    [Test]
    public void AddSong_ShouldAddSong_WhenGivenValidSong()
    {
        var song = new Song(1, new SongName("name"), new SongAuthor("author"));
        var preCount = _songModel._songList.Count;
        var res = _songModel.AddSong(song);
            

        Assert.That(preCount, Is.EqualTo(_songModel._songList.Count - 1));
    }
    [Test]
    public async Task ShowSongs_ShouldReturnSongList()
    {
        var test = await _songModel.ShowSongs();
        Assert.That(test, Is.TypeOf<List<Song>>());
    }

    [Test]
    public async Task CheckSong_ShouldReturnSong_WhenGivenExistingSongNameAndAuthor()
    {
        SongName songName = new SongName("name");
        SongAuthor songAuthor = new SongAuthor("author");
        var test = await _songModel.CheckSong(songName, songAuthor);
        Assert.That(test, Is.TypeOf<Song>());
        Assert.That(test.SongName.Name, Is.EqualTo("name"));
        Assert.That(test.SongAuthor.Author, Is.EqualTo("author"));
    }

    [Test]
    public async Task CheckSong_ShouldReturnNull_WhenGivenInvalidSongName()
    {
        SongName songName = new SongName("bad name");
        SongAuthor songAuthor = new SongAuthor("author");
        var test = await _songModel.CheckSong(songName, songAuthor);
        Assert.That(test, Is.Null);
    }
    [Test]
    public async Task CheckSong_ShouldReturnNull_WhenGivenInvalidSongAuthor()
    {
        SongName songName = new SongName("name");
        SongAuthor songAuthor = new SongAuthor("bad author");
        var test = await _songModel.CheckSong(songName, songAuthor);
        Assert.That(test, Is.Null);
    }

        

}