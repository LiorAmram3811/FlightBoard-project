// Purpose: React Query hooks to manage server-state for flights.
import { useQuery, useMutation } from "@tanstack/react-query";
import { fetchFlights, createFlight, deleteFlight } from "./api";
import { queryClient } from "../../app/queryClient";
import type { CreateFlightDto } from "../../shared/types/flights";

const keys = {
  flights: ["flights"] as const,
};

export function useFlights() {
  return useQuery({
    queryKey: keys.flights,
    queryFn: fetchFlights,
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
