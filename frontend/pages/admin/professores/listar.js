import { AuthService } from '../../../services/auth.service.js';
import { ProfessorService } from '../../../services/professor.service.js';
import { loadComponent, showToast } from '../../../utils/dom.js';

let professorToDelete = null;

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated() || AuthService.getUser()?.role !== 'admin') {
        window.location.href = '../../auth/login.html';
        return;
    }
    await loadComponent('#sidebar-container', '/components/sidebar-admin.html');
    await loadComponent('#navbar-container', '/components/navbar.html');
    setupSidebarActive();
    setupCommonUI();
    loadProfessores();
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
    document.getElementById('logoutBtn')?.addEventListener('click', () => AuthService.logout());
    document.getElementById('navLogoutBtn')?.addEventListener('click', () => AuthService.logout());
    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Professores';
}

function setupSidebarActive() {
    document.querySelectorAll('.sidebar-link').forEach(link => {
        link.classList.remove('active');
        if (link.href.includes('professores')) link.classList.add('active');
    });
}

async function loadProfessores() {
    const tbody = document.querySelector('#professoresTable tbody');
    try {
        tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-secondary">
            <span class="spinner-border spinner-border-sm me-2"></span>Carregando...</td></tr>`;

        const professores = await ProfessorService.listar();
        tbody.innerHTML = '';

        if (!professores || professores.length === 0) {
            tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-secondary">Nenhum professor encontrado.</td></tr>`;
            return;
        }

        professores.forEach(prof => {
            const tr = document.createElement('tr');
            const dataCadastro = prof.dataCadastro || prof.dataCriacao;
            const dataFormatada = dataCadastro ? new Date(dataCadastro).toLocaleDateString('pt-BR') : '—';
            tr.innerHTML = `
                <td class="ps-4 fw-medium text-main">${prof.nome}</td>
                <td class="text-secondary">${prof.email}</td>
                <td class="text-secondary">${dataFormatada}</td>
                <td class="text-end pe-4">
                    <a href="editar.html?id=${prof.id}" class="btn btn-sm btn-outline-primary me-2 p-1" title="Editar">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 20h9"/><path d="M16.5 3.5a2.12 2.12 0 0 1 3 3L7 19l-4 1 1-4Z"/></svg>
                    </a>
                    <button class="btn btn-sm btn-outline-danger btn-delete p-1" data-id="${prof.id}" data-nome="${prof.nome}" title="Remover">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 6h18"/><path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"/><path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"/><line x1="10" x2="10" y1="11" y2="17"/><line x1="14" x2="14" y1="11" y2="17"/></svg>
                    </button>
                </td>`;
            tbody.appendChild(tr);
        });

        document.querySelectorAll('.btn-delete').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const button = e.currentTarget;
                professorToDelete = button.getAttribute('data-id');
                document.getElementById('deleteProfessorName').innerText = button.getAttribute('data-nome');
                new bootstrap.Modal(document.getElementById('deleteModal')).show();
            });
        });

    } catch (error) {
        showToast('Erro ao carregar lista de professores', 'error');
        tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-danger">Erro de conexão. Tente novamente.</td></tr>`;
    }
}

async function handleDelete() {
    if (!professorToDelete) return;
    const btn = document.getElementById('confirmDeleteBtn');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Removendo...';
    try {
        const ok = await ProfessorService.remover(professorToDelete);
        if (!ok) throw new Error('Falha ao remover');
        showToast('Professor removido com sucesso!');
        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();
        loadProfessores();
    } catch (error) {
        showToast('Erro ao remover professor', 'error');
    } finally {
        btn.disabled = false;
        btn.innerText = 'Remover Professor';
        professorToDelete = null;
    }
}
