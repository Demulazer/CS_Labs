
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
        }

    }
}