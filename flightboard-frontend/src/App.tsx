// Purpose: Top-level app layout; router could be added later if needed.
import FlightBoardPage from "./pages/FlightBoardPage";

export default function App() {
  return (
    <div className="min-h-dvh bg-gray-50">
      <FlightBoardPage />
    </div>
  );
}
