import { useEffect, useState } from "react";
import { Recipe } from "../../types/recipe";
import { RecipeCard } from "../recipesPage/recipeCard/recipeCard";
import { useAppStore } from "../../hooks/useStore";
import RecipeService from "../../services/recipeService";
import styles from "./favoriteList.module.scss";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../constants/constants";
import { usePopupStore } from "../../hooks/usePopupStore";

export const FavoriteList = () => {
  const defaultPageSize = 4;
  const recipeService = new RecipeService();

  const { userId } = useAppStore();
  const { setIsLoginPopupOpen } = usePopupStore();

  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [pageNumber, setPageNumber] = useState(1);
  const navigate = useNavigate();

  const navigateToLogin = () => {
    navigate(ROUTES.HOME);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (!userId) {
          navigateToLogin();
          setIsLoginPopupOpen(true);
          return;
        }
        const data = await recipeService.getFavoriteRecipes(userId, { pageNumber, pageSize: defaultPageSize });
        setRecipes(data);
      } catch (error) {
        console.error(error);
      }
    };

    void fetchData();
  }, [userId]);

  return (
    <div className={styles.container}>
      <h3 className={styles.header}>Избранное</h3>
      <ul className={styles.recipeList}>
        {recipes.length > 0 ? (
          recipes.map((recipe) => (
            <li key={recipe.recipeId} className={styles.recipeCard}>
              <RecipeCard recipe={recipe} />
            </li>
          ))
        ) : (
          <p className={styles.notFoundText}>Ваш список пуст</p>
        )}
      </ul>
    </div>
  );
};
