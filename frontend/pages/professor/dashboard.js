import { AuthService } from '../../services/auth.service.js';
import { ProvaService } from '../../services/prova.service.js';
import { AlunoRespostaObjetivaService } from '../../services/aluno-resposta-objetiva.service.js';
import { loadComponent, showToast } from '../../utils/dom.js';

document.addEventListener('DOMContentLoaded', async () => {
    if (!AuthService.isAuthenticated()) {
        window.location.href = '../auth/login.html';
        return;
    }
    const user = AuthService.getUser();
    if (!user || user.role !== 'professor') {
        window.location.href = '../auth/login.html';
        return;
    }

    await loadComponent('#sidebar-container', '/components/sidebar-professor.html');
    await loadComponent('#navbar-container', '/components/navbar.html');
    setupCommonUI(user);
    await loadDashboardData(user);
});

function setupCommonUI(user) {
    const userNameDisplay = document.getElementById('userNameDisplay');
    const userInitials = document.getElementById('userInitials');
    if (user) {
        if (userNameDisplay) userNameDisplay.innerText = user.nome || 'Professor';
        if (userInitials) {
            const initials = user.nome ? user.nome.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase() : 'PR';
            userInitials.innerText = initials;
        }
    }
    const logoutHandler = () => { AuthService.logout(); };
    document.getElementById('logoutBtn')?.addEventListener('click', logoutHandler);
    document.getElementById('navLogoutBtn')?.addEventListener('click', logoutHandler);
    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle) pageTitle.innerText = 'Visão Geral';
}

async function loadDashboardData(user) {
    try {
        const provas = await ProvaService.listarPorProfessor(user.id);

        const totalProvasProfessor = document.getElementById('totalProvasProfessor');
        if (totalProvasProfessor) totalProvasProfessor.innerText = provas.length;

        // Busca pontuações de todas as provas para calcular alunos avaliados
        let totalAvaliados = 0;
        const mediasProvas = [];

        for (const prova of provas) {
            try {
                const pontuacoes = await AlunoRespostaObjetivaService.listarPontuacoesProva(prova.id);
                totalAvaliados += pontuacoes.length;
                if (pontuacoes.length > 0) {
                    const media = pontuacoes.reduce((acc, p) => acc + (p.pontuacao || p.nota || 0), 0) / pontuacoes.length;
                    mediasProvas.push({ titulo: prova.titulo, codigo: prova.codigoAcesso || prova.codigo || prova.id, media });
                }
            } catch (_) { /* prova sem pontuações */ }
        }

        const totalAlunosAvaliados = document.getElementById('totalAlunosAvaliados');
        if (totalAlunosAvaliados) totalAlunosAvaliados.innerText = totalAvaliados;

        // Tabela de provas recentes (últimas 5)
        const tbody = document.querySelector('#provasRecentesTable tbody');
        if (tbody) {
            tbody.innerHTML = '';
            const recentes = provas.slice(0, 5);
            if (recentes.length === 0) {
                tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-secondary">Nenhuma prova criada ainda.</td></tr>`;
            } else {
                recentes.forEach(p => {
                    const statusBadge = p.status === 'Ativa' || p.ativa
                        ? '<span class="badge bg-success bg-opacity-10 text-success border border-success border-opacity-25 px-2.5 py-1">Ativa</span>'
                        : '<span class="badge bg-secondary bg-opacity-10 text-secondary border border-secondary border-opacity-25 px-2.5 py-1">Fechada</span>';
                    const tr = document.createElement('tr');
                    tr.innerHTML = `
                        <td class="fw-medium text-main text-truncate" style="max-width: 180px;">${p.titulo}</td>
                        <td><code class="px-2 py-0.5 bg-dark rounded text-primary border border-opacity-50" style="font-size: 0.8rem">${p.codigoAcesso || p.codigo || '—'}</code></td>
                        <td>${statusBadge}</td>
                        <td class="text-end">
                            <a href="provas/listar.html" class="btn btn-sm btn-ghost p-1" title="Ver Detalhes">
                                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M2 12s3-7 10-7 10 7 10 7-3 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg>
                            </a>
                        </td>`;
                    tbody.appendChild(tr);
                });
            }
        }

        renderChart(mediasProvas);
    } catch (error) {
        showToast('Erro ao carregar dados do dashboard', 'error');
    }
}

function renderChart(mediasProvas) {
    const chartContainer = document.getElementById('gradesChart');
    if (!chartContainer) return;

    const categorias = mediasProvas.map(m => m.codigo);
    const dados = mediasProvas.map(m => parseFloat(m.media.toFixed(1)));

    const options = {
        series: [{ name: 'Média da Turma', data: dados.length > 0 ? dados : [0] }],
        chart: {
            type: 'bar', height: 320, toolbar: { show: false },
            fontFamily: 'Inter, system-ui, sans-serif', background: 'transparent'
        },
        theme: { mode: 'dark' },
        colors: ['#2563EB'],
        plotOptions: {
            bar: {
                borderRadius: 6, columnWidth: '40%',
                dataLabels: { position: 'top' }
            }
        },
        dataLabels: {
            enabled: true,
            formatter: (val) => val.toFixed(1),
            offsetY: -20,
            style: { fontSize: '11px', colors: ['#F8FAFC'] }
        },
        grid: {
            borderColor: 'rgba(255,255,255,0.06)',
            xaxis: { lines: { show: false } },
            yaxis: { lines: { show: true } }
        },
        xaxis: {
            categories: categorias.length > 0 ? categorias : ['Sem dados'],
            labels: { style: { colors: '#94A3B8', fontSize: '12px' } },
            axisBorder: { show: false }, axisTicks: { show: false }
        },
        yaxis: {
            max: 10,
            labels: { style: { colors: '#94A3B8', fontSize: '12px' } }
        },
        tooltip: { theme: 'dark', y: { formatter: (val) => val + ' / 10.0' } }
    };

    new ApexCharts(chartContainer, options).render();
}
