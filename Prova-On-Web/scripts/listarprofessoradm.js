
  const API_BASE_URL = "https://localhost:7141/Professor/ListarProfessor";

  async function carregarProfessores() {
    try {
      const response = await axios.get(API_BASE_URL);
      const professores = response.data;

      const tabela = document.getElementById('tabelaProfessores');
      tabela.innerHTML = '';

      if (!professores || professores.length === 0) {
        tabela.innerHTML = `<tr><td colspan="4" class="text-center text-muted">Nenhum professor encontrado.</td></tr>`;
        return;
      }

      professores.forEach(professor => {
        const dataRegistro = professor.dataRegistro 
          ? new Date(professor.dataRegistro).toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
          : '-';

        const tr = document.createElement('tr');
        tr.innerHTML = `
          <td style="vertical-align: middle;">${professor.id || '-'}</td>
          <td style="vertical-align: middle;">${professor.nome || '-'}</td>
          <td style="vertical-align: middle;">${professor.email || '-'}</td>
          <td style="vertical-align: middle;">${dataRegistro}</td>
        `;
        tabela.appendChild(tr);
      });

    } catch (error) {
      console.error('Erro ao carregar professores:', error);
      Swal.fire('Erro', 'Não foi possível carregar os professores.', 'error');
      const tabela = document.getElementById('tabelaProfessores');
      tabela.innerHTML = `<tr><td colspan="4" class="text-center text-danger">Erro ao carregar dados.</td></tr>`;
    }
  }

  // Carrega os professores assim que a página abrir
  carregarProfessores();