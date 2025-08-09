// Purpose: UI controls for filtering flights by status and destination with Search and Clear buttons.
import React from "react";
import { useDispatch, useSelector } from "react-redux";
import type { RootState } from "../store/store";
import { setFilters, clearFilters } from "../store/states/filtersSlice";

export default function FiltersBar() {
  const dispatch = useDispatch();
  const current = useSelector((s: RootState) => s.filters);

  const [status, setStatus] = React.useState(current.status);
  const [destination, setDestination] = React.useState(current.destination);

  const onSearch = () => {
    dispatch(setFilters({ status, destination: destination.trim() }));
  };

  const onClear = () => {
    setStatus("");
    setDestination("");
    dispatch(clearFilters());
  };

  return (
    <div className="bg-white p-4 rounded-2xl shadow flex flex-wrap gap-4 items-end">
      <div className="flex flex-col">
        <label className="text-sm font-medium mb-1">Status</label>
        <select
          className="rounded border px-3 py-2 min-w-[160px]"
          value={status}
          onChange={(e) => setStatus(e.target.value as any)}
        >
          <option value="">All</option>
          <option value="Scheduled">Scheduled</option>
          <option value="Boarding">Boarding</option>
          <option value="Departed">Departed</option>
          <option value="Landed">Landed</option>
        </select>
      </div>

      <div className="flex flex-col">
        <label className="text-sm font-medium mb-1">Destination</label>
        <input
          className="rounded border px-3 py-2 min-w-[220px]"
          placeholder="e.g., London"
          value={destination}
          onChange={(e) => setDestination(e.target.value)}
        />
      </div>

      <div className="flex gap-2">
        <button
          onClick={onSearch}
          className="px-4 py-2 rounded-xl bg-blue-600 text-white hover:bg-blue-700"
          type="button"
        >
          Search
        </button>
        <button
          onClick={onClear}
          className="px-4 py-2 rounded-xl border hover:bg-gray-50"
          type="button"
        >
          Clear Filters
        </button>
      </div>
    </div>
  );
}
