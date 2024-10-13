import { create } from "zustand";

type ApplicationStore = {
  isLogged: boolean;

  login: () => void;
  logout: () => void;
};

export const useAppStore = create<ApplicationStore>((set) => {
  const isLogged = false;

  const login = () => {
    set({ isLogged: true });
  };

  const logout = () => {
    set({ isLogged: false });
  };

  return {
    isLogged,
    login,
    logout,
  };
});
