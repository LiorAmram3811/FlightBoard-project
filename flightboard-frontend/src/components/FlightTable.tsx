// Purpose: Table rendering of flights with Israel-time date formatting.
import { useFlights, useDeleteFlight } from "../features/flights/hooks";
import { StatusBadge } from "./StatusBadge";
import { toast } from "react-toastify";
import type { RootState } from "../store/store";
import { useSelector } from "react-redux";

export default function FlightTable() {
  const filters = useSelector((s: RootState) => s.filters);
  const { data, isLoading, isError } = useFlights({
    status: filters.status || undefined,
    destination: filters.destination || undefined,
  });
  const del = useDeleteFlight();

  if (isLoading) return <div className="p-4">Loading flights…</div>;
  if (isError)
    return <div className="p-4 text-red-600">Failed to load flights.</div>;

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full border-collapse">
        <thead>
          <tr className="text-left border-b">
            <th className="p-2">Flight</th>
            <th className="p-2">Destination</th>
            <th className="p-2">Departure (UTC)</th>
            <th className="p-2">Gate</th>
            <th className="p-2">Status</th>
            <th className="p-2" />
          </tr>
        </thead>
        <tbody>
          {data?.map((f) => (
            <tr key={f.id} className="border-b hover:bg-gray-50">
              <td className="p-2 font-medium">{f.flightNumber}</td>
              <td className="p-2">{f.destination}</td>
              <td className="p-2">{f.departureTime}</td>
              <td className="p-2">{f.gate}</td>
              <td className="p-2">
                <StatusBadge status={f.status} />
              </td>
              <td className="p-2 text-right">
                <button
                  className="px-2 py-1 rounded bg-red-600 text-white text-sm hover:bg-red-700"
                  onClick={() => {
                    if (
                      !confirm(
                        "Clicking ok would delete this flight permanently."
                      )
                    )
                      return;
                    del.mutate(f.id, {
                      onSuccess: () =>
                        toast.success("Flight deleted successfully"),
                      onError: () => toast.error("Failed to delete flight"),
                    });
                  }}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
          {(!data || data.length === 0) && (
            <tr>
              <td colSpan={6} className="p-4 text-center text-gray-500">
                No flights yet
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
