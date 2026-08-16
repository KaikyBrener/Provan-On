"use client";

import { useQuery } from "@tanstack/react-query";
import { professorService } from "@/services/professor.service";
import { alunoService } from "@/services/aluno.service";
import { provaService } from "@/services/prova.service";
import { DashboardCard } from "@/components/ui/DashboardCard";
import { Users, UsersRound, FileText } from "lucide-react";
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend } from 'recharts';

export default function AdminDashboardPage() {
  const { data: professores, isLoading: loadingProfessores } = useQuery({
    queryKey: ["professores"],
    queryFn: professorService.listar,
  });

  const { data: alunos, isLoading: loadingAlunos } = useQuery({
    queryKey: ["alunos"],
    queryFn: alunoService.listar,
  });

  const { data: provas, isLoading: loadingProvas } = useQuery({
    queryKey: ["provas"],
    queryFn: provaService.listar,
  });

  const isLoading = loadingProfessores || loadingAlunos || loadingProvas;

  // Mocked chart data grouped by month
  const chartData = [
    { name: 'Jan', provas: 0, alunos: 0 },
    { name: 'Fev', provas: 0, alunos: 0 },
    { name: 'Mar', provas: 0, alunos: 0 },
    { name: 'Abr', provas: 0, alunos: 0 },
    { name: 'Mai', provas: 0, alunos: 0 },
    { name: 'Jun', provas: 0, alunos: 0 },
    { name: 'Jul', provas: 0, alunos: 0 },
    { name: 'Ago', provas: 0, alunos: 0 },
    { name: 'Set', provas: 0, alunos: 0 },
    { name: 'Out', provas: 0, alunos: 0 },
    { name: 'Nov', provas: 0, alunos: 0 },
    { name: 'Dez', provas: 0, alunos: 0 },
  ];

  if (provas && alunos) {
    provas.forEach(p => {
      const d = new Date(p.dataHoraInicio || "");
      if (!isNaN(d.getTime())) chartData[d.getMonth()].provas++;
    });
    alunos.forEach(a => {
      const d = new Date(a.dataCadastro || a.dataCriacao || "");
      if (!isNaN(d.getTime())) chartData[d.getMonth()].alunos++;
    });
  }

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <DashboardCard
          label="Total de Alunos"
          value={alunos?.length || 0}
          icon={<UsersRound size={28} />}
          variant="primary"
        />
        <DashboardCard
          label="Total de Professores"
          value={professores?.length || 0}
          icon={<Users size={28} />}
          variant="success"
        />
        <DashboardCard
          label="Total de Provas"
          value={provas?.length || 0}
          icon={<FileText size={28} />}
          variant="info"
        />
      </div>

      <div className="bg-surface border border-border rounded-xl p-6 shadow-sm">
        <h3 className="text-lg font-semibold text-text-main mb-6">Cadastros por Mês</h3>
        <div className="h-[300px] w-full">
          <ResponsiveContainer width="100%" height="100%">
            <AreaChart data={chartData} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
              <defs>
                <linearGradient id="colorProvas" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#3b82f6" stopOpacity={0.8}/>
                  <stop offset="95%" stopColor="#3b82f6" stopOpacity={0}/>
                </linearGradient>
                <linearGradient id="colorAlunos" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#10b981" stopOpacity={0.8}/>
                  <stop offset="95%" stopColor="#10b981" stopOpacity={0}/>
                </linearGradient>
              </defs>
              <XAxis dataKey="name" stroke="#94a3b8" fontSize={12} tickLine={false} axisLine={false} />
              <YAxis stroke="#94a3b8" fontSize={12} tickLine={false} axisLine={false} />
              <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#e2e8f0" />
              <Tooltip 
                contentStyle={{ borderRadius: '8px', border: '1px solid #e2e8f0', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
              />
              <Legend />
              <Area type="monotone" dataKey="provas" name="Provas Criadas" stroke="#3b82f6" fillOpacity={1} fill="url(#colorProvas)" />
              <Area type="monotone" dataKey="alunos" name="Alunos Cadastrados" stroke="#10b981" fillOpacity={1} fill="url(#colorAlunos)" />
            </AreaChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
}
