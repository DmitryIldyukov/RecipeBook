import { User, UserUpdateDto } from "../types/user";
import { fetchClient } from "./fetchClient";

export default class UserService {
  async getUser(userId: number): Promise<User> {
    return fetchClient<User>(`/api/User/${userId.toString()}`);
  }

  async saveUser(userId: number, user: UserUpdateDto): Promise<Response> {
    return fetchClient(`/api/User/${userId.toString()}`, {
      method: "PUT",
      body: JSON.stringify(user),
    });
  }
}
