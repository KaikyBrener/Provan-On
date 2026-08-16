import { api } from '@/lib/axios';

export interface LoginRequest {
  email: string;
  senhaHash: string;
}

export interface LoginResponse {
  token: string;
  user: {
    id: number;
    nome: string;
    email: string;
    role: string;
  };
}

export const authService = {
  async login(data: LoginRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>('/auth/login', data);
    
    if (response.data.token) {
      if (typeof window !== 'undefined') {
        sessionStorage.setItem('token', response.data.token);
        sessionStorage.setItem('user', JSON.stringify(response.data.user));
      }
    }
    
    return response.data;
  },

  logout() {
    if (typeof window !== 'undefined') {
      sessionStorage.removeItem('token');
      sessionStorage.removeItem('user');
      window.location.href = '/login';
    }
  },

  getUser() {
    if (typeof window !== 'undefined') {
      const userStr = sessionStorage.getItem('user');
      if (userStr) {
        try {
          return JSON.parse(userStr) as LoginResponse['user'];
        } catch {
          return null;
        }
      }
    }
    return null;
  },

  isAuthenticated() {
    if (typeof window !== 'undefined') {
      return !!sessionStorage.getItem('token');
    }
    return false;
  }
};
