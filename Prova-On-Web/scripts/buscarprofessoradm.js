
  const API_BASE_URL = "https://localhost:7141/Professor/BuscarProfessor"; // Ajuste para sua API real

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
      const response = await axios.get(`${API_BASE_URL}?id=${id}`);
      const professor = response.data;

      if (!professor) {
        Swal.fire({
          icon: 'info',
          title: 'Nenhum resultado',
          text: 'Nenhum professor encontrado com esse ID.'
        });
        limparTabelaProfessor();
        return;
      }

      renderizarProfessor(professor);
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
    }
  }

  function renderizarProfessor(professor) {
    const tbody = document.querySelector("#tabelaProfessor tbody");

    const dataRegistro = professor.dataRegistro 
      ? new Date(professor.dataRegistro).toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
      : '-';

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
