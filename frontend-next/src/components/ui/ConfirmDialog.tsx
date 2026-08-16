interface ConfirmDialogProps {
  isOpen: boolean;
  title: string;
  message: string;
  onConfirm: () => void;
  onCancel: () => void;
  confirmText?: string;
  cancelText?: string;
  isDestructive?: boolean;
}

export function ConfirmDialog({
  isOpen,
  title,
  message,
  onConfirm,
  onCancel,
  confirmText = "Confirmar",
  cancelText = "Cancelar",
  isDestructive = false
}: ConfirmDialogProps) {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-navy/50 backdrop-blur-sm">
      <div className="bg-surface w-full max-w-md rounded-2xl shadow-xl overflow-hidden animate-in fade-in zoom-in-95 duration-200">
        <div className="p-6">
          <h3 className={`text-lg font-bold mb-2 ${isDestructive ? 'text-red-600' : 'text-text-main'}`}>
            {title}
          </h3>
          <p className="text-sm text-text-muted">{message}</p>
        </div>
        <div className="px-6 py-4 bg-slate-50 flex justify-end gap-3 border-t border-border">
          <button
            onClick={onCancel}
            className="px-4 py-2 text-sm font-medium text-slate-600 bg-white border border-slate-200 rounded-lg hover:bg-slate-50 transition-colors"
          >
            {cancelText}
          </button>
          <button
            onClick={onConfirm}
            className={`px-4 py-2 text-sm font-medium text-white rounded-lg transition-colors ${
              isDestructive 
                ? 'bg-red-600 hover:bg-red-700' 
                : 'bg-primary hover:bg-primary-hover'
            }`}
          >
            {confirmText}
          </button>
        </div>
      </div>
    </div>
  );
}
