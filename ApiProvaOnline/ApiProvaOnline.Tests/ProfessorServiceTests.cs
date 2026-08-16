using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.Services;
using ApiProvaOnline.Models;
using FluentAssertions;
using Moq;

namespace ApiProvaOnline.Tests
{
    public class ProfessorServiceTests
    {
        private readonly Mock<IProfessorRepository> _professorRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ProfessorService _service;

        public ProfessorServiceTests()
        {
            _professorRepoMock = new Mock<IProfessorRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _service = new ProfessorService(_unitOfWorkMock.Object, _professorRepoMock.Object);
        }

        [Fact]
        public async Task CadastrarProfessorAsync_DeveHashearSenhaEChamarCommit()
        {
            // Arrange
            var professor = new Professor
            {
                Nome = "Prof. Teste",
                Email = "teste@email.com",
                Senha = "senha123"
            };

            _professorRepoMock.Setup(r => r.AddAsync(It.IsAny<Professor>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _service.CadastrarProfessorAsync(professor);

            // Assert
            professor.Senha.Should().StartWith("$2a$");
            professor.DataHoraRegistro.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            _professorRepoMock.Verify(r => r.AddAsync(professor), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task BuscarProfessorAsync_QuandoExiste_DeveRetornarProfessor()
        {
            // Arrange
            var professor = new Professor { Id = 1, Nome = "Prof. Teste", Email = "teste@email.com", Senha = "hash" };
            _professorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(professor);

            // Act
            var result = await _service.BuscarProfessorAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Nome.Should().Be("Prof. Teste");
        }

        [Fact]
        public async Task BuscarProfessorAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            // Arrange
            _professorRepoMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Professor?)null);

            // Act
            var result = await _service.BuscarProfessorAsync(99);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoverProfessorAsync_QuandoExiste_DeveRetornarTrue()
        {
            // Arrange
            var professor = new Professor { Id = 1, Nome = "Prof. Teste", Email = "t@t.com", Senha = "h" };
            _professorRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(professor);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.RemoverProfessorAsync(1);

            // Assert
            result.Should().BeTrue();
            _professorRepoMock.Verify(r => r.Remove(professor), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverProfessorAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Arrange
            _professorRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Professor?)null);

            // Act
            var result = await _service.RemoverProfessorAsync(99);

            // Assert
            result.Should().BeFalse();
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task LoginProfessorAsync_SenhaCorreta_DeveRetornarProfessor()
        {
            // Arrange
            var hash = BCrypt.Net.BCrypt.HashPassword("senha123");
            var professor = new Professor { Id = 1, Nome = "Prof. Teste", Email = "teste@email.com", Senha = hash };
            _professorRepoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Professor, bool>>>()))
                .ReturnsAsync(new List<Professor> { professor });

            // Act
            var result = await _service.LoginProfessorAsync("teste@email.com", "senha123");

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("teste@email.com");
        }

        [Fact]
        public async Task LoginProfessorAsync_SenhaIncorreta_DeveRetornarNull()
        {
            // Arrange
            var hash = BCrypt.Net.BCrypt.HashPassword("senha123");
            var professor = new Professor { Id = 1, Nome = "Prof. Teste", Email = "teste@email.com", Senha = hash };
            _professorRepoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Professor, bool>>>()))
                .ReturnsAsync(new List<Professor> { professor });

            // Act
            var result = await _service.LoginProfessorAsync("teste@email.com", "senhaErrada");

            // Assert
            result.Should().BeNull();
        }
    }
}
