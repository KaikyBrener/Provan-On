import { apiFetch } from './api.js';

export const PerguntaService = {
    async listarPorProva(provaId) {
        const response = await apiFetch(`/Pergunta/Prova/${provaId}`);
        if (!response.ok) throw new Error('Erro ao buscar perguntas');
        return await response.json();
    },
    async buscar(id) {
        const response = await apiFetch(`/Pergunta/${id}`);
        if (!response.ok) throw new Error('Erro ao buscar pergunta');
        return await response.json();
    },
    async criar(data) {
        const response = await apiFetch('/Pergunta', { method: 'POST', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao criar pergunta');
        return await response.json();
    },
    async atualizar(id, data) {
        const response = await apiFetch(`/Pergunta/${id}`, { method: 'PUT', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao atualizar pergunta');
        return await response.json();
    },
    async remover(id) {
        const response = await apiFetch(`/Pergunta/${id}`, { method: 'DELETE' });
        return response.ok;
    }
};
