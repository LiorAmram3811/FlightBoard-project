// Purpose: React Query hooks to manage server-state for flights.
import { useQuery, useMutation } from "@tanstack/react-query";
import { fetchFlights, createFlight, deleteFlight, searchFlights } from "./api";
import { queryClient } from "../../store/queryClient";
import type { CreateFlightDto } from "../../features/flights/types";

const keys = {
  flights: ["flights"] as const,
};

export function useFlights(filters?: {
  status?: string;
  destination?: string;
}) {
  return useQuery({
    queryKey:
      filters && (filters.status || filters.destination)
        ? ["flights", filters]
        : ["flights"],
    queryFn: () =>
      filters && (filters.status || filters.destination)
        ? searchFlights(filters)
        : fetchFlights(),
  });
}

export function useCreateFlight() {
  return useMutation({
    mutationFn: (payload: CreateFlightDto) => createFlight(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: keys.flights }),
  });
}

export function useDeleteFlight() {
  return useMutation({
    mutationFn: (id: number) => deleteFlight(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: keys.flights }),
  });
}
