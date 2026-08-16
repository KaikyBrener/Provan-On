import { api } from '@/lib/axios';

export const alunoRespostaService = {
  async cadastrar(data: any) {
    const response = await api.post('/AlunoResposta', data);
    return response.data;
  }
};
