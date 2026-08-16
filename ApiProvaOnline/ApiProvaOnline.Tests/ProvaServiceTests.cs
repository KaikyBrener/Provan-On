using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.Services;
using ApiProvaOnline.Models;
using FluentAssertions;
using Moq;

namespace ApiProvaOnline.Tests
{
    public class ProvaServiceTests
    {
        private readonly Mock<IProvaRepository> _provaRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ProvaService _service;

        public ProvaServiceTests()
        {
            _provaRepoMock = new Mock<IProvaRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _service = new ProvaService(_unitOfWorkMock.Object, _provaRepoMock.Object);
        }

        [Fact]
        public async Task CadastrarProvaAsync_DeveRegistrarDataEChamarCommit()
        {
            // Arrange
            var prova = new Prova
            {
                Titulo = "Prova de Matemática",
                Codigo = "MAT001",
                IdProfessor = 1
            };

            _provaRepoMock.Setup(r => r.AddAsync(It.IsAny<Prova>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _service.CadastrarProvaAsync(prova);

            // Assert
            prova.DataHoraRegistro.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            _provaRepoMock.Verify(r => r.AddAsync(prova), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ListarProvaAsync_DeveRetornarProvasOrdenadas()
        {
            // Arrange
            var provas = new List<Prova>
            {
                new Prova { Id = 1, Titulo = "Biologia", Codigo = "BIO001" },
                new Prova { Id = 2, Titulo = "Artes", Codigo = "ART001" },
                new Prova { Id = 3, Titulo = "Ciências", Codigo = "CIE001" }
            };
            _provaRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(provas);

            // Act
            var result = await _service.ListarProvaAsync();

            // Assert
            var lista = result.ToList();
            lista.Should().HaveCount(3);
            lista[0].Titulo.Should().Be("Artes");
            lista[1].Titulo.Should().Be("Biologia");
            lista[2].Titulo.Should().Be("Ciências");
        }

        [Fact]
        public async Task RemoverProvaAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Arrange
            _provaRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Prova?)null);

            // Act
            var result = await _service.RemoverProvaAsync(99);

            // Assert
            result.Should().BeFalse();
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task AtualizarProvaAsync_QuandoExiste_DeveAtualizarERetornarTrue()
        {
            // Arrange
            var existente = new Prova
            {
                Id = 1,
                Titulo = "Prova Antiga",
                Codigo = "OLD001",
                IdProfessor = 1,
                DataHoraRegistro = DateTime.Now.AddDays(-1)
            };
            var atualizada = new Prova
            {
                Id = 1,
                Titulo = "Prova Atualizada",
                Codigo = "NEW001",
                IdProfessor = 1,
                DataHoraRegistro = DateTime.Now
            };
            _provaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existente);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.AtualizarProvaAsync(atualizada);

            // Assert
            result.Should().BeTrue();
            existente.Titulo.Should().Be("Prova Atualizada");
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        // ── Testes de validação de período (ponto 4) ─────────────────────────

        [Fact]
        public async Task CadastrarProvaAsync_DeveDefinirDataHoraRegistro()
        {
            // Garante que DataHoraRegistro é preenchido automaticamente pelo serviço
            var prova = new Prova
            {
                Titulo = "Prova Teste",
                Codigo = "TST001",
                IdProfessor = 1,
                DataHoraInicio = DateTime.Now.AddHours(-1),
                DataHoraFim = DateTime.Now.AddHours(2)
            };
            _provaRepoMock.Setup(r => r.AddAsync(It.IsAny<Prova>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            await _service.CadastrarProvaAsync(prova);

            prova.DataHoraRegistro.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task BuscarProvaPorCodigoAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            _provaRepoMock.Setup(r => r.GetByCodigoAsync("INVALIDO")).ReturnsAsync((Prova?)null);

            var result = await _service.BuscarProvaPorCodigoAsync("INVALIDO");

            result.Should().BeNull();
        }
    }
}
