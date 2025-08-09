// Purpose: Example UI slice (placeholder) for client-only state.
import { createSlice } from "@reduxjs/toolkit";

const uiSlice = createSlice({
  name: "ui",
  initialState: { theme: "light" as "light" | "dark" },
  reducers: {
    toggleTheme: (s) => {
      s.theme = s.theme === "light" ? "dark" : "light";
    },
  },
});

export const { toggleTheme } = uiSlice.actions;
export default uiSlice.reducer;
