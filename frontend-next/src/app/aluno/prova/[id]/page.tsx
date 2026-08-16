"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { provaService } from "@/services/prova.service";
import { perguntaObjetivaService } from "@/services/pergunta-objetiva.service";
import { perguntaService } from "@/services/pergunta.service";
import { alunoRespostaObjetivaService } from "@/services/aluno-resposta-objetiva.service";
import { alunoRespostaService } from "@/services/aluno-resposta.service";
import { useQuery } from "@tanstack/react-query";
import { ConfirmDialog } from "@/components/ui/ConfirmDialog";
import { Clock } from "lucide-react";

export default function RealizacaoProvaPage() {
  const router = useRouter();
  const params = useParams();
  const id = Number(params.id);

  const [alunoId, setAlunoId] = useState<string | null>(null);
  const [respostasObjetivas, setRespostasObjetivas] = useState<Record<number, number>>({});
  const [respostasDiscursivas, setRespostasDiscursivas] = useState<Record<number, string>>({});
  const [showConfirm, setShowConfirm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [elapsedSeconds, setElapsedSeconds] = useState(0);

  useEffect(() => {
    const aid = sessionStorage.getItem("alunoId");
    const pid = sessionStorage.getItem("provaId");
    
    if (!aid || !pid || Number(pid) !== id) {
      router.push("/aluno");
      return;
    }
    setAlunoId(aid);

    const timer = setInterval(() => {
      setElapsedSeconds((prev) => prev + 1);
    }, 1000);

    return () => clearInterval(timer);
  }, [id, router]);

  const { data: prova, isLoading: loadingProva } = useQuery({
    queryKey: ["prova", id],
    queryFn: () => provaService.buscar(id),
    enabled: !!id,
  });

  const { data: questoesObjetivas, isLoading: loadingObjetivas } = useQuery({
    queryKey: ["questoes-objetivas", id],
    queryFn: () => perguntaObjetivaService.listarPorProva(id),
    enabled: !!id,
  });

  const { data: questoesDiscursivas, isLoading: loadingDiscursivas } = useQuery({
    queryKey: ["questoes-discursivas", id],
    queryFn: () => perguntaService.listarPorProva(id),
    enabled: !!id,
  });

  const isLoading = loadingProva || loadingObjetivas || loadingDiscursivas;

  const handleObjetivaChange = (perguntaId: number, alternativa: number) => {
    setRespostasObjetivas(prev => ({ ...prev, [perguntaId]: alternativa }));
  };

  const handleDiscursivaChange = (perguntaId: number, texto: string) => {
    setRespostasDiscursivas(prev => ({ ...prev, [perguntaId]: texto }));
  };

  const formatTime = (totalSeconds: number) => {
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;
    return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
  };

  const checkUnanswered = () => {
    const totalObjetivas = questoesObjetivas?.length || 0;
    const totalDiscursivas = questoesDiscursivas?.length || 0;
    const answeredObjetivas = Object.keys(respostasObjetivas).length;
    const answeredDiscursivas = Object.values(respostasDiscursivas).filter(t => t.trim().length > 0).length;
    return (answeredObjetivas + answeredDiscursivas) < (totalObjetivas + totalDiscursivas);
  };

  const submitProva = async () => {
    if (!alunoId) return;
    setIsSubmitting(true);
    try {
      // 1. Submit Objetivas
      for (const [perguntaId, alternativa] of Object.entries(respostasObjetivas)) {
        await alunoRespostaObjetivaService.cadastrar({
          alunoId: Number(alunoId),
          perguntaObjetivaId: Number(perguntaId),
          alternativaSelecionada: alternativa,
          provaId: id
        });
      }

      // 2. Submit Discursivas
      for (const [perguntaId, resposta] of Object.entries(respostasDiscursivas)) {
        if (resposta.trim()) {
          await alunoRespostaService.cadastrar({
            idAluno: Number(alunoId),
            idPergunta: Number(perguntaId),
            resposta: resposta.trim()
          });
        }
      }

      // 3. Corrigir (triggers the finalization/correction endpoint)
      await alunoRespostaObjetivaService.corrigirProva({ alunoId, provaId: id });

      router.push(`/aluno/resultado?provaId=${id}&alunoId=${alunoId}`);
    } catch (error) {
      console.error("Erro ao enviar prova:", error);
      alert("Erro ao enviar a prova. Tente novamente.");
      setIsSubmitting(false);
      setShowConfirm(false);
    }
  };

  if (isLoading || !alunoId) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }

  const hasUnanswered = checkUnanswered();

  return (
    <div className="space-y-6">
      <div className="bg-surface rounded-2xl shadow-sm border border-border p-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4 sticky top-4 z-10">
        <div>
          <h1 className="text-xl font-bold text-text-main">{prova?.titulo || 'Prova'}</h1>
          <p className="text-sm text-text-muted">
            {questoesObjetivas?.length || 0} questões objetivas • {questoesDiscursivas?.length || 0} questões discursivas
          </p>
        </div>
        <div className="flex items-center gap-2 bg-blue-50 text-blue-700 px-4 py-2 rounded-xl font-mono font-medium">
          <Clock size={18} />
          <span>{formatTime(elapsedSeconds)}</span>
        </div>
      </div>

      <div className="space-y-6">
        {questoesObjetivas?.map((q, index) => (
          <div key={`obj-${q.id}`} className="bg-surface rounded-2xl shadow-sm border border-border p-6 md:p-8">
            <div className="flex gap-4 mb-6">
              <div className="w-8 h-8 rounded-full bg-primary/10 text-primary flex items-center justify-center font-bold shrink-0">
                {index + 1}
              </div>
              <h3 className="text-base md:text-lg font-medium text-text-main pt-1">
                {q.descricao}
              </h3>
            </div>
            
            <div className="space-y-3 pl-12">
              {[q.opcaoA, q.opcaoB, q.opcaoC, q.opcaoD].map((alt: string, altIndex: number) => (
                <label 
                  key={altIndex} 
                  className={`flex items-center gap-3 p-4 rounded-xl border cursor-pointer transition-all ${
                    respostasObjetivas[q.id!] === altIndex 
                      ? 'border-primary bg-primary/5' 
                      : 'border-border hover:border-primary/50 hover:bg-surface-soft'
                  }`}
                >
                  <input 
                    type="radio" 
                    name={`questao_${q.id}`} 
                    value={altIndex}
                    checked={respostasObjetivas[q.id!] === altIndex}
                    onChange={() => handleObjetivaChange(q.id!, altIndex)}
                    className="w-4 h-4 text-primary border-slate-300 focus:ring-primary"
                  />
                  <span className="font-medium text-text-main">{String.fromCharCode(65 + altIndex)}.</span>
                  <span className="text-text-muted">{alt}</span>
                </label>
              ))}
            </div>
          </div>
        ))}

        {questoesDiscursivas?.map((q, index) => (
          <div key={`disc-${q.id}`} className="bg-surface rounded-2xl shadow-sm border border-border p-6 md:p-8">
            <div className="flex gap-4 mb-6">
              <div className="w-8 h-8 rounded-full bg-emerald-500/10 text-emerald-600 flex items-center justify-center font-bold shrink-0">
                {(questoesObjetivas?.length || 0) + index + 1}
              </div>
              <h3 className="text-base md:text-lg font-medium text-text-main pt-1">
                {q.descricao}
              </h3>
            </div>
            
            <div className="pl-12">
              <textarea
                rows={5}
                className="w-full px-4 py-3 rounded-xl border border-border focus:border-emerald-500 focus:ring-emerald-500/20 bg-surface outline-none transition-all resize-y"
                placeholder="Digite sua resposta aqui..."
                value={respostasDiscursivas[q.id!] || ''}
                onChange={(e) => handleDiscursivaChange(q.id!, e.target.value)}
              />
            </div>
          </div>
        ))}

        {(!questoesObjetivas?.length && !questoesDiscursivas?.length) && (
          <div className="bg-amber-50 border border-amber-200 text-amber-700 p-6 rounded-2xl text-center">
            Esta prova não possui questões cadastradas.
          </div>
        )}
      </div>

      <div className="flex justify-end pt-4 pb-8">
        <button
          onClick={() => setShowConfirm(true)}
          className="px-8 py-3.5 bg-gradient-to-r from-blue-600 to-indigo-600 text-white font-medium rounded-xl hover:from-blue-700 hover:to-indigo-700 transition-all shadow-md shadow-blue-500/25"
        >
          Finalizar Prova
        </button>
      </div>

      <ConfirmDialog
        isOpen={showConfirm}
        title="Finalizar Prova"
        message={hasUnanswered 
          ? "Atenção: Você ainda possui questões sem resposta. Tem certeza que deseja finalizar e enviar a prova?" 
          : "Tem certeza que deseja finalizar a prova? Após o envio não será possível alterar suas respostas."}
        confirmText={isSubmitting ? "Enviando..." : "Entregar Definitivamente"}
        cancelText="Revisar Prova"
        isDestructive={hasUnanswered}
        onConfirm={submitProva}
        onCancel={() => setShowConfirm(false)}
      />
    </div>
  );
}
