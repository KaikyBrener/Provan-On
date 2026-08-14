/**
 * resultado.js — Aluno exam result page.
 *
 * Aluno has no JWT token. Uses skipAuth: true on all apiFetch calls
 * to avoid unwanted login redirects on 401.
 *
 * alunoId and provaId are read from:
 *   1. URL query string (provaId=X&alunoId=Y)  ← preferred
 *   2. sessionStorage  (alunoId, provaId)       ← fallback
 */

import { apiFetch } from '../../services/api.js';
import { showToast } from '../../utils/dom.js';

document.addEventListener('DOMContentLoaded', async () => {
    const urlParams = new URLSearchParams(window.location.search);

    // Accept alunoId from URL or sessionStorage
    const alunoId = urlParams.get('alunoId') || sessionStorage.getItem('alunoId');
    const provaId = urlParams.get('provaId');

    if (!alunoId || !provaId) {
        showToast('Sessão inválida. Retornando ao início.', 'error');
        setTimeout(() => window.location.href = 'codigo-acesso.html', 1500);
        return;
    }

    await loadResultado(provaId, alunoId);
});

async function loadResultado(provaId, alunoId) {
    try {
        // Both calls use skipAuth — aluno has no JWT token
        const [pontuacaoRes, provaRes] = await Promise.all([
            apiFetch(`/AlunoRespostaObjetiva/PontuacaoAluno?alunoId=${alunoId}&provaId=${provaId}`, { skipAuth: true }),
            apiFetch(`/Prova/${provaId}`, { skipAuth: true })
        ]);

        if (!pontuacaoRes.ok) throw new Error('Erro ao buscar pontuação');
        const pontuacao = await pontuacaoRes.json();

        const prova       = provaRes.ok ? await provaRes.json() : null;
        const tituloProva = prova?.titulo || prova?.Titulo || 'Resultado da Prova';

        const nota     = pontuacao.notaParcial   ?? 0;
        const acertos  = pontuacao.acertos       ?? 0;
        const erros    = pontuacao.erros         ?? 0;
        const pendentes = pontuacao.discursivasPendentes ?? 0;

        document.getElementById('tituloProva').innerText    = tituloProva;
        document.getElementById('notaFinal').innerText      = typeof nota.toFixed === 'function' ? nota.toFixed(1) : nota;
        document.getElementById('acertosCount').innerText   = acertos;
        document.getElementById('errosCount').innerText     = erros;
        document.getElementById('pendentesCount').innerText = pendentes;

        const iconContainer = document.getElementById('iconContainer');
        const notaNum = parseFloat(nota);
        if (iconContainer) {
            if (notaNum >= 7) {
                iconContainer.innerHTML = '<i class="bi bi-trophy text-success" style="font-size: 5rem;"></i>';
                document.getElementById('notaFinal').classList.add('text-success');
            } else if (notaNum >= 5) {
                iconContainer.innerHTML = '<i class="bi bi-emoji-smile text-warning" style="font-size: 5rem;"></i>';
                document.getElementById('notaFinal').classList.add('text-warning');
            } else {
                iconContainer.innerHTML = '<i class="bi bi-exclamation-circle text-danger" style="font-size: 5rem;"></i>';
                document.getElementById('notaFinal').classList.add('text-danger');
            }
        }

        document.getElementById('loadingState').classList.add('d-none');
        document.getElementById('resultadoView').classList.remove('d-none');

    } catch (error) {
        console.error('[resultado.js] loadResultado:', error);
        showToast('Erro ao carregar o resultado.', 'error');
        document.getElementById('loadingState').innerHTML = `
            <div class="alert alert-warning">
                <strong>Resultado indisponível.</strong><br>
                Não foi possível carregar as informações da sua prova.
            </div>`;
    }
}
