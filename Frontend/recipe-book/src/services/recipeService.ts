import { Page } from "../types/page";
import { Recipe } from "../types/recipe";
import { fetchClient } from "./fetchClient";

class RecipeService {
  async getRecipeOfDay(): Promise<Recipe> {
    return fetchClient<Recipe>("/api/Recipe/DailyRecipe");
  }

  async getRecipeList(searchQueries: string[], page: Page): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/Recipe/GetRecipes`, {
      method: "POST",
      body: JSON.stringify({
        searchQueries,
        page,
      }),
    });
  }

  async getFavoriteRecipes(page: Page): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/Recipe/FavoriteRecipes`, {
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

  async addLike(recipeId: number): Promise<Response> {
    return fetchClient(`/api/Like/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async addFavorite(recipeId: number): Promise<Response> {
    return fetchClient(`/api/Favorite/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async removeLike(recipeId: number): Promise<Response> {
    return fetchClient(`/api/Like/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

  async removeFavorite(recipeId: number): Promise<Response> {
    return fetchClient(`/api/Favorite/${recipeId.toString()}`, {
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

  async getUserRecipes(): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/Recipe/MyRecipes`);
  }

  async deleteRecipe(recipeId: number): Promise<Response> {
    return fetchClient(`/api/Recipe/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }
}

export const recipeService = new RecipeService();
