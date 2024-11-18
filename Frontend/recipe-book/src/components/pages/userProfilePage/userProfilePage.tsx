import { useNavigate } from "react-router-dom";
import { useAppStore } from "../../../hooks/useStore";
import styles from "./userProfilePage.module.scss";
import { UserProfileHeader } from "./userProfileHeader/userProfileHeader";
import { UserProfileData } from "./userProfileData/userProfileData";
import { useEffect, useState } from "react";
import { ROUTES } from "../../../constants/constants";
import { UserProfileCountInfoSection } from "./userProfileCountInfoSection/userProfileCountInfoSection";
import { User } from "../../../types/user";
import UserService from "../../../services/userService";
import { UserRecipesList } from "./userRecipesList/userRecipesList";

export const UserProfilePage = () => {
  const userServie = new UserService();

  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);

  const { userId } = useAppStore();

  const getUserInfo = async (userId: number) => {
    try {
      const data = await userServie.getUser(userId);
      setUser(data);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    if (!userId) {
      navigate(ROUTES.HOME);
      return;
    }

    void getUserInfo(userId);
  }, [userId]);

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
