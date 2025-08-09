// Purpose: Raw HTTP calls for the flights backend.
import { http } from "../../shared/api/http";
import type { FlightDto, CreateFlightDto } from "../../shared/types/flights";

export async function fetchFlights(): Promise<FlightDto[]> {
  const { data } = await http.get("/api/flights");
  return data;
}

export async function createFlight(
  payload: CreateFlightDto
): Promise<FlightDto> {
  const { data } = await http.post("/api/flights", payload);
  return data;
}

export async function deleteFlight(id: number): Promise<void> {
  await http.delete(`/api/flights/${id}`);
}
