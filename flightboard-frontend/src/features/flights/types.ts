// Purpose: Type-safe shapes for flights exchanged with the backend.
export type FlightStatus = "Scheduled" | "Boarding" | "Departed" | "Landed";

export interface FlightDto {
  id: number;
  flightNumber: string;
  destination: string;
  departureTime: string;
  gate: string;
  status: FlightStatus;
}

export interface CreateFlightDto {
  flightNumber: string;
  destination: string;
  departureTime: string;
  gate: string;
}
