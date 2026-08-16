"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useMutation } from "@tanstack/react-query";
import { authService } from "@/services/auth.service";
import { useRouter } from "next/navigation";
import { Check, Mail, Lock, AlertCircle, Eye, EyeOff } from "lucide-react";
import { useState } from "react";

const loginSchema = z.object({
  email: z.string().email("E-mail inválido").min(1, "E-mail é obrigatório"),
  senhaHash: z.string().min(1, "Senha é obrigatória"),
});

type LoginForm = z.infer<typeof loginSchema>;

export default function LoginPage() {
  const router = useRouter();
  const [showPassword, setShowPassword] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
  });

  const loginMutation = useMutation({
    mutationFn: (data: LoginForm) => authService.login(data),
    onSuccess: () => {
      // Logic for role redirect could be added here
      router.push("/dashboard");
    },
  });

  const onSubmit = (data: LoginForm) => {
    loginMutation.mutate(data);
  };

  return (
    <div className="min-h-screen flex items-stretch bg-background relative overflow-hidden">
      {/* Left Panel - Decorative */}
      <div className="hidden lg:flex flex-1 flex-col items-center justify-center p-12 relative overflow-hidden bg-gradient-to-br from-[#1E2A4A] via-[#162040] to-[#0F1830]">
        <div className="absolute w-[500px] h-[500px] bg-blue-500/15 rounded-full blur-[100px] -top-20 -right-20"></div>
        <div className="absolute w-[400px] h-[400px] bg-blue-600/10 rounded-full blur-[80px] -bottom-20 -left-20"></div>

        <div className="relative z-10 text-center max-w-md">
          <div className="w-16 h-16 bg-gradient-to-br from-blue-400 to-blue-600 rounded-2xl flex items-center justify-center text-white shadow-[0_8px_32px_rgba(59,130,246,0.4)] mx-auto mb-6">
            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M21.42 10.922a1 1 0 0 0-.019-1.838L12.83 5.18a2 2 0 0 0-1.66 0L2.6 9.08a1 1 0 0 0 0 1.832l8.57 3.908a2 2 0 0 0 1.66 0z"/><path d="M6 12v5c0 2 2 3 6 3s6-1 6-3v-5"/><path d="M21.5 12v6"/></svg>
          </div>
          <h1 className="text-4xl font-extrabold text-white tracking-tight mb-2">
            Prova<span className="text-blue-400">On</span>
          </h1>
          <p className="text-white/60 mb-12">Plataforma de Provas Online</p>

          <ul className="space-y-4 text-left">
            {[
              "Criação e gerenciamento de provas",
              "Questões objetivas e discursivas",
              "Relatórios e análise de desempenho",
              "Correção automática de objetivas",
            ].map((feature, i) => (
              <li key={i} className="flex items-center gap-3 text-white/80 text-sm">
                <Check className="text-emerald-400 shrink-0" size={20} />
                {feature}
              </li>
            ))}
          </ul>
        </div>
      </div>

      {/* Right Panel - Form */}
      <div className="flex-1 lg:flex-none lg:w-[480px] flex items-center justify-center p-8 bg-surface shadow-[-4px_0_24px_rgba(15,23,42,0.02)] z-10">
        <div className="w-full max-w-sm">
          <div className="text-center mb-8">
            <div className="w-12 h-12 bg-gradient-to-br from-blue-400 to-blue-600 rounded-xl flex items-center justify-center text-white shadow-[0_4px_16px_rgba(37,99,235,0.25)] mx-auto mb-5 lg:hidden">
              <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M21.42 10.922a1 1 0 0 0-.019-1.838L12.83 5.18a2 2 0 0 0-1.66 0L2.6 9.08a1 1 0 0 0 0 1.832l8.57 3.908a2 2 0 0 0 1.66 0z"/><path d="M6 12v5c0 2 2 3 6 3s6-1 6-3v-5"/><path d="M21.5 12v6"/></svg>
            </div>
            <h2 className="text-2xl font-bold text-text-main tracking-tight">Bem-vindo(a)</h2>
            <p className="text-sm text-text-muted mt-1">Faça login para acessar sua conta</p>
          </div>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-5" noValidate>
            {loginMutation.isError && (
              <div className="flex items-center gap-2 p-3 rounded-lg bg-red-50 border border-red-200/50 text-red-600 text-sm">
                <AlertCircle size={16} className="shrink-0" />
                <span>E-mail ou senha inválidos.</span>
              </div>
            )}

            <div>
              <label className="block text-sm font-medium text-text-main mb-1.5">E-mail</label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3.5 flex items-center pointer-events-none text-slate-400">
                  <Mail size={18} />
                </div>
                <input
                  type="email"
                  {...register("email")}
                  className={`w-full pl-10 pr-4 py-2.5 bg-background border rounded-lg text-sm transition-colors focus:outline-none focus:ring-2 focus:ring-primary/20 ${
                    errors.email ? "border-red-400 focus:border-red-500" : "border-border focus:border-primary"
                  }`}
                  placeholder="seu@email.com"
                />
              </div>
              {errors.email && <p className="mt-1 text-xs text-red-500">{errors.email.message}</p>}
            </div>

            <div>
              <div className="flex items-center justify-between mb-1.5">
                <label className="block text-sm font-medium text-text-main">Senha</label>
                <a href="#" className="text-xs text-primary hover:text-primary-hover font-medium">Esqueceu a senha?</a>
              </div>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3.5 flex items-center pointer-events-none text-slate-400">
                  <Lock size={18} />
                </div>
                <input
                  type={showPassword ? "text" : "password"}
                  {...register("senhaHash")}
                  className={`w-full pl-10 pr-10 py-2.5 bg-background border rounded-lg text-sm transition-colors focus:outline-none focus:ring-2 focus:ring-primary/20 ${
                    errors.senhaHash ? "border-red-400 focus:border-red-500" : "border-border focus:border-primary"
                  }`}
                  placeholder="Digite sua senha"
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute inset-y-0 right-0 pr-3.5 flex items-center text-slate-400 hover:text-slate-600 transition-colors"
                >
                  {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
              {errors.senhaHash && <p className="mt-1 text-xs text-red-500">{errors.senhaHash.message}</p>}
            </div>

            <div className="flex items-center gap-2 mt-2">
              <input type="checkbox" id="remember" className="rounded border-slate-300 text-primary focus:ring-primary" />
              <label htmlFor="remember" className="text-sm text-text-muted cursor-pointer">Lembrar-me</label>
            </div>

            <button
              type="submit"
              disabled={loginMutation.isPending}
              className="w-full py-2.5 px-4 bg-primary hover:bg-primary-hover text-white rounded-lg text-sm font-semibold transition-colors disabled:opacity-70 flex justify-center items-center mt-2"
            >
              {loginMutation.isPending ? (
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
              ) : (
                "Entrar"
              )}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
