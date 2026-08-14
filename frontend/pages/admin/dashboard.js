import { AuthService } from '../../services/auth.service.js';
import { AlunoService } from '../../services/aluno.service.js';
import { ProfessorService } from '../../services/professor.service.js';
import { ProvaService } from '../../services/prova.service.js';
import { loadComponent, showToast } from '../../utils/dom.js';

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated()) {
        window.location.href = '../auth/login.html';
        return;
    }

    const user = AuthService.getUser();
    if (!user || user.role !== 'admin') {
        window.location.href = '../auth/login.html';
        return;
    }

    await loadComponent('#sidebar-container', '/components/sidebar-admin.html');
    await loadComponent('#navbar-container', '/components/navbar.html');

    setupCommonUI(user);
    await loadDashboardData();
});

function setupCommonUI(user) {
    const userNameDisplay = document.getElementById('userNameDisplay');
    const userInitials = document.getElementById('userInitials');
    if (user) {
        if (userNameDisplay) userNameDisplay.innerText = user.nome || 'Administrador';
        if (userInitials) {
            const initials = user.nome ? user.nome.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase() : 'AD';
            userInitials.innerText = initials;
        }
    }

    const logoutHandler = () => { AuthService.logout(); };

    const sidebarLogout = document.getElementById('logoutBtn');
    const navLogout = document.getElementById('navLogoutBtn');
    if (sidebarLogout) sidebarLogout.addEventListener('click', logoutHandler);
    if (navLogout) navLogout.addEventListener('click', logoutHandler);

    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Visão Geral';
}

async function loadDashboardData() {
    try {
        const [alunos, professores, provas] = await Promise.all([
            AlunoService.listar().catch(() => []),
            ProfessorService.listar().catch(() => []),
            ProvaService.listar().catch(() => [])
        ]);

        const totalAlunos = document.getElementById('totalAlunos');
        const totalProfessores = document.getElementById('totalProfessores');
        const totalProvas = document.getElementById('totalProvas');

        if (totalAlunos) totalAlunos.innerText = alunos.length;
        if (totalProfessores) totalProfessores.innerText = professores.length;
        if (totalProvas) totalProvas.innerText = provas.length;

        renderChart(alunos, provas);
    } catch (error) {
        showToast('Erro ao carregar dados do dashboard', 'error');
    }
}

function renderChart(alunos, provas) {
    const chartContainer = document.getElementById('performanceChart');
    if (!chartContainer) return;

    // Agrupar provas por mês (campo dataCriacao ou similar)
    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    const provasPorMes = Array(12).fill(0);
    const alunosPorMes = Array(12).fill(0);

    provas.forEach(p => {
        const d = new Date(p.dataCriacao || p.data || null);
        if (!isNaN(d)) provasPorMes[d.getMonth()]++;
    });
    alunos.forEach(a => {
        const d = new Date(a.dataCadastro || a.dataCriacao || null);
        if (!isNaN(d)) alunosPorMes[d.getMonth()]++;
    });

    const options = {
        series: [
            { name: 'Provas Criadas', data: provasPorMes },
            { name: 'Alunos Cadastrados', data: alunosPorMes }
        ],
        chart: {
            type: 'area', height: 320, toolbar: { show: false },
            zoom: { enabled: false }, fontFamily: 'Inter, system-ui, sans-serif',
            background: 'transparent'
        },
        theme: { mode: 'dark' },
        colors: ['#2563EB', '#22C55E'],
        dataLabels: { enabled: false },
        stroke: { curve: 'smooth', width: 2.5 },
        fill: {
            type: 'gradient',
            gradient: { shadeIntensity: 1, opacityFrom: 0.3, opacityTo: 0, stops: [0, 90, 100] }
        },
        grid: {
            borderColor: 'rgba(255,255,255,0.06)',
            xaxis: { lines: { show: false } },
            yaxis: { lines: { show: true } }
        },
        xaxis: {
            categories: meses,
            labels: { style: { colors: '#94A3B8', fontSize: '12px' } },
            axisBorder: { show: false }, axisTicks: { show: false }
        },
        yaxis: { labels: { style: { colors: '#94A3B8', fontSize: '12px' } } },
        tooltip: { theme: 'dark', x: { show: true }, style: { fontSize: '12px' } },
        legend: {
            position: 'top', horizontalAlign: 'right',
            labels: { colors: '#94A3B8' }, markers: { radius: 12 }
        }
    };

    const chart = new ApexCharts(chartContainer, options);
    chart.render();
}
