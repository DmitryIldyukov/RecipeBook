import { Ingredient } from "../../../../types/recipe";
import styles from "./ingredientsList.module.scss";
import closeIcon from "../../../../assets/images/close.svg";
import MyButton from "../../../customComponents/myButton/myButton";
import addImage from "../../../../assets/images/add-ptimary.svg";

type IngredientsListProps = {
  ingredients: Ingredient[];
  setIngredients: (ingredients: Ingredient[]) => void;
};

export const IngredientsList = ({ ingredients, setIngredients }: IngredientsListProps) => {
  const handleIngredientChange = (index: number, field: keyof Ingredient, value: string) => {
    const updatedIngredients = [...ingredients];
    (updatedIngredients[index][field] as string) = value;
    setIngredients(updatedIngredients);
  };

  const addIngredient = () => {
    setIngredients([...ingredients, { id: undefined, title: "", description: "" }]);
  };

  const removeIngredient = (index: number) => {
    if (ingredients.length > 1) {
      setIngredients(ingredients.filter((_, i) => i !== index));
    }
  };

  return (
    <div className={styles.ingredientBlock}>
      <p className={styles.ingredientBlockTitle}>Ингредиенты</p>
      <ul className={styles.ingredientBlockContent}>
        {ingredients.map((ingredient, index) => (
          <li key={index} className={styles.ingredientInfo}>
            <button
              type="button"
              className={styles.closeIconBox}
              onClick={() => {
                removeIngredient(index);
              }}
            >
              <img src={closeIcon} alt="close" className={styles.closeIcon} />
            </button>
            <div className={styles.ingredientData}>
              <input
                placeholder="Заголовок для ингридиентов"
                className={styles.ingredientTitle}
                value={ingredient.title}
                onChange={(e) => {
                  handleIngredientChange(index, "title", e.target.value);
                }}
              />
              <textarea
                placeholder="Список подуктов для категории"
                className={styles.ingredientDescription}
                value={ingredient.description}
                onChange={(e) => {
                  handleIngredientChange(index, "description", e.target.value);
                }}
              />
            </div>
          </li>
        ))}
      </ul>
      <MyButton isPrimary={false} onClick={addIngredient} width="380px" height="60px">
        <img src={addImage} alt="add recipe" /> Добавить заголовок
      </MyButton>
    </div>
  );
};
