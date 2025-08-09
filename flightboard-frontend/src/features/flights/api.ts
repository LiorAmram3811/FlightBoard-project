// Purpose: Raw HTTP calls for the flights backend.
import { http } from "../../lib/axios";
import type { FlightDto, CreateFlightDto } from "./types";

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

export async function searchFlights(params: {
  status?: string;
  destination?: string;
}) {
  const q = new URLSearchParams();
  if (params.status) q.set("status", params.status);
  if (params.destination) q.set("destination", params.destination);
  const { data } = await http.get(`/api/flights/search?${q.toString()}`);
  return data;
}
