"use client";

import { ReactNode } from "react";
import { GraduationCap, LayoutDashboard, FileText, Settings, LogOut, Menu } from "lucide-react";
import Link from "next/link";
import { authService } from "@/services/auth.service";

export default function DashboardLayout({ children }: { children: ReactNode }) {
  const user = authService.getUser();

  return (
    <div className="flex min-h-screen bg-background">
      {/* Sidebar */}
      <aside className="w-64 min-w-[16rem] bg-navy flex flex-col shadow-[2px_0_12px_rgba(15,23,42,0.1)] z-40 hidden md:flex">
        <div className="p-6 border-b border-navy-light flex items-center gap-3">
          <div className="w-10 h-10 bg-gradient-to-br from-blue-400 to-blue-600 rounded-xl flex items-center justify-center text-white shadow-lg shadow-blue-500/40 shrink-0">
            <GraduationCap size={22} />
          </div>
          <div>
            <div className="text-xl font-bold text-white tracking-tight">
              Prova<span className="text-blue-400">On</span>
            </div>
            <div className="text-[0.65rem] font-medium text-slate-400 uppercase tracking-widest mt-0.5">
              {user?.role || "Usuário"}
            </div>
          </div>
        </div>

        <nav className="flex-1 overflow-y-auto p-3 space-y-1">
          <div className="text-[0.65rem] font-semibold text-slate-400 uppercase tracking-widest p-3 mt-2">
            Principal
          </div>
          <Link href="/professor/dashboard" className="flex items-center gap-3 px-3.5 py-2.5 text-sm font-medium rounded-lg bg-blue-600 text-white">
            <LayoutDashboard size={18} />
            Dashboard
          </Link>
          <div className="text-[0.65rem] font-semibold text-slate-400 uppercase tracking-widest p-3 mt-2">
            Gestão
          </div>
          <Link href="/professor/provas" className="flex items-center gap-3 px-3.5 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-navy-light hover:text-white transition-colors">
            <FileText size={18} className="opacity-70" />
            Minhas Provas
          </Link>
        </nav>

        <div className="p-4 border-t border-navy-light">
          <button 
            onClick={() => authService.logout()}
            className="w-full flex items-center justify-center gap-2 px-4 py-2 text-sm font-medium rounded-lg border border-red-500/30 text-red-400 hover:bg-red-500/10 hover:border-red-500/50 transition-colors"
          >
            <LogOut size={16} />
            Sair
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col min-w-0">
        <header className="h-16 bg-surface border-b border-border flex items-center justify-between px-6 shrink-0 shadow-[0_1px_4px_rgba(15,23,42,0.04)]">
          <div className="flex items-center gap-3">
            <button className="md:hidden text-slate-500 hover:text-primary">
              <Menu size={24} />
            </button>
            <h1 className="text-sm font-semibold text-text-main hidden sm:block">Visão Geral</h1>
          </div>

          <div className="flex items-center gap-3">
            <div className="flex items-center gap-2 border-1.5 border-border rounded-lg px-3 py-1.5 hover:border-primary hover:bg-primary-soft transition-colors cursor-pointer">
              <div className="w-7 h-7 rounded-full bg-gradient-to-br from-primary to-primary-hover flex items-center justify-center text-xs font-bold text-white">
                {user?.nome ? user.nome.substring(0, 2).toUpperCase() : "PR"}
              </div>
              <span className="text-sm font-medium text-text-main hidden sm:block">
                {user?.nome || "Professor"}
              </span>
            </div>
          </div>
        </header>

        <main className="flex-1 overflow-y-auto p-6 md:p-8">
          {children}
        </main>
      </div>
    </div>
  );
}
