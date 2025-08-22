
    const listaPerguntas = document.getElementById('listaPerguntas');

    // Adicionar Pergunta Objetiva
    document.getElementById('formObjetiva').addEventListener('submit', function(e) {
      e.preventDefault();

      const enunciado = document.getElementById('enunciadoObjetiva').value.trim();
      const altA = document.getElementById('altA').value.trim();
      const altB = document.getElementById('altB').value.trim();
      const altC = document.getElementById('altC').value.trim();
      const altD = document.getElementById('altD').value.trim();
      const correta = document.getElementById('respostaCorreta').value;

      if (!enunciado || !altA || !altB || !altC || !altD || !correta) {
        alert("Preencha todos os campos.");
        return;
      }

      const item = document.createElement('li');
      item.className = 'list-group-item';
      item.style.borderRadius = '10px';
      item.style.marginBottom = '10px';
      item.style.backgroundColor = '#f8f9fa';
      item.style.boxShadow = '0 1px 5px rgba(0,0,0,0.05)';
      item.innerHTML = `
        <strong style="color:#3a3a3a;">Objetiva:</strong> ${enunciado}<br>
        <span style="color:#555;">A) ${altA}</span><br>
        <span style="color:#555;">B) ${altB}</span><br>
        <span style="color:#555;">C) ${altC}</span><br>
        <span style="color:#555;">D) ${altD}</span><br>
        <strong style="color:#28a745;">Correta:</strong> ${correta}
      `;
      listaPerguntas.appendChild(item);
      this.reset();
    });

    // Adicionar Pergunta Discursiva
    document.getElementById('formDiscursiva').addEventListener('submit', function(e) {
      e.preventDefault();

      const enunciado = document.getElementById('enunciadoDiscursiva').value.trim();
      const respostaModelo = document.getElementById('respostaModelo').value.trim();

      if (!enunciado) {
        alert("Preencha o enunciado.");
        return;
      }

      const item = document.createElement('li');
      item.className = 'list-group-item';
      item.style.borderRadius = '10px';
      item.style.marginBottom = '10px';
      item.style.backgroundColor = '#f8f9fa';
      item.style.boxShadow = '0 1px 5px rgba(0,0,0,0.05)';
      item.innerHTML = `
        <strong style="color:#3a3a3a;">Discursiva:</strong> ${enunciado}<br>
        ${respostaModelo ? `<em style="color:#666;">Resposta Modelo:</em> ${respostaModelo}` : ''}
      `;
      listaPerguntas.appendChild(item);
      this.reset();
    });