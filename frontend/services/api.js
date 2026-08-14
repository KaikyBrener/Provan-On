/**
 * api.js — Central API client for Prova-On frontend.
 *
 * Single source of truth for:
 *   - API base URL
 *   - In-memory token storage (avoids localStorage exposure)
 *   - Authenticated fetch wrapper (apiFetch)
 *
 * NOTE: RefreshToken was removed from the backend.
 * On 401, the session is cleared and the user is redirected to login.
 */

export const API_BASE_URL = 'https://localhost:7141/api';

// ── Token stored in memory only (not localStorage) ────────────────────────────
let _memoryToken = null;

export const getAccessToken = () => _memoryToken;

export const setAccessToken = (token) => {
    _memoryToken = token;
};

// ── Internal helper: redirect to login and clear session ──────────────────────
const redirectToLogin = () => {
    setAccessToken(null);
    sessionStorage.removeItem('user');
    sessionStorage.removeItem('role');

    // Resolve the login path relative to the current page
    const url = window.location.href;
    const idx = url.indexOf('/frontend/');
    const base = idx !== -1 ? url.substring(0, idx) : window.location.origin;
    window.location.href = `${base}/frontend/pages/auth/login.html`;
};

// ── Main fetch wrapper ────────────────────────────────────────────────────────
/**
 * apiFetch(endpoint, options)
 *
 * Wraps fetch() with:
 *   - Automatic base-URL prepending
 *   - Authorization: Bearer <token> header (only when a token exists)
 *   - 401 → clear session + redirect to login
 *   - 403 → throw "Acesso negado."
 *   - 5xx → throw "Erro interno do servidor."
 *
 * To call a public endpoint WITHOUT a token even if one exists,
 * pass options.skipAuth = true.
 */
export const apiFetch = async (endpoint, options = {}) => {
    const url = `${API_BASE_URL}${endpoint}`;

    const headers = {
        'Content-Type': 'application/json',
        ...options.headers
    };

    // Inject token unless caller explicitly opts out
    if (!options.skipAuth) {
        const token = getAccessToken();
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
    }

    // Build config — remove our custom flag before passing to fetch
    const { skipAuth, ...restOptions } = options;
    const config = { ...restOptions, headers };

    try {
        const response = await fetch(url, config);

        if (response.status === 401) {
            redirectToLogin();
            throw new Error('Sessão expirada. Faça login novamente.');
        }

        if (response.status === 403) {
            throw new Error('Acesso negado.');
        }

        if (response.status >= 500) {
            throw new Error('Erro interno do servidor.');
        }

        return response;
    } catch (error) {
        // Re-throw so callers can handle it
        console.error('[apiFetch]', endpoint, error.message);
        throw error;
    }
};
