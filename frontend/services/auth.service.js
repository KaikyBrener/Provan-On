/**
 * auth.service.js — Authentication service for Prova-On frontend.
 *
 * Handles:
 *   - Login (POST /api/Auth/login)
 *   - Logout (local only — backend has no logout endpoint)
 *   - Session read (getUser, getRole, isAuthenticated)
 *
 * Token field mapping (backend LoginResponse):
 *   response.token    → setAccessToken()
 *   response.usuario  → sessionStorage 'user'
 *
 * localStorage keys standardized:
 *   sessionStorage: 'user'   (JSON object: { id, nome, email, role })
 *   in-memory:      accessToken via api.js
 */

import { apiFetch, setAccessToken, getAccessToken } from './api.js';

// ── Internal: compute login page path dynamically ────────────────────────────
const getLoginPath = () => {
    const url = window.location.href;
    const idx = url.indexOf('/frontend/');
    const base = idx !== -1 ? url.substring(0, idx) : window.location.origin;
    return `${base}/frontend/pages/auth/login.html`;
};

// ── AuthService ───────────────────────────────────────────────────────────────
export const AuthService = {

    /**
     * Authenticate a professor or admin.
     * Backend returns: { token: string, usuario: { id, nome, email, role } }
     */
    async login(email, password) {
        const response = await apiFetch('/Auth/login', {
            method: 'POST',
            skipAuth: true,   // public endpoint — never send Bearer
            body: JSON.stringify({ email, senha: password })
        });

        if (!response.ok) {
            const err = await response.json().catch(() => null);
            throw new Error(err?.message || 'Credenciais inválidas');
        }

        const data = await response.json();

        // Map backend fields → frontend storage
        const token   = data.token   || data.accessToken;   // tolerate both spellings
        const usuario = data.usuario || data.user;           // tolerate both spellings

        if (!token || !usuario) {
            throw new Error('Resposta de login inválida do servidor.');
        }

        setAccessToken(token);
        sessionStorage.setItem('user', JSON.stringify(usuario));

        return usuario;
    },

    /**
     * Logout: clears in-memory token + sessionStorage, then redirects.
     * (Backend has no logout endpoint since RefreshToken was removed.)
     */
    logout() {
        setAccessToken(null);
        sessionStorage.removeItem('user');
        sessionStorage.removeItem('role');
        window.location.href = getLoginPath();
    },

    /**
     * Returns true if a token exists in memory OR a user is stored in session.
     * The in-memory token is lost on page refresh, so we also check sessionStorage
     * as a secondary indicator (user was logged in this tab session).
     */
    isAuthenticated() {
        return getAccessToken() !== null || sessionStorage.getItem('user') !== null;
    },

    /**
     * Returns the stored user object or null.
     */
    getUser() {
        const raw = sessionStorage.getItem('user');
        return raw ? JSON.parse(raw) : null;
    },

    /**
     * Returns the role string ('admin' | 'professor') or null.
     */
    getRole() {
        return this.getUser()?.role ?? null;
    }
};
