import { Page } from "../types/page";
import { Recipe, RecipeListResponse } from "../types/recipe";
import { fetchClient } from "./fetchClient";

class RecipeService {
  async getRecipeOfDay(): Promise<Recipe> {
    return fetchClient<Recipe>("/api/recipes/daily");
  }

  async getRecipeList(searchQueries: string[], page: Page): Promise<RecipeListResponse> {
    const queryString = new URLSearchParams();
    queryString.append("pageNumber", page.pageNumber.toString());
    queryString.append("pageSize", page.pageSize.toString());
    searchQueries.forEach((query) => {
      queryString.append("searchQueries", query);
    });

    const url = `/api/recipes/search?${queryString.toString()}`;

    return fetchClient<RecipeListResponse>(url, {
      method: "GET",
    });
  }

  async getFavoriteRecipes(userId: number, page: Page): Promise<RecipeListResponse> {
    const queryString = new URLSearchParams();
    queryString.append("pageNumber", page.pageNumber.toString());
    queryString.append("pageSize", page.pageSize.toString());

    const url = `/api/users/${userId.toString()}/favorites?${queryString.toString()}`;

    return fetchClient<RecipeListResponse>(url, {
      method: "GET",
    });
  }

  async getRecipeById(recipeId: number): Promise<Recipe> {
    return fetchClient<Recipe>(`/api/recipes/${recipeId.toString()}`);
  }

  async getRecipeImage(recipeId: number): Promise<string> {
    return fetchClient<string>(`/api/recipes/${recipeId.toString()}/images`);
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

  async getUserRecipes(userId: number): Promise<Recipe[]> {
    return fetchClient<Recipe[]>(`/api/users/${userId.toString()}/recipes`);
  }

  async deleteRecipe(recipeId: number): Promise<Response> {
    return fetchClient(`/api/recipes/${recipeId.toString()}`, {
      method: "DELETE",
    });
  }
}

export const recipeService = new RecipeService();