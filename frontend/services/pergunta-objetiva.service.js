import { apiFetch } from './api.js';

export const PerguntaObjetivaService = {
    async listarPorProva(provaId) {
        const response = await apiFetch(`/PerguntaObjetiva/Prova/${provaId}`);
        if (!response.ok) throw new Error('Erro ao buscar perguntas objetivas');
        return await response.json();
    },
    async buscar(id) {
        const response = await apiFetch(`/PerguntaObjetiva/${id}`);
        if (!response.ok) throw new Error('Erro ao buscar pergunta objetiva');
        return await response.json();
    },
    async criar(data) {
        const response = await apiFetch('/PerguntaObjetiva', { method: 'POST', body: JSON.stringify(data) });
        if (!response.ok) throw new Error('Erro ao criar pergunta objetiva');
        return await response.json();
    },
    async remover(id) {
        const response = await apiFetch(`/PerguntaObjetiva/${id}`, { method: 'DELETE' });
        return response.ok;
    }
};
