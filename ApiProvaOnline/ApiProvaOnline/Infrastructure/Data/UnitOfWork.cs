using ApiProvaOnline.Application.Interfaces;

namespace ApiProvaOnline.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProvaOnDbContext _context;

        public IProvaRepository Provas { get; }
        public IAlunoRepository Alunos { get; }
        public IProfessorRepository Professores { get; }
        public IAdministrativoRepository Administrativos { get; }
        public IPerguntaRepository Perguntas { get; }
        public IPerguntaObjetivaRepository PerguntasObjetivas { get; }
        public IAlunoRespostaRepository AlunoRespostas { get; }
        public IAlunoRespostaObjetivaRepository AlunoRespostasObjetivas { get; }

        public UnitOfWork(
            ProvaOnDbContext context,
            IProvaRepository provas,
            IAlunoRepository alunos,
            IProfessorRepository professores,
            IAdministrativoRepository administrativos,
            IPerguntaRepository perguntas,
            IPerguntaObjetivaRepository perguntasObjetivas,
            IAlunoRespostaRepository alunoRespostas,
            IAlunoRespostaObjetivaRepository alunoRespostasObjetivas)
        {
            _context = context;
            Provas = provas;
            Alunos = alunos;
            Professores = professores;
            Administrativos = administrativos;
            Perguntas = perguntas;
            PerguntasObjetivas = perguntasObjetivas;
            AlunoRespostas = alunoRespostas;
            AlunoRespostasObjetivas = alunoRespostasObjetivas;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
