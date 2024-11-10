import { create } from "zustand";

type ApplicationStore = {
  isLogged: boolean;
  userId: number | null;

  login: (userId: number | null) => void;
  logout: () => void;
};

export const useAppStore = create<ApplicationStore>((set) => ({
  isLogged: false,
  userId: null,

  login: (userId: number | null) => {
    set({ isLogged: true, userId: userId });
  },

  logout: () => {
    set({ isLogged: false, userId: null });
  },
}));
