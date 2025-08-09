// Purpose: Tiny presentational badge for flight status.
import type { FlightStatus } from "../shared/types/flights";

export function StatusBadge({ status }: { status: FlightStatus }) {
  const color =
    status === "Boarding"
      ? "bg-yellow-100 text-yellow-700"
      : status === "Departed"
      ? "bg-blue-100 text-blue-700"
      : status === "Landed"
      ? "bg-green-100 text-green-700"
      : "bg-gray-100 text-gray-700";

  return (
    <span className={`px-2 py-0.5 rounded text-sm font-medium ${color}`}>
      {status}
    </span>
  );
}
