import { authService } from "./authService";

export async function fetchClient<T>(url: string, options?: RequestInit): Promise<T> {
  const isFormData = options?.body instanceof FormData;
  let token = localStorage.getItem("access-token");

  const defaultHeaders: HeadersInit = {
    ...(token && { Authorization: `Bearer ${token}` }),
    ...(!isFormData && { "Content-Type": "application/json" }),
  };

  async function makeRequest(): Promise<Response> {
    return fetch(url, {
      ...options,
      headers: {
        ...defaultHeaders,
        ...options?.headers,
      },
    });
  }

  let response = await makeRequest();

  if (!response.ok) {
    if (response.status === 401) {
      const refreshResponse = await authService.refreshToken();
      token = refreshResponse.accessToken;
      localStorage.setItem("access-token", token);

      response = await makeRequest();
    } else {
      const errorText = await response.text();
      throw new Error(errorText);
    }
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
