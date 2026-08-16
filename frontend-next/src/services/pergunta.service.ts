import { api } from '@/lib/axios';

export interface PerguntaDiscursiva {
  id?: number;
  titulo: string;
  descricao: string;
  idProva: number;
  pontuacao: number;
}

export const perguntaService = {
  async listarPorProva(provaId: number) {
    const response = await api.get<PerguntaDiscursiva[]>(`/Pergunta/Prova/${provaId}`);
    return response.data;
  },
  async criar(data: PerguntaDiscursiva) {
    const response = await api.post<PerguntaDiscursiva>('/Pergunta', data);
    return response.data;
  },
  async atualizar(id: number, data: Partial<PerguntaDiscursiva>) {
    const response = await api.put<PerguntaDiscursiva>(`/Pergunta/${id}`, data);
    return response.data;
  },
  async remover(id: number) {
    await api.delete(`/Pergunta/${id}`);
    return true;
  }
};
