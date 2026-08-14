import { AuthService } from '../../../services/auth.service.js';
import { ProvaService } from '../../../services/prova.service.js';
import { loadComponent, showToast } from '../../../utils/dom.js';

let provaToDelete = null;

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated() || AuthService.getUser()?.role !== 'professor') {
        window.location.href = '../../auth/login.html';
        return;
    }
    await loadComponent('#sidebar-container', '/components/sidebar-professor.html');
    await loadComponent('#navbar-container', '/components/navbar.html');
    setupSidebarActive();
    setupCommonUI();
    loadProvas();
    document.getElementById('confirmDeleteBtn').addEventListener('click', handleDelete);
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
    if (pageTitle) pageTitle.innerText = 'Minhas Provas';
}

function setupSidebarActive() {
    document.querySelectorAll('.sidebar-link').forEach(link => {
        link.classList.remove('active');
        if (link.href.includes('provas')) link.classList.add('active');
    });
}

async function loadProvas() {
    const tbody = document.querySelector('#provasTable tbody');
    try {
        tbody.innerHTML = `<tr><td colspan="5" class="text-center py-4 text-secondary">
            <span class="spinner-border spinner-border-sm me-2"></span>Carregando...</td></tr>`;

        const user = AuthService.getUser();
        const provas = await ProvaService.listarPorProfessor(user.id);
        tbody.innerHTML = '';

        if (!provas || provas.length === 0) {
            tbody.innerHTML = `<tr><td colspan="5" class="text-center py-4 text-secondary">Nenhuma prova encontrada.</td></tr>`;
            return;
        }

        provas.forEach(prova => {
            const tr = document.createElement('tr');
            const statusAtivo = prova.status === 'Ativa' || prova.ativa;
            const statusBadge = statusAtivo
                ? '<span class="badge bg-success bg-opacity-10 text-success border border-success border-opacity-25 px-2.5 py-1">Ativa</span>'
                : '<span class="badge bg-secondary bg-opacity-10 text-secondary border border-secondary border-opacity-25 px-2.5 py-1">Fechada</span>';
            const data = prova.data || prova.dataCriacao;
            const dataFormatada = data ? new Date(data).toLocaleDateString('pt-BR') : '—';

            tr.innerHTML = `
                <td class="ps-4 fw-medium text-main">${prova.titulo}</td>
                <td><code class="px-2 py-0.5 bg-dark rounded text-primary border border-opacity-50" style="font-size: 0.8rem">${prova.codigoAcesso || prova.codigo || '—'}</code></td>
                <td class="text-secondary">${dataFormatada}</td>
                <td>${statusBadge}</td>
                <td class="text-end pe-4">
                    <button class="btn btn-sm btn-outline-danger btn-delete p-1" data-id="${prova.id}" data-titulo="${prova.titulo}" title="Remover">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 6h18"/><path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"/><path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"/><line x1="10" x2="10" y1="11" y2="17"/><line x1="14" x2="14" y1="11" y2="17"/></svg>
                    </button>
                </td>`;
            tbody.appendChild(tr);
        });

        document.querySelectorAll('.btn-delete').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const button = e.currentTarget;
                provaToDelete = button.getAttribute('data-id');
                document.getElementById('deleteProvaTitle').innerText = button.getAttribute('data-titulo');
                new bootstrap.Modal(document.getElementById('deleteModal')).show();
            });
        });

    } catch (error) {
        showToast('Erro ao carregar lista de provas', 'error');
        tbody.innerHTML = `<tr><td colspan="5" class="text-center py-4 text-danger">Erro de conexão. Tente novamente.</td></tr>`;
    }
}

async function handleDelete() {
    if (!provaToDelete) return;
    const btn = document.getElementById('confirmDeleteBtn');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Removendo...';
    try {
        const ok = await ProvaService.remover(provaToDelete);
        if (!ok) throw new Error('Falha ao remover');
        showToast('Prova removida com sucesso!');
        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();
        loadProvas();
    } catch (error) {
        showToast('Erro ao remover prova', 'error');
    } finally {
        btn.disabled = false;
        btn.innerText = 'Remover Prova';
        provaToDelete = null;
    }
}
