import { Ingredient } from "../../../../../types/recipe";
import styles from "./ingredientPart.module.scss";

type IngredientPartProps = {
  ingredients: Ingredient[];
};

export const IngredientPart = (props: IngredientPartProps) => {
  return (
    <div className={styles.ingredientBlock}>
      <p className={styles.ingredientBlockTitle}>Ингредиенты</p>
      <ul className={styles.ingredientBlockContent}>
        {props.ingredients.length > 0 ? (
          props.ingredients.map((ingredient) => (
            <li key={ingredient.id} className={styles.ingredientInfo}>
              <p className={styles.ingredientTitle}>{ingredient.title}</p>
              <p className={styles.ingredientDescription}>{ingredient.description}</p>
            </li>
          ))
        ) : (
          <p className={styles.notFoundText}>Ингредиенты не найдены.</p>
        )}
      </ul>
    </div>
  );
};
