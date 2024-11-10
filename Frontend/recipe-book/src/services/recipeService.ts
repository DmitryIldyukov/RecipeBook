import { Page } from "../types/page";
import { Recipe } from "../types/recipe";
import { fetchClient } from "./fetchClient";

class RecipeService {
  async getRecipeOfDay(): Promise<Recipe> {
    return fetchClient<Recipe>("/api/Recipe/DailyRecipe");
  }

  async getRecipeList(searchString: string, page: Page, userId?: number): Promise<Recipe[]> {
    const params = userId ? `?userId=${userId.toString()}` : "";

    return fetchClient<Recipe[]>(`/api/Recipe/GetRecipes${params}`, {
      method: "POST",
      body: JSON.stringify({
        searchString,
        page,
      }),
    });
  }

  async getFavoriteRecipes(userId: number, page: Page): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/Recipe/FavoriteRecipes/${userId.toString()}`, {
      method: "POST",
      body: JSON.stringify({
        page,
      }),
    });
  }

  async getRecipeById(recipeId: number, userId?: number): Promise<Recipe> {
    const params = userId ? `?userId=${userId.toString()}` : "";
    return fetchClient<Recipe>(`/api/Recipe/${recipeId.toString()}${params}`);
  }

  async getRecipeImage(recipeId: number): Promise<string> {
    return fetchClient<string>(`/api/Recipe/RecipeImage/${recipeId.toString()}`);
  }

  async addLike(userId: number, recipeId: number): Promise<Response> {
    return fetchClient(`/api/Like/${userId.toString()}/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async addFavorite(userId: number, recipeId: number): Promise<Response> {
    return fetchClient(`/api/Favorite/${userId.toString()}/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async removeLike(userId: number, recipeId: number): Promise<Response> {
    return fetchClient(`/api/Like/${userId.toString()}/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

  async removeFavorite(userId: number, recipeId: number): Promise<Response> {
    return fetchClient(`/api/Favorite/${userId.toString()}/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

  async createRecipe(recipe: FormData): Promise<Response> {
    return fetchClient("/api/Recipe", {
      method: "POST",
      body: recipe,
    });
  }

  async updateRecipe(recipeId: number, recipe: FormData): Promise<Response> {
    return fetchClient(`/api/Recipe/${recipeId.toString()}`, {
      method: "PUT",
      body: recipe,
    });
  }

  async getUserRecipes(userId: number): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/Recipe/User/${userId.toString()}`);
  }

  async deleteRecipe(recipeId: number): Promise<Response> {
    return fetchClient(`/api/Recipe/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }
}

export const recipeService = new RecipeService();
