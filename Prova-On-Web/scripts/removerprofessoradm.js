
  const API_BUSCAR = "https://localhost:7141/Professor/BuscarProfessor";
  const API_REMOVER = "https://localhost:7141/Professor/RemoverProfessor";
  let idProfessorSelecionado = null;

  async function buscarProfessor() {
    const id = document.getElementById("professorIdBuscar").value.trim();
    if (!id) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'Informe o ID do professor para buscar.'
      });
      return;
    }

    try {
      const response = await axios.get(`${API_BUSCAR}?id=${id}`);
      const professor = response.data;

      if (!professor) {
        Swal.fire({
          icon: 'info',
          title: 'Nenhum resultado',
          text: 'Nenhum professor encontrado com esse ID.'
        });
        limparTabelaProfessor();
        document.getElementById("btnRemover").style.display = "none";
        return;
      }

      idProfessorSelecionado = id;
      renderizarProfessor(professor);
      document.getElementById("btnRemover").style.display = "inline-block";

      Swal.fire({
        icon: 'success',
        title: 'Professor encontrado',
        text: `Mostrando dados de ${professor.nome || 'Professor'}.`
      });
    } catch (error) {
      console.error(error);
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: 'Erro ao buscar professor. Verifique sua conexão ou o ID.'
      });
      limparTabelaProfessor();
      document.getElementById("btnRemover").style.display = "none";
    }
  }

  function renderizarProfessor(professor) {
    const tbody = document.querySelector("#tabelaProfessor tbody");

    const dataRegistro = professor.dataHoraRegistro 
      ? new Date(professor.dataHoraRegistro).toLocaleString('pt-BR', {
          day: '2-digit',
          month: '2-digit',
          year: 'numeric',
          hour: '2-digit',
          minute: '2-digit'
        }) : '-';

    tbody.innerHTML = `
      <tr>
        <td>${professor.nome || '-'}</td>
        <td>${professor.email || '-'}</td>
        <td>${dataRegistro}</td>
      </tr>
    `;
  }

  function limparTabelaProfessor() {
    const tbody = document.querySelector("#tabelaProfessor tbody");
    tbody.innerHTML = `<tr><td colspan="3" class="text-center text-muted">Nenhum professor carregado.</td></tr>`;
  }

  async function removerProfessor() {
    if (!idProfessorSelecionado) return;

    const confirmacao = await Swal.fire({
      title: 'Tem certeza?',
      text: `Você deseja realmente remover o professor com ID ${idProfessorSelecionado}? Esta ação não pode ser desfeita.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#6c757d',
      confirmButtonText: 'Sim, remover!',
      cancelButtonText: 'Cancelar'
    });

    if (!confirmacao.isConfirmed) return;

    try {
      await axios.delete(`${API_REMOVER}?id=${idProfessorSelecionado}`);
      Swal.fire('Removido!', 'Professor removido com sucesso.', 'success');
      limparTabelaProfessor();
      document.getElementById("btnRemover").style.display = "none";
      idProfessorSelecionado = null;
      document.getElementById("professorIdBuscar").value = '';
    } catch (error) {
      console.error(error);
      Swal.fire('Erro', 'Erro ao remover professor.', 'error');
    }
  }