import { apiFetch } from './api.js';

export const ProvaService = {
    async listar() {
        const response = await apiFetch('/Prova');
        if (!response.ok) throw new Error('Erro ao listar provas');
        return await response.json();
    },
    async buscar(id) {
        const response = await apiFetch(`/Prova/${id}`);
        if (!response.ok) throw new Error('Erro ao buscar prova');
        return await response.json();
    },
    async criar(data) {
        const response = await apiFetch('/Prova', { method: 'POST', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao criar prova');
        return await response.json();
    },
    async atualizar(id, data) {
        const response = await apiFetch(`/Prova/${id}`, { method: 'PUT', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao atualizar prova');
        return await response.json();
    },
    async remover(id) {
        const response = await apiFetch(`/Prova/${id}`, { method: 'DELETE' });
        return response.ok;
    },
    async listarPorProfessor(professorId) {
        const response = await apiFetch(`/Prova/Professor/${professorId}`);
        if (!response.ok) throw new Error('Erro ao buscar provas do professor');
        return await response.json();
    },
    async verificarCodigo(codigo) {
        const response = await apiFetch(`/Prova/Codigo/${codigo}`);
        if (response.status === 404) return null;
        if (!response.ok) throw new Error('Erro ao verificar código');
        return await response.json();
    }
};
