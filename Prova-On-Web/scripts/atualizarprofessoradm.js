  const API_BUSCAR = "https://localhost:7141/Professor/BuscarProfessor";
  const API_ATUALIZAR = "https://localhost:7141/Professor/AtualizarProfessor";
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
        esconderFormulario();
        return;
      }

      idProfessorSelecionado = id;
      preencherFormulario(professor);
      Swal.fire({
        icon: 'success',
        title: 'Professor encontrado',
        text: `Editando dados de ${professor.nome || 'Professor'}.`
      });
    } catch (error) {
      console.error(error);
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: 'Erro ao buscar professor. Verifique sua conexão ou o ID.'
      });
      esconderFormulario();
    }
  }

  function preencherFormulario(professor) {
    document.getElementById("nomeProfessor").value = professor.nome || '';
    document.getElementById("emailProfessor").value = professor.email || '';
    document.getElementById("senhaProfessor").value = '';
    document.getElementById("formAtualizarProfessor").style.display = "block";
  }

  function esconderFormulario() {
    document.getElementById("formAtualizarProfessor").style.display = "none";
    idProfessorSelecionado = null;
  }

  document.getElementById("formAtualizarProfessor").addEventListener("submit", async (e) => {
    e.preventDefault();

    if (!idProfessorSelecionado) return;

    const nome = document.getElementById("nomeProfessor").value.trim();
    const email = document.getElementById("emailProfessor").value.trim();
    const senha = document.getElementById("senhaProfessor").value;

    if (!nome || !email) {
      Swal.fire({
        icon: 'warning',
        title: 'Campos obrigatórios',
        text: 'Nome e email são obrigatórios.'
      });
      return;
    }

    const dadosAtualizados = {
      id: idProfessorSelecionado,
      nome,
      email,
    };

    // Só envia senha se tiver valor (para não sobrescrever senha com vazio)
    if (senha) {
      dadosAtualizados.senha = senha;
    }

    try {
      await axios.put(API_ATUALIZAR, dadosAtualizados);
      Swal.fire({
        icon: 'success',
        title: 'Atualizado',
        text: 'Professor atualizado com sucesso!'
      });
      esconderFormulario();
      document.getElementById("professorIdBuscar").value = '';
    } catch (error) {
      console.error(error);
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: 'Erro ao atualizar professor.'
      });
    }
  });