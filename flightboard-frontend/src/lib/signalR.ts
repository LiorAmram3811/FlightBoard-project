// Purpose: Create and manage SignalR connection and hooks for event handling.
import * as signalR from "@microsoft/signalr";

export function createFlightsHub() {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_HUB_URL)
    .withAutomaticReconnect()
    .build();

  return connection;
}
