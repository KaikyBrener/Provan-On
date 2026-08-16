"use client";

import { useForm, useFieldArray, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useMutation } from "@tanstack/react-query";
import { authService } from "@/services/auth.service";
import { provaService } from "@/services/prova.service";
import { perguntaObjetivaService } from "@/services/pergunta-objetiva.service";
import { perguntaService } from "@/services/pergunta.service";
import { PageHeader } from "@/components/ui/PageHeader";
import { useRouter } from "next/navigation";
import { PlusCircle, Trash2, Save, ArrowLeft, AlertCircle } from "lucide-react";
import Link from "next/link";
import { useState } from "react";

const questaoSchema = z.object({
  tipo: z.enum(["objetiva", "discursiva"]),
  titulo: z.string().min(1, "Título é obrigatório"),
  enunciado: z.string().min(1, "O enunciado é obrigatório"),
  pontuacao: z.number().min(0.1, "Pontuação deve ser maior que zero"),
  // Objetivas
  opcaoA: z.string().optional(),
  opcaoB: z.string().optional(),
  opcaoC: z.string().optional(),
  opcaoD: z.string().optional(),
  opcaoCorreta: z.string().optional()
});

const provaFormSchema = z.object({
  titulo: z.string().min(1, "O título da prova é obrigatório"),
  questoes: z.array(questaoSchema).min(1, "Adicione pelo menos uma questão")
});

type ProvaFormValues = z.infer<typeof provaFormSchema>;

