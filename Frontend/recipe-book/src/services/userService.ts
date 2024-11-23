import { User, UserUpdateDto } from "../types/user";
import { fetchClient } from "./fetchClient";

class UserService {
  async getUser(): Promise<User> {
    return fetchClient<User>(`/api/users/me`);
  }

  async saveUser(user: UserUpdateDto): Promise<Response> {
    return fetchClient(`/api/users/me`, {
      method: "PUT",
      body: JSON.stringify(user),
    });
  }
}

export const userService = new UserService();
