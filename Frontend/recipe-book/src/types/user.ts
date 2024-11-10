export type User = {
  id: number;
  name: string;
  login: string;
  information: string;
  recipesCount: number;
  likesCount: number;
  favoritesCount: number;
};

export type UserUpdateDto = {
  name: string;
  login: string;
  password: string;
  information: string;
};
