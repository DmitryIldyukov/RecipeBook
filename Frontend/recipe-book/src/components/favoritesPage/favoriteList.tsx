import { useEffect, useState } from "react";
import { Recipe } from "../../types/recipe";
import { RecipeCard } from "../customComponents/recipeCard/recipeCard";
import { useAppStore } from "../../hooks/useStore";
import { recipeService } from "../../services/recipeService";
import styles from "./favoriteList.module.scss";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../constants/constants";
import { usePopupStore } from "../../hooks/usePopupStore";
import MyButton from "../customComponents/myButton/myButton";
import { handleError } from "../../utils/errorHandler";

export const FavoriteList = () => {
  const defaultPageSize = 4;

  const { userId } = useAppStore();
  const { setIsLoginPopupOpen } = usePopupStore();

  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [isCanLoadMore, setIsCanLoadMore] = useState<boolean>(false);
  const [pageNumber, setPageNumber] = useState(1);
  const navigate = useNavigate();

  const navigateToLogin = () => {
    navigate(ROUTES.HOME);
  };

  const getFavoriteRecipes = (page: { pageNumber: number; pageSize: number }) => {
    if (!userId) {
      navigateToLogin();
      setIsLoginPopupOpen(true);
      return;
    }

    recipeService
      .getFavoriteRecipes(page)
      .then((response) => {
        if (page.pageNumber === 1) {
          setRecipes(response);
        } else {
          setRecipes((prevRecipes) => [...prevRecipes, ...response]);
        }
        setIsCanLoadMore(response.length === defaultPageSize);
      })
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при загрузке избранных рецептов");
      });
  };

  useEffect(() => {
    setPageNumber(1);
    getFavoriteRecipes({ pageNumber, pageSize: defaultPageSize });
  }, [userId]);

  const handleLoadMore = () => {
    const nextPageNumber = pageNumber + 1;
    setPageNumber(nextPageNumber);
    getFavoriteRecipes({ pageNumber: nextPageNumber, pageSize: defaultPageSize });
  };

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

      {isCanLoadMore && (
        <div className={styles.loadBtn}>
          <MyButton onClick={handleLoadMore} isPrimary={false} width="309px" height="60px">
            Загрузить еще
          </MyButton>
        </div>
      )}
    </div>
  );
};
