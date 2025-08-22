    // Máscara opcional para e-mail (pode ser removida se causar problema)
    const emailInput = document.getElementById("email");
    Inputmask({ "mask": "*{1,15}@[a-z]{1,15}.com" }).mask(emailInput);

    // Form login
    document.getElementById("loginForm").addEventListener("submit", async function (event) {
      event.preventDefault();

      const email = document.getElementById("email").value.trim();
      const senha = document.getElementById("password").value.trim();

      if (!email || !senha) {
        Swal.fire({
          icon: "warning",
          title: "Atenção",
          text: "Preencha todos os campos."
        });
        return;
      }

      try {
        const response = await axios.post("https://localhost:7141/Professor/LoginProfessor", { email, senha });

        if (response.data.sucesso) {
          Swal.fire({
            icon: "success",
            title: "Sucesso!",
            text: "Login realizado com sucesso!"
          }).then(() => {
            window.location.href = "PainelProfessor.html";
          });
        } else {
          Swal.fire({
            icon: "error",
            title: "Erro",
            text: "E-mail ou senha inválidos."
          });
        }
      } catch (error) {
        console.error("Erro na requisição:", error);
        Swal.fire({
          icon: "error",
          title: "Erro",
          text: "Ocorreu um erro ao tentar realizar o login."
        });
      }
    });