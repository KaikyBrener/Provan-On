
  const API_BASE_URL = "https://localhost:7141";

  // Buscar provas do professor
  async function buscarProvasDoProfessor() {
    const idProfessor = document.getElementById("professorId").value.trim();
    if (!idProfessor) {
      Swal.fire('Atenção', 'Informe o ID do professor.', 'warning');
      return;
    }
    try {
      const response = await axios.get(`${API_BASE_URL}/Professor/BuscarProfessor?id=${idProfessor}`);
      const professor = response.data;

      if (!professor || !professor.provas || professor.provas.length === 0) {
        Swal.fire('Ops...', 'Professor ou provas não encontradas.', 'error');
        document.getElementById("listaProvas").innerHTML = "";
        document.getElementById("provaAluno").innerHTML = `<option value="">Selecione uma prova</option>`;
        return;
      }

      renderizarProvas(professor.provas);
      popularSelectProvas(professor.provas);
    } catch (error) {
      console.error(error);
      Swal.fire('Erro', 'Erro ao buscar provas do professor.', 'error');
    }
  }

  // Renderiza a lista de provas
  function renderizarProvas(provas) {
    const lista = document.getElementById("listaProvas");
    lista.innerHTML = "";
    provas.forEach(prova => {
      const li = document.createElement("li");
      li.className = "list-group-item d-flex justify-content-between align-items-center";
      li.innerHTML = `
        <div>
          <strong>${prova.titulo}</strong><br />
          Código: ${prova.codigo} | Início: ${new Date(prova.dataHoraInicio).toLocaleString()} | Fim: ${new Date(prova.dataHoraFim).toLocaleString()}
        </div>
      `;
      lista.appendChild(li);
    });
  }

  // Popular select de provas no cadastro de aluno
  function popularSelectProvas(provas) {
    const select = document.getElementById("provaAluno");
    select.innerHTML = `<option value="">Selecione uma prova</option>`;
    provas.forEach(prova => {
      const option = document.createElement("option");
      option.value = prova.id;
      option.textContent = prova.titulo;
      select.appendChild(option);
    });
  }

  // Cadastro de aluno
  document.getElementById("formCadastrarAluno").addEventListener("submit", async function(e) {
    e.preventDefault();

    const nome = document.getElementById("nomeAluno").value.trim();
    const dataNascimento = document.getElementById("dataNascimentoAluno").value;
    const idProva = document.getElementById("provaAluno").value;

    if (!nome || !dataNascimento || !idProva) {
      Swal.fire('Atenção', 'Preencha todos os campos.', 'warning');
      return;
    }

    const aluno = {
      id: 0,
      nome,
      dataNascimento: new Date(dataNascimento).toISOString(),
      dataHoraRegistro: new Date().toISOString(),
      idProva: parseInt(idProva)
    };

    try {
      await axios.post(`${API_BASE_URL}/Aluno/Cadastrar`, aluno);
      Swal.fire('Sucesso', 'Aluno cadastrado com sucesso!', 'success');
      this.reset();
    } catch (error) {
      Swal.fire('Erro', 'Erro ao cadastrar aluno.', 'error');
    }
  });

  // Confirmar remoção do professor com SweetAlert2
  async function confirmarRemoverProfessor() {
    const id = document.getElementById("removerProfessorId").value.trim();
    if (!id) {
      Swal.fire('Atenção', 'Informe o ID do professor para remover.', 'warning');
      return;
    }

    const { isConfirmed } = await Swal.fire({
      title: 'Você tem certeza?',
      text: `Deseja realmente remover o professor com ID ${id}? Essa ação não pode ser desfeita!`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, remover',
      cancelButtonText: 'Cancelar',
      reverseButtons: true
    });

    if (!isConfirmed) return;

    try {
      await axios.delete(`${API_BASE_URL}/Professor/RemoverProfessor?id=${id}`);
      Swal.fire('Removido!', 'Professor removido com sucesso.', 'success');
      document.getElementById("removerProfessorId").value = "";
      listarProfessores();
    } catch (error) {
      Swal.fire('Erro', 'Erro ao remover professor. Verifique se o ID está correto.', 'error');
    }
  }

  // Listar professores
  async function listarProfessores() {
    try {
      const response = await axios.get(`${API_BASE_URL}/Professor/ListarProfessor`);
      const lista = document.getElementById("listaProfessores");
      lista.innerHTML = "";

      if (!response.data || response.data.length === 0) {
        lista.innerHTML = `<li class="list-group-item text-center text-muted">Nenhum professor encontrado.</li>`;
        return;
      }

      response.data.forEach(professor => {
        const li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between align-items-center";
        li.style.cursor = "default";

        li.innerHTML = `
          <div>
            <strong>ID:</strong> ${professor.id} <br>
            <strong>Nome:</strong> ${professor.nome || 'Sem nome'}
          </div>
          <button class="btn btn-sm btn-danger fw-semibold" onclick="confirmarRemoverProfessorPorId(${professor.id}, '${professor.nome.replace(/'/g, "\\'")}')">Remover</button>
        `;
        lista.appendChild(li);
      });
    } catch (error) {
      Swal.fire('Erro', 'Erro ao listar professores.', 'error');
    }
  }

  // Função para confirmar remoção do professor via botão da lista
  async function confirmarRemoverProfessorPorId(id, nome) {
    const { isConfirmed } = await Swal.fire({
      title: 'Confirma remoção?',
      html: `Deseja remover o professor <strong>${nome}</strong> (ID: ${id})? Essa ação é irreversível!`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, remover',
      cancelButtonText: 'Cancelar',
      reverseButtons: true
    });

    if (!isConfirmed) return;

    try {
      await axios.delete(`${API_BASE_URL}/Professor/RemoverProfessor?id=${id}`);
      Swal.fire('Removido!', 'Professor removido com sucesso.', 'success');
      listarProfessores();
    } catch (error) {
      Swal.fire('Erro', 'Erro ao remover professor.', 'error');
    }
  }
