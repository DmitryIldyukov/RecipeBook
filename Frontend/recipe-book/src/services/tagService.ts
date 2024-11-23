import { Tag } from "../types/recipe";
import { fetchClient } from "./fetchClient";

export default class TagService {
  async getTags(): Promise<Tag[]> {
    return fetchClient(`/api/tags`);
  }

  async getPopularTags(count: number): Promise<Tag[]> {
    return fetchClient(`/api/tags/popular?count=${count.toString()}`);
  }
}
