import { api } from '@/lib/axios';

export interface Professor {
  id: number;
  nome: string;
  email: string;
  dataCadastro?: string;
  dataCriacao?: string;
  senha?: string;
}

export const professorService = {
  listar: async (): Promise<Professor[]> => {
    const { data } = await api.get('/Professor');
    return data;
  },
  
  buscar: async (id: number): Promise<Professor> => {
    const { data } = await api.get(`/Professor/${id}`);
    return data;
  },
  
  criar: async (professor: Partial<Professor>): Promise<Professor> => {
    const { data } = await api.post('/Professor', professor);
    return data;
  },
  
  atualizar: async (id: number, professor: Partial<Professor>): Promise<Professor> => {
    const { data } = await api.put(`/Professor/${id}`, professor);
    return data;
  },
  
  remover: async (id: number): Promise<boolean> => {
    const { status } = await api.delete(`/Professor/${id}`);
    return status === 200 || status === 204;
  }
};
