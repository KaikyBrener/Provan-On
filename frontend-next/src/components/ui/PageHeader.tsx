import { ReactNode } from "react";

interface PageHeaderProps {
  title: string;
  description: string;
  action?: ReactNode;
}

export function PageHeader({ title, description, action }: PageHeaderProps) {
  return (
    <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-8">
      <div>
        <h1 className="text-2xl font-bold text-text-main tracking-tight m-0">{title}</h1>
        <p className="text-sm text-text-muted mt-1 m-0">{description}</p>
      </div>
      {action && <div>{action}</div>}
    </div>
  );
}
