import { useEffect, useState } from "react";
import { Recipe } from "../../../types/recipe";
import styles from "./recipeOfDay.module.scss";
import image from "../../../assets/image.jpg";
import icon from "../../../assets/yummy.svg";
import likeIcon from "../../../assets/like.svg";
import timeIcon from "../../../assets/time.svg";
import RecipeService from "../../../services/recipeService";

export const RecipeOfDay = () => {
  const [recipe, setRecipe] = useState<Recipe | null>(null);
  const recipeService = new RecipeService();

  useEffect(() => {
    const fetchData = () => {
      try {
        const data = recipeService.getRecipeOfDay();
        setRecipe(data);
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className={styles.container}>
      <div className={styles.imgBlock}>
        <span className={styles.login}>{recipe?.authorLogin}</span>
        <img src={image} alt="recipe image" className={styles.image} />
      </div>
      <div className={styles.recipeInfo}>
        <div className={`${styles.row} ${styles.popularInfo}`}>
          <div className={`${styles.row} ${styles.gap7}`}>
            <img src={likeIcon} alt="like icon" />
            <p>{recipe?.likesCount}</p>
          </div>
          <div className={`${styles.row} ${styles.gap7}`}>
            <img src={timeIcon} alt="time icon" />
            <p className={styles.row}>{recipe?.cookTime} минут</p>
          </div>
        </div>
        <img src={icon} alt="recipe of day" width={100} height={95} className={styles.recipeOfDayIcon} />
        <div className={styles.recipeMainInfo}>
          <h3 className={styles.recipeName}>{recipe?.name}</h3>
          <p className={styles.recipeDescription}>{recipe?.description}</p>
        </div>
      </div>
    </div>
  );
};
