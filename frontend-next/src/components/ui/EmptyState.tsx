import { ReactNode } from "react";
import { Inbox } from "lucide-react";

interface EmptyStateProps {
  title: string;
  description: string;
  action?: ReactNode;
}

export function EmptyState({ title, description, action }: EmptyStateProps) {
  return (
    <div className="flex flex-col items-center justify-center p-12 text-center bg-surface border border-dashed border-border rounded-xl">
      <div className="w-16 h-16 bg-slate-50 text-slate-400 rounded-full flex items-center justify-center mb-4">
        <Inbox size={32} />
      </div>
      <h3 className="text-lg font-semibold text-text-main mb-1">{title}</h3>
      <p className="text-sm text-text-muted mb-6 max-w-sm">{description}</p>
      {action}
    </div>
  );
}
