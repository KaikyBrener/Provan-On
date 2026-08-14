import { apiFetch } from './api.js';

export const ProfessorService = {
    async listar() {
        const response = await apiFetch('/Professor');
        if (!response.ok) throw new Error('Erro ao listar professores');
        return await response.json();
    },
    async buscar(id) {
        const response = await apiFetch(`/Professor/${id}`);
        if (!response.ok) throw new Error('Erro ao buscar professor');
        return await response.json();
    },
    async criar(data) {
        const response = await apiFetch('/Professor', { method: 'POST', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao criar professor');
        return await response.json();
    },
    async atualizar(id, data) {
        const response = await apiFetch(`/Professor/${id}`, { method: 'PUT', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao atualizar professor');
        return await response.json();
    },
    async remover(id) {
        const response = await apiFetch(`/Professor/${id}`, { method: 'DELETE' });
        return response.ok;
    }
};
