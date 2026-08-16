import { api } from '@/lib/axios';

export interface Prova {
  id: number;
  titulo: string;
  codigoAcesso?: string;
  codigo?: string;
  idProfessor: number;
  dataHoraInicio?: string;
  dataHoraFim?: string;
  status?: string;
  ativa?: boolean;
}

export const provaService = {
  async listar() {
    const response = await api.get<Prova[]>('/Prova');
    return response.data;
  },
  async buscar(id: number) {
    const response = await api.get<Prova>(`/Prova/${id}`);
    return response.data;
  },
  async criar(data: Omit<Prova, 'id'>) {
    const response = await api.post<Prova>('/Prova', data);
    return response.data;
  },
  async atualizar(id: number, data: Partial<Prova>) {
    const response = await api.put<Prova>(`/Prova/${id}`, data);
    return response.data;
  },
  async remover(id: number) {
    await api.delete(`/Prova/${id}`);
    return true;
  },
  async listarPorProfessor(professorId: number) {
    const response = await api.get<Prova[]>(`/Prova/Professor/${professorId}`);
    return response.data;
  },
  async verificarCodigo(codigo: string) {
    try {
      const response = await api.get<Prova>(`/Prova/Codigo/${codigo}`);
      return response.data;
    } catch (e: any) {
      if (e.response?.status === 404) return null;
      throw e;
    }
  },
  async entrar(data: { nome: string; dataNascimento: string; codigoProva: string }) {
    const response = await api.post('/Prova/Entrar', data);
    return response.data;
  }
};
