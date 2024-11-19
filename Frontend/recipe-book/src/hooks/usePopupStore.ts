import { create } from "zustand";

type PopupState = {
  isRegistrationPopupOpen: boolean;
  isLoginPopupOpen: boolean;
  isLoginOrRegistraionPopupOpen: boolean;

  setIsLoginOrRegistraionPopupOpen: (isOpen: boolean) => void;
  setIsRegistrationPopupOpen: (isOpen: boolean) => void;
  setIsLoginPopupOpen: (isOpen: boolean) => void;
};

export const usePopupStore = create<PopupState>((set) => ({
  isRegistrationPopupOpen: false,
  isLoginPopupOpen: false,
  isLoginOrRegistraionPopupOpen: false,

  setIsLoginOrRegistraionPopupOpen: (isOpen: boolean) => {
    set({ isLoginOrRegistraionPopupOpen: isOpen });
  },

  setIsRegistrationPopupOpen: (isOpen: boolean) => {
    set({ isRegistrationPopupOpen: isOpen });
  },

  setIsLoginPopupOpen: (isOpen: boolean) => {
    set({ isLoginPopupOpen: isOpen });
  },
}));
