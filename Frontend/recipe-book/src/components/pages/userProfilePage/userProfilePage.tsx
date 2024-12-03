import { useNavigate } from "react-router-dom";
import { useAppStore } from "../../../hooks/useStore";
import styles from "./userProfilePage.module.scss";
import { UserProfileHeader } from "./userProfileHeader/userProfileHeader";
import { UserProfileData } from "./userProfileData/userProfileData";
import { useEffect, useState } from "react";
import { ROUTES } from "../../../constants/constants";
import { UserProfileCountInfoSection } from "./userProfileCountInfoSection/userProfileCountInfoSection";
import { User } from "../../../types/user";
import { UserRecipesList } from "./userRecipesList/userRecipesList";
import { userService } from "../../../services/userService";
import { handleError } from "../../../utils/errorHandler";

export const UserProfilePage = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);

  const { isAuth, userId } = useAppStore();

  const getUserInfo = () => {
    if (!userId) return;
    userService
      .getUser(userId)
      .then((data) => {
        setUser(data);
      })
      .catch((error: unknown) => {
        handleError(error, "Неудалось получить данные пользователя.");
      });
  };

  useEffect(() => {
    if (!isAuth) {
      navigate(ROUTES.HOME);
      return;
    }

    getUserInfo();
  }, [isAuth]);

  return (
    <div className={styles.container}>
      <UserProfileHeader />
      <UserProfileData user={user} />
      <UserProfileCountInfoSection
        likesCount={user?.likesCount}
        recipesCount={user?.recipesCount}
        favoritesCount={user?.favoritesCount}
      />
      <UserRecipesList />
    </div>
  );
};
