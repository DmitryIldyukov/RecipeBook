import { COOK_TIME_OPTIONS, PORTION_COUNT_OPTIONS } from "../../../../constants/constants";
import { Tag } from "../../../../types/recipe";
import { TagEditor } from "../tagEditor/tagEditor";
import styles from "./recipeForm.module.scss";

type RecipeFormProps = {
  name: string;
  description: string;
  cookTime: number;
  portionCount: number;
  tags: Tag[];
  setName: React.Dispatch<React.SetStateAction<string>>;
  setDescription: React.Dispatch<React.SetStateAction<string>>;
  setCookTime: React.Dispatch<React.SetStateAction<number>>;
  setPortionCount: React.Dispatch<React.SetStateAction<number>>;
  setTags: React.Dispatch<React.SetStateAction<Tag[]>>;
};

export const RecipeForm = ({
  name,
  description,
  cookTime,
  portionCount,
  tags,
  setName,
  setDescription,
  setCookTime,
  setPortionCount,
  setTags,
}: RecipeFormProps) => {
  return (
    <div className={styles.recipeMainInfoBlock}>
      <div className={styles.recipeInfo}>
        <input
          type="text"
          placeholder="Название рецепта"
          className={styles.inputText}
          value={name}
          onChange={(e) => {
            setName(e.target.value);
          }}
        />
        <textarea
          placeholder="Краткое описание рецепта (150 символов)"
          className={styles.area}
          maxLength={150}
          value={description}
          onChange={(e) => {
            setDescription(e.target.value);
          }}
        />
        <TagEditor tags={tags} setTags={setTags} />
        <div className={styles.secInfoBox}>
          <div className={`${styles.row} ${styles.gap10}`}>
            <select
              value={cookTime}
              onChange={(e) => {
                setCookTime(parseInt(e.target.value));
              }}
              className={styles.selectInput}
            >
              <option value="" disabled>
                Время готовки
              </option>
              {COOK_TIME_OPTIONS.map((option) => (
                <option key={option} value={option}>
                  {option}
                </option>
              ))}
            </select>
            <span className={styles.text}>Минут</span>
          </div>
          <div className={`${styles.row} ${styles.gap10}`}>
            <select
              value={portionCount}
              onChange={(e) => {
                setPortionCount(parseInt(e.target.value));
              }}
              className={styles.selectInput}
            >
              <option value="" disabled>
                Порций в блюде
              </option>
              {PORTION_COUNT_OPTIONS.map((option) => (
                <option key={option} value={option}>
                  {option}
                </option>
              ))}
            </select>
            <span className={styles.text}>Персон</span>
          </div>
        </div>
      </div>
    </div>
  );
};
