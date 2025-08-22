
    // Máscara para data de nascimento
    IMask(document.getElementById('dataNascimento'), {
      mask: '00/00/0000'
    });

    document.getElementById('formAluno').addEventListener('submit', async function(e) {
      e.preventDefault();

      const nome = document.getElementById('nome').value.trim();
      const data = document.getElementById('dataNascimento').value.trim();

      const partes = data.split('/');
      if (partes.length !== 3 || partes[2].length !== 4) {
        Swal.fire({
          icon: 'error',
          title: 'Data inválida',
          text: 'Por favor, insira uma data no formato dd/mm/aaaa.'
        });
        return;
      }

      const dataISO = `${partes[2]}-${partes[1]}-${partes[0]}`;

      try {
        const response = await axios.post('https://jsonplaceholder.typicode.com/posts', {
          nome: nome,
          dataNascimento: dataISO
        });

        Swal.fire({
          icon: 'success',
          title: 'Sucesso!',
          text: 'Aluno cadastrado com sucesso.'
        }).then(() => {
          window.location.href = "prova.html";
        });

        this.reset();
      } catch (error) {
        console.error(error);
        Swal.fire({
          icon: 'error',
          title: 'Erro ao cadastrar',
          text: 'Verifique se a API está ativa e funcionando corretamente.'
        });
      }
    });