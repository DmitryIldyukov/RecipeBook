import { useEffect, useState } from "react";
import { recipeService } from "../../../../services/recipeService";
import styles from "./userRecipesList.module.scss";
import { Recipe } from "../../../../types/recipe";
import { RecipeCard } from "../../../customComponents/recipeCard/recipeCard";
import { useAppStore } from "../../../../hooks/useStore";

export const UserRecipesList = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);

  const { userId } = useAppStore();

  useEffect(() => {
    const getRecipes = async () => {
      try {
        if (userId) {
          const data = await recipeService.getUserRecipes(userId);
          setRecipes(data);
        }
      } catch (error) {
        console.error(error);
      }
    };

    void getRecipes();
  }, []);

  return (
    <ul className={styles.recipeListBox}>
      {recipes.length > 0 ? (
        recipes.map((recipe) => <RecipeCard recipe={recipe} key={recipe.recipeId} />)
      ) : (
        <p className={styles.emptyRecipeListText}>Рецепты не найдены</p>
      )}
    </ul>
  );
};
