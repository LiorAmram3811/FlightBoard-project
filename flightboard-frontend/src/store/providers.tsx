import React from "react";
import { Provider } from "react-redux";
import { QueryClientProvider, useQueryClient } from "@tanstack/react-query";
import { store } from "./store";
import { queryClient } from "./queryClient";
import { ReactNode } from "react";
import { ToastContainer } from "react-toastify";
import { startFlightsLiveUpdates } from "../features/flights/live"; // ⬅️ add this

function LiveUpdatesMount() {
  const qc = useQueryClient();
  React.useEffect(() => startFlightsLiveUpdates(qc), [qc]);
  return null;
}

export function AppProviders({ children }: { children: ReactNode }) {
  return (
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        {children}
        <ToastContainer position="top-center" newestOnTop />
        <LiveUpdatesMount />
      </QueryClientProvider>
    </Provider>
  );
}
