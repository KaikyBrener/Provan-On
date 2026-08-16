import { Loader2 } from "lucide-react";

interface LoadingStateProps {
  message?: string;
}

export function LoadingState({ message = "Carregando..." }: LoadingStateProps) {
  return (
    <div className="flex flex-col items-center justify-center py-12 text-slate-400">
      <Loader2 className="w-8 h-8 animate-spin text-primary mb-3" />
      <p className="text-sm">{message}</p>
    </div>
  );
}
