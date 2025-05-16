using IdeaManager.Core.Entities;
using IdeaManager.Core.Interfaces;
using IdeaManager.Data.Repositories;
using Moq;

namespace IdeaManager.Tests.Services
{
    public class IdeaServiceTests
    {
        private readonly Mock<IIdeaRepository> _ideaRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IdeaService _ideaService;

        public IdeaServiceTests()
        {
            _ideaRepositoryMock = new Mock<IIdeaRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.IdeaRepository).Returns(_ideaRepositoryMock.Object);
            _ideaService = new IdeaService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task SubmitIdeaAsync_ValidIdea_AddsIdea()
        {
            var idea = new Idea { Title = "william", Description = "william", Id = 1 };
            await _ideaService.SubmitIdeaAsync(idea);
            _ideaRepositoryMock.Verify(r => r.AddAsync(It.Is<Idea>(i => i.Title == "william")), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SubmitIdeaAsync_EmptyTitle_ThrowsArgumentException()
        {
            var idea = new Idea { Title = "", Description = "william" };
            await Assert.ThrowsAsync<ArgumentException>(() => _ideaService.SubmitIdeaAsync(idea));
            _ideaRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Idea>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SubmitIdeaAsync_WithoutTitle_ThrowsArgumentException(string? invalidTitle)
        {
            var idea = new Idea { Title = invalidTitle };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _ideaService.SubmitIdeaAsync(idea));

            Assert.Equal("Le titre est obligatoire.", exception.Message);
        }
    }
}