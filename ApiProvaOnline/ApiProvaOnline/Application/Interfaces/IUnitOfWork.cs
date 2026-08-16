namespace ApiProvaOnline.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProvaRepository Provas { get; }
        IAlunoRepository Alunos { get; }
        IProfessorRepository Professores { get; }
        IAdministrativoRepository Administrativos { get; }
        IPerguntaRepository Perguntas { get; }
        IPerguntaObjetivaRepository PerguntasObjetivas { get; }
        IAlunoRespostaRepository AlunoRespostas { get; }
        IAlunoRespostaObjetivaRepository AlunoRespostasObjetivas { get; }

        Task<int> CommitAsync();
    }
}
