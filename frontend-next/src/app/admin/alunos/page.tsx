"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { alunoService, Aluno } from "@/services/aluno.service";
import Link from "next/link";
import { Plus, Edit2, Trash2 } from "lucide-react";
import { PageHeader } from "@/components/ui/PageHeader";
import { useState } from "react";
import { ConfirmDialog } from "@/components/ui/ConfirmDialog";

export default function AdminAlunosPage() {
  const queryClient = useQueryClient();
  const [alunoToDelete, setAlunoToDelete] = useState<Aluno | null>(null);

  const { data: alunos, isLoading } = useQuery({
    queryKey: ["alunos"],
    queryFn: alunoService.listar,
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => alunoService.remover(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["alunos"] });
      setAlunoToDelete(null);
    },
  });

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      <div className="flex justify-between items-center">
        <PageHeader title="Alunos" description="Gerencie os alunos cadastrados no sistema" />
        <Link
          href="/admin/alunos/cadastrar"
          className="flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary-hover transition-colors shadow-sm"
        >
          <Plus size={18} />
          <span>Novo Aluno</span>
        </Link>
      </div>

      <div className="bg-surface border border-border rounded-xl shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-surface-soft border-b border-border">
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider">Nome</th>
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider">Data Nascimento</th>
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider">Data Cadastro</th>
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider text-right">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border">
              {alunos?.length === 0 ? (
                <tr>
                  <td colSpan={4} className="px-6 py-8 text-center text-text-muted">
                    Nenhum aluno encontrado.
                  </td>
                </tr>
              ) : (
                alunos?.map((aluno) => (
                  <tr key={aluno.id} className="hover:bg-surface-soft/50 transition-colors">
                    <td className="px-6 py-4 font-medium text-text-main">{aluno.nome}</td>
                    <td className="px-6 py-4 text-text-muted">
                      {new Date(aluno.dataNascimento + 'T12:00:00').toLocaleDateString("pt-BR")}
                    </td>
                    <td className="px-6 py-4 text-text-muted">
                      {new Date(aluno.dataCadastro || aluno.dataCriacao || "").toLocaleDateString("pt-BR")}
                    </td>
                    <td className="px-6 py-4 text-right">
                      <div className="flex justify-end gap-2">
                        <Link
                          href={`/admin/alunos/editar/${aluno.id}`}
                          className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          title="Editar"
                        >
                          <Edit2 size={18} />
                        </Link>
                        <button
                          onClick={() => setAlunoToDelete(aluno)}
                          className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                          title="Remover"
                        >
                          <Trash2 size={18} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      <ConfirmDialog
        isOpen={!!alunoToDelete}
        title="Remover Aluno"
        message={`Tem certeza que deseja remover o aluno "${alunoToDelete?.nome}"?`}
        confirmText={deleteMutation.isPending ? "Removendo..." : "Remover"}
        cancelText="Cancelar"
        isDestructive={true}
        onConfirm={() => alunoToDelete && deleteMutation.mutate(alunoToDelete.id)}
        onCancel={() => setAlunoToDelete(null)}
      />
    </div>
  );
}
