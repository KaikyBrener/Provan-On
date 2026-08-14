import { apiFetch } from './api.js';

export const AlunoService = {
    async listar() {
        const response = await apiFetch('/Aluno');
        if (!response.ok) throw new Error('Erro ao listar alunos');
        return await response.json();
    },
    async buscar(id) {
        const response = await apiFetch(`/Aluno/${id}`);
        if (!response.ok) throw new Error('Erro ao buscar aluno');
        return await response.json();
    },
    async criar(data) {
        const response = await apiFetch('/Aluno', { method: 'POST', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao criar aluno');
        return await response.json();
    },
    async atualizar(id, data) {
        const response = await apiFetch(`/Aluno/${id}`, { method: 'PUT', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao atualizar aluno');
        return await response.json();
    },
    async remover(id) {
        const response = await apiFetch(`/Aluno/${id}`, { method: 'DELETE' });
        return response.ok;
    },
    async historico(id) {
        const response = await apiFetch(`/Aluno/${id}/Historico`);
        if (!response.ok) throw new Error('Erro ao buscar histórico');
        return await response.json();
    },
    async buscarProva(provaId) {
        const response = await apiFetch(`/Aluno/Prova/${provaId}`);
        if (!response.ok) throw new Error('Erro ao buscar prova do aluno');
        return await response.json();
    }
};
