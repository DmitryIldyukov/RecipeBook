import { LoginInfo, RegistrationInfo, TokenInfo } from "../types/auth";
import { fetchClient } from "./fetchClient";

class AuthService {
  async Registration(data: RegistrationInfo): Promise<Response> {
    return fetchClient("/api/User/Registration", {
      method: "POST",
      body: JSON.stringify(data),
    });
  }

  async login(data: LoginInfo): Promise<TokenInfo> {
    const response: TokenInfo = await fetchClient<TokenInfo>("/api/user/login", {
      method: "POST",
      body: JSON.stringify(data),
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.accessToken) {
      localStorage.removeItem("access-token");
      localStorage.setItem("access-token", response.accessToken);
    }

    return response;
  }

  async refreshToken(): Promise<TokenInfo> {
    const response: TokenInfo = await fetchClient<TokenInfo>("/api/user/refresh", {
      method: "Get",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.accessToken) {
      localStorage.removeItem("access-token");
      localStorage.setItem("access-token", response.accessToken);
    }

    return response;
  }
}

export const authService = new AuthService();
