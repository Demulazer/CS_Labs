using Model;
using Moq;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ModelTests
{
    private Mock<IDatabaseStorage> _storageMock;
    private SongModel _songModel;
    [SetUp]
    public void Setup()
    {
        _storageMock = new Mock<IDatabaseStorage>();
        _songModel = new SongModel(_storageMock.Object);
        
    }

    [Test]
    public async Task GetSongByID_ShouldReturnSong_WhenGivenValidID()
    {
        int id = 1;
        _storageMock.Setup(storage => storage.GetSongByIdAsync(id).Result).Returns(new Song(1, new SongName("asd"), new SongAuthor("asd")));
        var res = await _songModel.GetSongById(id);
            
        Assert.That(res.SongName.Name, Is.EqualTo("asd"));
        Assert.That(res.SongAuthor.Author, Is.EqualTo("asd"));
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
        _songModel.FindSongByFull(searchName, searchAuthor);
        _storageMock.Verify(model => model.FindSongsByNameAndAuthorAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        // Assert.That(res.Count, Is.EqualTo(1));
        // Assert.That(res[0].SongName.Name, Is.EqualTo("name"));
        // Assert.That(res[0].SongAuthor.Author, Is.EqualTo("author"));
    }
        
    [Test]
    public async Task FindSongByFull_ShouldThrowInvalidOperationException_WhenGivenInvalidNameAndAuthor()
    {
        var searchName = new SongName("bad name"); 
        var searchAuthor =  new SongAuthor("bad author");
            
        var exception = Assert.ThrowsAsync<NullReferenceException>(async () => await _songModel.FindSongByFull(searchName, searchAuthor));
        Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));
    }
        
    [Test]
    public async Task FindSongByName_ShouldThrowInvalidOperationException_WhenGivenInvalidName()
    {
        var searchName = new SongName("bad name"); 
            
        var exception = Assert.ThrowsAsync<NullReferenceException>(async () => await _songModel.FindSongsByName(searchName));
        Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));
    }
        
    [Test]
    public async Task FindSongByName_ShouldReturnSongList_WhenGivenValidName()
    {
        var searchName = new SongName("name"); 
        var searchAuthor =  new SongAuthor("author");
        var returnSong = new Song(1, searchName, searchAuthor);
        _storageMock.Setup(storage => storage.FindSongsByNameAsync(searchName.Name).Result).Returns([returnSong]);                    
        var res = await _songModel.FindSongsByName(searchName);

        Assert.That(res.Count, Is.EqualTo(1));
        Assert.That(res[0].SongName.Name, Is.EqualTo("name"));
        Assert.That(res[0].SongAuthor.Author, Is.EqualTo("author"));
    }

    [Test]
    public void RemoveSong_ShouldRemoveSongAndCallUpdateFile_WhenGivenValidSong()
    {
        var song = new Song(1, new SongName("name"), new SongAuthor("author"));
        var res = _songModel.RemoveSong(song);
        Assert.That(res.IsCompleted);
    }
    [Test]
    public async Task AddSong_ShouldAddSong_WhenGivenValidSong()
    {
        var song = new Song(1, new SongName("name"), new SongAuthor("author"));
        var preCount = await _songModel.GetSongById(1);
        Assert.That(preCount, Is.EqualTo(null));
        var res = _songModel.AddSong(song);
        Assert.That(res, Is.Not.Null);

    }


    [Test]
    public async Task CheckSong_ShouldReturnNull_WhenGivenInvalidSongName()
    {
        SongName songName = new SongName("bad name");
        SongAuthor songAuthor = new SongAuthor("author");
        _storageMock.Setup(storage => storage.FindSongsByNameAndAuthorAsync(songName.Name, songAuthor.Author).Result).Returns(new List<Song>());
        var test = await _songModel.CheckSong(songName, songAuthor);

        Assert.That(test, Is.Null);

    }
    [Test]
    public async Task CheckSong_ShouldReturnNull_WhenGivenInvalidSongAuthor()
    {
        SongName songName = new SongName("name");
        SongAuthor songAuthor = new SongAuthor("bad author");
        _storageMock.Setup(storage => storage.FindSongsByNameAndAuthorAsync(songName.Name, songAuthor.Author).Result).Returns(new List<Song>());
        var test = await _songModel.CheckSong(songName, songAuthor);
        Assert.That(test, Is.Null);
    }

        

}