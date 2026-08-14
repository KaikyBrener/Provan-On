/**
 * Calculates the base URL of the /frontend/ folder dynamically.
 */
const getFrontendBase = () => {
    const url = window.location.href;
    const idx = url.indexOf('/frontend/');
    if (idx !== -1) {
        return url.substring(0, idx + '/frontend'.length);
    }
    return window.location.origin + '/frontend';
};

/**
 * Loads HTML components dynamically and dispatches componentLoaded.
 * Also automatically registers mobile menu drawer behavior if sidebar/navbar load.
 */
export const loadComponent = async (selector, componentPath) => {
    try {
        const element = document.querySelector(selector);
        if (!element) {
            console.warn(`Element with selector '${selector}' not found.`);
            return;
        }

        const base = getFrontendBase();
        const cleanPath = componentPath.startsWith('/') ? componentPath : '/' + componentPath;
        const fullUrl = `${base}${cleanPath}`;

        const response = await fetch(fullUrl);
        if (!response.ok) throw new Error(`Failed to load component: ${fullUrl} (${response.status})`);
        
        const html = await response.text();
        element.innerHTML = html;
        element.dispatchEvent(new Event('componentLoaded'));

        // Initialize responsive drawer behavior automatically
        initResponsiveDrawer();
    } catch (error) {
        console.error('[loadComponent]', error.message);
    }
};

/**
 * Show a premium toast notification using custom theme classes and Lucide SVG icons.
 */
export const showToast = (message, type = 'success') => {
    let toastContainer = document.getElementById('toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.id = 'toast-container';
        document.body.appendChild(toastContainer);
    }

    let iconSvg = '';
    if (type === 'success') {
        iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-check-circle-2"><circle cx="12" cy="12" r="10"/><path d="m9 12 2 2 4-4"/></svg>`;
    } else if (type === 'error') {
        iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-alert-circle"><circle cx="12" cy="12" r="10"/><line x1="12" x2="12" y1="8" y2="12"/><line x1="12" x2="12.01" y1="16" y2="16"/></svg>`;
    } else if (type === 'warning') {
        iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-alert-triangle"><path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z"/><line x1="12" x2="12" y1="9" y2="13"/><line x1="12" x2="12.01" y1="17" y2="17"/></svg>`;
    } else {
        iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-info"><circle cx="12" cy="12" r="10"/><path d="M12 16v-4"/><path d="M12 8h.01"/></svg>`;
    }

    const toastId = 'toast_' + Date.now();
    const toastHtml = `
        <div class="pvo-toast toast-${type}" id="${toastId}">
            <div class="pvo-toast-icon">${iconSvg}</div>
            <div class="pvo-toast-message">${message}</div>
            <button class="pvo-toast-close" aria-label="Fechar">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-x"><line x1="18" x2="6" y1="6" y2="18"/><line x1="6" x2="18" y1="6" y2="18"/></svg>
            </button>
        </div>
    `;

    toastContainer.insertAdjacentHTML('beforeend', toastHtml);
    const toastEl = document.getElementById(toastId);

    // Bind close button
    const closeBtn = toastEl.querySelector('.pvo-toast-close');
    const dismiss = () => {
        if (toastEl.classList.contains('toast-exit')) return;
        toastEl.classList.add('toast-exit');
        toastEl.addEventListener('animationend', () => {
            toastEl.remove();
        });
    };

    closeBtn.addEventListener('click', dismiss);

    // Auto-remove after 4 seconds
    setTimeout(dismiss, 4000);
};

export const showSpinner = (selector) => {
    const el = document.querySelector(selector);
    if (el) {
        el.innerHTML = '<div class="d-flex justify-content-center"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Carregando...</span></div></div>';
    }
};

/**
 * Mobile drawer navigation logic. Ensures the hamburger button toggles
 * the sidebar, and creates/binds an overlay to close it when clicking outside.
 */
function initResponsiveDrawer() {
    const toggleBtn = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.sidebar');
    if (!toggleBtn || !sidebar) return;

    // Remove any existing overlay first
    let overlay = document.querySelector('.sidebar-overlay');
    if (!overlay) {
        overlay = document.createElement('div');
        overlay.className = 'sidebar-overlay';
        document.body.appendChild(overlay);
    }

    const openSidebar = () => {
        sidebar.classList.add('open');
        overlay.classList.add('active');
        document.body.style.overflow = 'hidden';
    };

    const closeSidebar = () => {
        sidebar.classList.remove('open');
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    };

    // Re-bind to prevent duplicate listeners
    toggleBtn.replaceWith(toggleBtn.cloneNode(true));
    const newToggleBtn = document.getElementById('sidebarToggle');
    
    newToggleBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        if (sidebar.classList.contains('open')) {
            closeSidebar();
        } else {
            openSidebar();
        }
    });

    overlay.replaceWith(overlay.cloneNode(true));
    const newOverlay = document.querySelector('.sidebar-overlay');
    newOverlay.addEventListener('click', closeSidebar);
}
