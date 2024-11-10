import styles from "./recipeFullInfo.module.scss";
import { Recipe } from "../../../types/recipe";
import { useEffect, useState } from "react";
import { RecipeCard } from "../recipeCard/recipeCard";
import arrow from "../../../assets/arrow.svg";
import RecipeService from "../../../services/recipeService";
import { RecipeInfoHeader } from "./RecipeInfoHeader/RecipeInfoHeader";
import { IngredientPart } from "./ingredientPart/ingredientPart";
import { StepPart } from "./stepPart/stepPart";
import { useParams } from "react-router-dom";

export const RecipeFullInfo = () => {
  const recipeService = new RecipeService();

  const { recipeId } = useParams();
  const [recipe, setRecipe] = useState<Recipe>();

  const getRecipeById = async (recipeId: number) => {
    try {
      const data = await recipeService.getRecipeById(recipeId);
      setRecipe(data);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    if (recipeId) {
      console.log("recipeId: ", recipeId);
      void getRecipeById(parseInt(recipeId));
    }
  }, [recipeId]);

  return (
    <div className={styles.container}>
      <button className={styles.backBtn}>
        <img src={arrow} alt="back" />
        Назад
      </button>
      <RecipeInfoHeader recipe={recipe} />
      {recipe ? (
        <div className={styles.RecipeInfoBlock}>
          <RecipeCard recipe={recipe} />
          <div className={styles.recipeSecondaryInfoBlock}>
            <IngredientPart ingredients={recipe.ingredients} />
            <StepPart steps={recipe.steps} />
          </div>
        </div>
      ) : (
        <p className={styles.notFoundText}>Рецепт не найден...</p>
      )}
    </div>
  );
};
