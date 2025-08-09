// Purpose: Type-safe shapes for flights exchanged with the backend.
export type FlightStatus = "Scheduled" | "Boarding" | "Departed" | "Landed";

export interface FlightDto {
  id: number;
  flightNumber: string;
  destination: string;
  departureTime: string; // ISO string (server returns Israel time per your backend mapping)
  gate: string;
  status: FlightStatus;
}

export interface CreateFlightDto {
  flightNumber: string;
  destination: string;
  // If sending Israel local with offset, prefer DateTimeOffset → ISO with +02/+03
  departureTime: string;
  gate: string;
}
