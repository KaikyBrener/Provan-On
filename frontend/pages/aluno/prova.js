/**
 * prova.js — Aluno exam-taking page.
 *
 * This page is accessed by students (alunos) who do NOT have a JWT token.
 * All API calls use skipAuth: true so that a 401 does not trigger a login redirect.
 *
 * The backend endpoints called here ideally should be [AllowAnonymous], but
 * until that backend change is made, we call them without triggering the
 * auth-redirect middleware by using skipAuth.
 */

import { apiFetch } from '../../services/api.js';
import { AlunoRespostaObjetivaService } from '../../services/aluno-resposta-objetiva.service.js';
import { showToast } from '../../utils/dom.js';

let timerInterval;
let startTime;
let questoesData = [];
let provaId  = null;
let alunoId  = null;

document.addEventListener('DOMContentLoaded', async () => {
    alunoId = sessionStorage.getItem('alunoId');
    if (!alunoId) {
        window.location.href = 'codigo-acesso.html';
        return;
    }

    const urlParams = new URLSearchParams(window.location.search);
    provaId = urlParams.get('id');

    if (!provaId) {
        showToast('Prova inválida.', 'error');
        setTimeout(() => window.location.href = 'codigo-acesso.html', 1500);
        return;
    }

    await loadProva(provaId);
    startTimer();

    document.getElementById('finalizarBtn').addEventListener('click', checkAndShowModal);
    document.getElementById('confirmSubmitAction').addEventListener('click', submitProva);
});

async function loadProva(id) {
    try {
        // skipAuth: aluno has no JWT token
        const [provaRes, questoesRes] = await Promise.all([
            apiFetch(`/Prova/${id}`,                      { skipAuth: true }),
            apiFetch(`/PerguntaObjetiva/Prova/${id}`,     { skipAuth: true })
        ]);

        if (!provaRes.ok)    throw new Error('Prova não encontrada');
        if (!questoesRes.ok) throw new Error('Questões não encontradas');

        const prova   = await provaRes.json();
        const questoes = await questoesRes.json();

        questoesData = questoes;
        document.getElementById('tituloProva').innerText = prova.titulo || prova.Titulo || 'Prova';

        const container = document.getElementById('questoesLista');
        container.innerHTML = '';

        if (!questoes || questoes.length === 0) {
            container.innerHTML = `<div class="alert alert-warning">Esta prova não possui questões cadastradas.</div>`;
            document.getElementById('loadingState').classList.add('d-none');
            document.getElementById('provaView').classList.remove('d-none');
            return;
        }

        questoes.forEach((q, index) => {
            const numero = index + 1;
            const div    = document.createElement('div');
            div.className = 'card p-4 mb-4 shadow-sm border-0';

            // Support different API field names for alternatives
            const opcoes = q.alternativas || q.opcoes || [];

            let html = `
                <h5 class="fw-bold mb-3"><span class="text-primary me-2">${numero}.</span>${q.enunciado}</h5>
                <div class="row g-3">`;

            opcoes.forEach((opcao, opIndex) => {
                html += `
                    <div class="col-12">
                        <div class="form-check p-0 m-0 position-relative">
                            <input class="form-check-input d-none opacity-0 position-absolute" type="radio"
                                name="questao_${q.id}" id="q${q.id}_op${opIndex}" value="${opIndex}">
                            <label class="opcao-label w-100 m-0" for="q${q.id}_op${opIndex}">
                                <span class="badge bg-secondary me-3" style="font-size: 0.9rem;">${String.fromCharCode(65 + opIndex)}</span>
                                <span class="text-main">${opcao}</span>
                            </label>
                        </div>
                    </div>`;
            });

            html += `</div>`;
            div.innerHTML = html;
            container.appendChild(div);
        });

        document.getElementById('loadingState').classList.add('d-none');
        document.getElementById('provaView').classList.remove('d-none');

    } catch (error) {
        showToast('Erro ao carregar a prova. Tente novamente.', 'error');
        console.error('[prova.js] loadProva:', error);
    }
}

function startTimer() {
    startTime = Date.now();
    const display = document.getElementById('timerDisplay');

    timerInterval = setInterval(() => {
        const elapsed      = Date.now() - startTime;
        const totalSeconds = Math.floor(elapsed / 1000);
        const hours   = String(Math.floor(totalSeconds / 3600)).padStart(2, '0');
        const minutes = String(Math.floor((totalSeconds % 3600) / 60)).padStart(2, '0');
        const seconds = String(totalSeconds % 60).padStart(2, '0');
        if (display) display.innerText = `${hours}:${minutes}:${seconds}`;
    }, 1000);
}

function checkAndShowModal() {
    const respostasCount = document.querySelectorAll('input[type="radio"]:checked').length;
    const warning = document.getElementById('unansweredWarning');

    if (respostasCount < questoesData.length) {
        warning?.classList.remove('d-none');
    } else {
        warning?.classList.add('d-none');
    }

    new bootstrap.Modal(document.getElementById('confirmSubmitModal')).show();
}

async function submitProva() {
    const btn = document.getElementById('confirmSubmitAction');
    btn.disabled  = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Enviando...';

    clearInterval(timerInterval);

    try {
        const inputs = document.querySelectorAll('input[type="radio"]:checked');

        // Submit each answer
        for (const input of inputs) {
            const perguntaId            = input.name.replace('questao_', '');
            const alternativaSelecionada = parseInt(input.value);

            await AlunoRespostaObjetivaService.cadastrar({
                alunoId:              alunoId,
                perguntaObjetivaId:   perguntaId,
                alternativaSelecionada,
                provaId:              provaId
            });
        }

        // Request correction
        await AlunoRespostaObjetivaService.corrigirProva({ alunoId, provaId });

        // Redirect to result page
        window.location.href = `resultado.html?provaId=${provaId}&alunoId=${alunoId}`;

    } catch (error) {
        showToast('Erro ao enviar prova. Tente novamente.', 'error');
        console.error('[prova.js] submitProva:', error);
        btn.disabled  = false;
        btn.innerText = 'Entregar Definitivamente';
        startTimer();
    }
}
