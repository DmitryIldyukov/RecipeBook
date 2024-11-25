import { create } from "zustand";
import { LoginInfo } from "../types/auth";
import { authService } from "../services/authService";
import { getUserIdFromToken } from "../utils/tokenDecoder";
import { handleError } from "../utils/errorHandler";

type ApplicationStore = {
  accessToken: string | null;
  userId: number | null;
  isAuth: boolean;

  login: (loginData: LoginInfo) => Promise<void>;
  logout: () => void;
};

export const useAppStore = create<ApplicationStore>((set) => {
  const accessToken = localStorage.getItem("access-token") ?? null;
  const userId = accessToken ? getUserIdFromToken(accessToken) : null;
  const isAuth = !!accessToken && !!userId;

  const login = async (loginData: LoginInfo) => {
    try {
      const tokenInfo = await authService.login(loginData);
      const userId = getUserIdFromToken(tokenInfo.accessToken);
      if (!userId) {
        throw new Error("Id пользователя не найден.");
      }

      localStorage.setItem("access-token", tokenInfo.accessToken);

      set({
        userId: userId,
        isAuth: true,
      });
    } catch (error) {
      handleError(error, "Произошла ошибка при авторизации.");
      set({ isAuth: false });
    }
  };

  const logout = () => {
    localStorage.removeItem("access-token");

    set({
      accessToken: null,
      userId: null,
      isAuth: false,
    });
  };

  return {
    accessToken,
    userId,
    isAuth,
    login,
    logout,
  };
});
