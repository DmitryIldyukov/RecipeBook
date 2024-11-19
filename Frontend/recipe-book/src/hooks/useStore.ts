import { create } from "zustand";
import { ROUTES } from "../constants/constants";

type ApplicationStore = {
  userId: number | null;

  login: (userId: number | null) => void;
  logout: () => void;
};

export const useAppStore = create<ApplicationStore>((set) => ({
  userId: localStorage.getItem("userId") ? Number(localStorage.getItem("userId")) : null,

  login: (userId: number | null) => {
    set({ userId: userId });
    if (userId !== null) {
      localStorage.setItem("userId", userId.toString());
    }
  },

  logout: () => {
    set({ userId: null });
    localStorage.removeItem("userId");
  },
}));
