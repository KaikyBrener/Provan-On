
    document.getElementById('formCodigo').addEventListener('submit', function(e) {
      e.preventDefault();
      const codigo = document.getElementById('input_codigo').value.trim();

      if (codigo === '') {
        alert('Por favor, insira um código.');
        return;
      }

      // Aqui você pode validar o código com Axios se necessário

      // Redireciona para a próxima etapa
      window.location.href = "CadastroAluno.html";
    });