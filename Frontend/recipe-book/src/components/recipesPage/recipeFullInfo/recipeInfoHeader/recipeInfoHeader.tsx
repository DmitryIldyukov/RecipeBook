import styles from "./RecipeInfoHeader.module.scss";
import editIcon from "../../../../assets/edit.svg";
import trashIcon from "../../../../assets/trash.svg";
import MyButton from "../../../customComponents/myButton/myButton";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../../constants/constants";
import { useAppStore } from "../../../../hooks/useStore";
import { Recipe } from "../../../../types/recipe";

type RecipeInfoHeaderProps = {
  recipe: Recipe | undefined;
};

export const RecipeInfoHeader = (props: RecipeInfoHeaderProps) => {
  const navigate = useNavigate();
  const { userId } = useAppStore();

  const navigateToRecipeEdit = () => {
    if (props.recipe?.recipeId) {
      navigate(`${ROUTES.EDIT_RECIPE}/${props.recipe.recipeId.toString()}`);
    }
  };

  return (
    <div className={styles.titleBar}>
      <h2 className={styles.recipeName}>{props.recipe?.name ? props.recipe.name : "Рецепт"}</h2>
      {userId === props.recipe?.authorId && (
        <div className={styles.actionBtnsBox}>
          <button className={styles.deleteBtn}>
            <img src={trashIcon} alt="Удалить" className={styles.p18} />
          </button>
          <MyButton onClick={navigateToRecipeEdit} isPrimary={true} width="278px" height="60px">
            <img src={editIcon} alt="Редактировать" />
            Редактировать
          </MyButton>
        </div>
      )}
    </div>
  );
};
