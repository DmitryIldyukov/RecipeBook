import { Recipe } from "../types/recipe";

export default class RecipeService {
  getRecipeOfDay(): Recipe {
    return {
      recipeId: 1,
      authorId: 1,
      authorLogin: "@glazest",
      name: "Тыквенный Cупчик На Кокосовом Молоке",
      description:
        "Если у вас осталась тыква, и вы не знаете что с ней сделать, то это решение для вас! Ароматный, согревающий суп-пюре на кокосовом молоке. Можно даже в Пост! ",
      tags: ["супы, диета"],
      favoritesCount: 45,
      likesCount: 356,
      cookTime: 35,
      portionCount: 4,
    };
  }
}
