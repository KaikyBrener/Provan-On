    document.addEventListener("DOMContentLoaded", () => {
    carregarProvas();
  });

  async function carregarProvas() {
    const tabelaProvas = document.getElementById("tabelaProvas");
    tabelaProvas.innerHTML = '<tr><td colspan="4" class="text-center text-muted" style="padding: 30px 0;">Carregando provas...</td></tr>';

    try {
      const resposta = await fetch("https://localhost:7141/ListarProvasProfessor/1"); // <-- Professor fixo ou qualquer ID
      const provas = await resposta.json();

      if (!provas || provas.length === 0) {
        tabelaProvas.innerHTML = '<tr><td colspan="4" class="text-center text-muted" style="padding: 30px 0;">Nenhuma prova cadastrada.</td></tr>';
        return;
      }

      tabelaProvas.innerHTML = '';

      provas.forEach(prova => {
        const dataFormatada = new Date(prova.dataHoraRegistro).toLocaleDateString('pt-BR');

        const tr = document.createElement('tr');
        tr.style.transition = 'background-color 0.3s ease';
        tr.onmouseenter = () => tr.style.backgroundColor = 'rgba(94, 53, 177, 0.05)';
        tr.onmouseleave = () => tr.style.backgroundColor = '';

        tr.innerHTML = `
          <td style="vertical-align: middle;">${prova.titulo}</td>
          <td style="vertical-align: middle; white-space: nowrap;">${dataFormatada}</td>
          <td style="vertical-align: middle; font-weight: 600;">${prova.codigo}</td>
          <td style="vertical-align: middle; text-align: center;">
            <button
              onclick="corrigirProva(${prova.id})"
              style="
                background-color: #5e35b1;
                border: none;
                color: white;
                padding: 6px 18px;
                border-radius: 10px;
                font-weight: 600;
                cursor: pointer;
                box-shadow: 0 4px 12px rgba(94, 53, 177, 0.4);
                transition: background-color 0.3s ease;
                display: inline-flex;
                align-items: center;
                gap: 6px;
                font-size: 14px;
              "
              onmouseover="this.style.backgroundColor='#4a278e'"
              onmouseout="this.style.backgroundColor='#5e35b1'"
              aria-label="Corrigir prova"
            >
              <i class="bi bi-clipboard-check" style="font-size: 18px;"></i> Corrigir
            </button>
          </td>
        `;
        tabelaProvas.appendChild(tr);
      });

    } catch (error) {
      console.error(error);
      tabelaProvas.innerHTML = '<tr><td colspan="4" class="text-center text-danger" style="padding: 30px 0;">Erro ao carregar provas.</td></tr>';
    }
  }

  function corrigirProva(provaId) {
    window.location.href = `Listaralunosprofessor.html?id=${provaId}`;
  }
