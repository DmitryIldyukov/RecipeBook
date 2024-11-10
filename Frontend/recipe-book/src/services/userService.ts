import { User } from "../types/user";
import { fetchClient } from "./fetchClient";

export default class UserService {
  async getUser(userId: number): Promise<User> {
    return fetchClient<User>(`/api/User/${userId.toString()}`);
  }
}
