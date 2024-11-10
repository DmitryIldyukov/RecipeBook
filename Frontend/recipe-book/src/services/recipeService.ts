import { Page } from "../types/page";
import { Recipe } from "../types/recipe";
import { fetchClient } from "./fetchClient";

export default class RecipeService {
  async getRecipeOfDay(): Promise<Recipe> {
    return fetchClient<Recipe>("/api/Recipe/DailyRecipe");
  }

  async getRecipeImage(recipeId: number): Promise<string> {
    return fetchClient<string>(`/api/Recipe/RecipeImage/${recipeId.toString()}`);
  }

  async addLike(userId: number, recipeId: number): Promise<void> {
    return fetchClient(`/api/Like/${userId.toString()}/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async addFavorite(userId: number, recipeId: number): Promise<void> {
    return fetchClient(`/api/Favorite/${userId.toString()}/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async removeLike(userId: number, recipeId: number): Promise<void> {
    return fetchClient(`/api/Like/${userId.toString()}/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

  async removeFavorite(userId: number, recipeId: number): Promise<void> {
    return fetchClient(`/api/Favorite/${userId.toString()}/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

}
