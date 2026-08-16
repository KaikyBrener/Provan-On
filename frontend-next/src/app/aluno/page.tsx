"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useMutation } from "@tanstack/react-query";
import { provaService } from "@/services/prova.service";
import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import { GraduationCap, ArrowRight } from "lucide-react";

const acessoSchema = z.object({
  nome: z.string().min(3, "Nome deve ter pelo menos 3 caracteres"),
  dataNascimento: z.string().min(1, "Data de nascimento é obrigatória"),
  codigoProva: z.string().min(1, "Código da prova é obrigatório").toUpperCase(),
});

type AcessoFormValues = z.infer<typeof acessoSchema>;

export default function AcessoProvaPage() {
  const router = useRouter();
  const [errorMsg, setErrorMsg] = useState("");

  useEffect(() => {
    // Limpar sessão anterior
    sessionStorage.removeItem("alunoId");
    sessionStorage.removeItem("provaId");
    sessionStorage.removeItem("alunoToken");
  }, []);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<AcessoFormValues>({
    resolver: zodResolver(acessoSchema),
  });

  const mutation = useMutation({
    mutationFn: (data: AcessoFormValues) => provaService.entrar(data),
    onSuccess: (data) => {
      sessionStorage.setItem("alunoId", data.alunoId.toString());
      sessionStorage.setItem("provaId", data.provaId.toString());
      if (data.token) sessionStorage.setItem("alunoToken", data.token);
      
      router.push(`/aluno/prova/${data.provaId}`);
    },
    onError: (error: any) => {
      setErrorMsg(error.response?.data?.message || "Código inválido ou prova não encontrada.");
    },
  });

  const onSubmit = (data: AcessoFormValues) => {
    setErrorMsg("");
    mutation.mutate(data);
  };

  return (
    <div className="max-w-md mx-auto w-full">
      <div className="bg-surface rounded-2xl shadow-xl overflow-hidden border border-border">
        <div className="p-8 text-center bg-gradient-to-br from-blue-50 to-indigo-50 border-b border-border">
          <div className="w-16 h-16 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl flex items-center justify-center text-white shadow-lg mx-auto mb-4">
            <GraduationCap size={32} />
          </div>
          <h2 className="text-2xl font-bold text-text-main mb-2">Acesso à Prova</h2>
          <p className="text-text-muted text-sm">Insira seus dados e o código fornecido pelo professor para iniciar a prova.</p>
        </div>

        <div className="p-8">
          {errorMsg && (
            <div className="mb-6 p-4 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm text-center">
              {errorMsg}
            </div>
          )}

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
            <div className="space-y-1.5">
              <label className="text-sm font-medium text-text-main">Nome Completo</label>
              <input
                type="text"
                {...register("nome")}
                className={`w-full px-4 py-3 rounded-xl border ${
                  errors.nome ? "border-red-300 focus:border-red-500 focus:ring-red-500/20" : "border-border focus:border-blue-500 focus:ring-blue-500/20"
                } bg-surface outline-none transition-all`}
                placeholder="Seu nome completo"
              />
              {errors.nome && <p className="text-sm text-red-500">{errors.nome.message}</p>}
            </div>

            <div className="space-y-1.5">
              <label className="text-sm font-medium text-text-main">Data de Nascimento</label>
              <input
                type="date"
                {...register("dataNascimento")}
                className={`w-full px-4 py-3 rounded-xl border ${
                  errors.dataNascimento ? "border-red-300 focus:border-red-500 focus:ring-red-500/20" : "border-border focus:border-blue-500 focus:ring-blue-500/20"
                } bg-surface outline-none transition-all`}
              />
              {errors.dataNascimento && <p className="text-sm text-red-500">{errors.dataNascimento.message}</p>}
            </div>

            <div className="space-y-1.5">
              <label className="text-sm font-medium text-text-main">Código da Prova</label>
              <input
                type="text"
                {...register("codigoProva")}
                className={`w-full px-4 py-3 rounded-xl border uppercase font-mono tracking-widest ${
                  errors.codigoProva ? "border-red-300 focus:border-red-500 focus:ring-red-500/20" : "border-border focus:border-blue-500 focus:ring-blue-500/20"
                } bg-surface outline-none transition-all`}
                placeholder="Ex: PROVA123"
              />
              {errors.codigoProva && <p className="text-sm text-red-500">{errors.codigoProva.message}</p>}
            </div>

            <button
              type="submit"
              disabled={mutation.isPending}
              className="w-full flex items-center justify-center gap-2 py-3.5 bg-gradient-to-r from-blue-600 to-indigo-600 text-white font-medium rounded-xl hover:from-blue-700 hover:to-indigo-700 transition-all shadow-md shadow-blue-500/25 disabled:opacity-70 mt-4"
            >
              {mutation.isPending ? (
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
              ) : (
                <>
                  <span>Acessar Prova</span>
                  <ArrowRight size={18} />
                </>
              )}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
