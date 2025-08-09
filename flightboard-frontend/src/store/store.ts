// Purpose: Configure Redux store for client-only UI state (keep server data in React Query).
import { configureStore } from "@reduxjs/toolkit";
import uiReducer from "./states/uiSlice";
import filtersReducer from "./states/filtersSlice";

export const store = configureStore({
  reducer: {
    ui: uiReducer,
    filters: filtersReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
