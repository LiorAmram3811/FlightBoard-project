// Purpose: Configure Redux store for client-only UI state (keep server data in React Query).
import { configureStore } from "@reduxjs/toolkit";
import uiReducer from "../features/state/uiSlice";

export const store = configureStore({
  reducer: {
    ui: uiReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
