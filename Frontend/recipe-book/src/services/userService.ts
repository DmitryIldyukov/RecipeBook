import { User, UserUpdateDto } from "../types/user";
import { fetchClient } from "./fetchClient";

class UserService {
  async getUser(userId: number): Promise<User> {
    return fetchClient<User>(`/api/users/${userId.toString()}`);
  }

  async saveUser(userId: number, user: UserUpdateDto): Promise<Response> {
    return fetchClient(`/api/users/${userId.toString()}`, {
      method: "PUT",
      body: JSON.stringify(user),
    });
  }
}

export const userService = new UserService();
