"use client";

import { useQuery } from "@tanstack/react-query";
import { authService } from "@/services/auth.service";
import { provaService } from "@/services/prova.service";
import { alunoRespostaObjetivaService } from "@/services/aluno-resposta-objetiva.service";
import { PageHeader } from "@/components/ui/PageHeader";
import { DashboardCard } from "@/components/ui/DashboardCard";
import { LoadingState } from "@/components/ui/LoadingState";
import { BookOpen, Users, CheckCircle, BarChart3, PlusCircle } from "lucide-react";
import Link from "next/link";
import {
  AreaChart,
  Area,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer
} from "recharts";

export default function ProfessorDashboard() {
  const user = authService.getUser();

  const { data: provas, isLoading: isLoadingProvas } = useQuery({
    queryKey: ["provas", user?.id],
    queryFn: () => provaService.listarPorProfessor(user!.id),
    enabled: !!user?.id,
  });

  const { data: pontuacoes, isLoading: isLoadingPontuacoes } = useQuery({
    queryKey: ["todas-pontuacoes", provas],
    queryFn: async () => {
      if (!provas || provas.length === 0) return [];
      const promises = provas.map((p) => alunoRespostaObjetivaService.listarPontuacoesProva(p.id));
      const results = await Promise.all(promises);
      return results.flat();
    },
    enabled: !!provas && provas.length > 0,
  });

  if (isLoadingProvas || isLoadingPontuacoes) {
    return <LoadingState message="Carregando informações do painel..." />;
  }

  // Estatísticas
  const totalProvas = provas?.length || 0;
  
  // Usaremos o length de pontuacoes como número de avaliações realizadas (cada pontuacao é um aluno que respondeu)
  const totalAvaliacoes = pontuacoes?.length || 0;
  
  // Número de alunos únicos não é trivial sem a API enviar, então usaremos totalAvaliacoes como proxy no mockup
  const totalAlunos = totalAvaliacoes;

  const somaNotas = pontuacoes?.reduce((acc, p) => acc + (p.pontuacaoFinal || p.pontuacao || p.nota || 0), 0) || 0;
  const mediaGeral = totalAvaliacoes > 0 ? (somaNotas / totalAvaliacoes).toFixed(1) : "0.0";

  // Mock dados para gráfico mensal para ilustrar
  const chartData = [
    { name: "Jan", media: 6.5 },
    { name: "Fev", media: 7.2 },
    { name: "Mar", media: 6.8 },
    { name: "Abr", media: 8.1 },
    { name: "Mai", media: 7.9 },
    { name: "Jun", media: parseFloat(mediaGeral) > 0 ? parseFloat(mediaGeral) : 8.5 }
  ];

  return (
    <>
      <PageHeader 
        title="Painel do Professor" 
        description="Bem-vindo ao seu painel de controle. Aqui está um resumo do seu ambiente."
        action={
          <Link href="/professor/provas/cadastrar" className="flex items-center gap-2 px-4 py-2 bg-primary hover:bg-primary-hover text-white rounded-lg text-sm font-semibold transition-colors shadow-sm">
            <PlusCircle size={18} />
            Nova Prova
          </Link>
        }
      />

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <DashboardCard 
          label="Total de Provas" 
          value={totalProvas} 
          icon={<BookOpen size={24} />} 
          variant="primary" 
        />
        <DashboardCard 
          label="Alunos Avaliados" 
          value={totalAlunos} 
          icon={<Users size={24} />} 
          variant="success" 
        />
        <DashboardCard 
          label="Provas Realizadas" 
          value={totalAvaliacoes} 
          icon={<CheckCircle size={24} />} 
          variant="info" 
        />
        <DashboardCard 
          label="Média Geral" 
          value={mediaGeral} 
          icon={<BarChart3 size={24} />} 
          variant="warning" 
        />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Gráfico */}
        <div className="lg:col-span-2 bg-surface border border-border rounded-xl p-6 shadow-sm">
          <h3 className="text-lg font-bold text-text-main mb-6">Desempenho Médio</h3>
          <div className="h-72 w-full">
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart data={chartData} margin={{ top: 10, right: 10, left: -20, bottom: 0 }}>
                <defs>
                  <linearGradient id="colorMedia" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#2563EB" stopOpacity={0.3}/>
                    <stop offset="95%" stopColor="#2563EB" stopOpacity={0}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#E2E8F0" />
                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{fill: '#64748B', fontSize: 12}} dy={10} />
                <YAxis axisLine={false} tickLine={false} tick={{fill: '#64748B', fontSize: 12}} />
                <Tooltip 
                  contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
                  itemStyle={{ color: '#0F172A', fontWeight: 600 }}
                />
                <Area type="monotone" dataKey="media" stroke="#2563EB" strokeWidth={3} fillOpacity={1} fill="url(#colorMedia)" />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Provas Recentes */}
        <div className="bg-surface border border-border rounded-xl p-6 shadow-sm">
          <div className="flex items-center justify-between mb-6">
            <h3 className="text-lg font-bold text-text-main">Provas Recentes</h3>
            <Link href="/professor/provas" className="text-sm font-medium text-primary hover:text-primary-hover">Ver todas</Link>
          </div>
          
          {(!provas || provas.length === 0) ? (
            <p className="text-sm text-text-muted text-center py-8">Nenhuma prova cadastrada.</p>
          ) : (
            <div className="space-y-4">
              {provas.slice(-5).reverse().map(prova => (
                <div key={prova.id} className="flex items-center justify-between p-3 rounded-lg border border-border hover:border-primary/30 transition-colors">
                  <div>
                    <h4 className="text-sm font-bold text-text-main line-clamp-1">{prova.titulo}</h4>
                    <p className="text-xs text-text-muted mt-0.5">{prova.codigoAcesso || 'Sem código'}</p>
                  </div>
                  <span className={`px-2.5 py-1 text-[10px] font-bold rounded-full ${prova.ativa !== false ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-600'}`}>
                    {prova.ativa !== false ? 'Ativa' : 'Inativa'}
                  </span>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </>
  );
}
