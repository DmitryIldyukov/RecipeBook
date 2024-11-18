import MyButton from "../../customComponents/myButton/myButton";
import styles from "./recipesPageHeader.module.scss";
import addImage from "../../../assets/images/add.svg";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../constants/constants";
import { usePopupStore } from "../../../hooks/usePopupStore";
import { useAppStore } from "../../../hooks/useStore";

export const RecipesPageHeader = () => {
  const navigate = useNavigate();
  const { userId } = useAppStore();
  const { setIsLoginOrRegistraionPopupOpen } = usePopupStore();

  const navigateToAddRecipe = () => {
    if (userId) {
      navigate(ROUTES.EDIT_RECIPE);
    } else {
      setIsLoginOrRegistraionPopupOpen(true);
    }
  };

  return (
    <div className={styles.subHeader}>
      <h3 className={styles.title}>Рецепты</h3>
      <MyButton onClick={navigateToAddRecipe} isPrimary={true} width="278px" height="60px">
        <img src={addImage} alt="add recipe" /> Добавить рецепт
      </MyButton>
    </div>
  );
};
