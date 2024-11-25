import { useAppStore } from "../hooks/useStore";
import { authService } from "./authService";

let isRefreshing = false;
let refreshPromise: Promise<void> | null = null;

export const fetchClient = async <T>(url: string, options?: RequestInit): Promise<T> => {
  const { logout } = useAppStore.getState();
  const isFormData = options?.body instanceof FormData;
  let token = localStorage.getItem("access-token");

  const makeRequest = async (): Promise<Response> => {
    const defaultHeaders: HeadersInit = {
      ...(token && { Authorization: `Bearer ${token}` }),
      ...(!isFormData && { "Content-Type": "application/json" }),
    };

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
      if (!isRefreshing) {
        isRefreshing = true;
        refreshPromise = authService.refreshToken()
          .then(newTokenInfo => {
            token = newTokenInfo.accessToken;
          })
          .catch(() => {
            logout();
          })
          .finally(() => {
            isRefreshing = false;
            refreshPromise = null;
          });
        await refreshPromise;
      }
      else if (refreshPromise) {
        await refreshPromise;
      }

      token = localStorage.getItem("access-token");
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
