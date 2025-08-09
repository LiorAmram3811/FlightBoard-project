// Purpose: Central QueryClient with sensible defaults (retries, cache).
import { QueryClient } from "@tanstack/react-query";

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 2,
      refetchOnWindowFocus: false,
      staleTime: 10_000,
    },
    mutations: {
      retry: 1,
    },
  },
});
