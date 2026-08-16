"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useMutation, useQuery } from "@tanstack/react-query";
import { alunoService } from "@/services/aluno.service";
import { provaService } from "@/services/prova.service";
import { useRouter } from "next/navigation";
import { PageHeader } from "@/components/ui/PageHeader";
import Link from "next/link";
import { ArrowLeft, Save } from "lucide-react";
import { useState } from "react";

const alunoSchema = z.object({
  nome: z.string().min(3, "O nome deve ter no mínimo 3 caracteres"),
  dataNascimento: z.string().min(1, "Data de nascimento é obrigatória"),
  idProva: z.number().min(1, "Selecione uma prova"),
});

type AlunoFormValues = z.infer<typeof alunoSchema>;

export default function CadastrarAlunoPage() {
  const router = useRouter();
  const [errorMsg, setErrorMsg] = useState("");

  const { data: provas } = useQuery({
    queryKey: ["provas"],
    queryFn: provaService.listar,
  });

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<AlunoFormValues>({
    resolver: zodResolver(alunoSchema),
  });

  const mutation = useMutation({
    mutationFn: (data: AlunoFormValues) => alunoService.criar(data),
    onSuccess: () => {
      router.push("/admin/alunos");
    },
    onError: (error: any) => {
      setErrorMsg(error.response?.data?.message || "Erro ao cadastrar aluno.");
    },
  });

  const onSubmit = (data: AlunoFormValues) => {
    setErrorMsg("");
    mutation.mutate(data);
  };

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <Link
          href="/admin/alunos"
          className="p-2 rounded-lg hover:bg-surface-soft text-text-muted transition-colors"
        >
          <ArrowLeft size={20} />
        </Link>
        <PageHeader title="Novo Aluno" description="Cadastre um novo aluno no sistema" />
      </div>

      <div className="bg-surface border border-border rounded-xl shadow-sm p-6">
        {errorMsg && (
          <div className="mb-6 p-4 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
            {errorMsg}
          </div>
        )}

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <div className="space-y-1.5">
            <label className="text-sm font-medium text-text-main">Nome Completo</label>
            <input
              type="text"
              {...register("nome")}
              className={`w-full px-4 py-2.5 rounded-lg border ${
                errors.nome ? "border-red-300 focus:border-red-500 focus:ring-red-500/20" : "border-border focus:border-primary focus:ring-primary/20"
              } bg-surface outline-none transition-all`}
              placeholder="Nome completo"
            />
            {errors.nome && <p className="text-sm text-red-500">{errors.nome.message}</p>}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
            <div className="space-y-1.5">
              <label className="text-sm font-medium text-text-main">Data de Nascimento</label>
              <input
                type="date"
                {...register("dataNascimento")}
                className={`w-full px-4 py-2.5 rounded-lg border ${
                  errors.dataNascimento ? "border-red-300 focus:border-red-500 focus:ring-red-500/20" : "border-border focus:border-primary focus:ring-primary/20"
                } bg-surface outline-none transition-all`}
              />
              {errors.dataNascimento && <p className="text-sm text-red-500">{errors.dataNascimento.message}</p>}
            </div>

            <div className="space-y-1.5">
              <label className="text-sm font-medium text-text-main">Prova Vinculada</label>
              <select
                {...register("idProva", { valueAsNumber: true })}
                className={`w-full px-4 py-2.5 rounded-lg border ${
                  errors.idProva ? "border-red-300 focus:border-red-500 focus:ring-red-500/20" : "border-border focus:border-primary focus:ring-primary/20"
                } bg-surface outline-none transition-all`}
              >
                <option value="">Selecione uma prova...</option>
                {provas?.map(p => (
                  <option key={p.id} value={p.id}>{p.titulo || p.id}</option>
                ))}
              </select>
              {errors.idProva && <p className="text-sm text-red-500">{errors.idProva.message}</p>}
            </div>
          </div>

          <div className="pt-4 flex justify-end">
            <button
              type="submit"
              disabled={mutation.isPending}
              className="flex items-center gap-2 px-6 py-2.5 bg-primary text-white rounded-lg hover:bg-primary-hover transition-colors shadow-sm disabled:opacity-70"
            >
              {mutation.isPending ? (
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
              ) : (
                <Save size={18} />
              )}
              <span>{mutation.isPending ? "Salvando..." : "Salvar"}</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
