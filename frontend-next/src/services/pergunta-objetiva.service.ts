import { api } from '@/lib/axios';

export interface PerguntaObjetiva {
  id?: number;
  titulo: string;
  descricao: string;
  opcaoA: string;
  opcaoB: string;
  opcaoC: string;
  opcaoD: string;
  opcaoCorreta: string;
  idProva: number;
  pontuacao: number;
}

export const perguntaObjetivaService = {
  async listarPorProva(provaId: number) {
    const response = await api.get<PerguntaObjetiva[]>(`/PerguntaObjetiva/Prova/${provaId}`);
    return response.data;
  },
  async criar(data: PerguntaObjetiva) {
    const response = await api.post<PerguntaObjetiva>('/PerguntaObjetiva', data);
    return response.data;
  },
  async remover(id: number) {
    await api.delete(`/PerguntaObjetiva/${id}`);
    return true;
  }
};
