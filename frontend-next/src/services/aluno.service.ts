import { api } from '@/lib/axios';

export interface Aluno {
  id: number;
  nome: string;
  dataNascimento: string;
  dataCadastro?: string;
  dataCriacao?: string;
}

export const alunoService = {
  listar: async (): Promise<Aluno[]> => {
    const { data } = await api.get('/Aluno');
    return data;
  },
  
  buscar: async (id: number): Promise<Aluno> => {
    const { data } = await api.get(`/Aluno/${id}`);
    return data;
  },
  
  criar: async (aluno: Partial<Aluno>): Promise<Aluno> => {
    const { data } = await api.post('/Aluno', aluno);
    return data;
  },
  
  atualizar: async (id: number, aluno: Partial<Aluno>): Promise<Aluno> => {
    const { data } = await api.put(`/Aluno/${id}`, aluno);
    return data;
  },
  
  remover: async (id: number): Promise<boolean> => {
    const { status } = await api.delete(`/Aluno/${id}`);
    return status === 200 || status === 204;
  }
};
