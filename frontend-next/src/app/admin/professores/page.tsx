"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { professorService, Professor } from "@/services/professor.service";
import Link from "next/link";
import { Plus, Edit2, Trash2 } from "lucide-react";
import { PageHeader } from "@/components/ui/PageHeader";
import { useState } from "react";
import { ConfirmDialog } from "@/components/ui/ConfirmDialog";

export default function AdminProfessoresPage() {
  const queryClient = useQueryClient();
  const [professorToDelete, setProfessorToDelete] = useState<Professor | null>(null);

  const { data: professores, isLoading } = useQuery({
    queryKey: ["professores"],
    queryFn: professorService.listar,
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => professorService.remover(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["professores"] });
      setProfessorToDelete(null);
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
        <PageHeader title="Professores" description="Gerencie os professores do sistema" />
        <Link
          href="/admin/professores/cadastrar"
          className="flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary-hover transition-colors shadow-sm"
        >
          <Plus size={18} />
          <span>Novo Professor</span>
        </Link>
      </div>

      <div className="bg-surface border border-border rounded-xl shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-surface-soft border-b border-border">
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider">Nome</th>
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider">Email</th>
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider">Data Cadastro</th>
                <th className="px-6 py-4 text-xs font-semibold text-text-muted uppercase tracking-wider text-right">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border">
              {professores?.length === 0 ? (
                <tr>
                  <td colSpan={4} className="px-6 py-8 text-center text-text-muted">
                    Nenhum professor encontrado.
                  </td>
                </tr>
              ) : (
                professores?.map((prof) => (
                  <tr key={prof.id} className="hover:bg-surface-soft/50 transition-colors">
                    <td className="px-6 py-4 font-medium text-text-main">{prof.nome}</td>
                    <td className="px-6 py-4 text-text-muted">{prof.email}</td>
                    <td className="px-6 py-4 text-text-muted">
                      {new Date(prof.dataCadastro || prof.dataCriacao || "").toLocaleDateString("pt-BR")}
                    </td>
                    <td className="px-6 py-4 text-right">
                      <div className="flex justify-end gap-2">
                        <Link
                          href={`/admin/professores/editar/${prof.id}`}
                          className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          title="Editar"
                        >
                          <Edit2 size={18} />
                        </Link>
                        <button
                          onClick={() => setProfessorToDelete(prof)}
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
        isOpen={!!professorToDelete}
        title="Remover Professor"
        message={`Tem certeza que deseja remover o professor "${professorToDelete?.nome}"? Esta ação não pode ser desfeita.`}
        confirmText={deleteMutation.isPending ? "Removendo..." : "Remover"}
        cancelText="Cancelar"
        isDestructive={true}
        onConfirm={() => professorToDelete && deleteMutation.mutate(professorToDelete.id)}
        onCancel={() => setProfessorToDelete(null)}
      />
    </div>
  );
}
