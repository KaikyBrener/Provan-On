import { AuthService } from '../../../services/auth.service.js';
import { ProvaService } from '../../../services/prova.service.js';
import { PerguntaObjetivaService } from '../../../services/pergunta-objetiva.service.js';
import { loadComponent, showToast } from '../../../utils/dom.js';

let questaoCount = 0;

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated() || AuthService.getUser()?.role !== 'professor') {
        window.location.href = '../../auth/login.html';
        return;
    }
    await loadComponent('#sidebar-container', '/components/sidebar-professor.html');
    await loadComponent('#navbar-container', '/components/navbar.html');
    setupSidebarActive();
    setupCommonUI();
    setupForm();
});

function setupCommonUI() {
    const user = AuthService.getUser();
    const userNameDisplay = document.getElementById('userNameDisplay');
    const userInitials = document.getElementById('userInitials');
    if (user) {
        if (userNameDisplay) userNameDisplay.innerText = user.nome || 'Professor';
        if (userInitials) {
            const initials = user.nome ? user.nome.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase() : 'PR';
            userInitials.innerText = initials;
        }
    }
    document.getElementById('logoutBtn')?.addEventListener('click', () => AuthService.logout());
    document.getElementById('navLogoutBtn')?.addEventListener('click', () => AuthService.logout());
    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Criar Prova';
}

function setupSidebarActive() {
    document.querySelectorAll('.sidebar-link').forEach(link => {
        link.classList.remove('active');
        if (link.href.includes('provas')) link.classList.add('active');
    });
}

function setupForm() {
    document.getElementById('addQuestaoBtn').addEventListener('click', renderNovaQuestao);

    const form = document.getElementById('cadastrarProvaForm');
    const submitBtn = document.getElementById('submitBtn');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        if (!form.checkValidity()) { form.classList.add('was-validated'); return; }
        if (questaoCount === 0) { showToast('Adicione pelo menos uma questão à prova.', 'error'); return; }
        form.classList.add('was-validated');

        const user = AuthService.getUser();
        const questoes = collectQuestoes();

        const provaData = {
            titulo: document.getElementById('titulo').value,
            codigoAcesso: document.getElementById('codigoAcesso').value.toUpperCase(),
            professorId: user.id
        };

        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Salvando...';
        submitBtn.disabled = true;

        try {
            // 1. Cria a prova
            const prova = await ProvaService.criar(provaData);

            // 2. Cria cada questão objetiva vinculada à prova
            for (const q of questoes) {
                await PerguntaObjetivaService.criar({
                    enunciado: q.enunciado,
                    alternativas: q.opcoes,
                    respostaCorreta: q.correta,
                    provaId: prova.id
                });
            }

            showToast('Prova criada com sucesso!', 'success');
            setTimeout(() => window.location.href = 'listar.html', 1000);
        } catch (error) {
            showToast(error.message || 'Erro ao criar prova.', 'error');
        } finally {
            submitBtn.innerText = 'Salvar e Publicar Prova';
            submitBtn.disabled = false;
        }
    });
}

function renderNovaQuestao() {
    document.getElementById('emptyState')?.classList.add('d-none');
    questaoCount++;
    const container = document.getElementById('questoesContainer');

    const div = document.createElement('div');
    div.className = 'card p-4 mb-4 questao-item';
    div.style.border = '1px solid var(--border-color)';
    div.dataset.index = questaoCount;

    div.innerHTML = `
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h6 class="fw-bold mb-0" style="color: var(--primary-light)">Questão ${questaoCount}</h6>
            <button type="button" class="btn btn-sm btn-link text-danger p-0 btn-remove-questao d-flex align-items-center gap-1 text-decoration-none">
                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 6h18"/><path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"/><path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"/></svg>
                Remover Questão
            </button>
        </div>
        <div class="mb-4">
            <label class="form-label small text-secondary">Enunciado da Questão</label>
            <textarea class="form-control" placeholder="Digite o enunciado completo..." required rows="2" name="enunciado_${questaoCount}"></textarea>
        </div>
        <div class="mb-2">
            <label class="form-label small text-secondary">Alternativas (Marque a opção correta)</label>
        </div>
        <div class="row g-3">
            <div class="col-md-6 d-flex align-items-center">
                <div class="form-check me-2"><input class="form-check-input" type="radio" name="correta_${questaoCount}" value="0" required></div>
                <input type="text" class="form-control" placeholder="Alternativa A" required name="opA_${questaoCount}">
            </div>
            <div class="col-md-6 d-flex align-items-center">
                <div class="form-check me-2"><input class="form-check-input" type="radio" name="correta_${questaoCount}" value="1" required></div>
                <input type="text" class="form-control" placeholder="Alternativa B" required name="opB_${questaoCount}">
            </div>
            <div class="col-md-6 d-flex align-items-center">
                <div class="form-check me-2"><input class="form-check-input" type="radio" name="correta_${questaoCount}" value="2" required></div>
                <input type="text" class="form-control" placeholder="Alternativa C" required name="opC_${questaoCount}">
            </div>
            <div class="col-md-6 d-flex align-items-center">
                <div class="form-check me-2"><input class="form-check-input" type="radio" name="correta_${questaoCount}" value="3" required></div>
                <input type="text" class="form-control" placeholder="Alternativa D" required name="opD_${questaoCount}">
            </div>
        </div>`;

    div.querySelector('.btn-remove-questao').addEventListener('click', () => {
        div.remove();
        questaoCount--;
        if (questaoCount === 0) document.getElementById('emptyState')?.classList.remove('d-none');
        renumberQuestoes();
    });

    container.appendChild(div);
}

function renumberQuestoes() {
    document.querySelectorAll('.questao-item').forEach((item, idx) => {
        const num = idx + 1;
        item.dataset.index = num;
        item.querySelector('h6').innerText = `Questão ${num}`;
        item.querySelector('textarea').name = `enunciado_${num}`;
        item.querySelectorAll('input[type="radio"]').forEach(r => r.name = `correta_${num}`);
        item.querySelector('[placeholder="Alternativa A"]').name = `opA_${num}`;
        item.querySelector('[placeholder="Alternativa B"]').name = `opB_${num}`;
        item.querySelector('[placeholder="Alternativa C"]').name = `opC_${num}`;
        item.querySelector('[placeholder="Alternativa D"]').name = `opD_${num}`;
    });
    questaoCount = document.querySelectorAll('.questao-item').length;
}

function collectQuestoes() {
    const itens = document.querySelectorAll('.questao-item');
    const result = [];
    itens.forEach(item => {
        const index = item.dataset.index;
        result.push({
            enunciado: document.querySelector(`[name="enunciado_${index}"]`).value,
            opcoes: [
                document.querySelector(`[name="opA_${index}"]`).value,
                document.querySelector(`[name="opB_${index}"]`).value,
                document.querySelector(`[name="opC_${index}"]`).value,
                document.querySelector(`[name="opD_${index}"]`).value,
            ],
            correta: parseInt(document.querySelector(`[name="correta_${index}"]:checked`)?.value)
        });
    });
    return result;
}
