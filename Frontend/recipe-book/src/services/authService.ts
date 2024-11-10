import { LoginInfo, RegistrationInfo } from "../types/auth";
import { fetchClient } from "./fetchClient";

export default class AuthService {
  async Registration(data: RegistrationInfo): Promise<Response> {
    return await fetchClient("/api/User/Registration", {
      method: "POST",
      body: JSON.stringify(data),
    });
  }

  async Login(data: LoginInfo): Promise<number> {
    return await fetchClient("/api/User/Login", {
      method: "POST",
      body: JSON.stringify(data),
    });
  }
}
