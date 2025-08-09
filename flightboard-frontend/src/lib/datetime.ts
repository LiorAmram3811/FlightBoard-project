export function formatLocalDateTime(
  iso: string,
  opts: Intl.DateTimeFormatOptions = {
    year: "numeric",
    month: "short",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
  }
) {
  return new Intl.DateTimeFormat(undefined, opts).format(new Date(iso));
}
