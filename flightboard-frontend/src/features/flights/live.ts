// Purpose: connect to the SignalR hub and keep React Query cache fresh.
import * as signalR from "@microsoft/signalr";
import { QueryClient } from "@tanstack/react-query";

export function startFlightsLiveUpdates(queryClient: QueryClient) {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_HUB_URL || "/hubs/flights")
    .withAutomaticReconnect()
    .build();

  // Invalidate flights list whenever server says something changed
  const refresh = () =>
    queryClient.invalidateQueries({ queryKey: ["flights"] });

  connection.on("FlightAdded", refresh);
  connection.on("FlightDeleted", refresh);

  connection.start().catch((err) => {
    console.error("SignalR connection failed:", err);
  });
  return () => {
    connection.stop();
  };
}
