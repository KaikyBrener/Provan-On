import { AuthService } from '../../../services/auth.service.js';
import { AlunoService } from '../../../services/aluno.service.js';
import { loadComponent, showToast } from '../../../utils/dom.js';

let alunoToDelete = null;

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated() || AuthService.getUser()?.role !== 'admin') {
        window.location.href = '../../auth/login.html';
        return;
    }

    await loadComponent('#sidebar-container', '/components/sidebar-admin.html');
    await loadComponent('#navbar-container', '/components/navbar.html');
    setupSidebarActive();
    setupCommonUI();

    loadAlunos();
    document.getElementById('confirmDeleteBtn').addEventListener('click', handleDelete);
});

function setupCommonUI() {
    const user = AuthService.getUser();
    const userNameDisplay = document.getElementById('userNameDisplay');
    const userInitials = document.getElementById('userInitials');
    if (user) {
        if (userNameDisplay) userNameDisplay.innerText = user.nome || 'Admin';
        if (userInitials) {
            const initials = user.nome ? user.nome.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase() : 'AD';
            userInitials.innerText = initials;
        }
    }
    const logoutBtn = document.getElementById('logoutBtn');
    if (logoutBtn) logoutBtn.addEventListener('click', () => AuthService.logout());
    const navLogout = document.getElementById('navLogoutBtn');
    if (navLogout) navLogout.addEventListener('click', () => AuthService.logout());
    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Alunos';
}

function setupSidebarActive() {
    document.querySelectorAll('.sidebar-link').forEach(link => {
        link.classList.remove('active');
        if (link.href.includes('alunos')) link.classList.add('active');
    });
}

async function loadAlunos() {
    const tbody = document.querySelector('#alunosTable tbody');
    try {
        tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-secondary">
            <span class="spinner-border spinner-border-sm me-2"></span>Carregando...</td></tr>`;

        const alunos = await AlunoService.listar();
        tbody.innerHTML = '';

        if (!alunos || alunos.length === 0) {
            tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-secondary">Nenhum aluno encontrado.</td></tr>`;
            return;
        }

        alunos.forEach(aluno => {
            const tr = document.createElement('tr');
            const dataCadastro = aluno.dataCadastro || aluno.dataCriacao;
            const dataFormatada = dataCadastro ? new Date(dataCadastro).toLocaleDateString('pt-BR') : '—';
            tr.innerHTML = `
                <td class="ps-4 fw-medium text-main">${aluno.nome}</td>
                <td class="text-secondary">${aluno.email}</td>
                <td class="text-secondary">${dataFormatada}</td>
                <td class="text-end pe-4">
                    <a href="editar.html?id=${aluno.id}" class="btn btn-sm btn-outline-primary me-2 p-1" title="Editar">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 20h9"/><path d="M16.5 3.5a2.12 2.12 0 0 1 3 3L7 19l-4 1 1-4Z"/></svg>
                    </a>
                    <button class="btn btn-sm btn-outline-danger btn-delete p-1" data-id="${aluno.id}" data-nome="${aluno.nome}" title="Remover">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 6h18"/><path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"/><path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"/><line x1="10" x2="10" y1="11" y2="17"/><line x1="14" x2="14" y1="11" y2="17"/></svg>
                    </button>
                </td>`;
            tbody.appendChild(tr);
        });

        document.querySelectorAll('.btn-delete').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const button = e.currentTarget;
                alunoToDelete = button.getAttribute('data-id');
                document.getElementById('deleteAlunoName').innerText = button.getAttribute('data-nome');
                new bootstrap.Modal(document.getElementById('deleteModal')).show();
            });
        });

    } catch (error) {
        showToast('Erro ao carregar lista de alunos', 'error');
        tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-danger">Erro de conexão. Tente novamente.</td></tr>`;
    }
}

async function handleDelete() {
    if (!alunoToDelete) return;
    const btn = document.getElementById('confirmDeleteBtn');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Removendo...';
    try {
        const ok = await AlunoService.remover(alunoToDelete);
        if (!ok) throw new Error('Falha ao remover');
        showToast('Aluno removido com sucesso!');
        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();
        loadAlunos();
    } catch (error) {
        showToast('Erro ao remover aluno', 'error');
    } finally {
        btn.disabled = false;
        btn.innerText = 'Remover Aluno';
        alunoToDelete = null;
    }
}
