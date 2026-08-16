"use client";

import { useEffect, useState, Suspense } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import { useQuery } from "@tanstack/react-query";
import { alunoRespostaObjetivaService } from "@/services/aluno-resposta-objetiva.service";
import { provaService } from "@/services/prova.service";
import { Trophy, CheckCircle2, XCircle, AlertCircle, ArrowLeft } from "lucide-react";
import Link from "next/link";

function ResultadoContent() {
  const searchParams = useSearchParams();
  const router = useRouter();
  
  const [alunoId, setAlunoId] = useState<string | null>(null);
  const [provaId, setProvaId] = useState<string | null>(null);

  useEffect(() => {
    const urlAlunoId = searchParams.get("alunoId");
    const urlProvaId = searchParams.get("provaId");
    
    const sAlunoId = urlAlunoId || sessionStorage.getItem("alunoId");
    const sProvaId = urlProvaId || sessionStorage.getItem("provaId");

    if (!sAlunoId || !sProvaId) {
      router.push("/aluno");
      return;
    }

    setAlunoId(sAlunoId);
    setProvaId(sProvaId);
  }, [searchParams, router]);

  const { data: pontuacao, isLoading: loadingPontuacao } = useQuery({
    queryKey: ["pontuacao", alunoId, provaId],
    queryFn: () => alunoRespostaObjetivaService.pontuacaoAluno(alunoId!, provaId!),
    enabled: !!alunoId && !!provaId,
  });

  const { data: prova, isLoading: loadingProva } = useQuery({
    queryKey: ["prova", provaId],
    queryFn: () => provaService.buscar(Number(provaId)),
    enabled: !!provaId,
  });

  if (!alunoId || !provaId || loadingPontuacao || loadingProva) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }

  const nota = pontuacao?.notaParcial ?? 0;
  const acertos = pontuacao?.acertos ?? 0;
  const erros = pontuacao?.erros ?? 0;
  const pendentes = pontuacao?.discursivasPendentes ?? 0;

  return (
    <div className="max-w-3xl mx-auto">
      <div className="bg-surface rounded-2xl shadow-xl overflow-hidden border border-border">
        <div className="p-8 text-center bg-gradient-to-br from-blue-50 to-indigo-50 border-b border-border">
          <div className="flex justify-center mb-6">
            {nota >= 7 ? (
              <div className="w-24 h-24 bg-gradient-to-br from-emerald-400 to-emerald-600 rounded-full flex items-center justify-center text-white shadow-xl shadow-emerald-500/30">
                <Trophy size={48} />
              </div>
            ) : nota >= 5 ? (
              <div className="w-24 h-24 bg-gradient-to-br from-amber-400 to-amber-600 rounded-full flex items-center justify-center text-white shadow-xl shadow-amber-500/30">
                <CheckCircle2 size={48} />
              </div>
            ) : (
              <div className="w-24 h-24 bg-gradient-to-br from-red-400 to-red-600 rounded-full flex items-center justify-center text-white shadow-xl shadow-red-500/30">
                <XCircle size={48} />
              </div>
            )}
          </div>
          
          <h2 className="text-3xl font-bold text-text-main mb-2">
            {prova?.titulo || 'Resultado da Prova'}
          </h2>
          <p className="text-text-muted">A prova foi finalizada com sucesso!</p>
        </div>

        <div className="p-8 md:p-12">
          <div className="text-center mb-10">
            <p className="text-sm font-semibold text-text-muted uppercase tracking-wider mb-2">Sua Nota</p>
            <div className={`text-6xl font-black tracking-tighter ${
              nota >= 7 ? 'text-emerald-600' : nota >= 5 ? 'text-amber-600' : 'text-red-600'
            }`}>
              {typeof nota.toFixed === 'function' ? nota.toFixed(1) : nota}
            </div>
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div className="bg-emerald-50 border border-emerald-100 rounded-xl p-6 text-center">
              <CheckCircle2 className="mx-auto text-emerald-500 mb-2" size={28} />
              <p className="text-3xl font-bold text-emerald-700">{acertos}</p>
              <p className="text-sm font-medium text-emerald-600">Acertos</p>
            </div>
            
            <div className="bg-red-50 border border-red-100 rounded-xl p-6 text-center">
              <XCircle className="mx-auto text-red-500 mb-2" size={28} />
              <p className="text-3xl font-bold text-red-700">{erros}</p>
              <p className="text-sm font-medium text-red-600">Erros</p>
            </div>

            <div className="bg-amber-50 border border-amber-100 rounded-xl p-6 text-center">
              <AlertCircle className="mx-auto text-amber-500 mb-2" size={28} />
              <p className="text-3xl font-bold text-amber-700">{pendentes}</p>
              <p className="text-sm font-medium text-amber-600">Pendentes</p>
            </div>
          </div>

          {pendentes > 0 && (
            <div className="mt-8 p-4 bg-blue-50 border border-blue-200 text-blue-700 rounded-xl text-sm text-center">
              Você possui <strong>{pendentes}</strong> {pendentes === 1 ? 'questão discursiva aguardando correção' : 'questões discursivas aguardando correção'}. 
              Sua nota final pode aumentar.
            </div>
          )}

          <div className="mt-10 flex justify-center">
            <Link
              href="/aluno"
              onClick={() => {
                sessionStorage.removeItem("alunoId");
                sessionStorage.removeItem("provaId");
                sessionStorage.removeItem("alunoToken");
              }}
              className="flex items-center gap-2 px-6 py-3 bg-surface border border-border text-text-main rounded-xl hover:bg-surface-soft hover:border-primary/30 transition-all font-medium"
            >
              <ArrowLeft size={18} />
              <span>Sair e Voltar ao Início</span>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default function ResultadoProvaPage() {
  return (
    <Suspense fallback={<div className="flex justify-center items-center h-64"><div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div></div>}>
      <ResultadoContent />
    </Suspense>
  );
}
