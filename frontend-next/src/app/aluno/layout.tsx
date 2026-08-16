"use client";

import { ReactNode } from "react";
import { GraduationCap } from "lucide-react";

export default function AlunoLayout({ children }: { children: ReactNode }) {
  return (
    <div className="min-h-screen bg-background flex flex-col">
      <header className="h-16 bg-surface border-b border-border flex items-center justify-between px-4 sm:px-6 md:px-8 shrink-0 shadow-sm">
        <div className="flex items-center gap-3">
          <div className="w-9 h-9 bg-gradient-to-br from-blue-500 to-blue-700 rounded-lg flex items-center justify-center text-white shadow-md">
            <GraduationCap size={20} />
          </div>
          <div className="text-xl font-bold text-text-main tracking-tight">
            Prova<span className="text-blue-600">On</span>
          </div>
        </div>
      </header>

      <main className="flex-1 flex flex-col items-center justify-center p-4 sm:p-6 md:p-8">
        <div className="w-full max-w-4xl">
          {children}
        </div>
      </main>
      
      <footer className="py-6 text-center text-sm text-text-muted">
        <p>&copy; {new Date().getFullYear()} ProvaOn. Todos os direitos reservados.</p>
      </footer>
    </div>
  );
}
