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
    const errorText = await response.text();
    throw new Error(errorText);
  }

  const contentType = response.headers.get("Content-Type");

  if (contentType?.includes("image")) {
    return URL.createObjectURL(await response.blob()) as unknown as T;
  } else if (contentType?.includes("application/json")) {
    return (await response.json()) as T;
  } else {
    return (await response.text()) as unknown as T;
  }
}
