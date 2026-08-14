/**
 * aluno-resposta-objetiva.service.js
 *
 * All endpoints are under api/AlunoRespostaObjetiva (same host as the rest of the API).
 * Uses apiFetch from api.js — token is injected automatically from memory.
 *
 * NOTE: /CorrigirProva and /PontuacaoAluno use query-string parameters,
 * NOT request body, as defined by the backend controller.
 */

import { apiFetch } from './api.js';

export const AlunoRespostaObjetivaService = {

    /**
     * POST /api/AlunoRespostaObjetiva/CadastrarAlunoRespostaObjetiva
     * Body: { alunoId, perguntaObjetivaId, alternativaSelecionada, provaId }
     */
    async cadastrar(data) {
        const response = await apiFetch('/AlunoRespostaObjetiva/CadastrarAlunoRespostaObjetiva', {
            method: 'POST',
            body: JSON.stringify(data)
        });
        if (!response.ok) throw new Error('Erro ao cadastrar resposta');
        return await response.json();
    },

    /**
     * POST /api/AlunoRespostaObjetiva/CorrigirProva?alunoId=X&provaId=Y
     * Backend signature: CorrigirProva(int alunoId, int provaId) — query params only.
     */
    async corrigirProva({ alunoId, provaId }) {
        const qs = new URLSearchParams({ alunoId, provaId }).toString();
        const response = await apiFetch(`/AlunoRespostaObjetiva/CorrigirProva?${qs}`, {
            method: 'POST'
        });
        if (!response.ok) throw new Error('Erro ao corrigir prova');
        return await response.text(); // backend returns a plain string message
    },

    /**
     * GET /api/AlunoRespostaObjetiva/PontuacaoAluno?alunoId=X&provaId=Y
     */
    async pontuacaoAluno({ alunoId, provaId }) {
        const qs = new URLSearchParams({ alunoId, provaId }).toString();
        const response = await apiFetch(`/AlunoRespostaObjetiva/PontuacaoAluno?${qs}`);
        if (!response.ok) throw new Error('Erro ao buscar pontuação');
        return await response.json();
    },

    /**
     * GET /api/AlunoRespostaObjetiva/ListarPontuacoesProva/{provaId}
     */
    async listarPontuacoesProva(provaId) {
        const response = await apiFetch(`/AlunoRespostaObjetiva/ListarPontuacoesProva/${provaId}`);
        if (!response.ok) throw new Error('Erro ao listar pontuações');
        return await response.json();
    }
};
