import styles from "./recipeFullInfo.module.scss";
import { Recipe } from "../../../../types/recipe";
import { useEffect, useState } from "react";
import { RecipeCard } from "../../../customComponents/recipeCard/recipeCard";
import { recipeService } from "../../../../services/recipeService";
import { IngredientPart } from "./ingredientPart/ingredientPart";
import { StepPart } from "./stepPart/stepPart";
import { useParams } from "react-router-dom";
import { RecipeInfoHeader } from "./recipeInfoHeader/recipeInfoHeader";
import { useAppStore } from "../../../../hooks/useStore";
import { BackBtn } from "../../../customComponents/backBtn/backBtn";
import { handleError } from "../../../../utils/errorHandler";

export const RecipeFullInfo = () => {
  const { recipeId } = useParams();
  const [recipe, setRecipe] = useState<Recipe>();

  const { userId } = useAppStore();

  const getRecipeById = async (recipeId: number) => {
    try {
      const data = await recipeService.getRecipeById(recipeId, userId ? userId : undefined);
      setRecipe(data);
    } catch (error) {
      handleError(error);
    }
  };

  useEffect(() => {
    if (recipeId) {
      void getRecipeById(parseInt(recipeId));
    }
  }, [recipeId]);

  return (
    <div className={styles.container}>
      <BackBtn />
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
