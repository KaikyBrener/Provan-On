
  const API_CADASTRO_URL = "https://localhost:7141/Professor/CadastrarProfessor";

  async function cadastrarProfessor(event) {
    event.preventDefault();

    const nome = document.getElementById('nome').value.trim();
    const email = document.getElementById('email').value.trim();
    const senha = document.getElementById('senha').value;

    if (!nome || !email || !senha) {
      Swal.fire('Atenção', 'Por favor, preencha todos os campos.', 'warning');
      return;
    }

    try {
      const response = await axios.post(API_CADASTRO_URL, {
        nome,
        email,
        senha
      });

      if (response.status === 200 || response.status === 201) {
        Swal.fire({
          icon: 'success',
          title: 'Professor cadastrado!',
          html: `ID: <b>${response.data.id}</b><br>Nome: <b>${response.data.nome}</b><br>E-mail: <b>${response.data.email}</b>`,
          confirmButtonText: 'Ok'
        });

        document.getElementById('formCadastro').reset();
      } else {
        Swal.fire('Erro', 'Não foi possível cadastrar o professor.', 'error');
      }
    } catch (error) {
      console.error('Erro no cadastro:', error);
      Swal.fire('Erro', 'Erro ao cadastrar professor. Verifique os dados e a conexão.', 'error');
    }
}
