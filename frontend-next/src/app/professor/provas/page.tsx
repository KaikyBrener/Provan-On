"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { authService } from "@/services/auth.service";
import { provaService } from "@/services/prova.service";
import { PageHeader } from "@/components/ui/PageHeader";
import { EmptyState } from "@/components/ui/EmptyState";
import { LoadingState } from "@/components/ui/LoadingState";
import { ConfirmDialog } from "@/components/ui/ConfirmDialog";
import { PlusCircle, Search, Trash2, Edit, Copy, Link as LinkIcon } from "lucide-react";
import Link from "next/link";
import { useState } from "react";

export default function ProvasListPage() {
  const user = authService.getUser();
  const queryClient = useQueryClient();
  const [deleteId, setDeleteId] = useState<number | null>(null);
  const [search, setSearch] = useState("");

  const { data: provas, isLoading } = useQuery({
    queryKey: ["provas", user?.id],
    queryFn: () => provaService.listarPorProfessor(user!.id),
    enabled: !!user?.id,
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => provaService.remover(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["provas"] });
      setDeleteId(null);
    },
  });

  const filteredProvas = provas?.filter(p => p.titulo.toLowerCase().includes(search.toLowerCase()) || p.codigoAcesso?.toLowerCase().includes(search.toLowerCase()));

  const handleCopyLink = (codigo: string) => {
    const link = `${window.location.origin}/aluno/codigo-acesso?codigo=${codigo}`;
    navigator.clipboard.writeText(link);
    alert("Link copiado para a área de transferência!");
  };

  if (isLoading) return <LoadingState message="Carregando suas provas..." />;

  return (
    <>
      <PageHeader 
        title="Minhas Provas" 
        description="Gerencie, visualize e crie novas avaliações."
        action={
          <Link href="/professor/provas/cadastrar" className="flex items-center gap-2 px-4 py-2 bg-primary hover:bg-primary-hover text-white rounded-lg text-sm font-semibold transition-colors shadow-sm">
            <PlusCircle size={18} />
            Nova Prova
          </Link>
        }
      />

      <div className="bg-surface border border-border rounded-xl shadow-sm overflow-hidden">
        <div className="p-4 border-b border-border flex flex-col sm:flex-row gap-4 justify-between items-center bg-slate-50/50">
          <div className="relative w-full sm:max-w-md">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" size={18} />
            <input 
              type="text"
              placeholder="Buscar provas..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-border rounded-lg text-sm focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary"
            />
          </div>
        </div>

        {(!filteredProvas || filteredProvas.length === 0) ? (
          <EmptyState 
            title={search ? "Nenhuma prova encontrada" : "Você ainda não possui provas"}
            description={search ? "Tente buscar com outros termos." : "Crie sua primeira avaliação para começar a usar a plataforma."}
            action={!search && (
              <Link href="/professor/provas/cadastrar" className="flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-lg text-sm font-semibold hover:bg-primary-hover">
                <PlusCircle size={18} />
                Criar Primeira Prova
              </Link>
            )}
          />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-left text-sm whitespace-nowrap">
              <thead className="bg-slate-50 border-b border-border text-slate-500 uppercase text-[11px] font-bold tracking-wider">
                <tr>
                  <th className="px-6 py-4">Título</th>
                  <th className="px-6 py-4">Código de Acesso</th>
                  <th className="px-6 py-4">Status</th>
                  <th className="px-6 py-4 text-right">Ações</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-border">
                {filteredProvas.map((prova) => (
                  <tr key={prova.id} className="hover:bg-slate-50/50 transition-colors">
                    <td className="px-6 py-4 font-medium text-text-main">{prova.titulo}</td>
                    <td className="px-6 py-4">
                      {prova.codigoAcesso ? (
                        <div className="flex items-center gap-2">
                          <span className="font-mono bg-slate-100 text-slate-700 px-2 py-1 rounded text-xs border border-slate-200">
                            {prova.codigoAcesso}
                          </span>
                          <button onClick={() => handleCopyLink(prova.codigoAcesso!)} className="text-slate-400 hover:text-primary transition-colors" title="Copiar link de acesso">
                            <Copy size={14} />
                          </button>
                        </div>
                      ) : (
                        <span className="text-slate-400 italic text-xs">Sem código</span>
                      )}
                    </td>
                    <td className="px-6 py-4">
                      <span className={`px-2.5 py-1 text-[11px] font-bold rounded-full border ${prova.ativa !== false ? 'bg-green-50 text-green-700 border-green-200' : 'bg-slate-100 text-slate-600 border-slate-200'}`}>
                        {prova.ativa !== false ? 'Ativa' : 'Inativa'}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Link href={`/professor/provas/${prova.id}`} className="p-2 text-slate-400 hover:text-primary hover:bg-primary-soft rounded-lg transition-colors" title="Editar">
                          <Edit size={16} />
                        </Link>
                        <button 
                          onClick={() => setDeleteId(prova.id)}
                          className="p-2 text-slate-400 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                          title="Excluir"
                        >
                          <Trash2 size={16} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <ConfirmDialog 
        isOpen={deleteId !== null}
        title="Excluir Prova"
        message="Tem certeza que deseja excluir esta prova? Esta ação não poderá ser desfeita e os alunos perderão o acesso a ela."
        isDestructive={true}
        confirmText={deleteMutation.isPending ? "Excluindo..." : "Sim, Excluir"}
        onConfirm={() => deleteId && deleteMutation.mutate(deleteId)}
        onCancel={() => setDeleteId(null)}
      />
    </>
  );
}
