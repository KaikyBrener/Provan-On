"use client";

import { useEffect, useState } from "react";
import { authService } from "@/services/auth.service";
import DashboardLayout from "@/components/layout/DashboardLayout";
import { useRouter } from "next/navigation";

export default function ProfessorLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
    if (!authService.isAuthenticated()) {
      router.push("/login");
    } else {
      const user = authService.getUser();
      if (user?.role !== "professor") {
        router.push("/login");
      }
    }
  }, [router]);

  if (!mounted) return null;

  return <DashboardLayout>{children}</DashboardLayout>;
}
