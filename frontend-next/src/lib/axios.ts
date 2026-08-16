import axios from 'axios';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5240';

export const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  if (typeof window !== 'undefined') {
    const token = sessionStorage.getItem('token') || sessionStorage.getItem('alunoToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      const status = error.response.status;
      if (status === 401) {
        if (typeof window !== 'undefined') {
          sessionStorage.removeItem('token');
          sessionStorage.removeItem('user');
          sessionStorage.removeItem('alunoToken');
          
          if (window.location.pathname.startsWith('/aluno')) {
            window.location.href = '/aluno';
          } else {
            window.location.href = '/login';
          }
        }
      } else if (status === 403) {
        console.error('Acesso negado (403).');
      } else if (status === 429) {
        console.error('Muitas requisições (429). Tente novamente mais tarde.');
      }
    }
    return Promise.reject(error);
  }
);
