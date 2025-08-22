 document.getElementById("logoutBtn").addEventListener("click", () => {
      localStorage.removeItem("professorLogado");
      Swal.fire({
        icon: 'info',
        title: 'Logout realizado',
        timer: 1500,
        showConfirmButton: false
      }).then(() => {
        window.location.href = "Login.html";
      });
    });