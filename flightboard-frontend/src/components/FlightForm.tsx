// Purpose: Simple creation form; posts Israel local time with offset (+02/+03).
import { useState } from "react";
import { useCreateFlight } from "../features/flights/hooks";

export default function FlightForm() {
  const [flightNumber, setFlightNumber] = useState("");
  const [destination, setDestination] = useState("");
  const [gate, setGate] = useState("");
  const [departure, setDeparture] = useState(""); // HTML datetime-local (no offset)
  const create = useCreateFlight();

  const onSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!departure) return;

    // Convert local (browser) time to ISO with current offset
    const local = new Date(departure);
    const isoWithOffset = new Date(
      local.getTime() - local.getTimezoneOffset() * 60000
    )
      .toISOString()
      .replace("Z", ""); // keep offset-less; backend should treat as Israel local OR accept DateTimeOffset if you append +02:00/+03:00

    create.mutate(
      {
        flightNumber,
        destination,
        gate,
        departureTime: isoWithOffset, // adjust if backend expects explicit offset
      },
      {
        onSuccess: () => {
          setFlightNumber("");
          setDestination("");
          setGate("");
          setDeparture("");
        },
      }
    );
  };

  return (
    <form onSubmit={onSubmit} className="space-y-3 bg-white p-4 rounded border">
      <div className="grid grid-cols-2 gap-3">
        <input
          className="border rounded px-3 py-2"
          placeholder="Flight #"
          value={flightNumber}
          onChange={(e) => setFlightNumber(e.target.value)}
          required
        />
        <input
          className="border rounded px-3 py-2"
          placeholder="Destination"
          value={destination}
          onChange={(e) => setDestination(e.target.value)}
          required
        />
        <input
          className="border rounded px-3 py-2"
          placeholder="Gate"
          value={gate}
          onChange={(e) => setGate(e.target.value)}
          required
        />
        <input
          className="border rounded px-3 py-2"
          type="datetime-local"
          value={departure}
          onChange={(e) => setDeparture(e.target.value)}
          required
        />
      </div>
      <button
        disabled={create.isPending}
        className="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-60"
      >
        {create.isPending ? "Saving…" : "Add Flight"}
      </button>
      {create.isError && (
        <p className="text-sm text-red-600">Failed to create flight.</p>
      )}
    </form>
  );
}
