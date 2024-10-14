import MyButton from "../../customComponents/myButton/myButton";
import styles from "./searchBar.module.scss";

export const SearchBar = () => {
  return (
    <div className={styles.container}>
      <div className={styles.line}>
        <p className={styles.title}>Поиск рецепта</p>
        <div className={styles.searchBar}>
          <input type="text" placeholder="Название Блюда...." className={styles.searchBarInput} />
          <MyButton
            isPrimary={true}
            onClick={() => {
              console.log("Search");
            }}
            width="152px"
            height="73px"
          >
            Поиск
          </MyButton>
        </div>
      </div>
      <div className={styles.line}>
        <div className={styles.tagBar}>
          <p className={styles.tag}>Мясо</p>
          <p className={styles.tag}>Деликатесы</p>
          <p className={styles.tag}>Пироги</p>
          <p className={styles.tag}>Рыба</p>
        </div>
      </div>
    </div>
  );
};
