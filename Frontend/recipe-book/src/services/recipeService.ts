import { Page } from "../types/page";
import { Recipe } from "../types/recipe";
import { fetchClient } from "./fetchClient";

class RecipeService {
  async getRecipeOfDay(): Promise<Recipe> {
    return fetchClient<Recipe>("/api/recipes/daily");
  }

  async getRecipeList(searchQueries: string[], page: Page): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/recipes/filter`, {
      method: "POST",
      body: JSON.stringify({
        searchQueries,
        page,
      }),
    });
  }

  async getFavoriteRecipes(page: Page): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/recipes/favorite`, {
      method: "POST",
      body: JSON.stringify({
        page,
      }),
    });
  }

  async getRecipeById(recipeId: number): Promise<Recipe> {
    return fetchClient<Recipe>(`/api/recipes/${recipeId.toString()}`);
  }

  async getRecipeImage(recipeId: number): Promise<string> {
    return fetchClient<string>(`/api/recipes/${recipeId.toString()}/image`);
  }

  async addLike(recipeId: number): Promise<Response> {
    return fetchClient(`/api/likes/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async addFavorite(recipeId: number): Promise<Response> {
    return fetchClient(`/api/favorites/${recipeId.toString()}`, {
      method: "POST",
    });
  }

  async removeLike(recipeId: number): Promise<Response> {
    return fetchClient(`/api/likes/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

  async removeFavorite(recipeId: number): Promise<Response> {
    return fetchClient(`/api/favorites/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }

  async createRecipe(recipe: FormData): Promise<Response> {
    return fetchClient("/api/recipes", {
      method: "POST",
      body: recipe,
    });
  }

  async updateRecipe(recipeId: number, recipe: FormData): Promise<Response> {
    return fetchClient(`/api/recipes/${recipeId.toString()}`, {
      method: "PUT",
      body: recipe,
    });
  }

  async getUserRecipes(): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/recipes/my`);
  }

  async deleteRecipe(recipeId: number): Promise<Response> {
    return fetchClient(`/api/recipes/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }
}

export const recipeService = new RecipeService();