export default function CadastrarProvaPage() {
  const router = useRouter();
  const user = authService.getUser();
  const [globalError, setGlobalError] = useState("");

  const { register, control, handleSubmit, formState: { errors, isSubmitting }, watch } = useForm<ProvaFormValues>({
    resolver: zodResolver(provaFormSchema),
    defaultValues: {
      titulo: "",
      questoes: []
    }
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "questoes"
  });

  const watchQuestoes = watch("questoes");

  const submitMutation = useMutation({
    mutationFn: async (data: ProvaFormValues) => {
      if (!user) throw new Error("Usuário não autenticado");
      
      // 1. Criar Prova
      const prova = await provaService.criar({
        titulo: data.titulo,
        idProfessor: user.id
      });

      // 2. Criar Questões
      for (const questao of data.questoes) {
        if (questao.tipo === "objetiva") {
          if (!questao.opcaoA || !questao.opcaoB || !questao.opcaoC || !questao.opcaoD || !questao.opcaoCorreta) {
            throw new Error("Questão objetiva incompleta");
          }
          await perguntaObjetivaService.criar({
            idProva: prova.id,
            titulo: questao.titulo,
            descricao: questao.enunciado,
            pontuacao: questao.pontuacao,
            opcaoA: questao.opcaoA,
            opcaoB: questao.opcaoB,
            opcaoC: questao.opcaoC,
            opcaoD: questao.opcaoD,
            opcaoCorreta: questao.opcaoCorreta
          });
        } else {
          await perguntaService.criar({
            idProva: prova.id,
            titulo: questao.titulo,
            descricao: questao.enunciado,
            pontuacao: questao.pontuacao
          });
        }
      }

      return prova;
    },
    onSuccess: () => {
      router.push("/professor/provas");
    },
    onError: (error: any) => {
      setGlobalError(error.message || "Erro ao salvar a prova. Tente novamente.");
    }
  });

  const onSubmit = (data: any) => {
    setGlobalError("");
    submitMutation.mutate(data);
  };

  return (
    <>
      <PageHeader 
        title="Nova Prova" 
        description="Crie uma nova avaliação e adicione questões."
        action={
          <Link href="/professor/provas" className="flex items-center gap-2 px-4 py-2 bg-white border border-border text-slate-600 hover:bg-slate-50 rounded-lg text-sm font-medium transition-colors shadow-sm">
            <ArrowLeft size={16} />
            Voltar
          </Link>
        }
      />

      {globalError && (
        <div className="mb-6 flex items-center gap-2 p-4 rounded-lg bg-red-50 border border-red-200 text-red-700 text-sm">
          <AlertCircle size={18} />
          {globalError}
        </div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        {/* Dados da Prova */}
        <div className="bg-surface border border-border rounded-xl p-6 shadow-sm">
          <h2 className="text-lg font-bold text-text-main mb-4 border-b border-border pb-2">Detalhes da Prova</h2>
          
          <div>
            <label className="block text-sm font-medium text-text-main mb-1.5">Título da Prova</label>
            <input 
              type="text" 
              {...register("titulo")}
              placeholder="Ex: Avaliação Bimestral de Matemática"
              className={`w-full px-4 py-2 bg-background border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/20 ${errors.titulo ? 'border-red-400 focus:border-red-500' : 'border-border focus:border-primary'}`}
            />
            {errors.titulo && <p className="mt-1 text-xs text-red-500">{errors.titulo.message}</p>}
          </div>
        </div>

        {/* Questões */}
        <div className="flex items-center justify-between mt-8 mb-4">
          <h2 className="text-lg font-bold text-text-main">Questões</h2>
          <div className="flex gap-2">
            <button
              type="button"
              onClick={() => append({ tipo: "objetiva", titulo: "Questão", enunciado: "", pontuacao: 1.0, opcaoA: "", opcaoB: "", opcaoC: "", opcaoD: "", opcaoCorreta: "A" })}
              className="flex items-center gap-1.5 px-3 py-1.5 bg-blue-50 text-blue-600 hover:bg-blue-100 rounded-lg text-sm font-medium transition-colors border border-blue-200"
            >
              <PlusCircle size={16} />
              Adicionar Objetiva
            </button>
            <button
              type="button"
              onClick={() => append({ tipo: "discursiva", titulo: "Questão", enunciado: "", pontuacao: 1.0 })}
              className="flex items-center gap-1.5 px-3 py-1.5 bg-amber-50 text-amber-600 hover:bg-amber-100 rounded-lg text-sm font-medium transition-colors border border-amber-200"
            >
              <PlusCircle size={16} />
              Adicionar Discursiva
            </button>
          </div>
        </div>

        {errors.questoes?.root && <p className="text-sm text-red-500 mb-4">{errors.questoes.root.message}</p>}
        {fields.length === 0 && !errors.questoes?.root && (
          <div className="text-center p-12 border-2 border-dashed border-border rounded-xl text-text-muted text-sm">
            Nenhuma questão adicionada. Utilize os botões acima para começar.
          </div>
        )}

        <div className="space-y-6">
          {fields.map((field, index) => {
            const tipo = watchQuestoes[index]?.tipo || field.tipo;
            return (
              <div key={field.id} className="bg-surface border border-border rounded-xl p-6 shadow-sm relative animate-in fade-in slide-in-from-bottom-4">
                <div className="flex items-center justify-between mb-4">
                  <div className="flex items-center gap-3">
                    <h3 className="font-bold text-primary">Questão {index + 1}</h3>
                    <span className="px-2 py-0.5 text-[10px] uppercase font-bold tracking-wider rounded bg-slate-100 text-slate-600 border border-slate-200">
                      {tipo === 'objetiva' ? 'Múltipla Escolha' : 'Discursiva'}
                    </span>
                  </div>
                  <button 
                    type="button" 
                    onClick={() => remove(index)}
                    className="text-slate-400 hover:text-red-500 p-1 rounded-md hover:bg-red-50 transition-colors"
                  >
                    <Trash2 size={18} />
                  </button>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4">
                  <div className="md:col-span-3">
                    <label className="block text-xs font-medium text-text-main mb-1">Enunciado</label>
                    <textarea 
                      {...register(`questoes.${index}.enunciado`)}
                      rows={3}
                      className={`w-full px-3 py-2 bg-background border rounded-lg text-sm focus:outline-none focus:ring-1 focus:ring-primary ${errors.questoes?.[index]?.enunciado ? 'border-red-400' : 'border-border'}`}
                      placeholder="Digite a pergunta aqui..."
                    />
                    {errors.questoes?.[index]?.enunciado && <p className="mt-1 text-xs text-red-500">{errors.questoes[index]?.enunciado?.message}</p>}
                  </div>
                  <div>
                    <label className="block text-xs font-medium text-text-main mb-1">Pontuação</label>
                    <input 
                      type="number"
                      step="0.1"
                      {...register(`questoes.${index}.pontuacao` as const, { valueAsNumber: true })}
                      className={`w-full px-3 py-2 bg-background border rounded-lg text-sm focus:outline-none focus:ring-1 focus:ring-primary ${errors.questoes?.[index]?.pontuacao ? 'border-red-400' : 'border-border'}`}
                    />
                    {errors.questoes?.[index]?.pontuacao && <p className="mt-1 text-xs text-red-500">{errors.questoes[index]?.pontuacao?.message}</p>}
                  </div>
                </div>

                {tipo === "objetiva" && (
                  <div className="space-y-3 mt-4 border-t border-slate-100 pt-4 bg-slate-50/50 -mx-6 px-6 pb-2 rounded-b-xl">
                    <label className="block text-xs font-semibold text-text-main uppercase tracking-wider mb-2">Alternativas</label>
                    {['A', 'B', 'C', 'D'].map((letra) => {
                      const fieldName = `opcao${letra}` as "opcaoA"|"opcaoB"|"opcaoC"|"opcaoD";
                      return (
                        <div key={letra} className="flex items-start gap-3">
                          <div className="mt-1">
                            <input 
                              type="radio" 
                              id={`q${index}_opt${letra}`}
                              value={letra}
                              {...register(`questoes.${index}.opcaoCorreta`)}
                              className="w-4 h-4 text-primary border-slate-300 focus:ring-primary cursor-pointer"
                            />
                          </div>
                          <div className="flex-1">
                            <div className="relative">
                              <span className="absolute left-3 top-2 text-xs font-bold text-slate-400">{letra})</span>
                              <input 
                                type="text"
                                {...register(`questoes.${index}.${fieldName}`)}
                                className={`w-full pl-8 pr-3 py-1.5 bg-white border rounded-md text-sm focus:outline-none focus:border-primary ${errors.questoes?.[index]?.[fieldName] ? 'border-red-400' : 'border-border'}`}
                                placeholder={`Texto da alternativa ${letra}`}
                              />
                            </div>
                            {errors.questoes?.[index]?.[fieldName] && <p className="mt-0.5 text-xs text-red-500">{errors.questoes[index]?.[fieldName]?.message}</p>}
                          </div>
                        </div>
                      );
                    })}
                    {errors.questoes?.[index]?.opcaoCorreta && <p className="mt-2 text-xs text-red-500 font-medium">{errors.questoes[index]?.opcaoCorreta?.message}</p>}
                  </div>
                )}
              </div>
            );
          })}
        </div>

        <div className="pt-6 border-t border-border flex justify-end">
          <button
            type="submit"
            disabled={isSubmitting || fields.length === 0}
            className="flex items-center gap-2 px-6 py-2.5 bg-primary hover:bg-primary-hover text-white rounded-lg text-sm font-semibold transition-colors disabled:opacity-50 shadow-sm"
          >
            {isSubmitting ? (
              <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
            ) : (
              <Save size={18} />
            )}
            Salvar Prova
          </button>
        </div>
      </form>
    </>
  );
}
