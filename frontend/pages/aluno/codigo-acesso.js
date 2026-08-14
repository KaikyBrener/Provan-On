/**
 * codigo-acesso.js — Student exam access page.
 *
 * Endpoint: POST /api/Prova/Entrar  [AllowAnonymous]
 * No JWT token is needed or sent.
 */

import { apiFetch } from '../../services/api.js';
import { showToast } from '../../utils/dom.js';

let provaEncontradaId = null;

document.addEventListener('DOMContentLoaded', async () => {
    // Clear any previous student session data
    sessionStorage.removeItem('alunoId');
    sessionStorage.removeItem('provaId');

    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Acessar Prova';

    setupForm();
});

function setupForm() {
    const form = document.getElementById('acessoForm');
    const btn  = document.getElementById('acessarBtn');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        if (!form.checkValidity()) { form.classList.add('was-validated'); return; }

        const nome           = document.getElementById('nome').value.trim();
        const dataNascimento = document.getElementById('dataNascimento').value;
        const codigo         = document.getElementById('codigo').value.toUpperCase().trim();

        const originalText = btn.innerText;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Verificando...';
        btn.disabled  = true;

        try {
            // skipAuth: this is a public endpoint — never send Bearer
            const response = await apiFetch('/Prova/Entrar', {
                method: 'POST',
                skipAuth: true,
                body: JSON.stringify({ nome, dataNascimento, codigoProva: codigo })
            });

            if (!response.ok) {
                const err = await response.json().catch(() => null);
                showToast(err?.message || 'Código inválido ou prova não encontrada.', 'error');
                return;
            }

            const data = await response.json();

            // Store student session data
            sessionStorage.setItem('alunoId', data.alunoId);
            sessionStorage.setItem('provaId', data.provaId);
            provaEncontradaId = data.provaId;

            document.getElementById('provaTitulo').innerText = data.titulo || 'Prova';
            const qtdEl = document.getElementById('provaQtd');
            if (qtdEl) {
                qtdEl.innerText = data.totalQuestoes
                    ? `${data.totalQuestoes} questões`
                    : 'Questões serão carregadas ao iniciar';
            }

            new bootstrap.Modal(document.getElementById('infoProvaModal')).show();

        } catch (error) {
            console.error('[codigo-acesso.js]', error);
            showToast('Erro ao acessar a prova. Verifique seus dados e o código.', 'error');
        } finally {
            btn.innerText = originalText;
            btn.disabled  = false;
        }
    });

    document.getElementById('iniciarProvaBtn').addEventListener('click', () => {
        if (provaEncontradaId) {
            window.location.href = `prova.html?id=${provaEncontradaId}`;
        }
    });
}
