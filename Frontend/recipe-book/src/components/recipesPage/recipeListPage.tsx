import addImage from "../../assets/add.svg";
import MyButton from "../customComponents/myButton/myButton";
import styles from "./recipeListPage.module.scss";
import icon1 from "../../assets/ic-menu.svg";
import icon2 from "../../assets/ic-cook.svg";
import icon3 from "../../assets/ic-chef.svg";
import icon4 from "../../assets/ic-hlop.svg";
import { SearchBar } from "./searchBar/searchBar";

export const RecipesPage = () => {
  return (
    <div className={styles.container}>
      <div className={styles.subHeader}>
        <h3 className={styles.title}>Рецепты</h3>
        <MyButton
          onClick={() => {
            console.log("add recipe");
          }}
          isPrimary={true}
          width="278px"
          height="60px"
        >
          <img src={addImage} alt="add recipe" /> Добавить рецепт
        </MyButton>
      </div>
      <div className={styles.cardBox}>
        <div className={styles.card}>
          <div className={styles.cardImage}>
            <img src={icon1} alt="" />
          </div>
          <p className={styles.cardTitle}>Простые блюда</p>
        </div>
        <div className={styles.card}>
          <div className={styles.cardImage}>
            <img src={icon2} alt="" />
          </div>
          <p className={styles.cardTitle}>Детское</p>
        </div>
        <div className={styles.card}>
          <div className={styles.cardImage}>
            <img src={icon3} alt="" />
          </div>
          <p className={styles.cardTitle}>От шеф-поваров</p>
        </div>
        <div className={styles.card}>
          <div className={styles.cardImage}>
            <img src={icon4} alt="" />
          </div>
          <p className={styles.cardTitle}>На праздник</p>
        </div>
      </div>

      <SearchBar />
    </div>
  );
};
