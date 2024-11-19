export type Recipe = {
  recipeId: number;
  authorId: number;
  login: string;
  name: string;
  description: string;
  tags: Tag[];
  steps: Step[];
  ingredients: Ingredient[];
  favoritesCount: number;
  isFavorite: boolean;
  likesCount: number;
  isLiked: boolean;
  cookTime: number;
  portionCount: number;
  imageName: string;
};

export type Tag = {
  id: number | undefined;
  name: string;
};

export type Step = {
  id: number | undefined;
  description: string;
};

export type Ingredient = {
  id: number | undefined;
  title: string;
  description: string;
};
