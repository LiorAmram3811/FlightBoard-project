// Purpose: Preconfigured Axios instance with base URL and JSON handling.
import axios from "axios";

export const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_DOCKER,
  headers: { "Content-Type": "application/json" },
  timeout: 15_000,
});
