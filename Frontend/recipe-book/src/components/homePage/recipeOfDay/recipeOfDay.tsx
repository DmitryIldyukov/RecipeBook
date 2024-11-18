import { useEffect, useState } from "react";
import { Recipe } from "../../../types/recipe";
import styles from "./recipeOfDay.module.scss";
import icon from "../../../assets/yummy.svg";
import likeIcon from "../../../assets/like.svg";
import emptyLikeIcon from "../../../assets/emptyLikeIcon.svg";
import timeIcon from "../../../assets/time.svg";
import { recipeService } from "../../../services/recipeService";
import { useAppStore } from "../../../hooks/useStore";
import { usePopupStore } from "../../../hooks/usePopupStore";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../constants/constants";
import { handleError } from "../../../utils/errorHandler";

export const RecipeOfDay = () => {
  const [recipe, setRecipe] = useState<Recipe | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [image, setImage] = useState<string>("");
  const [loadingImage, setLoadingImage] = useState<boolean>(true);

  const { userId } = useAppStore();
  const { setIsLoginPopupOpen } = usePopupStore();

  const navigate = useNavigate();

  const getRecipeOfDay = async () => {
    setLoading(true);
    try {
      const data = await recipeService.getRecipeOfDay();
      setRecipe(data);
    } catch (error) {
      handleError(error, "Произошла ошибка при загрузке рецепта дня");
    } finally {
      setLoading(false);
    }
  };

  const getRecipeImage = async (recipeId: number) => {
    try {
      const response = await recipeService.getRecipeImage(recipeId);
      setImage(response);
    } catch (error) {
      console.error("Ошибка загрузки изображения рецепта:", error);
    } finally {
      setLoadingImage(false);
    }
  };

  const addLike = async () => {
    try {
      if (recipe && userId) {
        await recipeService.addLike(userId, recipe.recipeId);
        void getRecipeOfDay();
      }
    } catch (error) {
      handleError(error, "Произошла ошибка при добавлении рецепта в понравившиеся.");
    }
  };

  const setLike = (event: React.MouseEvent) => {
    event.stopPropagation();

    if (!userId) {
      setIsLoginPopupOpen(true);
      return;
    }

    void addLike();
  };

  const navigateToRecipeInfo = () => {
    if (recipe) {
      navigate(`${ROUTES.RECIPE_INFO}/${recipe.recipeId.toString()}`);
    }
  };

  useEffect(() => {
    void getRecipeOfDay();
  }, []);

  useEffect(() => {
    if (recipe) {
      void getRecipeImage(recipe.recipeId);
    }
  }, [recipe]);

  if (loading) {
    return <div className={styles.loader}>Загрузка...</div>;
  }

  return recipe ? (
    <div onClick={navigateToRecipeInfo} className={styles.container}>
      <div className={styles.imgBlock}>
        <span className={styles.login}>{recipe.login}</span>
        {loadingImage ? (
          <div className={styles.loader}>Загрузка изображения...</div>
        ) : (
          <img src={image} alt="recipe image" className={styles.image} />
        )}
      </div>
      <div className={styles.recipeInfo}>
        <div className={`${styles.row} ${styles.popularInfo}`}>
          <button onClick={setLike} className={`${styles.row} ${styles.gap7}`}>
            <img src={recipe.isLiked ? likeIcon : emptyLikeIcon} alt="like icon" />
            <p className={styles.text}>{recipe.likesCount}</p>
          </button>
          <div className={`${styles.row} ${styles.gap7}`}>
            <img src={timeIcon} alt="time icon" />
            <p className={styles.text}>{recipe.cookTime} минут</p>
          </div>
        </div>
        <img src={icon} alt="recipe of day" width={100} height={95} className={styles.recipeOfDayIcon} />
        <div className={styles.recipeMainInfo}>
          <h3 className={styles.recipeName}>{recipe.name}</h3>
          <p className={styles.recipeDescription}>{recipe.description}</p>
        </div>
      </div>
    </div>
  ) : (
    <p className={styles.loader}>Рецепт дня не найден</p>
  );
};
