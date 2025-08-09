// Purpose: Page composition with form + table.
import FiltersBar from "../components/FiltersBar";
import FlightForm from "../components/FlightForm";
import FlightTable from "../components/FlightTable";

export default function FlightBoardPage() {
  return (
    <div className="max-w-5xl mx-auto p-6 space-y-6">
      <h1 className="text-3xl font-bold">Flight Board</h1>
      <FlightForm />
      <FiltersBar />
      <FlightTable />
    </div>
  );
}
