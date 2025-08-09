import * as yup from "yup";

// Keep field names aligned with your CreateFlightDto
export const createFlightSchema = yup.object({
  flightNumber: yup
    .string()
    .trim()
    .matches(/^[A-Z0-9]{2,8}$/, "Use 2–8 uppercase letters/numbers")
    .required("Flight number is required"),
  destination: yup
    .string()
    .trim()
    .min(2, "Destination must be at least 2 characters")
    .required("Destination is required"),
  gate: yup
    .string()
    .trim()
    .matches(
      /^[A-Za-z]\d{1,3}$/,
      "Use a letter followed by 1–3 digits, e.g. A12"
    )
    .required("Gate is required"),
  departureTime: yup
    .string()
    .required("Departure time is required")
    .test(
      "future",
      "Departure must be in the future",
      (v) => !!v && new Date(v).getTime() > Date.now()
    ),
});

export type CreateFlightForm = yup.InferType<typeof createFlightSchema>;
