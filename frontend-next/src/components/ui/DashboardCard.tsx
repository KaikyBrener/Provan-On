import { ReactNode } from "react";
import { cn } from "@/lib/utils";

interface DashboardCardProps {
  label: string;
  value: ReactNode;
  icon: ReactNode;
  variant: "primary" | "success" | "warning" | "info" | "danger";
  className?: string;
}

export function DashboardCard({ label, value, icon, variant, className }: DashboardCardProps) {
  const variantStyles = {
    primary: "bg-blue-50 text-blue-600",
    success: "bg-green-50 text-green-600",
    warning: "bg-amber-50 text-amber-600",
    info: "bg-sky-50 text-sky-600",
    danger: "bg-red-50 text-red-600",
  };

  return (
    <div className={cn("bg-surface border border-border rounded-xl p-6 flex items-center justify-between gap-4 shadow-sm hover:shadow-md hover:border-primary/50 transition-all", className)}>
      <div>
        <p className="text-xs font-semibold text-text-muted uppercase tracking-wider mb-1.5">{label}</p>
        <h3 className="text-3xl font-bold text-text-main tracking-tight m-0">{value}</h3>
      </div>
      <div className={cn("w-14 h-14 rounded-2xl flex items-center justify-center shrink-0", variantStyles[variant])}>
        {icon}
      </div>
    </div>
  );
}
