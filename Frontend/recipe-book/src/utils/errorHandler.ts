import toast from "react-hot-toast";

export function handleError(error: unknown, defaultMessage = "Произошла ошибка") {
  if (error instanceof Error) {
    try {
      const parsedMessage = JSON.parse(error.message) as string[];

      if (Array.isArray(parsedMessage)) {
        toast.error(parsedMessage.join("\n"));
      } else {
        toast.error(error.message);
      }
    } catch {
      toast.error(error.message);
    }
  } else {
    toast.error(defaultMessage);
  }
}
