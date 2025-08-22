
window.onload = function() {
  Swal.fire({
    title: 'Deseja iniciar a prova?',
    text: "Você terá 60 minutos para completá-la.",
    icon: 'question',
    showCancelButton: true,
    confirmButtonText: 'Sim',
    cancelButtonText: 'Não',
    allowOutsideClick: false,
    allowEscapeKey: false
  }).then((result) => {
    if(!result.isConfirmed){
      window.location.href = 'CodigoAcessoProva.html';
    } else {
      iniciarTimer();
      document.documentElement.requestFullscreen();
    }
  });
};

let totalSegundos = 60*60; // 60 minutos
let intervalo;
function iniciarTimer(){
  const timerElemento = document.getElementById('timer');
  intervalo = setInterval(function(){
    const minutos = Math.floor(totalSegundos/60);
    const segundos = totalSegundos%60;
    timerElemento.textContent = `${String(minutos).padStart(2,'0')}:${String(segundos).padStart(2,'0')}`;
    if(totalSegundos<=0){
      clearInterval(intervalo);
      Swal.fire({
        icon:'info',
        title:'Tempo esgotado!',
        text:'O tempo da prova terminou.',
        confirmButtonColor:'#673ab7'
      });
      const form = document.getElementById('formProva');
      form.querySelectorAll('input,textarea,select,button').forEach(el=>el.disabled=true);
    }
    totalSegundos--;
  },1000);
}

document.getElementById('formProva').addEventListener('submit', async function(e){
  e.preventDefault();
  const dados = new FormData(this);
  const respostas = {};
  dados.forEach((valor,chave) => {
    respostas[chave] = valor.trim();
  });

  // Salva as respostas no localStorage para a correção posterior
  localStorage.setItem('respostasProvaBD', JSON.stringify(respostas));

  // Simulação de envio (aqui você pode usar axios para enviar pra API real)
  try {
    // Exemplo de envio via axios:
    // await axios.post('https://exemplo.com/enviar-prova-bd', respostas);

    Swal.fire({
      icon:'success',
      title:'Prova enviada!',
      text:'Sua prova foi enviada e salva localmente para correção.',
      confirmButtonColor:'#673ab7'
    }).then(() => {
      // Opcional: redirecionar, ou resetar o form
      this.reset();
      // Sair do fullscreen
      if(document.fullscreenElement) document.exitFullscreen();
      // Redireciona para "página inicial" ou qualquer outra página:
      window.location.href = 'pagina_agradecimento.html';
    });

  } catch(err) {
    Swal.fire({
      icon:'error',
      title:'Erro!',
      text:'Não foi possível enviar a prova.',
      confirmButtonColor:'#673ab7'
    });
  }
});

document.addEventListener('fullscreenchange', () => {
  if(!document.fullscreenElement){
    window.location.href = 'CodigoAcessoProva.html';
  }
});
