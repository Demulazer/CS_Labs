
using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Model;
using Presenter;

namespace Model.Tests
{
    
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

    }
}