import { LoginInfo, RegistrationInfo } from "../types/auth";
import { fetchClient } from "./fetchClient";

class AuthService {
  async Registration(data: RegistrationInfo): Promise<Response> {
    return fetchClient("/api/User/Registration", {
      method: "POST",
      body: JSON.stringify(data),
    });
  }

  async login(data: LoginInfo): Promise<number> {
    return fetchClient("/api/User/Login", {
      method: "POST",
      body: JSON.stringify(data),
    });
  }
}

export const authService = new AuthService();
