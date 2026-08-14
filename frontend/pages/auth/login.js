import { AuthService } from '../../services/auth.service.js';
import { showToast } from '../../utils/dom.js';

document.addEventListener('DOMContentLoaded', () => {
    // If already authenticated, skip to dashboard
    if (AuthService.isAuthenticated()) {
        const user = AuthService.getUser();
        if (user?.role === 'admin')     { window.location.href = '../admin/dashboard.html'; return; }
        if (user?.role === 'professor') { window.location.href = '../professor/dashboard.html'; return; }
    }

    const loginForm = document.getElementById('loginForm');
    const loginBtn  = document.getElementById('loginBtn');

    loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const email    = document.getElementById('email').value.trim();
        const password = document.getElementById('password').value;

        loginBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Entrando...';
        loginBtn.disabled  = true;

        try {
            const user = await AuthService.login(email, password);

            showToast('Login realizado com sucesso!');

            // Redirect based on role — short delay so the toast is visible
            setTimeout(() => {
                if (user.role === 'admin') {
                    window.location.href = '../admin/dashboard.html';
                } else if (user.role === 'professor') {
                    window.location.href = '../professor/dashboard.html';
                } else {
                    // Unexpected role — clear session and show error
                    AuthService.logout();
                    showToast('Perfil sem acesso ao painel. Use o código de acesso da prova.', 'error');
                }
            }, 800);

        } catch (error) {
            showToast(error.message || 'Credenciais inválidas.', 'error');
            loginBtn.innerText = 'Entrar';
            loginBtn.disabled  = false;
        }
    });
});
