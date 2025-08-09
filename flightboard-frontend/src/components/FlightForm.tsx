// src/components/FlightForm.tsx
// Purpose: Flight creation form with client-side validation (Yup) and nice toasts.

import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { toast } from "react-toastify";
import { useCreateFlight } from "../features/flights/hooks";
import {
  createFlightSchema,
  type CreateFlightForm,
} from "../features/flights/validation";
import type { CreateFlightDto } from "../features/flights/types";

export default function FlightForm() {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<CreateFlightForm>({
    resolver: yupResolver(createFlightSchema),
    defaultValues: {
      flightNumber: "",
      destination: "",
      gate: "",
      departureTime: "",
    },
  });

  const create = useCreateFlight();

  const onSubmit = async (values: CreateFlightForm) => {
    const payload: CreateFlightDto = {
      flightNumber: values.flightNumber.trim(),
      destination: values.destination.trim(),
      gate: values.gate.trim(),
      departureTime: new Date(values.departureTime).toISOString(),
    };

    try {
      await create.mutateAsync(payload);
      toast.success("Flight created ✅");
      reset();
    } catch (err: unknown) {
      toast.error(
        "Failed to create flight. Please check the form or try again."
      );
    }
  };

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="bg-white p-4 rounded-2xl shadow space-y-4"
    >
      <div className="grid md:grid-cols-4 gap-4">
        <div>
          <label className="block text-sm font-medium mb-1">
            Flight Number
          </label>
          <input
            {...register("flightNumber")}
            className="w-full rounded border px-3 py-2"
            placeholder="e.g., LY315"
            autoComplete="off"
          />
          {errors.flightNumber && (
            <p className="mt-1 text-sm text-red-600">
              {errors.flightNumber.message}
            </p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Destination</label>
          <input
            {...register("destination")}
            className="w-full rounded border px-3 py-2"
            placeholder="e.g., London"
            autoComplete="off"
          />
          {errors.destination && (
            <p className="mt-1 text-sm text-red-600">
              {errors.destination.message}
            </p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Gate</label>
          <input
            {...register("gate")}
            className="w-full rounded border px-3 py-2"
            placeholder="e.g., A12"
            autoComplete="off"
          />
          {errors.gate && (
            <p className="mt-1 text-sm text-red-600">{errors.gate.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Departure</label>
          <input
            type="datetime-local"
            {...register("departureTime")}
            className="w-full rounded border px-3 py-2"
          />
          {errors.departureTime && (
            <p className="mt-1 text-sm text-red-600">
              {errors.departureTime.message}
            </p>
          )}
        </div>
      </div>

      <div className="flex items-center gap-3">
        <button
          type="submit"
          disabled={isSubmitting || create.isPending}
          className="px-4 py-2 rounded-xl bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50"
        >
          {create.isPending ? "Creating…" : "Add flight"}
        </button>
        {create.isError && (
          <span className="text-sm text-red-600">
            Server error while creating.
          </span>
        )}
      </div>
    </form>
  );
}
