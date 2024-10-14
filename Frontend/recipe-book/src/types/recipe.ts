export type Recipe = {
  recipeId: number;
  authorId: number;
  authorLogin: string;
  name: string;
  description: string;
  tags: string[];
  favoritesCount: number;
  likesCount: number;
  cookTime: number;
  portionCount: number;
};
