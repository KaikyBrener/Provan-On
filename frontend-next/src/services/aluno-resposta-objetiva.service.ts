import { api } from '@/lib/axios';

export interface Pontuacao {
  pontuacaoFinal?: number;
  pontuacao?: number;
  nota?: number;
}

export const alunoRespostaObjetivaService = {
  async listarPontuacoesProva(provaId: number) {
    const response = await api.get<Pontuacao[]>(`/AlunoRespostaObjetiva/ListarPontuacoesProva/${provaId}`);
    return response.data;
  },
  async pontuacaoAluno(alunoId: number | string, provaId: number | string) {
    const response = await api.get(`/AlunoRespostaObjetiva/PontuacaoAluno?alunoId=${alunoId}&provaId=${provaId}`);
    return response.data;
  },
  async cadastrar(data: any) {
    const response = await api.post('/AlunoRespostaObjetiva', data);
    return response.data;
  },
  async corrigirProva(data: { alunoId: number | string; provaId: number | string }) {
    const response = await api.post('/AlunoRespostaObjetiva/CorrigirProva', data);
    return response.data;
  }
};
