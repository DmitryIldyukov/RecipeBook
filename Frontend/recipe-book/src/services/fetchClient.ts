export async function fetchClient<T>(url: string, options?: RequestInit): Promise<T> {
  const isFormData = options?.body instanceof FormData;

  const response = await fetch(url, {
    ...options,
    headers: {
      ...options?.headers,
      ...(!isFormData && { "Content-Type": "application/json" }),
    },
  });

  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status.toString()}`);
  }

  if (response.headers.get("Content-Type")?.includes("image")) {
    return URL.createObjectURL(await response.blob()) as unknown as T;
  }

  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return response.json();
}
