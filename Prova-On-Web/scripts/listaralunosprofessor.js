
  async function carregarAlunos() {
    const divLista = document.getElementById('listaAlunos');
    divLista.innerHTML = '<p style="grid-column: 1 / -1; text-align:center; color:#888;">Carregando alunos...</p>';

    try {
      const resposta = await fetch('https://localhost:7141/Aluno/ListarAlunos');
      const alunos = await resposta.json();

      if (!alunos || alunos.length === 0) {
        divLista.innerHTML = '<p style="grid-column: 1 / -1; text-align:center; color:#888;">Nenhum aluno encontrado.</p>';
        return;
      }

      divLista.innerHTML = '';
      alunos.forEach(aluno => {
        const card = document.createElement('div');
        card.style.background = '#fafafa';
        card.style.border = '1px solid #ddd';
        card.style.borderRadius = '10px';
        card.style.padding = '18px 25px';
        card.style.cursor = 'pointer';
        card.style.boxShadow = '0 2px 8px rgba(0,0,0,0.08)';
        card.style.transition = 'box-shadow 0.3s ease, border-color 0.3s ease';
        card.style.userSelect = 'none';
        card.innerText = aluno.nome;

        card.onmouseover = () => {
          card.style.boxShadow = '0 6px 20px rgba(94, 53, 177, 0.3)';
          card.style.borderColor = '#5e35b1';
        };
        card.onmouseout = () => {
          card.style.boxShadow = '0 2px 8px rgba(0,0,0,0.08)';
          card.style.borderColor = '#ddd';
        };
        card.onclick = () => selecionarAluno(aluno.id, aluno.nome);

        divLista.appendChild(card);
      });

    } catch (error) {
      console.error(error);
      divLista.innerHTML = '<p style="grid-column: 1 / -1; text-align:center; color:#d32f2f;">Erro ao carregar alunos.</p>';
    }
  }

  const respostasAluno = {
    q1: 'Banco de dados relacional',
    q2: 'SQL (Structured Query Language)',
    q3: 'Entidade, Atributo, Relacionamento',
    q4: 'Chave primária',
    q5: 'SELECT * FROM tabela WHERE id = 10;',
    q6: 'Normalização é o processo de organizar os dados para reduzir redundâncias.',
    q7: 'JOIN é usado para combinar linhas de duas ou mais tabelas.',
    q8: 'Integridade referencial garante que chaves estrangeiras correspondam a chaves primárias.',
    q9: 'Uma transação deve ser atômica, consistente, isolada e durável (ACID).',
    q10:'Backup é essencial para recuperar dados em caso de falhas.',
  };

  const questoesTexto = {
    1: 'O que é um banco de dados relacional?',
    2: 'Qual linguagem é usada para manipular bancos de dados relacionais?',
    3: 'Quais são os principais componentes do modelo entidade-relacionamento?',
    4: 'O que é uma chave primária?',
    5: 'Exemplo de comando SQL para selecionar todos os registros de uma tabela chamada "tabela" onde o id seja 10?',
    6: 'Explique o que é normalização em bancos de dados.',
    7: 'Para que serve o comando JOIN no SQL?',
    8: 'O que é integridade referencial?',
    9: 'Quais são as propriedades ACID de uma transação em banco de dados?',
    10:'Qual a importância do backup em bancos de dados?',
  };

  const maxNotaQuestao = {
    1: 1,
    2: 1,
    3: 1,
    4: 1,
    5: 1,
    6: 2,
    7: 2,
    8: 2,
    9: 2,
    10:2
  };

  const container = document.getElementById('questoesContainer');
  let alunoSelecionado = null;

  function selecionarAluno(id, nome) {
    alunoSelecionado = {id, nome};
    document.getElementById('listaAlunos').style.display = 'none';
    document.getElementById('painelCorrecao').style.display = 'block';
    document.getElementById('tituloPainel').textContent = `Painel de Correção da Prova de Banco de Dados - ${nome}`;

    container.innerHTML = '';

    for(let i=1; i<=10; i++){
      const divQuestao = document.createElement('div');
      divQuestao.style.borderBottom = '1px solid #eee';
      divQuestao.style.paddingBottom = '18px';
      divQuestao.style.marginBottom = '24px';

      divQuestao.innerHTML = `
        <h5 style="font-weight:600; color:#333;">Questão ${i}: ${questoesTexto[i]}</h5>
        <p><strong>Resposta do aluno:</strong></p>
        <p style="
          background:#f9f9f9;
          padding:14px 20px;
          border-radius:10px;
          font-style: italic;
          color:#444;
          white-space: pre-wrap;
          border: 1px solid #ddd;
          margin-top:6px;
          ">
          ${respostasAluno['q'+i]}
        </p>
        
        <div style="margin-top:12px; display:flex; align-items:center; gap: 22px;">
          <div>
            <input class="form-check-input" type="radio" name="correcao_q${i}" id="correto_q${i}" value="correto" required style="cursor:pointer;"/>
            <label for="correto_q${i}" style="font-weight:600; color:#2e7d32; cursor:pointer;">Correto</label>
          </div>
          <div>
            <input class="form-check-input" type="radio" name="correcao_q${i}" id="errado_q${i}" value="errado" required style="cursor:pointer;"/>
            <label for="errado_q${i}" style="font-weight:600; color:#d32f2f; cursor:pointer;">Errado</label>
          </div>

          <div style="margin-left:auto;">
            <label for="nota_q${i}" style="font-weight:600; color:#555;">Nota (0 a ${maxNotaQuestao[i]}):</label><br/>
            <input type="number" min="0" max="${maxNotaQuestao[i]}" step="0.1" id="nota_q${i}" name="nota_q${i}" required
              style="
                border-radius:8px; 
                border:1px solid #ccc; 
                width:90px;
                padding:6px 8px;
                font-size:14px;
                color:#444;
              ">
          </div>
        </div>
      `;

      container.appendChild(divQuestao);
    }
  }

  document.getElementById('btnCalcular').addEventListener('click', () => {
    const form = document.getElementById('formCorrecao');
    if(!form.checkValidity()){
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'Por favor, marque certo ou errado e informe a nota para todas as questões.',
        confirmButtonColor: '#5e35b1'
      });
      form.reportValidity();
      return;
    }

    let somaNotas = 0;
    let detalhes = '';

    for(let i=1; i<=10; i++){
      const correcoes = form.elements['correcao_q'+i];
      let marcado = null;
      for(const el of correcoes){
        if(el.checked){
          marcado = el.value;
          break;
        }
      }
      const nota = parseFloat(form.elements['nota_q'+i].value);
      somaNotas += nota;

      detalhes += `
        <div style="margin-bottom:10px; font-size:15px;">
          <strong>Questão ${i}:</strong> ${marcado === 'correto' ? 
            `<span style="color:#2e7d32;">Correto</span>` : 
            `<span style="color:#d32f2f;">Errado</span>`} | 
          Nota: <span style="font-weight:700; color:#5e35b1;">${nota.toFixed(1)}</span>
        </div>
      `;
    }

    const notaFinal = somaNotas.toFixed(1);
    document.getElementById('resultadoFinal').innerHTML = `
      <h4>Nota Final: ${notaFinal} / 15</h4>
      <hr style="border-color:#ddd;" />
      <div style="max-height:300px; overflow-y:auto; text-align:left;">${detalhes}</div>
    `;

    Swal.fire({
      icon: 'success',
      title: 'Correção salva!',
      text: `A nota final de ${alunoSelecionado.nome} é ${notaFinal} / 15.`,
      confirmButtonColor: '#5e35b1'
    });
  });

  carregarAlunos();
