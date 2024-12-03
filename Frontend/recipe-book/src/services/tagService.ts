import { Tag } from "../types/recipe";
import { fetchClient } from "./fetchClient";

export default class TagService {
  async getTags(): Promise<Tag[]> {
    return fetchClient(`/api/Tag`);
  }

  async getPopularTags(count: number): Promise<Tag[]> {
    return fetchClient(`/api/Tag/GetPopularTags?count=${count.toString()}`);
  }
}
