"use client";

import { useEffect, useState } from "react";
import { authService } from "@/services/auth.service";
import AdminLayout from "@/components/layout/AdminLayout";
import { useRouter } from "next/navigation";

export default function AdminRootLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
    if (!authService.isAuthenticated()) {
      router.push("/login");
    } else {
      const user = authService.getUser();
      if (user?.role !== "admin") {
        router.push("/login");
      }
    }
  }, [router]);

  if (!mounted) return null;

  return <AdminLayout>{children}</AdminLayout>;
}
