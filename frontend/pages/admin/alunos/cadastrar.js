import { AuthService } from '../../../services/auth.service.js';
import { AlunoService } from '../../../services/aluno.service.js';
import { loadComponent, showToast } from '../../../utils/dom.js';

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated() || AuthService.getUser()?.role !== 'admin') {
        window.location.href = '../../auth/login.html';
        return;
    }
    await loadComponent('#sidebar-container', '/components/sidebar-admin.html');
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
        if (userNameDisplay) userNameDisplay.innerText = user.nome || 'Admin';
        if (userInitials) {
            const initials = user.nome ? user.nome.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase() : 'AD';
            userInitials.innerText = initials;
        }
    }
    document.getElementById('logoutBtn')?.addEventListener('click', () => AuthService.logout());
    document.getElementById('navLogoutBtn')?.addEventListener('click', () => AuthService.logout());
    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Cadastrar Aluno';
}

function setupSidebarActive() {
    document.querySelectorAll('.sidebar-link').forEach(link => {
        link.classList.remove('active');
        if (link.href.includes('alunos')) link.classList.add('active');
    });
}

function setupForm() {
    const toggleSenhaBtn = document.getElementById('toggleSenha');
    const senhaInput = document.getElementById('senha');
    if (toggleSenhaBtn && senhaInput) {
        toggleSenhaBtn.addEventListener('click', () => {
            const type = senhaInput.getAttribute('type') === 'password' ? 'text' : 'password';
            senhaInput.setAttribute('type', type);
            document.getElementById('eyeOpen')?.classList.toggle('d-none', type !== 'password');
            document.getElementById('eyeClosed')?.classList.toggle('d-none', type === 'password');
        });
    }

    const form = document.getElementById('cadastrarAlunoForm');
    const submitBtn = document.getElementById('submitBtn');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        if (!form.checkValidity()) { form.classList.add('was-validated'); return; }
        form.classList.add('was-validated');

        const data = {
            nome: document.getElementById('nome').value,
            email: document.getElementById('email').value,
            senha: senhaInput.value,
            matricula: document.getElementById('matricula').value
        };

        const originalText = submitBtn.innerText;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Salvando...';
        submitBtn.disabled = true;

        try {
            await AlunoService.criar(data);
            showToast('Aluno cadastrado com sucesso!', 'success');
            setTimeout(() => window.location.href = 'listar.html', 1000);
        } catch (error) {
            showToast(error.message || 'Erro ao cadastrar aluno.', 'error');
        } finally {
            submitBtn.innerText = originalText;
            submitBtn.disabled = false;
        }
    });
}
