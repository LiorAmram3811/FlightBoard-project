// Purpose: Redux slice for search filters (status, destination) with explicit 'submit' behavior.
import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export type FlightStatusFilter =
  | ""
  | "Scheduled"
  | "Boarding"
  | "Departed"
  | "Landed";

export interface FiltersState {
  status: FlightStatusFilter;
  destination: string;
}

const initialState: FiltersState = {
  status: "",
  destination: "",
};

const filtersSlice = createSlice({
  name: "filters",
  initialState,
  reducers: {
    setFilters: (state, action: PayloadAction<FiltersState>) => {
      state.status = action.payload.status;
      state.destination = action.payload.destination;
    },
    clearFilters: (state) => {
      state.status = "";
      state.destination = "";
    },
  },
});

export const { setFilters, clearFilters } = filtersSlice.actions;
export default filtersSlice.reducer;
