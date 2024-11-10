import { Recipe } from "../../../types/recipe";
import styles from "./recipeCard.module.scss";
import emptyLikeIcon from "../../../assets/emptyLikeIcon.svg";
import emptyFavoriteIcon from "../../../assets/emptyFavoriteIcon.svg";
import likeIcon from "../../../assets/like.svg";
import favoriteIcon from "../../../assets/favorite.svg";
import portionIcon from "../../../assets/portionIcon.svg";
import cookTimeIcon from "../../../assets/cookTimeIcon.svg";
import { recipeService } from "../../../services/recipeService";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAppStore } from "../../../hooks/useStore";
import { usePopupStore } from "../../../hooks/usePopupStore";

type RecipeCardProps = {
  recipe: Recipe;
};

export const RecipeCard = ({ recipe }: RecipeCardProps) => {
  const { userId } = useAppStore();
  const { setIsLoginPopupOpen } = usePopupStore();

  const navigate = useNavigate();
  const [image, setImage] = useState<string>("");
  const [loadingImage, setLoadingImage] = useState<boolean>(true);
  const [likesCount, setLikesCount] = useState<number>(recipe.likesCount);
  const [isLiked, setIsLiked] = useState<boolean>(recipe.isLiked);
  const [favoritesCount, setFavoritesCount] = useState<number>(recipe.favoritesCount);
  const [isFavorite, setIsFavorite] = useState<boolean>(recipe.isFavorite);

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

  const addLike = async (recipeId: number) => {
    if (!userId) {
      setIsLoginPopupOpen(true);
      return;
    }

    try {
      if (userId) {
        if (isLiked) {
          await recipeService.removeLike(userId, recipeId);
          setLikesCount((prev) => prev - 1);
          setIsLiked(false);
        } else {
          await recipeService.addLike(userId, recipeId);
          setLikesCount((prev) => prev + 1);
          setIsLiked(true);
        }
      }
    } catch (error) {
      console.error("Ошибка добавления лайка:", error);
    }
  };

  const addFavorite = async (recipeId: number) => {
    if (!userId) {
      setIsLoginPopupOpen(true);
      return;
    }

    try {
      if (userId) {
        if (isFavorite) {
          await recipeService.removeFavorite(userId, recipeId);
          setFavoritesCount((prev) => prev - 1);
          setIsFavorite(false);
        } else {
          await recipeService.addFavorite(userId, recipeId);
          setFavoritesCount((prev) => prev + 1);
          setIsFavorite(true);
        }
      }
    } catch (error) {
      console.error("Ошибка добавления в избранное:", error);
    }
  };

  const recipeHandler = () => {
    navigate(`/recipe/${recipe.recipeId.toString()}`);
  };

  useEffect(() => {
    void getRecipeImage(recipe.recipeId);
  }, [recipe.recipeId]);

  return (
    <div className={styles.container} onClick={recipeHandler}>
      <div className={styles.imgBlock}>
        <span className={styles.login}>{recipe.login}</span>
        {loadingImage ? (
          <p>Загрузка изображения...</p>
        ) : (
          <img src={image} alt="recipe image" className={styles.image} />
        )}
      </div>
      <div className={styles.recipeInfo}>
        <div className={styles.popularInfo}>
          <div className={styles.tagsBox}>
            {recipe.tags.map((tag) => (
              <div className={styles.tag} key={tag.id}>
                <p className={styles.tagName}>{tag.name}</p>
              </div>
            ))}
          </div>
          <div className={styles.actionBtnBox}>
            <button
              onClick={(e) => {
                e.stopPropagation();
                void addFavorite(recipe.recipeId);
              }}
              className={styles.actionBtn}
            >
              <img src={isFavorite ? favoriteIcon : emptyFavoriteIcon} alt="favorite icon" />
              <span>{favoritesCount}</span>
            </button>
            <button
              onClick={(e) => {
                e.stopPropagation();
                void addLike(recipe.recipeId);
              }}
              className={styles.actionBtn}
            >
              <img src={isLiked ? likeIcon : emptyLikeIcon} alt="like icon" />
              <span>{likesCount}</span>
            </button>
          </div>
        </div>

        <div className={styles.recipeMainInfo}>
          <h3 className={styles.recipeName}>{recipe.name}</h3>
          <p className={styles.recipeDescription}>{recipe.description}</p>
        </div>

        <div className={styles.recipeSecondaryInfo}>
          <div className={styles.secInfoBox}>
            <img src={cookTimeIcon} alt="cook time" />
            <div className={styles.secInfoText}>
              <p className={styles.secInfoTitle}>Время приготовления:</p>
              <p className={styles.secInfoData}>{recipe.cookTime} мин</p>
            </div>
          </div>
          <div className={styles.secInfoBox}>
            <img src={portionIcon} alt="portion" />
            <div className={styles.secInfoText}>
              <p className={styles.secInfoTitle}>Рецепт на:</p>
              <p className={styles.secInfoData}>{recipe.portionCount} персон</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
